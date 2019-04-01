using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win_Controls
{
    public partial class Bitmap_Control : UserControl
    {
        public Bitmap_Control()
        {
            InitializeComponent();
        }

        #region 自定义方法

        /// <summary>
        /// 初始化
        /// </summary>
        public void InIt()
        {
            //pic_img.Height = picMap .Height-200;
            //pic_img.Width = picMap.Width;
            pic_img.BackgroundImage = _Bitmap;
            pic_img.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_img.BackgroundImageLayout = ImageLayout.Stretch;

            lbTitle.Text = _Title;
        }
        #endregion

        #region 自定义属性方法

        /// <summary>
        /// 标题
        /// </summary>
        private string _Title = "";
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }

        /// <summary>
        /// 传入图片
        /// </summary>
        private Bitmap _Bitmap = new Bitmap(1000, 400);

        public Bitmap Bitmap
        {
            get
            {
                return this._Bitmap;
            }
            set
            {
                this._Bitmap = value;
            }
        }


        /// <summary>
        /// 高度
        /// </summary>
        private int _Height = 0;

        public int Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                this._Height = value;
            }
        }

        /// <summary>
        /// 宽度度
        /// </summary>
        private int _Width = 0;

        public int Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                this._Width = value;
            }
        }


        #endregion

    }
}
