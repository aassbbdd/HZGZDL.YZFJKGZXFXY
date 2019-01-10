using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

using Basic_Controls;

namespace Load_Tap_Changer_Test.Staffgauge
{
    public partial class Staffgauge : UserControl
    {
        public Staffgauge()
        {
            InitializeComponent();


        }
        #region 基础配置
        private Color _borderColor;
        private int _borderWidth = 1;
        private DashStyle _borderStyle = DashStyle.Solid;
        private int _opacity = 125;


        #region Property
        [Category("Custom"), Description("Border Color")]
        public Color BorderColor
        {
            set { _borderColor = value; }
            get { return _borderColor; }
        }

        [Category("Custom"), Description("Border Width"), DefaultValue(1)]
        public int BorderWidth
        {
            set
            {
                if (value < 0) value = 0;
                _borderWidth = value;
            }
            get { return _borderWidth; }
        }

        [Category("Custom"), Description("Border Style"), DefaultValue(DashStyle.Solid)]
        public DashStyle BorderStyle
        {
            set { this._borderStyle = value; this.Invalidate(); }
            get { return this._borderStyle; }
        }

        //[Bindable(true), Category("Custom"), DefaultValue(125), Description("背景的透明度. 有效值0-255")]
        //public int Opacity
        //{
        //    get { return _opacity; }
        //    set
        //    {
        //        if (value > 255) value = 255;
        //        else if (value < 0) value = 0;
        //        _opacity = value;
        //        this.Invalidate();
        //    }
        //}
        #endregion

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //do not allow the background to be painted
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (this._opacity > 0)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(this._opacity, this.BackColor)),
                                         this.ClientRectangle);
            }

            if (this._borderWidth > 0)
            {

                Pen pen = new Pen(this._borderColor, this._borderWidth);
                pen.DashStyle = this._borderStyle;

                //  e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, this.Width - 1, this.Height - 1);

                e.Graphics.DrawRectangle(pen, e.ClipRectangle.Left, e.ClipRectangle.Top, this.Width - 1, this.Height - 1);




                pen.Dispose();
            }
        }
        #endregion
        #region 自定义属性方法

        #region 属性
        /// <summary>
        ///外部传出X
        /// </summary>
        private int _X = 0;
        public int XValue
        {
            get
            {
                return this._X;
            }
            set
            {
                this._X = value;
                X.Text = _X.ToString();
                // base.Refresh();
            }
        }

        /// <summary>
        ///外部传入Y
        /// </summary>
        private int _Y = 0;
        public int YValue
        {
            get
            {
                return this._Y;
            }
            set
            {
                this._Y = value;
                Y.Text = _Y.ToString();
                // base.Refresh();
            }
        }
        /// <summary>
        /// 传入X
        /// </summary>
        private string _XX;
        public string XXValue
        {
            get
            {
                return this._XX;
            }
            set
            {
                this._XX = value;
                XX.Text = _XX;
                // base.Refresh();
            }
        }
        /// <summary>
        /// 传入Y
        /// </summary>
        private string _YY;
        public string YYValue
        {
            get
            {
                return this._YY;
            }
            set
            {
                this._YY = value;
                YY.Text = _YY;
                // base.Refresh();
            }
        }



        /// <summary>
        /// 是否选中
        /// </summary>
        private bool _IsPc = false;
        public bool IsPcValue
        {
            get
            {
                return this._IsPc;
            }
            set
            {
                this._IsPc = value;
            }
        }

        #endregion

        #region 鼠标操作事件
        /// <summary>
        /// 松开鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staffgauge_MouseUp(object sender, MouseEventArgs e)
        {
            pcAfter.Location = new Point(e.X, pcAfter.Location.Y);
            pcFront.Location = new Point(e.X - 50, pcAfter.Location.Y);
            // pcFront.Refresh();
        }

        /// <summary>
        /// 按下鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staffgauge_MouseDown(object sender, MouseEventArgs e)
        {
            // sendpic();



        }

        /// <summary>
        /// 移动鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Staffgauge_MouseMove(object sender, MouseEventArgs e)
        {
            this._opacity = 0;
            //this._borderWidth = 0;

            //if (pcAfter.Location.X <= e.X && pcAfter.Location.X + pcAfter.Width >= e.X)
            //{
            //    pcAfter.Location = new Point(e.X, pcAfter.Location.Y);
            //    pcFront.Location = new Point(e.X - 50, pcAfter.Location.Y);

            //}

            //YValue = e.Y;
            //XValue = e.X;

            //pcFront.Location = new Point(e.X, pcFront.Location.Y);
            //this.DoubleBuffered = false;
            // label3.Location = new Point(e.X, pcFront.Location.Y);
            //this.pcFront.Refresh();
            // this.Refresh();




        }
        #endregion

        #region 私有方法
        PictureBox pictureBox_Loading;


        private void Staffgauge_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered = true;
            //pictureBox_Loading = new PictureBox();

            //pictureBox_Loading.Size = new System.Drawing.Size(11, 2248);

            //pictureBox_Loading.Image = bitmap;

            //this.Controls.Add(pictureBox_Loading);
            pcAfter.Enabled = false;
            pcAfter.Visible = false;

            pcFront.Enabled = false;
            pcFront.Visible = false;



        }


        /// 选择图片
        /// </summary>
        /// <param name="colerIndex"></param>
        /// <returns></returns>
        private Bitmap GetImage(int colerIndex, int initHeight)
        {

            Random rd = new Random();
            int num = rd.Next(1, 7);//1-7随机数
            int initWidth = 256;
            // int initHeight = 256;

            Bitmap image = new Bitmap(initWidth, initHeight);//初始化大小
            Graphics g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;//设置图片质量
            switch (colerIndex)//设置图片不同背景色
            {
                case 1:
                    g.Clear(Color.FromArgb(255, 139, 139));
                    break;
                case 2:
                    g.Clear(Color.FromArgb(97, 191, 173));
                    break;
                case 3:
                    g.Clear(Color.FromArgb(22, 124, 127));
                    break;
                case 4:
                    g.Clear(Color.FromArgb(50, 182, 122));
                    break;
                case 5:
                    g.Clear(Color.FromArgb(191, 181, 215));
                    break;
                case 6:
                    g.Clear(Color.FromArgb(240, 207, 97));
                    break;
                case 7:
                    g.Clear(Color.FromArgb(5, 90, 91));
                    break;
                default:
                    g.Clear(Color.FromArgb(5, 90, 91));
                    break;
            }

            // Font f = new Font("Arial ", 88);//, System.Drawing.FontStyle.Bold);//设置字体样式，大小
            Brush b = new SolidBrush(Color.White);
            Brush r = new SolidBrush(Color.FromArgb(166, 8, 8));

            return image;
        }
        #endregion

        #region 公共外部调用方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="MarginLeft">距离左边边框</param>
        /// <param name="MarginTop">距离顶部边框</param>
        /// <param name="MarginRight">距离右边</param>
        /// <param name="MarginBottom">距离底部</param>
        public void Init(double MarginLeft
                        , double MarginTop
                        , double MarginRight
                        , double MarginBottom)
        {



            int Width = this.Width;
            int Height = this.Height;

            var ddd = this.Location;
            pcFront.Image = GetImage(4, Height);

            pcFront.Location = new Point(200, ((int)MarginTop * 10) - (int)MarginTop);

            pcAfter.Image = GetImage(6, Height);

        }




        #endregion
    }

    #endregion

}




