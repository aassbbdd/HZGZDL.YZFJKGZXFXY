using DbHelper;
using DbHelper.Sqlite_Db;
using DocDecrypt.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basic_Controls
{
    public partial class BaseFrom : DevExpress.XtraEditors.XtraForm
    {
        public BaseFrom()
        {
            InitializeComponent();
        }

        string path = "";


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            CreateDb();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            CreateDb();
            string tablename = "test21222";
            bool IsTableExist = SQLiteHelper.Instance.IsTableExist(tablename);
            if (IsTableExist)
            {
                string DbStr = Create_Table.Instance.Create_AA(tablename);
                SQLiteHelper.NewTable(DbStr);
            }
        }

        private void CreateDb()
        {
            try
            {
                SQLiteHelper.NewDbFile(path);//生成SQL文件
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
