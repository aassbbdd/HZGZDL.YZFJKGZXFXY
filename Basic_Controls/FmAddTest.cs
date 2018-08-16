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
    public partial class FmAddTest : DevExpress.XtraEditors.XtraForm
    {
        public FmAddTest()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdoGetInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string GetType = this.rdoGetInfo.Text;
            if (GetType == "1")
            {
                this.txtGetUnit.Enabled = false;

                this.txtSA.Enabled = true;
                this.txtEA.Enabled = true;
            }
            else
            {
                this.txtGetUnit.Enabled = true;
                this.txtSA.Enabled = false;
                this.txtEA.Enabled = false;
            }


        }
    }
}