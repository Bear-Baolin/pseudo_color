using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;//定义BitMap类
        Bitmap rgbmap;//
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //读取图片
            string startupPath = Application.StartupPath;
            startupPath = startupPath.Substring(0, startupPath.LastIndexOf("\\"));
            string imagePath = startupPath + "\\" + "image";
            string imageName = "test.jpg";
            string image = imagePath + "\\" + imageName;
            bitmap = new Bitmap(image);//通过bitMap类引入图像对象
            pictureBox1.Image = bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);//创建可以容纳bitmap的矩形
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);//讲Bitmap的值锁存到bitmapdata中
            int stride = bitmapdata.Stride; //stride 指图像一行的字节数
            int bytes = bitmap.Width * bitmap.Height * 3;//伪彩色有三个色道
            byte[] rgbvalue = new byte[bytes];
            byte[] grayvalue = new byte[bytes];
            IntPtr intPtr = bitmapdata.Scan0;//返回bitmapdata的首地址
            Array.Clear(rgbvalue, 0, rgbvalue.Length);//清零
            Marshal.Copy(intPtr, grayvalue, 0, bytes);//将intPtr的值赋给grayvalue
            byte tempB;
            byte seg = 100;//强度分层中的分层数
            //强度分层法
            for(int i = 0; i < bytes; i += 3)
            {
                byte ser = (byte)(256 / seg); //根据强度的分层数确定步长
                tempB = (byte)(grayvalue[i] * ser);//将灰度根据步长进行映射
                rgbvalue[i] = (byte)((seg - 1 - tempB) * ser);//填充三个色道
                rgbvalue[i + 1] = (byte)(tempB * ser);//
                rgbvalue[i + 2] = 0;//
            }
            rgbmap = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format24bppRgb);
            BitmapData rgbmapdata = rgbmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr rgbintptr = rgbmapdata.Scan0;
            Marshal.Copy(rgbvalue, 0, rgbintptr, bytes);
            bitmap.UnlockBits(bitmapdata);
            rgbmap.UnlockBits(rgbmapdata);
            pictureBox1.Image = rgbmap;
        }
    }
}
