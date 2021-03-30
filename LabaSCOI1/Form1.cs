using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabaKG1
{
    public partial class Form1 : Form
    {
        List<Bitmap> img = new List<Bitmap>();
        static List<PictureBox> PicBox = new List<PictureBox>();
        static List<ComboBox> ComBox = new List<ComboBox>();
        static List<CheckBox> CheBox = new List<CheckBox>();
        static List<TrackBar> TraBar = new List<TrackBar>();
        public Bitmap _resultImage = new Bitmap(width: 1, height: 1);

        List<int> y = new List<int>();
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
        public Form1()
        {
            InitializeComponent();
        }
        public void AbraCadabra()
        {
            int k = 0;
            string[] RGB = new string[3];
            RGB[0] = "R";
            RGB[1] = "G";
            RGB[2] = "B";
            string[] Opper = new string[6];
            Opper[0] = "Нет";
            Opper[1] = "Сумма";
            Opper[2] = "Умножение";
            Opper[3] = "Среднее арифметическое";
            Opper[4] = "Минимум";
            Opper[5] = "Максимум";
            y.Add(15);
            y.Add(202);
            y.Add(228);
            y.Add(246);
            y.Add(266);
            y.Add(288);
            //if (img.Count >= 2)
            //{
            //    for (int i = 0; i < y.Count; i++)
            //    {
            //        y[i] += 320;
            //    }
            //}
            PicBox.Add(new PictureBox());
            //PicBox[img.Count - 1].Location = new Point(570, y[k]);
            PicBox[img.Count - 1].SizeMode = PictureBoxSizeMode.Zoom;
            PicBox[img.Count - 1].Size = new Size(257, 183);
            PicBox[img.Count - 1].BorderStyle = BorderStyle.Fixed3D;
            PicBox[img.Count - 1].Image = img[img.Count - 1];

            //k++;
            ComBox.Add(new ComboBox());
            //ComBox[img.Count - 1].Location = new Point(570, y[k]);
            ComBox[img.Count - 1].Size = new Size(257, 24);
            ComBox[img.Count - 1].Items.Add(Opper[0]);
            ComBox[img.Count - 1].Items.Add(Opper[1]);
            ComBox[img.Count - 1].Items.Add(Opper[2]);
            ComBox[img.Count - 1].Items.Add(Opper[3]);
            ComBox[img.Count - 1].Items.Add(Opper[4]);
            ComBox[img.Count - 1].Items.Add(Opper[5]);
            ComBox[img.Count - 1].SelectedItem = Opper[0];
            //k++;
            CheBox.Add(new CheckBox());
            CheBox[(img.Count * 3) - 3].Text = RGB[0];
            CheBox[(img.Count * 3) - 3].Checked = true;
            //CheBox[(img.Count * 3) - 3].Location = new Point(570, y[k]);
            CheBox[(img.Count * 3) - 3].Size = new Size(40, 21);
            //k++;
            CheBox.Add(new CheckBox());
            CheBox[(img.Count * 3) - 2].Text = RGB[1];
            CheBox[(img.Count * 3) - 2].Checked = true;
            CheBox[(img.Count * 3) - 2].Location = new Point(570, y[k]);
            CheBox[(img.Count * 3) - 2].Size = new Size(40, 21);
            //k++;
            CheBox.Add(new CheckBox());
            CheBox[(img.Count * 3) - 1].Text = RGB[2];
            CheBox[(img.Count * 3) - 1].Checked = true;
            CheBox[(img.Count * 3) - 1].Location = new Point(570, y[k]);
            CheBox[(img.Count * 3) - 1].Size = new Size(40, 21);
            //k++;
            TraBar.Add(new TrackBar());
            //TraBar[img.Count - 1].Location = new Point(570, y[k]);
            TraBar[img.Count - 1].Size = new Size(257, 56);
            TraBar[img.Count - 1].Maximum = 100;
            TraBar[img.Count - 1].Minimum = 1;
            TraBar[img.Count - 1].Value = 1;
            TraBar[img.Count - 1].AutoSize = false;
            FlowLayoutPanel FLP = new FlowLayoutPanel();
            FLP.Controls.Add(PicBox[img.Count - 1]);
            FLP.Controls.Add(ComBox[img.Count - 1]);
            FLP.Controls.Add(CheBox[(img.Count * 3) - 3]);
            FLP.Controls.Add(CheBox[(img.Count * 3) - 2]);
            FLP.Controls.Add(CheBox[(img.Count * 3) - 1]);
            FLP.Controls.Add(TraBar[img.Count - 1]);
            FLP.Dock = DockStyle.Top;
            FLP.Height = 340;
            FLP.Margin = new Padding(0);
            


            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 257));
            ++tableLayoutPanel1.RowCount;
            tableLayoutPanel1.Controls.Add(FLP, 0, tableLayoutPanel1.RowCount - 1);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                foreach(string FileName in openFileDialog.FileNames)
                {
                    img.Add(To24bppRgb(new Bitmap(FileName)));
                }

            }
            if(img.Count <= 1)
            {
                pictureBox1.Image = (Bitmap)img[img.Count - 1].Clone();
            }    
            AbraCadabra();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileFialog = new SaveFileDialog();
            saveFileFialog.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileFialog.Filter = "Картинки (png, jpg, bmp, gif) |*.png;*.jpg;*.bmp;*.gif|All files (*.*)|*.*";
            saveFileFialog.RestoreDirectory = true;

            if (saveFileFialog.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(saveFileFialog.FileName);
                }
            }
        }

        private static byte SumByte(float bt1, float bt2)
        {
            float result = bt1 + bt2;
            if (result > 255)
                return 255;
            if (result < 0)
                return 0;
            return (byte)result;
        }
        private static byte MinByte(float bt1, float bt2)
        {
            float result = Math.Min(bt1, bt2);
            if (result > 255)
                return 255;
            if (result < 0)
                return 0;
            return (byte)result;
        }
        private static byte MaxByte(float bt1, float bt2)
        {
            float result = Math.Max(bt1, bt2);
            if (result > 255)
                return 255;
            if (result < 0)
                return 0;
            return (byte)result;
        }
        public static bool Summary(Bitmap Source, Bitmap Changer, bool r, bool g, bool b, int Transparency = 0)
        {
            var bmpData = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr = bmpData.Scan0;
            var Size = bmpData.Stride * bmpData.Height;
            byte[] bytes1 = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes1, 0, Size);

            var bmpData1 = Changer.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr1 = bmpData1.Scan0;
            var Size1 = bmpData1.Stride * bmpData1.Height;
            byte[] bytes2 = new byte[Size1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bytes2, 0, Size1);

            float TranspCoef = 1 - (Transparency / 100.0f);
            int count = bytes2.Length;
            if (r == true && g == true && b == true)
            {
                for (int i = 0; i < count; i++) //bgr
                {
                    bytes1[i] = SumByte(bytes1[i], TranspCoef * bytes2[i]);
                }
            }
            if (r == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = SumByte(bytes1[i + 1], TranspCoef * bytes2[i + 1]);
                    bytes1[i + 2] = SumByte(bytes1[i + 2], TranspCoef * bytes2[i + 2]);
                }
            }
            if (r == true && b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = SumByte(bytes1[i], TranspCoef * bytes2[i]);
                    bytes1[i + 2] = SumByte(bytes1[i + 2], TranspCoef * bytes2[i + 2]);
                }
            }
            if (b == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = SumByte(bytes1[i], TranspCoef * bytes2[i]);
                    bytes1[i + 1] = SumByte(bytes1[i + 1], TranspCoef * bytes2[i + 1]);
                }
            }
            if (g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = SumByte(bytes1[i + 1], TranspCoef * bytes2[i + 1]);
                }
            }
            if (b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = SumByte(bytes1[i], TranspCoef * bytes2[i]);
                }
            }
            if (r == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 2] = SumByte(bytes1[i + 2], TranspCoef * bytes2[i + 2]);
                }
            }
            //Редачим сурс
            System.Runtime.InteropServices.Marshal.Copy(bytes1, 0, ptr, Size);
            Source.UnlockBits(bmpData);
            Changer.UnlockBits(bmpData1);
            return true;
        }
        public bool Summary(Bitmap Source, Bitmap img, int i)
        {
            return Summary(Source, img, CheBox[i*3].Checked, CheBox[i*3+1].Checked, CheBox[i*3+2].Checked, TraBar[i].Value);
        }
        public static bool Multiply(Bitmap Source, Bitmap Changer, bool r, bool g, bool b, int Transparency)
        {
            var bmpData = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr = bmpData.Scan0;
            var Size = bmpData.Stride * bmpData.Height;
            byte[] bytes1 = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes1, 0, Size);

            var bmpData1 = Changer.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr1 = bmpData1.Scan0;
            var Size1 = bmpData1.Stride * bmpData1.Height;
            byte[] bytes2 = new byte[Size1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bytes2, 0, Size1);

            float TranspCoef = 1 - (Transparency / 100.0f);
            int count = bytes2.Length;
            if (r == true && g == true && b == true)
            {
                for (int i = 0; i < count; i++) //bgr
                {
                    bytes1[i] = (byte)(bytes1[i] * (TranspCoef * (bytes2[i] / 255.0f)));    
                }
            }
            if (r == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = (byte)(bytes1[i + 1] * (TranspCoef * (bytes2[i + 1] / 255.0f)));
                    bytes1[i + 2] = (byte)(bytes1[i + 2] * (TranspCoef * (bytes2[i + 2] / 255.0f)));
                }
            }
            if (r == true && b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)(bytes1[i] * (TranspCoef * (bytes2[i] / 255.0f)));
                    bytes1[i + 2] = (byte)(bytes1[i + 2] * (TranspCoef * (bytes2[i + 2] / 255.0f)));
                }
            }
            if (b == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)(bytes1[i] * (TranspCoef * (bytes2[i] / 255.0f)));
                    bytes1[i + 1] = (byte)(bytes1[i + 1] * (TranspCoef * (bytes2[i + 1] / 255.0f)));
                }
            }
            if (g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = (byte)(bytes1[i + 1] * (TranspCoef * (bytes2[i + 1] / 255.0f)));
                }
            }
            if (b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)(bytes1[i] * (TranspCoef * (bytes2[i] / 255.0f)));
                }
            }
            if (r == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 2] = (byte)(bytes1[i] * (TranspCoef * (bytes2[i] / 255.0f)));
                }
            }
            //Редачим сурс
            System.Runtime.InteropServices.Marshal.Copy(bytes1, 0, ptr, Size);
            Source.UnlockBits(bmpData);
            Changer.UnlockBits(bmpData1);
            return true;
        }
        public bool Multiply(Bitmap Source, Bitmap img, int i)
        {
            return Multiply(Source, img, CheBox[i * 3].Checked, CheBox[i * 3 + 1].Checked, CheBox[i * 3 + 2].Checked, TraBar[i].Value);
        }

        public static bool Average(Bitmap Source, Bitmap Changer, bool r, bool g, bool b, int Transparency = 0)
        {
            var bmpData = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr = bmpData.Scan0;
            var Size = bmpData.Stride * bmpData.Height;
            byte[] bytes1 = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes1, 0, Size);

            var bmpData1 = Changer.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr1 = bmpData1.Scan0;
            var Size1 = bmpData1.Stride * bmpData1.Height;
            byte[] bytes2 = new byte[Size1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bytes2, 0, Size1);

            float TranspCoef = 1 - (Transparency / 100.0f);
            int count = bytes2.Length;
            if (r == true && g == true && b == true)
            {
                for (int i = 0; i < count; i++) //bgr
                {
                    bytes1[i] = (byte)((bytes1[i] + bytes2[i]) / 2);
                }
            }
            if (r == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = (byte)((bytes1[i + 1] + bytes2[i + 1]) / 2.0);
                    bytes1[i + 2] = (byte)((bytes1[i + 2] + bytes2[i + 2]) / 2.0);
                }
            }
            if (r == true && b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)((bytes1[i] + bytes2[i]) / 2.0);
                    bytes1[i + 2] = (byte)((bytes1[i + 2] + bytes2[i + 2]) / 2.0);
                }
            }
            if (b == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)((bytes1[i] + bytes2[i]) / 2.0);
                    bytes1[i + 1] = (byte)((bytes1[i + 1] + bytes2[i + 1]) / 2.0);
                }
            }
            if (g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = (byte)((bytes1[i + 1] + bytes2[i + 1]) / 2.0);
                }
            }
            if (b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = (byte)((bytes1[i] + bytes2[i]) / 2.0);
                }
            }
            if (r == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 2] = (byte)((bytes1[i + 2] + bytes2[i + 2]) / 2.0);
                }
            }
            //Редачим сурс
            System.Runtime.InteropServices.Marshal.Copy(bytes1, 0, ptr, Size);
            Source.UnlockBits(bmpData);
            Changer.UnlockBits(bmpData1);
            return true;
        }
        public bool Average(Bitmap Source, Bitmap img, int i)
        {
            return Average(Source, img, CheBox[i * 3].Checked, CheBox[i * 3 + 1].Checked, CheBox[i * 3 + 2].Checked, TraBar[i].Value);
        }
        public static bool Max(Bitmap Source, Bitmap Changer, bool r, bool g, bool b, int Transparency = 0)
        {
            var bmpData = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr = bmpData.Scan0;
            var Size = bmpData.Stride * bmpData.Height;
            byte[] bytes1 = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes1, 0, Size);

            var bmpData1 = Changer.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr1 = bmpData1.Scan0;
            var Size1 = bmpData1.Stride * bmpData1.Height;
            byte[] bytes2 = new byte[Size1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bytes2, 0, Size1);

            float TranspCoef = 1 - (Transparency / 100.0f);
            int count = bytes2.Length;
            if (r == true && g == true && b == true)
            {
                for (int i = 0; i < count; i++) //bgr
                {
                    bytes1[i] = bytes1[i] > bytes2[i] ? bytes1[i] : (byte)(TranspCoef * bytes2[i]);
                }
            }
            if (r == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? bytes1[i + 1] : (byte)(TranspCoef * bytes2[i + 1]);
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? bytes1[i + 2] : (byte)(TranspCoef * bytes2[i + 2]);
                }
            }
            if (r == true && b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? bytes1[i] : (byte)(TranspCoef * bytes2[i]);
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? bytes1[i + 2] : (byte)(TranspCoef * bytes2[i + 2]);
                }
            }
            if (b == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? bytes1[i] : (byte)(TranspCoef * bytes2[i]);
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? bytes1[i + 1] : (byte)(TranspCoef * bytes2[i + 1]);
                }
            }
            if (g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? bytes1[i + 1] : (byte)(TranspCoef * bytes2[i + 1]);
                }
            }
            if (b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? bytes1[i] : (byte)(TranspCoef * bytes2[i]);
                }
            }
            if (r == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? bytes1[i + 2] : (byte)(TranspCoef * bytes2[i + 2]);
                }
            }
            //Редачим сурс
            System.Runtime.InteropServices.Marshal.Copy(bytes1, 0, ptr, Size);
            Source.UnlockBits(bmpData);
            Changer.UnlockBits(bmpData1);
            return true;
        }
        public bool Max(Bitmap Source, Bitmap img, int i)
        {
            return Max(Source, img, CheBox[i * 3].Checked, CheBox[i * 3 + 1].Checked, CheBox[i * 3 + 2].Checked, TraBar[i].Value);
        }
        public static bool Min(Bitmap Source, Bitmap Changer, bool r, bool g, bool b, int Transparency = 0)
        {
            var bmpData = Source.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr = bmpData.Scan0;
            var Size = bmpData.Stride * bmpData.Height;
            byte[] bytes1 = new byte[Size];
            System.Runtime.InteropServices.Marshal.Copy(ptr, bytes1, 0, Size);

            var bmpData1 = Changer.LockBits(new Rectangle(0, 0, Source.Width, Source.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, Source.PixelFormat);
            var ptr1 = bmpData1.Scan0;
            var Size1 = bmpData1.Stride * bmpData1.Height;
            byte[] bytes2 = new byte[Size1];
            System.Runtime.InteropServices.Marshal.Copy(ptr1, bytes2, 0, Size1);

            float TranspCoef = 1 - (Transparency / 100.0f);
            int count = bytes2.Length;
            if (r == true && g == true && b == true)
            {
                for (int i = 0; i < count; i++) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? (byte)(TranspCoef * bytes2[i]) : bytes1[i];
                }
            }
            if (r == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? (byte)(TranspCoef * bytes2[i + 1]) : bytes1[i + 1];
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? (byte)(TranspCoef * bytes2[i + 2]) : bytes1[i + 2];
                }
            }
            if (r == true && b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? (byte)(TranspCoef * bytes2[i]) : bytes1[i];
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? (byte)(TranspCoef * bytes2[i + 2]) : bytes1[i + 2];
                }
            }
            if (b == true && g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? (byte)(TranspCoef * bytes2[i]) : bytes1[i];
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? (byte)(TranspCoef * bytes2[i + 1]) : bytes1[i + 1];
                }
            }
            if (g == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 1] = bytes1[i + 1] > TranspCoef * bytes2[i + 1] ? (byte)(TranspCoef * bytes2[i + 1]) : bytes1[i + 1];
                }
            }
            if (b == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i] = bytes1[i] > TranspCoef * bytes2[i] ? (byte)(TranspCoef * bytes2[i]) : bytes1[i];
                }
            }
            if (r == true)
            {
                for (int i = 0; i < count; i += 3) //bgr
                {
                    bytes1[i + 2] = bytes1[i + 2] > TranspCoef * bytes2[i + 2] ? (byte)(TranspCoef * bytes2[i + 2]) : bytes1[i + 2];
                }
            }
            //Редачим сурс
            System.Runtime.InteropServices.Marshal.Copy(bytes1, 0, ptr, Size);
            Source.UnlockBits(bmpData);
            Changer.UnlockBits(bmpData1);
            return true;
        }
        public bool Min(Bitmap Source, Bitmap img, int i)
        {
            return Min(Source, img, CheBox[i * 3].Checked, CheBox[i * 3 + 1].Checked, CheBox[i * 3 + 2].Checked, TraBar[i].Value);
        }

        public Bitmap To24bppRgb(Bitmap bitmap) //Возвращает битмап в заданном формате
        {
            var newbit24rgb = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), PixelFormat.Format24bppRgb);
            bitmap.Dispose();
            return newbit24rgb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _resultImage = img[img.Count - 1].Clone(new Rectangle(0, 0, img[img.Count - 1].Width, img[img.Count - 1].Height), PixelFormat.Format24bppRgb);
            _resultImage = To24bppRgb(_resultImage);
            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = img.Count - 2; i >= 0; i--)
            {
                Bitmap promj1 = new Bitmap(_resultImage, Math.Max(_resultImage.Width, img[i].Width), Math.Max(_resultImage.Height, img[i].Height));
                _resultImage.Dispose();
                var promj = promj1.Clone() as Bitmap;
                _resultImage = To24bppRgb(promj1);
                switch (ComBox[i].SelectedIndex)
                {
                    case 0:
                        break;
                    case 1:
                        Summary(Source: _resultImage, new Bitmap(img[i], Math.Max(promj.Width, img[i].Width), Math.Max(promj.Height, img[i].Height)), i);
                        break;
                    case 2:
                        Multiply(Source: _resultImage, new Bitmap(img[i], Math.Max(promj.Width, img[i].Width), Math.Max(promj.Height, img[i].Height)), i);
                        break;
                    case 3:
                        Average(Source: _resultImage, new Bitmap(img[i], Math.Max(promj.Width, img[i].Width), Math.Max(promj.Height, img[i].Height)), i);
                        break;
                    case 4:
                        Min(Source: _resultImage, new Bitmap(img[i], Math.Max(promj.Width, img[i].Width), Math.Max(promj.Height, img[i].Height)), i);
                        break;
                    case 5:
                        Max(Source: _resultImage, new Bitmap(img[i], Math.Max(promj.Width, img[i].Width), Math.Max(promj.Height, img[i].Height)), i);
                        break;
                    default:
                        break;
                }
            }
            timer.Stop();
            label2.Text = Convert.ToString(timer.ElapsedMilliseconds / 1000.0);
            label2.Visible = true;
            pictureBox1.Image = _resultImage;
            //image7 = (Bitmap)image3.Clone();
        }
    }
}
