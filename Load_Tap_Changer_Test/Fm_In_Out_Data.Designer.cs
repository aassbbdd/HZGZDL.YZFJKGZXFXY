namespace Load_Tap_Changer_Test
{
    partial class Fm_In_Out_Data
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fm_In_Out_Data));
            this.pc2 = new DevExpress.XtraEditors.PanelControl();
            this.treeList = new DevExpress.XtraTreeList.TreeList();
            this.ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.删除 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnOutData = new DevExpress.XtraEditors.SimpleButton();
            this.rdo_In_Out_Select = new DevExpress.XtraEditors.RadioGroup();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnInData = new DevExpress.XtraEditors.SimpleButton();
            this.splashScreenManager = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::Load_Tap_Changer_Test.WaitForm), true, true);
            ((System.ComponentModel.ISupportInitialize)(this.pc2)).BeginInit();
            this.pc2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.删除)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdo_In_Out_Select.Properties)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pc2
            // 
            this.pc2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pc2.Controls.Add(this.treeList);
            this.pc2.Location = new System.Drawing.Point(0, 1);
            this.pc2.Name = "pc2";
            this.pc2.Size = new System.Drawing.Size(371, 412);
            this.pc2.TabIndex = 59;
            // 
            // treeList
            // 
            this.treeList.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.treeList.Appearance.Empty.Options.UseBackColor = true;
            this.treeList.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.treeList.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.White;
            this.treeList.Appearance.HeaderPanelBackground.Options.UseBackColor = true;
            this.treeList.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.treeList.Appearance.Row.Options.UseBackColor = true;
            this.treeList.AppearancePrint.BandPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList.AppearancePrint.BandPanel.Options.UseBackColor = true;
            this.treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.ID});
            this.treeList.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeList.CustomizationFormBounds = new System.Drawing.Rectangle(1309, 565, 220, 265);
            this.treeList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeList.Location = new System.Drawing.Point(2, 2);
            this.treeList.MaximumSize = new System.Drawing.Size(367, 408);
            this.treeList.MinimumSize = new System.Drawing.Size(367, 408);
            this.treeList.Name = "treeList";
            this.treeList.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList.OptionsMenu.EnableColumnMenu = false;
            this.treeList.OptionsMenu.ShowConditionalFormattingItem = true;
            this.treeList.OptionsView.ShowIndicator = false;
            this.treeList.ParentFieldName = "PARENTID";
            this.treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.删除,
            this.repositoryItemButtonEdit1});
            this.treeList.Size = new System.Drawing.Size(367, 408);
            this.treeList.TabIndex = 58;
            this.treeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged);
            // 
            // ID
            // 
            this.ID.Caption = "设备名字";
            this.ID.FieldName = "DVNAME";
            this.ID.MinWidth = 34;
            this.ID.Name = "ID";
            this.ID.OptionsColumn.AllowEdit = false;
            this.ID.OptionsColumn.AllowMove = false;
            this.ID.OptionsColumn.AllowSort = false;
            this.ID.OptionsColumn.ReadOnly = true;
            this.ID.Visible = true;
            this.ID.VisibleIndex = 0;
            // 
            // 删除
            // 
            this.删除.AutoHeight = false;
            this.删除.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.删除.Name = "删除";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.labelControl1);
            this.groupBox1.Controls.Add(this.btnOutData);
            this.groupBox1.Controls.Add(this.rdo_In_Out_Select);
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point(372, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 261);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导出";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(98, 168);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(190, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "选择-导出所有-单击【导出】";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(98, 70);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(300, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "选择-导出选中-选中第二个文件-单击【导出】";
            // 
            // btnOutData
            // 
            this.btnOutData.Location = new System.Drawing.Point(373, 206);
            this.btnOutData.Name = "btnOutData";
            this.btnOutData.Size = new System.Drawing.Size(103, 28);
            this.btnOutData.TabIndex = 1;
            this.btnOutData.Text = "导出";
            this.btnOutData.Click += new System.EventHandler(this.btnOutData_Click);
            // 
            // rdo_In_Out_Select
            // 
            this.rdo_In_Out_Select.EditValue = 0;
            this.rdo_In_Out_Select.Location = new System.Drawing.Point(6, 25);
            this.rdo_In_Out_Select.Name = "rdo_In_Out_Select";
            this.rdo_In_Out_Select.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(0, "导出选中"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(1, "导出所有")});
            this.rdo_In_Out_Select.Size = new System.Drawing.Size(89, 209);
            this.rdo_In_Out_Select.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.labelControl3);
            this.groupBox2.Controls.Add(this.btnInData);
            this.groupBox2.Location = new System.Drawing.Point(372, 268);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(489, 145);
            this.groupBox2.TabIndex = 61;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "导入";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(3, 67);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(298, 18);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "单击【导入】-选择【Xml_Data】文件夹-确定";
            // 
            // btnInData
            // 
            this.btnInData.Location = new System.Drawing.Point(379, 62);
            this.btnInData.Name = "btnInData";
            this.btnInData.Size = new System.Drawing.Size(103, 28);
            this.btnInData.TabIndex = 2;
            this.btnInData.Text = "导入";
            this.btnInData.Click += new System.EventHandler(this.btnInData_Click);
            // 
            // splashScreenManager
            // 
            this.splashScreenManager.ClosingDelay = 500;
            // 
            // Fm_In_Out_Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 416);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pc2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fm_In_Out_Data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "导入导出数据";
            ((System.ComponentModel.ISupportInitialize)(this.pc2)).EndInit();
            this.pc2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.删除)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdo_In_Out_Select.Properties)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl pc2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraTreeList.TreeList treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit 删除;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraEditors.RadioGroup rdo_In_Out_Select;
        private DevExpress.XtraEditors.SimpleButton btnOutData;
        private DevExpress.XtraEditors.SimpleButton btnInData;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager;
    }
}