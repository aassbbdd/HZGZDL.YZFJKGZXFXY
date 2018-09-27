namespace Load_Tap_Changer_Test
{
    partial class FmContrast
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
            this.rdoV = new DevExpress.XtraEditors.RadioGroup();
            this.rdoC = new DevExpress.XtraEditors.RadioGroup();
            this.ID = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.删除 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemButtonEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemButtonEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repositoryItemButtonEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.lbVibrate = new System.Windows.Forms.Label();
            this.lbCurrent = new System.Windows.Forms.Label();
            this.lbContrast = new System.Windows.Forms.Label();
            this.lbSample = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.rdoV.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.删除)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit7)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoV
            // 
            this.rdoV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoV.EditValue = "1";
            this.rdoV.Location = new System.Drawing.Point(424, 5);
            this.rdoV.Name = "rdoV";
            this.rdoV.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.rdoV.Properties.Appearance.Options.UseBackColor = true;
            this.rdoV.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "震动1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "震动2"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "震动3")});
            this.rdoV.Properties.SelectedIndexChanged += new System.EventHandler(this.rdov_Properties_SelectedIndexChanged);
            this.rdoV.Size = new System.Drawing.Size(77, 76);
            this.rdoV.TabIndex = 19;
            // 
            // rdoC
            // 
            this.rdoC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoC.EditValue = "1";
            this.rdoC.Location = new System.Drawing.Point(341, 5);
            this.rdoC.Name = "rdoC";
            this.rdoC.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.rdoC.Properties.Appearance.Options.UseBackColor = true;
            this.rdoC.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "电流1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "电流2"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "电流3")});
            this.rdoC.Size = new System.Drawing.Size(77, 76);
            this.rdoC.TabIndex = 24;
            this.rdoC.SelectedIndexChanged += new System.EventHandler(this.rdoC_SelectedIndexChanged);
            // 
            // ID
            // 
            this.ID.Caption = "设备名字(对比数据)";
            this.ID.FieldName = "DVNAME";
            this.ID.MinWidth = 34;
            this.ID.Name = "ID";
            this.ID.OptionsColumn.AllowEdit = false;
            this.ID.OptionsColumn.AllowMove = false;
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
            // repositoryItemButtonEdit2
            // 
            this.repositoryItemButtonEdit2.AutoHeight = false;
            this.repositoryItemButtonEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit2.Name = "repositoryItemButtonEdit2";
            // 
            // repositoryItemButtonEdit3
            // 
            this.repositoryItemButtonEdit3.AutoHeight = false;
            this.repositoryItemButtonEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit3.Name = "repositoryItemButtonEdit3";
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.Caption = "设备名字";
            this.treeListColumn1.FieldName = "DVNAME";
            this.treeListColumn1.MinWidth = 34;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.OptionsColumn.AllowEdit = false;
            this.treeListColumn1.OptionsColumn.AllowMove = false;
            this.treeListColumn1.OptionsColumn.ReadOnly = true;
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            // 
            // treeList1
            // 
            this.treeList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList1.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.treeList1.Appearance.Empty.Options.UseBackColor = true;
            this.treeList1.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList1.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.treeList1.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.White;
            this.treeList1.Appearance.HeaderPanelBackground.Options.UseBackColor = true;
            this.treeList1.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.treeList1.Appearance.Row.Options.UseBackColor = true;
            this.treeList1.AppearancePrint.BandPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList1.AppearancePrint.BandPanel.Options.UseBackColor = true;
            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2});
            this.treeList1.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeList1.CustomizationFormBounds = new System.Drawing.Rectangle(1309, 565, 220, 265);
            this.treeList1.Location = new System.Drawing.Point(3, 3);
            this.treeList1.Name = "treeList1";
            this.treeList1.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList1.OptionsMenu.ShowConditionalFormattingItem = true;
            this.treeList1.OptionsView.ShowIndicator = false;
            this.treeList1.ParentFieldName = "PARENTID";
            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit4,
            this.repositoryItemButtonEdit5});
            this.treeList1.Size = new System.Drawing.Size(170, 491);
            this.treeList1.TabIndex = 58;
            this.treeList1.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList1_FocusedNodeChanged);
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "设备名字(样本数据)";
            this.treeListColumn2.FieldName = "DVNAME";
            this.treeListColumn2.MinWidth = 34;
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.OptionsColumn.AllowMove = false;
            this.treeListColumn2.OptionsColumn.ReadOnly = true;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // repositoryItemButtonEdit4
            // 
            this.repositoryItemButtonEdit4.AutoHeight = false;
            this.repositoryItemButtonEdit4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit4.Name = "repositoryItemButtonEdit4";
            // 
            // repositoryItemButtonEdit5
            // 
            this.repositoryItemButtonEdit5.AutoHeight = false;
            this.repositoryItemButtonEdit5.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit5.Name = "repositoryItemButtonEdit5";
            // 
            // treeList2
            // 
            this.treeList2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeList2.Appearance.Empty.BackColor = System.Drawing.Color.White;
            this.treeList2.Appearance.Empty.Options.UseBackColor = true;
            this.treeList2.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList2.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.treeList2.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.White;
            this.treeList2.Appearance.HeaderPanelBackground.Options.UseBackColor = true;
            this.treeList2.Appearance.Row.BackColor = System.Drawing.Color.White;
            this.treeList2.Appearance.Row.Options.UseBackColor = true;
            this.treeList2.AppearancePrint.BandPanel.BackColor = System.Drawing.Color.Blue;
            this.treeList2.AppearancePrint.BandPanel.Options.UseBackColor = true;
            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn3});
            this.treeList2.Cursor = System.Windows.Forms.Cursors.Default;
            this.treeList2.CustomizationFormBounds = new System.Drawing.Rectangle(1309, 565, 220, 265);
            this.treeList2.Location = new System.Drawing.Point(179, 3);
            this.treeList2.Name = "treeList2";
            this.treeList2.OptionsBehavior.PopulateServiceColumns = true;
            this.treeList2.OptionsMenu.ShowConditionalFormattingItem = true;
            this.treeList2.OptionsView.ShowIndicator = false;
            this.treeList2.ParentFieldName = "PARENTID";
            this.treeList2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemButtonEdit6,
            this.repositoryItemButtonEdit7});
            this.treeList2.Size = new System.Drawing.Size(156, 491);
            this.treeList2.TabIndex = 59;
            this.treeList2.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList2_FocusedNodeChanged);
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "设备名字(对比数据)";
            this.treeListColumn3.FieldName = "DVNAME";
            this.treeListColumn3.MinWidth = 34;
            this.treeListColumn3.Name = "treeListColumn3";
            this.treeListColumn3.OptionsColumn.AllowEdit = false;
            this.treeListColumn3.OptionsColumn.AllowMove = false;
            this.treeListColumn3.OptionsColumn.ReadOnly = true;
            this.treeListColumn3.Visible = true;
            this.treeListColumn3.VisibleIndex = 0;
            // 
            // repositoryItemButtonEdit6
            // 
            this.repositoryItemButtonEdit6.AutoHeight = false;
            this.repositoryItemButtonEdit6.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit6.Name = "repositoryItemButtonEdit6";
            // 
            // repositoryItemButtonEdit7
            // 
            this.repositoryItemButtonEdit7.AutoHeight = false;
            this.repositoryItemButtonEdit7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit7.Name = "repositoryItemButtonEdit7";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 18);
            this.label1.TabIndex = 60;
            this.label1.Text = "样本数据:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(345, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 18);
            this.label2.TabIndex = 61;
            this.label2.Text = "对比数据:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(375, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 18);
            this.label3.TabIndex = 62;
            this.label3.Text = "电流:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(375, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 18);
            this.label4.TabIndex = 63;
            this.label4.Text = "震动:";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(541, 453);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(54, 28);
            this.btnOk.TabIndex = 64;
            this.btnOk.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(599, 453);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(54, 28);
            this.btnClose.TabIndex = 65;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbVibrate
            // 
            this.lbVibrate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbVibrate.AutoSize = true;
            this.lbVibrate.Location = new System.Drawing.Point(423, 199);
            this.lbVibrate.Name = "lbVibrate";
            this.lbVibrate.Size = new System.Drawing.Size(43, 18);
            this.lbVibrate.TabIndex = 69;
            this.lbVibrate.Text = "震动:";
            // 
            // lbCurrent
            // 
            this.lbCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCurrent.AutoSize = true;
            this.lbCurrent.Location = new System.Drawing.Point(423, 159);
            this.lbCurrent.Name = "lbCurrent";
            this.lbCurrent.Size = new System.Drawing.Size(43, 18);
            this.lbCurrent.TabIndex = 68;
            this.lbCurrent.Text = "电流:";
            // 
            // lbContrast
            // 
            this.lbContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbContrast.AutoSize = true;
            this.lbContrast.Location = new System.Drawing.Point(423, 122);
            this.lbContrast.Name = "lbContrast";
            this.lbContrast.Size = new System.Drawing.Size(73, 18);
            this.lbContrast.TabIndex = 67;
            this.lbContrast.Text = "对比数据:";
            // 
            // lbSample
            // 
            this.lbSample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSample.AutoSize = true;
            this.lbSample.Location = new System.Drawing.Point(423, 84);
            this.lbSample.Name = "lbSample";
            this.lbSample.Size = new System.Drawing.Size(73, 18);
            this.lbSample.TabIndex = 66;
            this.lbSample.Text = "样本数据:";
            // 
            // FmContrast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 493);
            this.Controls.Add(this.lbVibrate);
            this.Controls.Add(this.lbCurrent);
            this.Controls.Add(this.lbContrast);
            this.Controls.Add(this.lbSample);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeList2);
            this.Controls.Add(this.treeList1);
            this.Controls.Add(this.rdoC);
            this.Controls.Add(this.rdoV);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(683, 540);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(683, 540);
            this.Name = "FmContrast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FmContrast";
            this.Load += new System.EventHandler(this.FmContrast_Load);
            ((System.ComponentModel.ISupportInitialize)(this.rdoV.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.删除)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup rdoV;
        private DevExpress.XtraEditors.RadioGroup rdoC;

        private DevExpress.XtraTreeList.Columns.TreeListColumn ID;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit 删除;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
         private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit3;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit5;
        private DevExpress.XtraTreeList.TreeList treeList2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit6;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Label lbVibrate;
        private System.Windows.Forms.Label lbCurrent;
        private System.Windows.Forms.Label lbContrast;
        private System.Windows.Forms.Label lbSample;
    }
}