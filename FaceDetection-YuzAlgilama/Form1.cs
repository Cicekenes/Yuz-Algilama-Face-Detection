using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection_YuzAlgilama
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FilterInfoCollection filter; // Bilgisayarımızdaki cihazların tutulduğu sınıftır.
        VideoCaptureDevice device; //Webcam başlatma

        private void Form1_Load(object sender, EventArgs e)
        {
            filter = new FilterInfoCollection(FilterCategory.VideoInputDevice); //Cihazımızda bulunan video kayıt araçları
            foreach (FilterInfo  device in filter )
            
                cmbCihazlar.Items.Add(device.Name); // foreach döngüsü ile cihazımızda kayıtlı olan cihazları comboboxa yazdırma
            cmbCihazlar.SelectedIndex = 0;
            device = new VideoCaptureDevice();
        }


        private void BtnAlgila_Click(object sender, EventArgs e)
        {
            device = new VideoCaptureDevice(filter[cmbCihazlar.SelectedIndex].MonikerString);
            device.NewFrame += Device_NewFrame;
            device.Start();
        }
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        private void Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> griResim = new Image<Bgr, byte>(bitmap);
            Rectangle[] dikdortgen = cascadeClassifier.DetectMultiScale(griResim, 1.2, 1);
            foreach (Rectangle rectangle in dikdortgen)
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Green, 1))
                    {
                        graphics.DrawRectangle(pen, rectangle);

                    }
                }

            }
            pictureBox1.Image = bitmap;
        }
    }
}
