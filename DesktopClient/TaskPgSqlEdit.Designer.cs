namespace DesktopClient
{
    partial class TaskPgSqlEdit
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
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.textBoxPass = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelConnectionStatus = new System.Windows.Forms.Label();
            this.comboBoxDatabases = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxSchedule = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Хост: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Имя пользователя: ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "Пароль:";
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Location = new System.Drawing.Point(143, 70);
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(121, 23);
            this.textBoxUserId.TabIndex = 2;
            this.textBoxUserId.Text = "BackupRole";
            // 
            // textBoxPass
            // 
            this.textBoxPass.Location = new System.Drawing.Point(143, 99);
            this.textBoxPass.Name = "textBoxPass";
            this.textBoxPass.PasswordChar = '*';
            this.textBoxPass.Size = new System.Drawing.Size(121, 23);
            this.textBoxPass.TabIndex = 3;
            this.textBoxPass.Text = "12345";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 146);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(98, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelConnectionStatus
            // 
            this.labelConnectionStatus.AutoSize = true;
            this.labelConnectionStatus.Location = new System.Drawing.Point(141, 150);
            this.labelConnectionStatus.Name = "labelConnectionStatus";
            this.labelConnectionStatus.Size = new System.Drawing.Size(163, 15);
            this.labelConnectionStatus.TabIndex = 7;
            this.labelConnectionStatus.Text = "Соединение не установлено";
            // 
            // comboBoxDatabases
            // 
            this.comboBoxDatabases.FormattingEnabled = true;
            this.comboBoxDatabases.Location = new System.Drawing.Point(143, 178);
            this.comboBoxDatabases.Name = "comboBoxDatabases";
            this.comboBoxDatabases.Size = new System.Drawing.Size(121, 23);
            this.comboBoxDatabases.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "База данных:";
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(143, 12);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(121, 23);
            this.textBoxHost.TabIndex = 1;
            this.textBoxHost.Text = "localhost";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 210);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 15);
            this.label5.TabIndex = 12;
            this.label5.Text = "Расписание:";
            // 
            // comboBoxSchedule
            // 
            this.comboBoxSchedule.FormattingEnabled = true;
            this.comboBoxSchedule.Items.AddRange(new object[] {
            "Ежедневно",
            "Еженедельно",
            "Ежемесячно"});
            this.comboBoxSchedule.Location = new System.Drawing.Point(143, 207);
            this.comboBoxSchedule.Name = "comboBoxSchedule";
            this.comboBoxSchedule.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSchedule.TabIndex = 6;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker1.Location = new System.Drawing.Point(283, 207);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(84, 23);
            this.dateTimePicker1.TabIndex = 7;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(229, 265);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 8;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(310, 265);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(260, 15);
            this.label6.TabIndex = 13;
            this.label6.Text = "Максимальное количество резервных копий:";
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(283, 236);
            this.textBoxCount.MaxLength = 5;
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(56, 23);
            this.textBoxCount.TabIndex = 14;
            this.textBoxCount.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 15);
            this.label7.TabIndex = 15;
            this.label7.Text = "Порт:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(143, 41);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(121, 23);
            this.textBoxPort.TabIndex = 16;
            this.textBoxPort.Text = "5432";
            // 
            // TaskPgSqlEdit
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(397, 300);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBoxSchedule);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxHost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxDatabases);
            this.Controls.Add(this.labelConnectionStatus);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPass);
            this.Controls.Add(this.textBoxUserId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskPgSqlEdit";
            this.Text = "Редактирование задания резервирования PostgreSQL";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox textBoxUserId;
        private TextBox textBoxPass;
        private Button buttonConnect;
        private Label labelConnectionStatus;
        private ComboBox comboBoxDatabases;
        private Label label4;
        private TextBox textBoxHost;
        private Label label5;
        private ComboBox comboBoxSchedule;
        private DateTimePicker dateTimePicker1;
        private Button buttonOk;
        private Button buttonCancel;
        private Label label6;
        private TextBox textBoxCount;
        private Label label7;
        private TextBox textBoxPort;
    }
}