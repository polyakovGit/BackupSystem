namespace DesktopClient
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
            this.elements = new System.Windows.Forms.FlowLayoutPanel();
            this.add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // elements
            // 
            this.elements.AutoScroll = true;
            this.elements.Location = new System.Drawing.Point(1, 0);
            this.elements.Name = "elements";
            this.elements.Size = new System.Drawing.Size(390, 410);
            this.elements.TabIndex = 1;
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(165, 415);
            this.add.Name = "addBtn";
            this.add.Text = "Добавить";
            this.add.Click += AddBtnClick;
            this.add.Size = new System.Drawing.Size(60, 30);
            this.add.TabIndex = 2;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 450);
            this.Controls.Add(this.elements);
            this.Controls.Add(this.add);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Управление данными";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.FlowLayoutPanel elements;
        private System.Windows.Forms.Button add;

        #endregion
    }
}