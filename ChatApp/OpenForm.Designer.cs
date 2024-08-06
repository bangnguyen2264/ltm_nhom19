namespace ChatApp
{
    partial class OpenForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnComeIn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(12, 201);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 29);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your username";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(267, 201);
            this.txtUsername.Multiline = true;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(203, 30);
            this.txtUsername.TabIndex = 1;
            // 
            // btnComeIn
            // 
            this.btnComeIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnComeIn.Location = new System.Drawing.Point(179, 267);
            this.btnComeIn.Name = "btnComeIn";
            this.btnComeIn.Size = new System.Drawing.Size(122, 43);
            this.btnComeIn.TabIndex = 2;
            this.btnComeIn.Text = "JOIN";
            this.btnComeIn.UseVisualStyleBackColor = true;
            this.btnComeIn.Click += new System.EventHandler(this.btnComeIn_Click);
            // 
            // OpenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 453);
            this.Controls.Add(this.btnComeIn);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.label1);
            this.Name = "OpenForm";
            this.Text = "OpenForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnComeIn;
    }
}