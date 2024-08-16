namespace ChatApp
{
    partial class SelectChatRoom
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
            this.btnPrChat = new System.Windows.Forms.Button();
            this.btnGrChat = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrChat
            // 
            this.btnPrChat.Location = new System.Drawing.Point(90, 157);
            this.btnPrChat.Name = "btnPrChat";
            this.btnPrChat.Size = new System.Drawing.Size(125, 50);
            this.btnPrChat.TabIndex = 0;
            this.btnPrChat.Text = "Private Chat";
            this.btnPrChat.UseVisualStyleBackColor = true;
            this.btnPrChat.Click += new System.EventHandler(this.btnPrChat_Click);
            // 
            // btnGrChat
            // 
            this.btnGrChat.Location = new System.Drawing.Point(266, 157);
            this.btnGrChat.Name = "btnGrChat";
            this.btnGrChat.Size = new System.Drawing.Size(125, 50);
            this.btnGrChat.TabIndex = 1;
            this.btnGrChat.Text = "Group Chat";
            this.btnGrChat.UseVisualStyleBackColor = true;
            this.btnGrChat.Click += new System.EventHandler(this.btnGrChat_Click);
            // 
            // SelectChatRoom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 453);
            this.Controls.Add(this.btnGrChat);
            this.Controls.Add(this.btnPrChat);
            this.Name = "SelectChatRoom";
            this.Text = "SelectChatRoom";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrChat;
        private System.Windows.Forms.Button btnGrChat;
    }
}