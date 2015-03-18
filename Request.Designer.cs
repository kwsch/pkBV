namespace pkBV
{
    partial class Request
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
            this.L_Web = new System.Windows.Forms.Label();
            this.TB_Web = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_File = new System.Windows.Forms.TextBox();
            this.RTB_1 = new System.Windows.Forms.RichTextBox();
            this.RTB_2 = new System.Windows.Forms.RichTextBox();
            this.B_Request = new System.Windows.Forms.Button();
            this.B_Convert = new System.Windows.Forms.Button();
            this.TB_Shared = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // L_Web
            // 
            this.L_Web.AutoSize = true;
            this.L_Web.Location = new System.Drawing.Point(12, 9);
            this.L_Web.Name = "L_Web";
            this.L_Web.Size = new System.Drawing.Size(62, 13);
            this.L_Web.TabIndex = 0;
            this.L_Web.Text = "Webserver:";
            // 
            // TB_Web
            // 
            this.TB_Web.Location = new System.Drawing.Point(77, 6);
            this.TB_Web.Name = "TB_Web";
            this.TB_Web.Size = new System.Drawing.Size(390, 20);
            this.TB_Web.TabIndex = 1;
            this.TB_Web.Text = " https://ctr-ekja-live.s3.amazonaws.com/10.CTR_EKJA_datastore/ds/1/data/";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "FileCode:";
            // 
            // TB_File
            // 
            this.TB_File.Enabled = false;
            this.TB_File.Location = new System.Drawing.Point(77, 29);
            this.TB_File.Name = "TB_File";
            this.TB_File.Size = new System.Drawing.Size(111, 20);
            this.TB_File.TabIndex = 3;
            this.TB_File.Text = "00010087124-00001";
            // 
            // RTB_1
            // 
            this.RTB_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RTB_1.Location = new System.Drawing.Point(17, 55);
            this.RTB_1.Name = "RTB_1";
            this.RTB_1.Size = new System.Drawing.Size(220, 195);
            this.RTB_1.TabIndex = 4;
            this.RTB_1.Text = "der";
            // 
            // RTB_2
            // 
            this.RTB_2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RTB_2.Location = new System.Drawing.Point(247, 55);
            this.RTB_2.Name = "RTB_2";
            this.RTB_2.Size = new System.Drawing.Size(220, 195);
            this.RTB_2.TabIndex = 5;
            this.RTB_2.Text = "key";
            // 
            // B_Request
            // 
            this.B_Request.Location = new System.Drawing.Point(392, 27);
            this.B_Request.Name = "B_Request";
            this.B_Request.Size = new System.Drawing.Size(75, 23);
            this.B_Request.TabIndex = 6;
            this.B_Request.Text = "Request BV";
            this.B_Request.UseVisualStyleBackColor = true;
            this.B_Request.Click += new System.EventHandler(this.B_Request_Click);
            // 
            // B_Convert
            // 
            this.B_Convert.Location = new System.Drawing.Point(194, 27);
            this.B_Convert.Name = "B_Convert";
            this.B_Convert.Size = new System.Drawing.Size(22, 23);
            this.B_Convert.TabIndex = 7;
            this.B_Convert.Text = "<";
            this.B_Convert.UseVisualStyleBackColor = true;
            this.B_Convert.Click += new System.EventHandler(this.B_Convert_Click);
            // 
            // TB_Shared
            // 
            this.TB_Shared.Enabled = false;
            this.TB_Shared.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_Shared.Location = new System.Drawing.Point(222, 29);
            this.TB_Shared.Name = "TB_Shared";
            this.TB_Shared.Size = new System.Drawing.Size(164, 20);
            this.TB_Shared.TabIndex = 8;
            this.TB_Shared.Text = "BATTLE VIDEO CODE HERE";
            // 
            // Request
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 262);
            this.Controls.Add(this.TB_Shared);
            this.Controls.Add(this.B_Convert);
            this.Controls.Add(this.B_Request);
            this.Controls.Add(this.RTB_2);
            this.Controls.Add(this.RTB_1);
            this.Controls.Add(this.TB_File);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TB_Web);
            this.Controls.Add(this.L_Web);
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "Request";
            this.Text = "Request";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_Web;
        private System.Windows.Forms.TextBox TB_Web;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TB_File;
        private System.Windows.Forms.RichTextBox RTB_1;
        private System.Windows.Forms.RichTextBox RTB_2;
        private System.Windows.Forms.Button B_Request;
        private System.Windows.Forms.Button B_Convert;
        private System.Windows.Forms.TextBox TB_Shared;
    }
}