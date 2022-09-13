using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCOI5
{
    public partial class Form1 : Form
    {
        private Bitmap image1 = null;
        TypeFilterFreq Type;
        public Form1()
        {
            string[] Filter = new string[2];
            Filter[0] = "Высокочастотный";
            Filter[1] = "Низкочастотный";
            InitializeComponent();
            comboBox1.Items.Add(Filter[0]);
            comboBox1.Items.Add(Filter[1]);
            comboBox1.SelectedItem = Filter[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";

            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (image1 != null)
                {
                    pictureBox1.Image = null;
                    
                }

                image1 = new Bitmap(openFileDialog.FileName);
                var image2 = new Bitmap(image1, new Size(pictureBox1.Size.Width, pictureBox1.Size.Height));
                pictureBox1.Image = image2;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool flag = false;
            Type = (TypeFilterFreq)comboBox1.SelectedIndex;
            var Filters = richTextBox1.Text.Split('\n');
            List<FilterCircle> Spisok = new List<FilterCircle>();
            foreach (var filter in Filters)
            {
                Spisok.Add(new FilterCircle(filter));
            }
            if(checkBox1.Checked == true)
            {
                flag = true;
            }
            var Images = Img.Obrabotka(image1, Spisok, Type, flag);
            pictureBox2.Image = new Bitmap(Images.NewImage, new Size(pictureBox2.Size.Width, pictureBox2.Size.Height));
            pictureBox3.Image = new Bitmap(Images.FilterImage, new Size(pictureBox2.Size.Width, pictureBox2.Size.Height));
            pictureBox4.Image = new Bitmap(Images.FourierImage, new Size(pictureBox2.Size.Width, pictureBox2.Size.Height));
        }
    }
    public static class FT
    {
        public static Complex[] ditfft2(Complex[] arr, int x0, int N, int s)
        {
            Complex[] X = new Complex[N];
            if (N == 1)
            {
                X[0] = arr[x0];
            }
            else
            {
                ditfft2(arr, x0, N / 2, 2 * s).CopyTo(X, 0);
                ditfft2(arr, x0 + s, N / 2, 2 * s).CopyTo(X, N / 2);

                for (int k = 0; k < N / 2; k++)
                {
                    var t = X[k];
                    double u = -2.0 * Math.PI * k / N;
                    X[k] = t + new Complex(Math.Cos(u), Math.Sin(u)) * X[k + N / 2];
                    X[k + N / 2] = t - new Complex(Math.Cos(u), Math.Sin(u)) * X[k + N / 2];
                }
            }

            return X;
        }

        public static Complex[] ditfft2d(Complex[] arr, int width, int height, bool use_FFT = true) //прямое фурье преобразование
        {
            Complex[] X = new Complex[arr.Length];

            ParallelOptions opt = new ParallelOptions();
            if (Environment.ProcessorCount > 2)
                opt.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;
            else opt.MaxDegreeOfParallelism = 1;
            //for (int i = 0; i < height; ++i)
            Parallel.For(0, height, opt, i =>
            {
                Complex[] tmp = new Complex[width];
                Array.Copy(arr, i * width, tmp, 0, width);
                tmp = ditfft2(tmp, 0, tmp.Length, 1);

                for (int k = 0; k < width; ++k)
                    X[i * width + k] = tmp[k] / width;
            }
            );
            //for (int j = 0; j < width; ++j)
            Parallel.For(0, width, opt, j =>
            {
                Complex[] tmp = new Complex[height];
                for (int k = 0; k < height; ++k)
                    tmp[k] = X[j + k * width];
                tmp = ditfft2(tmp, 0, tmp.Length, 1);

                for (int k = 0; k < height; ++k)
                    X[j + k * width] = tmp[k] / height;
            }
            );
            return X;
        }

        public static Complex[] ditifft2d(Complex[] arr, int width, int height, bool use_FFT = true) //обратное фурье преобразование
        {
            Complex[] X = new Complex[arr.Length];

            ParallelOptions opt = new ParallelOptions();
            if (Environment.ProcessorCount > 2)
                opt.MaxDegreeOfParallelism = Environment.ProcessorCount - 1;
            else opt.MaxDegreeOfParallelism = 1;

            Parallel.For(0, height, opt, i =>
            {
                Complex[] tmp = new Complex[width];
                Array.Copy(arr, i * width, tmp, 0, width);
                for (int k = 0; k < width; ++k)
                    tmp[k] = new Complex(arr[i * width + k].Real, -arr[i * width + k].Imaginary);

                tmp = ditfft2(tmp, 0, tmp.Length, 1);

                for (int k = 0; k < width; ++k)
                    X[i * width + k] = new Complex(tmp[k].Real, -tmp[k].Imaginary);

            }
            );

            Parallel.For(0, width, opt, j =>
            {
                Complex[] tmp = new Complex[height];
                for (int k = 0; k < height; ++k)
                    tmp[k] = new Complex(X[j + k * width].Real, -X[j + k * width].Imaginary);

                tmp = ditfft2(tmp, 0, tmp.Length, 1);

                for (int k = 0; k < height; ++k)
                    X[j + k * width] = (new Complex(tmp[k].Real, -tmp[k].Imaginary));
            }
            );
            return X;
        }
    }
    public static class Img
    {

        public static Bitmap ToBitmapSource(this byte[] Bytes, int stride, int width, int height)
        {
            Bitmap Bimage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, Bimage.Width, Bimage.Height);
            BitmapData bmpData =
                Bimage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * Bimage.Height;
            System.Runtime.InteropServices.Marshal.Copy(Bytes, 0, ptr, bytes);

            Bimage.UnlockBits(bmpData);

            return Bimage;
        }   
        public static double F(double x)
        {
            return Math.Log(x + 1);
        }
        static byte clmp(double d)
        {
            if (d > 255)
                return 255;
            if (d < 0)
                return 0;
            return (byte)d;
        }
        public static ResultImages Obrabotka(Bitmap source, List<FilterCircle> Filters, TypeFilterFreq FilterFreq, bool flag)
        {

            if (source == null)
            {
                return new ResultImages() { FilterImage = null, FourierImage = null, NewImage = null };
            }

            int _stride;
            int _height = source.Height;
            int _width = source.Width;
            int _bitsPerPixel;

            int new_width = _width;
            int new_height = _height;

            var p = Math.Log(_width,2);
            if (p != Math.Floor(p))
                new_width = (int)Math.Pow(2, Math.Ceiling(p));
            p = Math.Log(_height,2);
            if (p != Math.Floor(p))
                new_height = (int)Math.Pow(2, Math.Ceiling(p));

            Bitmap _tmp = new Bitmap(new_width, new_height, PixelFormat.Format32bppArgb);
            _tmp.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            Graphics g = Graphics.FromImage(_tmp);
            g.DrawImageUnscaled(source, 0, 0);

            byte[] oldBytes = _tmp.ToByte(out _stride, out _height, out _width);
            int ink = 4;

            byte[] newBytes = new byte[oldBytes.Length]; //обычное изображение
            byte[] fourBytes = new byte[oldBytes.Length]; //фурье образ
            byte[] fourFilterBytes = new byte[oldBytes.Length]; //изображение с фильтрами

            Complex[] complex_bytes = new Complex[_width * _height];
            int BytesOnPixel = 4;
            for (int color = 0; color <= 2; color++)
            {

                for (int i = 0; i < _width * _height; ++i)
                {
                    int y = i / _width;
                    int x = i - y * _width;
                    complex_bytes[i] = Math.Pow(-1, x + y) * oldBytes[i * BytesOnPixel + color];
                }

                complex_bytes = FT.ditfft2d(complex_bytes, _width, _height);

                var max_ma = complex_bytes.Max(x => F(x.Imaginary));

                Complex[] complex_bytes_filtered = (Complex[])complex_bytes.Clone();
                if (Filters != null)
                {
                    if (flag == true)
                    {
                        Filters.AddMirrors();
                    }

                    int centerX = _width / 2;
                    int centerY = _height / 2;

                    for (int i = 0; i < (_width * _height); i++)
                    {

                        bool inFigure = false;
                        int y = i / _width;
                        int x = i % _width;
                        for (int filterCount = 0; filterCount < Filters.Count; filterCount++)
                        {
                            if (Filters[filterCount].FilterCoef(x - centerX, centerY - y) == 1)
                            {
                                inFigure = true;
                                break;
                            }
                        }
                        switch (FilterFreq)
                        {
                            case TypeFilterFreq.HightFreq:
                                {
                                    if (inFigure)
                                    {
                                        complex_bytes_filtered[i] *= 0; //если пиксель не попадает в кружок
                                    }
                                    else
                                    {
                                        fourFilterBytes[i * ink] = 255;
                                        fourFilterBytes[i * ink + 1] = 255;
                                        fourFilterBytes[i * ink + 2] = 255;
                                    }
                                    break;
                                }
                            case TypeFilterFreq.LowFreq:
                                {
                                    if (!inFigure)
                                    {
                                        complex_bytes_filtered[i] *= 0;
                                    }
                                    else
                                    {
                                        fourFilterBytes[i * ink] = 255;
                                        fourFilterBytes[i * ink + 1] = 255;
                                        fourFilterBytes[i * ink + 2] = 255;
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                    }
                }
                var complex_bytes_result = FT.ditifft2d(complex_bytes_filtered, _width, _height);

                for (int i = 0; i < _width * _height; ++i)
                {
                    int y = i / _width;
                    int x = i - y * _width;
                    y -= _height / 2;
                    x -= _width / 2;
                    newBytes[i * BytesOnPixel + color] = clmp(Math.Round((Math.Pow(-1, x + y) * complex_bytes_result[i]).Real));
                    fourBytes[i * BytesOnPixel + color] = clmp(10 * F(complex_bytes[i].Magnitude) * 255 / max_ma);
                }
            }
            for(int i = 0; i < _width*_height;i++)
            {
                newBytes[i * 4 +3] = fourBytes[i * 4 + 3] = fourFilterBytes[i * 4 + 3] = 255;
            }
            return new ResultImages() { NewImage = newBytes.ToBitmapSource(_stride, _width, _height), FourierImage = fourBytes.ToBitmapSource(_stride, _width, _height), FilterImage = fourFilterBytes.ToBitmapSource(_stride, _width, _height) };

        }

        public static byte[] ToByte(this Bitmap bitmaps, out int Stride, out int Height, out int Width) //битмап сурс превращает в массив байтов
        {
            Width = bitmaps.Width;
            Height = bitmaps.Height;

            Rectangle rect = new Rectangle(0, 0, Width, Height);
            BitmapData bmpData =
                bitmaps.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;
            Stride = bmpData.Stride;
            int bytes = Math.Abs(bmpData.Stride) * bitmaps.Height;
            byte[] bgrAValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, bgrAValues, 0, bytes);

            bitmaps.UnlockBits(bmpData);

            return bgrAValues;
        }
    }
    public class FilterCircle
    {
        public double Ycenter { get; set; }
        public double Xcenter { get; set; }
        public double Radius { get; set; }

        /// <summary>
        /// 1 если точка внутри круга
        /// 0 если точка вне круга
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public FilterCircle()
        {

        }
        public FilterCircle(string Data)
        {
            var nums = Data.Split(';');

            Xcenter = double.Parse(nums[0]);
            Ycenter = double.Parse(nums[1]);
            Radius = double.Parse(nums[2]);

        }
        public double FilterCoef(double x, double y)
        {
            if (Math.Pow(x - Xcenter, 2) + Math.Pow(y - Ycenter, 2) < Radius * Radius)
                return 1;
            else
                return 0;
        }

        public FilterCircle MirrorFilterFigure(FilterCircle filter)
        {
            return new FilterCircle() { Xcenter = -((FilterCircle)filter).Xcenter, Ycenter = -((FilterCircle)filter).Ycenter, Radius = ((FilterCircle)filter).Radius };
        }
    }

    public static class FilterFigureMirror
    {
        public static void AddMirrors(this List<FilterCircle> filters)
        {
            int Leng = filters.Count;
            for (int i = 0; i < Leng; i++)
            {
                filters.Add(filters[i].MirrorFilterFigure(filters[i]));
            }
        }
    }

    public enum TypeFilterFreq
    {
        HightFreq,
        LowFreq
    }
    public class ResultImages
    {
        public ResultImages()
        {
        }

        public Bitmap FilterImage { get; set; }
        public Bitmap FourierImage { get; set; }
        public Bitmap NewImage { get; set; }
    }
}
