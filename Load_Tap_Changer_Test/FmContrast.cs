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
using DbHelper.Db_Model;
using DbHelper.Sqlite_Db;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DevExpress.XtraEditors.Controls;

namespace Load_Tap_Changer_Test
{
    public partial class FmContrast : DevExpress.XtraEditors.XtraForm
    {
        public FmContrast()
        {
            InitializeComponent();
        }
        #region 基础参数
        List<Test_Plan> list;
        List<Test_Plan> list1;

        public TreeListNode node1;
        public TreeListNode node2;
        public string C;
        public string V;
        #endregion
        #region 绑定测试计划
        /// <summary>
        /// 绑定测试计划 刷新左边树
        /// </summary>
        public void Tester_List_Bind()
        {
            list = Db_Select.Instance.All_Test_Cofig_Get();
            list1 = Db_Select.Instance.All_Test_Cofig_Get();

            treeList1.DataSource = list;
            treeList1.Refresh();

            treeList2.DataSource = list1;
            treeList2.Refresh();
        }
        /// <summary>
        /// 绑定震动
        /// </summary>
        private void Bind_V()
        {
            V = rdoV.Text;
            lbVibrate.Text = "震动 " + V;
        }
        /// <summary>
        /// 绑定电流
        /// </summary>
        private void Bind_C()
        {
            C = rdoC.Text;
            lbCurrent.Text = "电流 " + C;
        }
        #endregion

        private void FmContrast_Load(object sender, EventArgs e)
        {
            Tester_List_Bind();
            Bind_V();
            Bind_C();
        }


        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node.Selected)
            {
                bool v = e.Node["PARENTID"].ToString() != "0";
                if (v)
                {
                    if (node2 != null && e.Node["ID"].ToString() == node2["ID"].ToString())
                    {
                        MessageBox.Show("样本数据不能和对比数据一样!");
                        return;
                    }
                    else
                    {
                        if (treeList1.Appearance.FocusedCell.BackColor != Color.Red)
                        {
                            treeList1.Appearance.FocusedCell.BackColor = Color.Red;
                        };
                        node1 = e.Node;
                        lbContrast.Text = node1["DVNAME"].ToString();

                        rdoC.Properties.Items.Clear();
                        rdoV.Properties.Items.Clear();

                        if (node1["C1"].ToString() == "1")
                        {
                            rdoC.Properties.Items.Add(new RadioGroupItem ("1", "电流1"));
                        }
                        if (node1["C2"].ToString() == "1")
                        {
                            rdoC.Properties.Items.Add(new RadioGroupItem("2", "电流2"));
                        }

                        if (node1["C3"].ToString() == "1")
                        {
                            rdoC.Properties.Items.Add(new RadioGroupItem("3", "电流3"));
                        }

                        if (node1["V1"].ToString() == "1")
                        {
                            rdoV.Properties.Items.Add(new RadioGroupItem("1", "震动1"));
                        }
                        if (node1["V2"].ToString() == "1")
                        {
                            rdoV.Properties.Items.Add(new RadioGroupItem("2", "震动2"));
                        }

                        if (node1["V3"].ToString() == "1")
                        {
                            rdoV.Properties.Items.Add(new RadioGroupItem("3", "震动3"));
                        }
                    }
                }
            }
        }
        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node.Selected)
            {
                bool v = e.Node["PARENTID"].ToString() != "0";
                if (v)
                {
                    if (node1 != null && e.Node["ID"].ToString() == node1["ID"].ToString())
                    {
                        MessageBox.Show("对比数据不能和样本数据一样!");
                        return;
                    }
                    else
                    {

                        if (treeList2.Appearance.FocusedCell.BackColor != Color.SteelBlue)
                        {
                            treeList2.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                        };
                        node2 = e.Node;
                        lbSample.Text = node2["DVNAME"].ToString();
                    }
                }
            }
        }
        /// <summary>
        /// 确定对比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// 取消页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 选择电流
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoC_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_C();
        }
        /// <summary>
        /// 选择震动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdov_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_V();
        }

    }
}