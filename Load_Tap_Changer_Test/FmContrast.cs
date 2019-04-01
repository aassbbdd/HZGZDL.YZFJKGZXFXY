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
        /// <summary>
        /// 是否显示包络线
        /// </summary>
        public bool IsBlx;
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
            List<TreeListNode> listNode = treeList2.GetNodeList();
            TreeListNode node = treeList1.FocusedNode;
            for (int i = 0; i <= listNode.Count; i++)
            {
                if (listNode.Count > 2
                && node.GetValue("ID").ToString() != listNode[i].GetValue("ID").ToString()
                && listNode[i].GetValue("PARENTID").ToString() != "0")
                {
                    treeList2.SetFocusedNode(listNode[i]);
                    break;
                }
            }
            treeList2.Refresh();
        }
        /// <summary>
        /// 绑定振动
        /// </summary>
        private void Bind_V()
        {
            V = rdoV.Text;
            lbVibrate.Text = "振动 " + V;
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
            isfirst = true;
        }
        bool isfirst = false;
        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node.Selected)
            {
                //bool v = e.Node["PARENTID"].ToString() != "0";
                //if (v)
                //{
                node1 = e.Node;

                if (node2 != null
                    && e.Node["ID"].ToString() == node2["ID"].ToString()
                    && isfirst)
                {
                    treeList1.SetFocusedNode(e.OldNode);
                    if (treeList1.Appearance.FocusedCell.BackColor != Color.Red)
                    {
                        treeList1.Appearance.FocusedCell.BackColor = Color.Red;
                    };

                    MessageBox.Show("样本数据不能和对比数据一样!");
                    return;
                }
                else if (e.Node.GetValue("PARENTID").ToString() == "0")
                {
                    treeList1.SetFocusedNode(e.OldNode);
                    if (treeList1.Appearance.FocusedCell.BackColor != Color.Red)
                    {
                        treeList1.Appearance.FocusedCell.BackColor = Color.Red;
                    };
                }
                else
                {
                    if (treeList1.Appearance.FocusedCell.BackColor != Color.Red)
                    {
                        treeList1.Appearance.FocusedCell.BackColor = Color.Red;
                    };

                    lbContrast.Text = node1["DVNAME"].ToString();

                    rdoC.Properties.Items.Clear();
                    rdoV.Properties.Items.Clear();

                    if (node1["C1"].ToString() == "1")
                    {
                        rdoC.Properties.Items.Add(new RadioGroupItem("1", "电流1"));
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
                        rdoV.Properties.Items.Add(new RadioGroupItem("1", "振动1"));
                    }
                    if (node1["V2"].ToString() == "1")
                    {
                        rdoV.Properties.Items.Add(new RadioGroupItem("2", "振动2"));
                    }

                    if (node1["V3"].ToString() == "1")
                    {
                        rdoV.Properties.Items.Add(new RadioGroupItem("3", "振动3"));
                    }
                }
            }
            //}
        }
        private void treeList2_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (e.Node.Selected)
            {
                //bool v = e.Node["PARENTID"].ToString() != "0";
                //if (v)
                //{
                node2 = e.Node;
                if (node1 != null
                    && e.Node["ID"].ToString() == node1["ID"].ToString()
                    && isfirst)
                {
                    treeList2.SetFocusedNode(e.OldNode);
                    if (treeList2.Appearance.FocusedCell.BackColor != Color.SteelBlue)
                    {
                        treeList2.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                    };
                    MessageBox.Show("对比数据不能和样本数据一样!");
                    return;
                }
                else if (e.Node.GetValue("PARENTID").ToString() == "0")
                {
                    treeList2.SetFocusedNode(e.OldNode);
                    if (treeList2.Appearance.FocusedCell.BackColor != Color.SteelBlue)
                    {
                        treeList2.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                    };
                }
                else
                {
                    if (treeList2.Appearance.FocusedCell.BackColor != Color.SteelBlue)
                    {
                        treeList2.Appearance.FocusedCell.BackColor = Color.SteelBlue;
                    };

                    lbSample.Text = node2["DVNAME"].ToString();
                }

                //}

            }
        }
        /// <summary>
        /// 确定对比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            IsBlx = this.ckblx.Checked;
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
        /// 选择振动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdov_Properties_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind_V();
        }

    }
}