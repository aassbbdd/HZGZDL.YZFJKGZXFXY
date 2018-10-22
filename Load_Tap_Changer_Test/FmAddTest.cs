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
using DbHelper.Db_Model;
using DevExpress.Utils;

namespace Basic_Controls
{
    public partial class FmAddTest : DevExpress.XtraEditors.XtraForm
    {
        public string id;
        public FmAddTest()
        {

            InitializeComponent();
           
        }
        public FmAddTest(Test_Plan model)
        {
            InitializeComponent();
            this.oldmodel = model;

        }
        Test_Plan oldmodel;
        private void FmAddTest_Load(object sender, EventArgs e)
        {
            txtTEST_TIME.Properties.VistaEditTime = DefaultBoolean.True;
            if (oldmodel != null && !string.IsNullOrEmpty(oldmodel.DVNAME))
            {
                Page_Data_Bind();
                txtTEST_TIME.Enabled = false;
            }
            else
            {
                Init();
            }
        }

        #region 执行方法

        private void Init()
        {
            oldmodel = new Test_Plan();
            oldmodel.ISEDIT = "1";
            txtCONTACT_NUM.Text = "5";
            this.txtTEST_TIME.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            rdoTEST_TYPE.SelectedIndex = 1;

            rdoGETINFO.SelectedIndex = 0;
            this.txtSA.Text = "0.8";
            this.txtEA.Text = "0.2";

            this.txtGetUnit.Text = "10";
            //判断采样信息是那种状态
            Get_Info_Type();


            this.ckV1.Checked = true;
            this.ckC1.Checked = true;
        }

        /// <summary>
        /// 判断采样信息
        /// </summary>
        private void Get_Info_Type()
        {
            string GetType = this.rdoGETINFO.Text;
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

        /// <summary>
        /// 初始化绑定页面数据
        /// </summary>
        private void Page_Data_Bind()
        {
            string dvname = oldmodel.DVNAME;
            //if (oldmodel.ISEDIT == "1")
            //{
            //    dvname += "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //}
            this.txtDVNAME.Text = dvname;
            this.txtDVPOSITION.Text = oldmodel.DVPOSITION;
            this.txtDVID.Text = oldmodel.DVID;
            this.txtTESTER.Text = oldmodel.TESTER;

            this.txtOLTC_TS.Text = oldmodel.OLTC_TS;
            this.txtCONTACT_NUM.Text = oldmodel.CONTACT_NUM;
            this.txtTEST_NUM.Text = oldmodel.TEST_NUM;
            this.cmbSPLACE.Text = oldmodel.SPLACE;
            this.txtOILTEMP.Text = oldmodel.OILTEMP;

            //判断采样信息是那种状态
            Get_Info_Type();

            //this.txtTEST_TIME.Text = oldmodel.TEST_TIME;
            this.txtTEST_TIME.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (oldmodel.TEST_TYPE == "1")
            {
                rdoTEST_TYPE.SelectedIndex = 1;
            }
            else if (oldmodel.TEST_TYPE == "2")
            {
                rdoTEST_TYPE.SelectedIndex = 2;
            }
            else
            {
                rdoTEST_TYPE.SelectedIndex = 3;
            }

            if (oldmodel.GETINFO == "1")
            {
                rdoGETINFO.SelectedIndex = 0;
                this.txtSA.Text = oldmodel.SCURRENT;
                this.txtEA.Text = oldmodel.ECURRENT;

            }
            else
            {
                rdoGETINFO.SelectedIndex = 1;
                this.txtGetUnit.Text = oldmodel.TIME_UNIT;
            }
            string[] tests = oldmodel.TESTSTAGE.Split(',');
            foreach (string test in tests)
            {
                if (test == "1")
                {
                    this.cktest1.Checked = true;
                }
                if (test == "2")
                {
                    this.cktest2.Checked = true;
                }
                if (test == "3")
                {
                    this.cktest3.Checked = true;
                }
                if (test == "4")
                {
                    this.cktest4.Checked = true;
                }
            }
            string[] cks = oldmodel.DJUST.Split(',');
            foreach (string ck in cks)
            {
                if (ck == "1")
                {
                    this.ckzy.Checked = true;
                }
                if (ck == "2")
                {
                    this.cktj.Checked = true;
                }
                if (ck == "3")
                {
                    this.ckwh.Checked = true;
                }
            }

            this.txtDescribe.Text = oldmodel.DESCRIBE;

            if (oldmodel.V1 == "1")
            {
                this.ckV1.Checked = true;
            }
            if (oldmodel.V2 == "1")
            {
                this.ckV2.Checked = true;
            }
            if (oldmodel.V3 == "1")
            {
                this.ckV3.Checked = true;
            }
            if (oldmodel.C1 == "1")
            {
                this.ckC1.Checked = true;
            }
            if (oldmodel.C2 == "1")
            {
                this.ckC2.Checked = true;
            }
            if (oldmodel.C3 == "1")
            {
                this.ckC3.Checked = true;
            }
        }

        #region  保存数据库

        /// <summary>
        /// 保存数据
        /// </summary>
        private void Save()
        {
            try
            {
                Test_Plan model = new Test_Plan();
                model.DVNAME = this.txtDVNAME.Text;
                model.DVPOSITION = this.txtDVPOSITION.Text;
                model.DVID = this.txtDVID.Text;
                model.TESTER = this.txtTESTER.Text;

                model.OLTC_TS = this.txtOLTC_TS.Text;
                model.CONTACT_NUM = this.txtCONTACT_NUM.Text;
                model.TEST_NUM = this.txtTEST_NUM.Text;
                model.SPLACE = this.cmbSPLACE.Text;
                model.OILTEMP = this.txtOILTEMP.Text;

                model.TEST_TIME = this.txtTEST_TIME.Text;
                model.TEST_TYPE = this.rdoTEST_TYPE.Text;

                #region 测试阶段赋值
                if (this.cktest1.Checked)
                {
                    model.TESTSTAGE += "1";
                }
                if (this.cktest2.Checked)
                {
                    model.TESTSTAGE += ",2";
                }
                if (this.cktest3.Checked)
                {
                    model.TESTSTAGE += ",3";
                }
                if (this.cktest4.Checked)
                {
                    model.TESTSTAGE += ",4";
                }
                #endregion
                #region 测试阶段赋值
                if (this.ckzy.Checked)
                {
                    model.DJUST += "1";
                }

                if (this.cktj.Checked)
                {
                    model.DJUST += ",2";
                }

                if (this.ckwh.Checked)
                {
                    model.DJUST += ",3";
                }

                #endregion

                model.DESCRIBE = this.txtDescribe.Text;

                model.GETINFO = this.rdoGETINFO.Text;
                model.SCURRENT = model.GETINFO == "1" ? this.txtSA.Text : "";
                model.ECURRENT = model.GETINFO == "1" ? this.txtEA.Text : "";
                model.TIME_UNIT = model.GETINFO == "2" ? this.txtGetUnit.Text : "";
                model.PARENTID = string.IsNullOrEmpty(oldmodel.PARENTID) ? "0" : oldmodel.PARENTID;

                model.V1 = ckV1.Checked ? "1" : "0";
                model.V2 = ckV2.Checked ? "1" : "0";
                model.V3 = ckV3.Checked ? "1" : "0";

                model.C1 = ckC1.Checked ? "1" : "0";
                model.C2 = ckC2.Checked ? "1" : "0";
                model.C3 = ckC3.Checked ? "1" : "0";

                int count = 0;

                if (oldmodel.ISEDIT == "1")
                {
                    count = Db_Action.Instance.Test_Confige_Insert(model);
                }
                else
                {
                    model.ID = oldmodel.ID;
                    count = Db_Action.Instance.Test_Confige_Edit(model);
                }
                if (count >= 1)
                {
                    id = count.ToString();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #endregion

        #endregion

        #region 按键事件
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(txtGetUnit.Text, out n))
            {
                if (Convert.ToInt32(txtGetUnit.Text) > 20|| Convert.ToInt32(txtGetUnit.Text)<5)
                {
                    MessageBox.Show("请输入2-20 范围的时间");
                    this.txtGetUnit.Focus();
                    txtGetUnit.Text = "";
                    return;

                }
            }
            else
            {
                MessageBox.Show("请输入正确的时间");
                this.txtGetUnit.Focus();
                txtGetUnit.Text = "";
                return;
            }


            Save();
            this.DialogResult = DialogResult.OK;
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

        /// <summary>
        /// 判断采样信息是那种状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoGetInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //判断采样信息是那种状态
            Get_Info_Type();
        }

        /// <summary>
        /// 输入文字自动循环
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCONTACT_NUM_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtCONTACT_NUM.Text))
            {
                int CONTACT_NUM = Convert.ToInt32(this.txtCONTACT_NUM.Text);
                string heardnum = "";
                for (int i = 1; i <= CONTACT_NUM; i++)
                {
                    heardnum += i.ToString() + ",";
                    cmbSPLACE.Properties.Items.Add(i);
                }
                this.lbHearder.Text = heardnum;
            }
            else
            {
                this.lbHearder.Text = "";
            }
        }
        #endregion


    }
}