namespace WindowsFormsApp1
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
            this.txtMsg = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtPorit = new DevExpress.XtraEditors.TextEdit();
            this.txtIp = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.rtBox = new System.Windows.Forms.RichTextBox();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.v2 = new DevExpress.XtraEditors.CheckEdit();
            this.v3 = new DevExpress.XtraEditors.CheckEdit();
            this.c1 = new DevExpress.XtraEditors.CheckEdit();
            this.c2 = new DevExpress.XtraEditors.CheckEdit();
            this.c3 = new DevExpress.XtraEditors.CheckEdit();
            this.v1 = new DevExpress.XtraEditors.CheckEdit();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.zd1 = new ZedGraph.ZedGraphControl();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtMsg.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIp.Properties)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.v2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.v1.Properties)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMsg
            // 
            this.txtMsg.EditValue = "03550355";
            this.txtMsg.Location = new System.Drawing.Point(108, 92);
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.Size = new System.Drawing.Size(398, 24);
            this.txtMsg.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(512, 85);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(86, 31);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "发送";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtPorit
            // 
            this.txtPorit.EditValue = "4000";
            this.txtPorit.Location = new System.Drawing.Point(108, 62);
            this.txtPorit.Name = "txtPorit";
            this.txtPorit.Size = new System.Drawing.Size(398, 24);
            this.txtPorit.TabIndex = 3;
            // 
            // txtIp
            // 
            this.txtIp.EditValue = "192.168.1.5";
            this.txtIp.Location = new System.Drawing.Point(108, 32);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(398, 24);
            this.txtIp.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(59, 18);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "发送IP：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(1, 95);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "发送内容：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(1, 65);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(75, 18);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "发送端口：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(31, 119);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 8;
            this.labelControl4.Text = "接收：";
            // 
            // rtBox
            // 
            this.rtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtBox.Location = new System.Drawing.Point(108, 128);
            this.rtBox.Name = "rtBox";
            this.rtBox.Size = new System.Drawing.Size(676, 571);
            this.rtBox.TabIndex = 10;
            this.rtBox.Text = "";
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(696, 85);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(86, 31);
            this.simpleButton2.TabIndex = 11;
            this.simpleButton2.Text = "停用";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(604, 85);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(86, 31);
            this.simpleButton3.TabIndex = 12;
            this.simpleButton3.Text = "开始";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(512, 35);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(86, 31);
            this.simpleButton4.TabIndex = 13;
            this.simpleButton4.Text = "清空";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(696, 35);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(86, 31);
            this.simpleButton6.TabIndex = 15;
            this.simpleButton6.Text = "生成";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 741);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelControl4);
            this.tabPage1.Controls.Add(this.simpleButton6);
            this.tabPage1.Controls.Add(this.txtMsg);
            this.tabPage1.Controls.Add(this.simpleButton1);
            this.tabPage1.Controls.Add(this.simpleButton4);
            this.tabPage1.Controls.Add(this.txtPorit);
            this.tabPage1.Controls.Add(this.simpleButton3);
            this.tabPage1.Controls.Add(this.txtIp);
            this.tabPage1.Controls.Add(this.simpleButton2);
            this.tabPage1.Controls.Add(this.labelControl1);
            this.tabPage1.Controls.Add(this.rtBox);
            this.tabPage1.Controls.Add(this.labelControl2);
            this.tabPage1.Controls.Add(this.labelControl3);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 712);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Udp通信";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.simpleButton7);
            this.tabPage2.Controls.Add(this.simpleButton8);
            this.tabPage2.Controls.Add(this.simpleButton9);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.v2);
            this.tabPage2.Controls.Add(this.v3);
            this.tabPage2.Controls.Add(this.c1);
            this.tabPage2.Controls.Add(this.c2);
            this.tabPage2.Controls.Add(this.c3);
            this.tabPage2.Controls.Add(this.v1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 712);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "TeeChart图表";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(190, 15);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(86, 31);
            this.simpleButton7.TabIndex = 16;
            this.simpleButton7.Text = "清空";
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Location = new System.Drawing.Point(6, 15);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(86, 31);
            this.simpleButton8.TabIndex = 15;
            this.simpleButton8.Text = "开始";
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // simpleButton9
            // 
            this.simpleButton9.Location = new System.Drawing.Point(98, 15);
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Size = new System.Drawing.Size(86, 31);
            this.simpleButton9.TabIndex = 14;
            this.simpleButton9.Text = "停用";
            this.simpleButton9.Click += new System.EventHandler(this.simpleButton9_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(6, 113);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 573);
            this.panel1.TabIndex = 12;
            // 
            // v2
            // 
            this.v2.Location = new System.Drawing.Point(559, 13);
            this.v2.Name = "v2";
            this.v2.Properties.Caption = "震动2";
            this.v2.Size = new System.Drawing.Size(94, 22);
            this.v2.TabIndex = 11;
            // 
            // v3
            // 
            this.v3.Location = new System.Drawing.Point(659, 13);
            this.v3.Name = "v3";
            this.v3.Properties.Caption = "震动3";
            this.v3.Size = new System.Drawing.Size(94, 22);
            this.v3.TabIndex = 10;
            // 
            // c1
            // 
            this.c1.EditValue = true;
            this.c1.Location = new System.Drawing.Point(459, 41);
            this.c1.Name = "c1";
            this.c1.Properties.Caption = "电流1";
            this.c1.Size = new System.Drawing.Size(94, 22);
            this.c1.TabIndex = 9;
            // 
            // c2
            // 
            this.c2.Location = new System.Drawing.Point(559, 41);
            this.c2.Name = "c2";
            this.c2.Properties.Caption = "电流2";
            this.c2.Size = new System.Drawing.Size(94, 22);
            this.c2.TabIndex = 8;
            // 
            // c3
            // 
            this.c3.Location = new System.Drawing.Point(659, 41);
            this.c3.Name = "c3";
            this.c3.Properties.Caption = "电流3";
            this.c3.Size = new System.Drawing.Size(94, 22);
            this.c3.TabIndex = 7;
            // 
            // v1
            // 
            this.v1.EditValue = true;
            this.v1.Location = new System.Drawing.Point(459, 13);
            this.v1.Name = "v1";
            this.v1.Properties.Caption = "震动1";
            this.v1.Size = new System.Drawing.Size(94, 22);
            this.v1.TabIndex = 6;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.simpleButton5);
            this.tabPage3.Controls.Add(this.simpleButton10);
            this.tabPage3.Controls.Add(this.zd1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(792, 712);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // zd1
            // 
            this.zd1.IsShowPointValues = false;
            this.zd1.Location = new System.Drawing.Point(0, 162);
            this.zd1.Name = "zd1";
            this.zd1.PointValueFormat = "G";
            this.zd1.Size = new System.Drawing.Size(738, 506);
            this.zd1.TabIndex = 0;
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(61, 42);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(86, 31);
            this.simpleButton5.TabIndex = 17;
            this.simpleButton5.Text = "开始";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Location = new System.Drawing.Point(153, 42);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(86, 31);
            this.simpleButton10.TabIndex = 16;
            this.simpleButton10.Text = "停用";
            this.simpleButton10.Click += new System.EventHandler(this.simpleButton10_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 741);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "通信测试工具 v1.0";
            ((System.ComponentModel.ISupportInitialize)(this.txtMsg.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPorit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIp.Properties)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.v2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.v1.Properties)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtMsg;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtPorit;
        private DevExpress.XtraEditors.TextEdit txtIp;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.RichTextBox rtBox;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DevExpress.XtraEditors.CheckEdit v2;
        private DevExpress.XtraEditors.CheckEdit v3;
        private DevExpress.XtraEditors.CheckEdit c1;
        private DevExpress.XtraEditors.CheckEdit c2;
        private DevExpress.XtraEditors.CheckEdit c3;
        private DevExpress.XtraEditors.CheckEdit v1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private System.Windows.Forms.TabPage tabPage3;
        private ZedGraph.ZedGraphControl zd1;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
    }
}

