namespace Load_Tap_Changer_Test.Staffgauge
{
    partial class Staffgauge
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.XX = new System.Windows.Forms.Label();
            this.YY = new System.Windows.Forms.Label();
            this.X = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.Label();
            this.pcFront = new System.Windows.Forms.PictureBox();
            this.pcAfter = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcFront)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcAfter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(740, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "这是遮罩";
            // 
            // XX
            // 
            this.XX.AutoSize = true;
            this.XX.Location = new System.Drawing.Point(267, 40);
            this.XX.Name = "XX";
            this.XX.Size = new System.Drawing.Size(23, 15);
            this.XX.TabIndex = 1;
            this.XX.Text = "XX";
            // 
            // YY
            // 
            this.YY.AutoSize = true;
            this.YY.Location = new System.Drawing.Point(267, 67);
            this.YY.Name = "YY";
            this.YY.Size = new System.Drawing.Size(23, 15);
            this.YY.TabIndex = 2;
            this.YY.Text = "YY";
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(37, 40);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(15, 15);
            this.X.TabIndex = 3;
            this.X.Text = "X";
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(37, 67);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(15, 15);
            this.Y.TabIndex = 4;
            this.Y.Text = "Y";
            // 
            // pcFront
            // 
            this.pcFront.BackColor = System.Drawing.Color.Transparent;
            this.pcFront.Location = new System.Drawing.Point(280, 156);
            this.pcFront.Name = "pcFront";
            this.pcFront.Size = new System.Drawing.Size(10, 562);
            this.pcFront.TabIndex = 5;
            this.pcFront.TabStop = false;
            // 
            // pcAfter
            // 
            this.pcAfter.BackColor = System.Drawing.Color.Transparent;
            this.pcAfter.Location = new System.Drawing.Point(516, 156);
            this.pcAfter.Name = "pcAfter";
            this.pcAfter.Size = new System.Drawing.Size(10, 562);
            this.pcAfter.TabIndex = 6;
            this.pcAfter.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(697, 272);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(46, 68);
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(455, 426);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 409);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "label3";
            // 
            // Staffgauge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.pcAfter);
            this.Controls.Add(this.pcFront);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Controls.Add(this.YY);
            this.Controls.Add(this.XX);
            this.Controls.Add(this.label1);
            this.Name = "Staffgauge";
            this.Size = new System.Drawing.Size(918, 764);
            this.Load += new System.EventHandler(this.Staffgauge_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Staffgauge_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Staffgauge_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Staffgauge_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pcFront)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcAfter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label XX;
        private System.Windows.Forms.Label YY;
        private System.Windows.Forms.Label X;
        private System.Windows.Forms.Label Y;
        private System.Windows.Forms.PictureBox pcFront;
        private System.Windows.Forms.PictureBox pcAfter;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
