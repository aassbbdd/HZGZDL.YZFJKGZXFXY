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
using DbHelper;
using DbHelper.Sqlite_Db;

namespace Basic_Controls
{
    public partial class FmTestSelect : DevExpress.XtraEditors.XtraForm
    {
        public FmTestSelect()
        {
            InitializeComponent();
        }
        public int index = 3;
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            index = Convert.ToInt32(radioGroup.Text);

            this.Close();
        }
    }
}