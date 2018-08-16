using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Basic_Controls
{
    public partial class XForm : DevExpress.XtraEditors.XtraForm
    {
        public XForm()
        {
            InitializeComponent();
        }

        #region 按键
        /// <summary>
        /// 新建测试计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTest_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int index = 0;
            using (FmTestSelect form = new FmTestSelect())
            {
                form.ShowDialog();
                index = form.index;
            }


            if (index == 1)// 加载一个存在的测试计划
            {


            }
            else if (index == 2)//加载分析测试数据
            {


            }
            else if (index == 3)///新建测试计划
            {
                using (FmAddTest form = new FmAddTest())
                {
                    form.ShowDialog();
               
                }

            }
            else//使用默认测试计划
            {

            }


        }





        #endregion
    }
}