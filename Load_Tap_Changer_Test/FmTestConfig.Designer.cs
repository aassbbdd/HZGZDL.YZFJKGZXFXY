﻿namespace Load_Tap_Changer_Test
{
    partial class FmTestConfig
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoTestType = new DevExpress.XtraEditors.RadioGroup();
            this.gbSINGLE = new System.Windows.Forms.GroupBox();
            this.RdoOrder = new DevExpress.XtraEditors.RadioGroup();
            this.CmbPlace = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.gbDOUBLE = new System.Windows.Forms.GroupBox();
            this.txtEPlace = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtSPlace = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lbHearder = new DevExpress.XtraEditors.LabelControl();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.RdoBaseC = new DevExpress.XtraEditors.RadioGroup();
            this.ckC2 = new DevExpress.XtraEditors.CheckEdit();
            this.ckC3 = new DevExpress.XtraEditors.CheckEdit();
            this.ckV2 = new DevExpress.XtraEditors.CheckEdit();
            this.ckV3 = new DevExpress.XtraEditors.CheckEdit();
            this.ckC1 = new DevExpress.XtraEditors.CheckEdit();
            this.ckV1 = new DevExpress.XtraEditors.CheckEdit();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtGetUnit = new DevExpress.XtraEditors.TextEdit();
            this.txtEA = new DevExpress.XtraEditors.TextEdit();
            this.txtSA = new DevExpress.XtraEditors.TextEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.rdoGETINFO = new DevExpress.XtraEditors.RadioGroup();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbBaseC = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdoTestType.Properties)).BeginInit();
            this.gbSINGLE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RdoOrder.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPlace.Properties)).BeginInit();
            this.gbDOUBLE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEPlace.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPlace.Properties)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RdoBaseC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV1.Properties)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGetUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSA.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGETINFO.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseC.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rdoTestType);
            this.groupBox3.Controls.Add(this.gbSINGLE);
            this.groupBox3.Controls.Add(this.gbDOUBLE);
            this.groupBox3.Location = new System.Drawing.Point(12, 100);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(506, 184);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "档位配置";
            // 
            // rdoTestType
            // 
            this.rdoTestType.EditValue = "1";
            this.rdoTestType.Location = new System.Drawing.Point(16, 23);
            this.rdoTestType.Name = "rdoTestType";
            this.rdoTestType.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.rdoTestType.Properties.Appearance.Options.UseBackColor = true;
            this.rdoTestType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "连续测试"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "单次测试")});
            this.rdoTestType.Size = new System.Drawing.Size(481, 30);
            this.rdoTestType.TabIndex = 26;
            this.rdoTestType.SelectedIndexChanged += new System.EventHandler(this.rdoTestType_SelectedIndexChanged);
            // 
            // gbSINGLE
            // 
            this.gbSINGLE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSINGLE.Controls.Add(this.RdoOrder);
            this.gbSINGLE.Controls.Add(this.CmbPlace);
            this.gbSINGLE.Controls.Add(this.labelControl3);
            this.gbSINGLE.Location = new System.Drawing.Point(247, 58);
            this.gbSINGLE.Name = "gbSINGLE";
            this.gbSINGLE.Size = new System.Drawing.Size(250, 126);
            this.gbSINGLE.TabIndex = 25;
            this.gbSINGLE.TabStop = false;
            this.gbSINGLE.Text = "单次测试";
            // 
            // RdoOrder
            // 
            this.RdoOrder.EditValue = "1";
            this.RdoOrder.Location = new System.Drawing.Point(6, 55);
            this.RdoOrder.Name = "RdoOrder";
            this.RdoOrder.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.RdoOrder.Properties.Appearance.Options.UseBackColor = true;
            this.RdoOrder.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "前往后"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "后往前")});
            this.RdoOrder.Size = new System.Drawing.Size(238, 65);
            this.RdoOrder.TabIndex = 22;
            this.RdoOrder.SelectedIndexChanged += new System.EventHandler(this.RdoOrder_SelectedIndexChanged);
            // 
            // CmbPlace
            // 
            this.CmbPlace.Location = new System.Drawing.Point(89, 24);
            this.CmbPlace.Name = "CmbPlace";
            this.CmbPlace.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbPlace.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbPlace.Size = new System.Drawing.Size(153, 24);
            this.CmbPlace.TabIndex = 21;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(6, 27);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(65, 18);
            this.labelControl3.TabIndex = 20;
            this.labelControl3.Text = "当前位置:";
            // 
            // gbDOUBLE
            // 
            this.gbDOUBLE.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDOUBLE.Controls.Add(this.txtEPlace);
            this.gbDOUBLE.Controls.Add(this.labelControl5);
            this.gbDOUBLE.Controls.Add(this.txtSPlace);
            this.gbDOUBLE.Controls.Add(this.labelControl4);
            this.gbDOUBLE.Controls.Add(this.lbHearder);
            this.gbDOUBLE.Location = new System.Drawing.Point(16, 58);
            this.gbDOUBLE.Name = "gbDOUBLE";
            this.gbDOUBLE.Size = new System.Drawing.Size(218, 108);
            this.gbDOUBLE.TabIndex = 24;
            this.gbDOUBLE.TabStop = false;
            this.gbDOUBLE.Text = "连续测试";
            // 
            // txtEPlace
            // 
            this.txtEPlace.Location = new System.Drawing.Point(86, 61);
            this.txtEPlace.Name = "txtEPlace";
            this.txtEPlace.Properties.Mask.EditMask = "[0-9]*";
            this.txtEPlace.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEPlace.Size = new System.Drawing.Size(117, 24);
            this.txtEPlace.TabIndex = 27;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(15, 64);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(65, 18);
            this.labelControl5.TabIndex = 26;
            this.labelControl5.Text = "结束位置:";
            // 
            // txtSPlace
            // 
            this.txtSPlace.Location = new System.Drawing.Point(86, 27);
            this.txtSPlace.Name = "txtSPlace";
            this.txtSPlace.Properties.Mask.EditMask = "[0-9]*";
            this.txtSPlace.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSPlace.Size = new System.Drawing.Size(117, 24);
            this.txtSPlace.TabIndex = 25;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(15, 30);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(65, 18);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "当前位置:";
            // 
            // lbHearder
            // 
            this.lbHearder.Location = new System.Drawing.Point(30, 39);
            this.lbHearder.Name = "lbHearder";
            this.lbHearder.Size = new System.Drawing.Size(0, 18);
            this.lbHearder.TabIndex = 13;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.labelControl7);
            this.groupBox5.Controls.Add(this.RdoBaseC);
            this.groupBox5.Controls.Add(this.ckC2);
            this.groupBox5.Controls.Add(this.ckC3);
            this.groupBox5.Controls.Add(this.ckV2);
            this.groupBox5.Controls.Add(this.ckV3);
            this.groupBox5.Controls.Add(this.ckC1);
            this.groupBox5.Controls.Add(this.ckV1);
            this.groupBox5.Location = new System.Drawing.Point(12, 290);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(506, 129);
            this.groupBox5.TabIndex = 25;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "测试通道";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(163, 13);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 18);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "基准电流";
            // 
            // RdoBaseC
            // 
            this.RdoBaseC.EditValue = "1";
            this.RdoBaseC.Location = new System.Drawing.Point(157, 37);
            this.RdoBaseC.Name = "RdoBaseC";
            this.RdoBaseC.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.RdoBaseC.Properties.Appearance.Options.UseBackColor = true;
            this.RdoBaseC.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "电流1"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "电流2"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(3, "电流3")});
            this.RdoBaseC.Size = new System.Drawing.Size(77, 76);
            this.RdoBaseC.TabIndex = 18;
            // 
            // ckC2
            // 
            this.ckC2.Location = new System.Drawing.Point(84, 63);
            this.ckC2.Name = "ckC2";
            this.ckC2.Properties.Caption = "电流2";
            this.ckC2.Size = new System.Drawing.Size(62, 22);
            this.ckC2.TabIndex = 11;
            // 
            // ckC3
            // 
            this.ckC3.Location = new System.Drawing.Point(84, 91);
            this.ckC3.Name = "ckC3";
            this.ckC3.Properties.Caption = "电流3";
            this.ckC3.Size = new System.Drawing.Size(62, 22);
            this.ckC3.TabIndex = 10;
            // 
            // ckV2
            // 
            this.ckV2.Location = new System.Drawing.Point(16, 63);
            this.ckV2.Name = "ckV2";
            this.ckV2.Properties.Caption = "震动2";
            this.ckV2.Size = new System.Drawing.Size(62, 22);
            this.ckV2.TabIndex = 9;
            // 
            // ckV3
            // 
            this.ckV3.Location = new System.Drawing.Point(16, 91);
            this.ckV3.Name = "ckV3";
            this.ckV3.Properties.Caption = "震动3";
            this.ckV3.Size = new System.Drawing.Size(62, 22);
            this.ckV3.TabIndex = 8;
            // 
            // ckC1
            // 
            this.ckC1.EditValue = true;
            this.ckC1.Location = new System.Drawing.Point(84, 35);
            this.ckC1.Name = "ckC1";
            this.ckC1.Properties.Caption = "电流1";
            this.ckC1.Size = new System.Drawing.Size(62, 22);
            this.ckC1.TabIndex = 5;
            // 
            // ckV1
            // 
            this.ckV1.EditValue = true;
            this.ckV1.Location = new System.Drawing.Point(16, 35);
            this.ckV1.Name = "ckV1";
            this.ckV1.Properties.Caption = "震动1";
            this.ckV1.Size = new System.Drawing.Size(62, 22);
            this.ckV1.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(313, 428);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(101, 32);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(420, 428);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 32);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.labelControl6);
            this.groupBox4.Controls.Add(this.txtGetUnit);
            this.groupBox4.Controls.Add(this.txtEA);
            this.groupBox4.Controls.Add(this.txtSA);
            this.groupBox4.Controls.Add(this.labelControl13);
            this.groupBox4.Controls.Add(this.labelControl14);
            this.groupBox4.Controls.Add(this.labelControl15);
            this.groupBox4.Controls.Add(this.rdoGETINFO);
            this.groupBox4.Controls.Add(this.labelControl16);
            this.groupBox4.Location = new System.Drawing.Point(12, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(346, 89);
            this.groupBox4.TabIndex = 27;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "采样信息";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(224, 53);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(10, 18);
            this.labelControl6.TabIndex = 18;
            this.labelControl6.Text = "A";
            // 
            // txtGetUnit
            // 
            this.txtGetUnit.Location = new System.Drawing.Point(186, 52);
            this.txtGetUnit.Name = "txtGetUnit";
            this.txtGetUnit.Properties.Mask.EditMask = "[0-9]*";
            this.txtGetUnit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtGetUnit.Size = new System.Drawing.Size(37, 24);
            this.txtGetUnit.TabIndex = 17;
            // 
            // txtEA
            // 
            this.txtEA.Location = new System.Drawing.Point(283, 26);
            this.txtEA.Name = "txtEA";
            this.txtEA.Properties.Mask.EditMask = "\\d+(\\R.\\d{0,2})?";
            this.txtEA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEA.Size = new System.Drawing.Size(37, 24);
            this.txtEA.TabIndex = 15;
            // 
            // txtSA
            // 
            this.txtSA.Location = new System.Drawing.Point(186, 26);
            this.txtSA.Name = "txtSA";
            this.txtSA.Properties.Mask.EditMask = "\\d+(\\R.\\d{0,2})?";
            this.txtSA.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtSA.Size = new System.Drawing.Size(37, 24);
            this.txtSA.TabIndex = 12;
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(321, 29);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(10, 18);
            this.labelControl13.TabIndex = 16;
            this.labelControl13.Text = "A";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(247, 29);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(35, 18);
            this.labelControl14.TabIndex = 14;
            this.labelControl14.Text = "结束:";
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(224, 29);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(10, 18);
            this.labelControl15.TabIndex = 13;
            this.labelControl15.Text = "A";
            // 
            // rdoGETINFO
            // 
            this.rdoGETINFO.EditValue = "1";
            this.rdoGETINFO.Location = new System.Drawing.Point(4, 22);
            this.rdoGETINFO.Name = "rdoGETINFO";
            this.rdoGETINFO.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.rdoGETINFO.Properties.Appearance.Options.UseBackColor = true;
            this.rdoGETINFO.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("1", "触发后采样"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("2", "以秒为单位采样")});
            this.rdoGETINFO.Size = new System.Drawing.Size(136, 54);
            this.rdoGETINFO.TabIndex = 1;
            this.rdoGETINFO.SelectedIndexChanged += new System.EventHandler(this.rdoGetInfo_SelectedIndexChanged);
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(146, 29);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(35, 18);
            this.labelControl16.TabIndex = 11;
            this.labelControl16.Text = "开始:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(364, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 18);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "测试电流:";
            // 
            // cmbBaseC
            // 
            this.cmbBaseC.EditValue = "10";
            this.cmbBaseC.Location = new System.Drawing.Point(438, 38);
            this.cmbBaseC.Name = "cmbBaseC";
            this.cmbBaseC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBaseC.Properties.Items.AddRange(new object[] {
            "10",
            "100"});
            this.cmbBaseC.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbBaseC.Size = new System.Drawing.Size(50, 24);
            this.cmbBaseC.TabIndex = 15;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(495, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(10, 18);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "A";
            // 
            // FmTestConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 472);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cmbBaseC);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Name = "FmTestConfig";
            this.Text = "配置测试内容";
            this.Load += new System.EventHandler(this.FmTestConfig_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rdoTestType.Properties)).EndInit();
            this.gbSINGLE.ResumeLayout(false);
            this.gbSINGLE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RdoOrder.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPlace.Properties)).EndInit();
            this.gbDOUBLE.ResumeLayout(false);
            this.gbDOUBLE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtEPlace.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSPlace.Properties)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RdoBaseC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckC1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ckV1.Properties)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtGetUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtEA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSA.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoGETINFO.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBaseC.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox5;
        private DevExpress.XtraEditors.CheckEdit ckC1;
        private DevExpress.XtraEditors.CheckEdit ckV1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.GroupBox groupBox4;
        private DevExpress.XtraEditors.TextEdit txtGetUnit;
        private DevExpress.XtraEditors.TextEdit txtEA;
        private DevExpress.XtraEditors.TextEdit txtSA;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.RadioGroup rdoGETINFO;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit cmbBaseC;
        private DevExpress.XtraEditors.RadioGroup RdoBaseC;
        private DevExpress.XtraEditors.CheckEdit ckC2;
        private DevExpress.XtraEditors.CheckEdit ckC3;
        private DevExpress.XtraEditors.CheckEdit ckV2;
        private DevExpress.XtraEditors.CheckEdit ckV3;
        private System.Windows.Forms.GroupBox gbSINGLE;
        private DevExpress.XtraEditors.RadioGroup RdoOrder;
        private DevExpress.XtraEditors.ComboBoxEdit CmbPlace;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private System.Windows.Forms.GroupBox gbDOUBLE;
        private DevExpress.XtraEditors.TextEdit txtEPlace;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtSPlace;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lbHearder;
        private DevExpress.XtraEditors.RadioGroup rdoTestType;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}