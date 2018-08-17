namespace Basic_Controls
{
    partial class FmTestSelect
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioGroup);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 261);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "请选择测试计划";
            // 
            // radioGroup
            // 
            this.radioGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioGroup.EditValue = "3";
            this.radioGroup.Location = new System.Drawing.Point(3, 20);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.radioGroup.Properties.Appearance.Options.UseBackColor = true;
            this.radioGroup.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("0", "使用默认测试计划"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "加载一个存在测试计划"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "加载分析测试数据"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("3", "新建测试计划")});
            this.radioGroup.Size = new System.Drawing.Size(298, 238);
            this.radioGroup.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(192, 29);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "欢迎使用本系统！";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(198, 274);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(109, 30);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "选择";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // FmTestSelect
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 318);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 365);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 365);
            this.Name = "FmTestSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择测试";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
        private DevExpress.XtraEditors.SimpleButton btnSelect;
    }
}