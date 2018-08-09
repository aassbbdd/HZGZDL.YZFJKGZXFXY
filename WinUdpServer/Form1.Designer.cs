namespace WindowsFormsApp4
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.listBox = new DevExpress.XtraEditors.ListBoxControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lbPort = new DevExpress.XtraEditors.LabelControl();
            this.lbPortdddd = new DevExpress.XtraEditors.LabelControl();
            this.lbIp = new DevExpress.XtraEditors.LabelControl();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.listBox)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(63, 12);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(132, 29);
            this.simpleButton1.TabIndex = 2;
            this.simpleButton1.Text = "启用";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(214, 12);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(132, 29);
            this.simpleButton2.TabIndex = 3;
            this.simpleButton2.Text = "关闭";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 70);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "接收：";
            // 
            // listBox
            // 
            this.listBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.listBox.Location = new System.Drawing.Point(63, 68);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(706, 626);
            this.listBox.TabIndex = 5;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(584, 11);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(14, 18);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "IP";
            // 
            // lbPort
            // 
            this.lbPort.Location = new System.Drawing.Point(616, 35);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(81, 18);
            this.lbPort.TabIndex = 7;
            this.lbPort.Text = "labelControl3";
            // 
            // lbPortdddd
            // 
            this.lbPortdddd.Location = new System.Drawing.Point(560, 35);
            this.lbPortdddd.Name = "lbPortdddd";
            this.lbPortdddd.Size = new System.Drawing.Size(38, 18);
            this.lbPortdddd.TabIndex = 8;
            this.lbPortdddd.Text = "PORT";
            // 
            // lbIp
            // 
            this.lbIp.Location = new System.Drawing.Point(616, 11);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(81, 18);
            this.lbIp.TabIndex = 9;
            this.lbIp.Text = "labelControl5";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(365, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(132, 29);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "清空";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 706);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.lbPortdddd);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Name = "Form1";
            this.Text = "服务器";
            ((System.ComponentModel.ISupportInitialize)(this.listBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ListBoxControl listBox;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lbPort;
        private DevExpress.XtraEditors.LabelControl lbPortdddd;
        private DevExpress.XtraEditors.LabelControl lbIp;
        private DevExpress.XtraEditors.SimpleButton btnClear;
    }
}

