namespace GeoWallE
{
    partial class InputForm
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
            this.lblMsg = new System.Windows.Forms.Label();
            this.nudInput1 = new System.Windows.Forms.NumericUpDown();
            this.lblInput1 = new System.Windows.Forms.Label();
            this.nudInput2 = new System.Windows.Forms.NumericUpDown();
            this.lblInput2 = new System.Windows.Forms.Label();
            this.nudInput3 = new System.Windows.Forms.NumericUpDown();
            this.lblInput3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput3)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Kristen ITC", 12F, System.Drawing.FontStyle.Bold);
            this.lblMsg.Location = new System.Drawing.Point(12, 9);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(78, 23);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "Enter...";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudInput1
            // 
            this.nudInput1.DecimalPlaces = 2;
            this.nudInput1.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudInput1.Location = new System.Drawing.Point(98, 61);
            this.nudInput1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInput1.Name = "nudInput1";
            this.nudInput1.Size = new System.Drawing.Size(69, 20);
            this.nudInput1.TabIndex = 1;
            this.nudInput1.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblInput1
            // 
            this.lblInput1.AutoSize = true;
            this.lblInput1.Font = new System.Drawing.Font("Kristen ITC", 10F, System.Drawing.FontStyle.Bold);
            this.lblInput1.Location = new System.Drawing.Point(12, 61);
            this.lblInput1.Name = "lblInput1";
            this.lblInput1.Size = new System.Drawing.Size(68, 19);
            this.lblInput1.TabIndex = 2;
            this.lblInput1.Text = "Enter...";
            this.lblInput1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudInput2
            // 
            this.nudInput2.DecimalPlaces = 2;
            this.nudInput2.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudInput2.Location = new System.Drawing.Point(347, 60);
            this.nudInput2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInput2.Name = "nudInput2";
            this.nudInput2.Size = new System.Drawing.Size(69, 20);
            this.nudInput2.TabIndex = 3;
            this.nudInput2.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblInput2
            // 
            this.lblInput2.AutoSize = true;
            this.lblInput2.Font = new System.Drawing.Font("Kristen ITC", 10F, System.Drawing.FontStyle.Bold);
            this.lblInput2.Location = new System.Drawing.Point(261, 61);
            this.lblInput2.Name = "lblInput2";
            this.lblInput2.Size = new System.Drawing.Size(68, 19);
            this.lblInput2.TabIndex = 4;
            this.lblInput2.Text = "Enter...";
            this.lblInput2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudInput3
            // 
            this.nudInput3.DecimalPlaces = 2;
            this.nudInput3.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.nudInput3.Location = new System.Drawing.Point(197, 106);
            this.nudInput3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInput3.Name = "nudInput3";
            this.nudInput3.Size = new System.Drawing.Size(69, 20);
            this.nudInput3.TabIndex = 5;
            this.nudInput3.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblInput3
            // 
            this.lblInput3.AutoSize = true;
            this.lblInput3.Font = new System.Drawing.Font("Kristen ITC", 10F, System.Drawing.FontStyle.Bold);
            this.lblInput3.Location = new System.Drawing.Point(99, 107);
            this.lblInput3.Name = "lblInput3";
            this.lblInput3.Size = new System.Drawing.Size(68, 19);
            this.lblInput3.TabIndex = 6;
            this.lblInput3.Text = "Enter...";
            this.lblInput3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Kristen ITC", 10F, System.Drawing.FontStyle.Bold);
            this.btnOk.Location = new System.Drawing.Point(46, 161);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(121, 32);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnRandom
            // 
            this.btnRandom.Font = new System.Drawing.Font("Kristen ITC", 10F, System.Drawing.FontStyle.Bold);
            this.btnRandom.Location = new System.Drawing.Point(265, 161);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(121, 32);
            this.btnRandom.TabIndex = 8;
            this.btnRandom.Text = "Random";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(469, 205);
            this.Controls.Add(this.btnRandom);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblInput3);
            this.Controls.Add(this.nudInput3);
            this.Controls.Add(this.lblInput2);
            this.Controls.Add(this.nudInput2);
            this.Controls.Add(this.lblInput1);
            this.Controls.Add(this.nudInput1);
            this.Controls.Add(this.lblMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "InputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputForm";
            ((System.ComponentModel.ISupportInitialize)(this.nudInput1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.NumericUpDown nudInput1;
        private System.Windows.Forms.Label lblInput1;
        private System.Windows.Forms.NumericUpDown nudInput2;
        private System.Windows.Forms.Label lblInput2;
        private System.Windows.Forms.NumericUpDown nudInput3;
        private System.Windows.Forms.Label lblInput3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnRandom;
    }
}