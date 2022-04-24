using System.ComponentModel;

namespace DesktopClient
{
    partial class BackupSettings
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SelectData = new System.Windows.Forms.Button();
            this.TimeBackup = new System.Windows.Forms.DateTimePicker();
            this.SelectTimeBackup = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // SelectData
            // 
            this.SelectData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.SelectData.Location = new System.Drawing.Point(36, 20);
            this.SelectData.Name = "SelectData";
            this.SelectData.Size = new System.Drawing.Size(139, 46);
            this.SelectData.TabIndex = 1;
            this.SelectData.Text = "Выберите данные";
            this.SelectData.UseVisualStyleBackColor = true;
            this.SelectData.Click += new System.EventHandler(this.SelectData_Click);
            // 
            // TimeBackup
            // 
            this.TimeBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.TimeBackup.Location = new System.Drawing.Point(271, 29);
            this.TimeBackup.Name = "TimeBackup";
            this.TimeBackup.Size = new System.Drawing.Size(80, 26);
            this.TimeBackup.TabIndex = 2;
            this.TimeBackup.Format = DateTimePickerFormat.Time;
            this.TimeBackup.ShowUpDown = true;
            // 
            // SelectTimeBackup
            // 
            this.SelectTimeBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.SelectTimeBackup.Location = new System.Drawing.Point(181, 29);
            this.SelectTimeBackup.Name = "SelectTimeBackup";
            this.SelectTimeBackup.Size = new System.Drawing.Size(80, 26);
            this.SelectTimeBackup.TabIndex = 3;
            this.SelectTimeBackup.Items.AddRange(new []
            {
                "Ежедневно",
                "Еженедельно",
                "Ежемесячно"
            });
            this.SelectTimeBackup.SelectedIndex = 0;
            // 
            // BackupSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.TimeBackup);
            this.Controls.Add(this.SelectData);
            this.Controls.Add(this.SelectTimeBackup);
            this.Name = "BackupSettings";
            this.Size = new System.Drawing.Size(362, 83);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DateTimePicker TimeBackup;
        private System.Windows.Forms.ComboBox SelectTimeBackup;
        private System.Windows.Forms.Button SelectData;

        #endregion
    }
}