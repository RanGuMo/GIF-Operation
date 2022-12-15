using Gif.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIF_Operation
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载GIF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(textBox1.Text);
            lb_img.Image = img;
        }
        /// <summary>
        /// 分割GIF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //设置GIF图片保存路径
            string savePath = Application.StartupPath + "\\images\\";
            if (Directory.Exists(savePath))//存在该路径则删除
            {
                Directory.Delete(savePath, true);
            }
            //重新创建一个GIf图片的保存路径
            Directory.CreateDirectory(savePath);

            Image img = lb_img.Image;
            //FrameDimension frameDim = new FrameDimension(img.FrameDimensionsList[0]);
            // 得到帧数
            int frame = img.GetFrameCount(FrameDimension.Time);
            //遍历帧数 当小于0时表示遍历完毕
            while (frame-- > 0)
            {
                //SelectActiveFrame 方法用于选择多帧图像中的一帧，并将其设为活动帧。
                //使用这个方法选择某一帧，然后进行图像处理操作，比如添加滤镜、编辑像素等。
                img.SelectActiveFrame(FrameDimension.Time, frame);
                //保存图片
                //ImageFormat.Jpeg 指定了图像的格式为 JPEG，并将图像保存为 "XXX.jpg" 文件
                img.Save(savePath + "frame_" + frame + ".jpg", ImageFormat.Jpeg);
            }
            if (MessageBox.Show("GIF分割完成,是否打开文件夹?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var psi = new System.Diagnostics.ProcessStartInfo() { FileName = savePath, UseShellExecute = true };
                System.Diagnostics.Process.Start(psi);
                //Process.Start(savePath);  //直接使用Process.Start()方法打开C盘文件夹,访问权限不足
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = Application.StartupPath + "\\GIF\\tom.gif";
            textBox2.Text = Application.StartupPath + "\\images\\";

        }

        string newPath = Application.StartupPath + "\\new.gif";


        /// <summary>
        /// 合并GIF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            List<string> imgs = Directory.GetFiles(textBox2.Text, "*.jpg").ToList();
            imgs.Sort((a, b) =>
            {
                string reg = "[0-9]+";
                int a_index = int.Parse(Regex.Match(a, reg).Value);
                int b_index = int.Parse(Regex.Match(b, reg).Value);
                return a_index > b_index ? 1 : a_index < b_index ? -1 : 0;
            });
            AnimatedGifEncoder ae = new AnimatedGifEncoder();
            ae.Start(newPath);
            ae.SetDelay(120);   // 延迟间隔
            ae.SetRepeat(0);  //-1:不循环,0:循环播放
            for (int i = 0; i < imgs.Count; i++)
            {
                ae.AddFrame(Image.FromFile(imgs[i]));
            }
            ae.Finish();
            MessageBox.Show("处理完成,请点击预览查看");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Image img = Image.FromFile(newPath);
            lb_img.Image = img;
        }
    }
}
