namespace ChatClient
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            outputBox = new RichTextBox();
            inputBox = new RichTextBox();
            sendBtn = new Button();
            SuspendLayout();
            // 
            // outputBox
            // 
            outputBox.Location = new Point(12, 12);
            outputBox.Name = "outputBox";
            outputBox.ReadOnly = true;
            outputBox.Size = new Size(353, 382);
            outputBox.TabIndex = 0;
            outputBox.Text = "";
            // 
            // inputBox
            // 
            inputBox.Location = new Point(12, 400);
            inputBox.Name = "inputBox";
            inputBox.Size = new Size(303, 38);
            inputBox.TabIndex = 1;
            inputBox.Text = "";
            // 
            // sendBtn
            // 
            sendBtn.BackColor = Color.White;
            sendBtn.BackgroundImage = (Image)resources.GetObject("sendBtn.BackgroundImage");
            sendBtn.BackgroundImageLayout = ImageLayout.Stretch;
            sendBtn.FlatStyle = FlatStyle.Flat;
            sendBtn.Location = new Point(321, 400);
            sendBtn.Name = "sendBtn";
            sendBtn.Size = new Size(44, 38);
            sendBtn.TabIndex = 2;
            sendBtn.UseVisualStyleBackColor = false;
            sendBtn.Click += sendBtn_Click;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(sendBtn);
            Controls.Add(inputBox);
            Controls.Add(outputBox);
            Name = "Main";
            Text = "Main";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox outputBox;
        private RichTextBox inputBox;
        private Button sendBtn;
    }
}