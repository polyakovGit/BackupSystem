namespace DesktopClient
{
    partial class TaskDatabaseEdit
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxNameServer = new System.Windows.Forms.ComboBox();
            this.textBoxNameUser = new System.Windows.Forms.TextBox();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.comboBoxDatabasesForBackup = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя сервера: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Имя пользователя: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пароль:";
            // 
            // comboBoxNameServer
            // 
            this.comboBoxNameServer.FormattingEnabled = true;
            this.comboBoxNameServer.Items.AddRange(new object[] {
            "XENO-B-STATION"});
            this.comboBoxNameServer.Location = new System.Drawing.Point(143, 12);
            this.comboBoxNameServer.Name = "comboBoxNameServer";
            this.comboBoxNameServer.Size = new System.Drawing.Size(121, 23);
            this.comboBoxNameServer.TabIndex = 3;
            this.comboBoxNameServer.Text = "XENO-B-STATION";
            // 
            // textBoxNameUser
            // 
            this.textBoxNameUser.Location = new System.Drawing.Point(143, 41);
            this.textBoxNameUser.Name = "textBoxNameUser";
            this.textBoxNameUser.Size = new System.Drawing.Size(121, 23);
            this.textBoxNameUser.TabIndex = 4;
            this.textBoxNameUser.Text = "BackupRole";
            // 
            // textBoxPass
            // 
            this.textBoxPass.Location = new System.Drawing.Point(143, 71);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.PasswordChar = '*';
            this.textBoxPass.Size = new System.Drawing.Size(121, 23);
            this.textBoxPass.TabIndex = 5;
            this.textBoxPass.Text = "12345";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 109);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(98, 23);
            this.buttonConnect.TabIndex = 6;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new System.Drawing.Point(143, 117);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(163, 15);
            this.labelConnectionStatus.TabIndex = 7;
            this.labelConnectionStatus.Text = "Соединение не установлено";
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(9, 148);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(101, 23);
            this.buttonDisconnect.TabIndex = 8;
            this.buttonDisconnect.Text = "Отключиться";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // comboBoxDatabasesForBackup
            // 
            this.comboBoxDatabasesForBackup.FormattingEnabled = true;
            this.comboBoxDatabasesForBackup.Location = new System.Drawing.Point(143, 217);
            this.comboBoxDatabasesForBackup.Name = "comboBoxDatabasesForBackup";
            this.comboBoxDatabasesForBackup.Size = new System.Drawing.Size(121, 23);
            this.comboBoxDatabasesForBackup.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 220);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Базы данных";
            // 
            // TaskDatabaseEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 381);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxDatabasesForBackup);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.labelConnectionStatus);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPass);
            this.Controls.Add(this.textBoxNameUser);
            this.Controls.Add(this.comboBoxNameServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TaskDatabaseEdit";
            this.Text = "Редактирование задачи резервирования баз данных";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox comboBoxNameServer;
        private TextBox textBoxNameUser;
        private TextBox textBoxPass;
        private Button buttonConnect;
        private Label labelConnectionStatus;
        private Button buttonDisconnect;
        private ComboBox comboBoxDatabasesForBackup;
        private Label label4;
    }
}