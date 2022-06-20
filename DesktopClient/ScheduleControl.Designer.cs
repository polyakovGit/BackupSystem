namespace DesktopClient
{
    partial class ScheduleControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.labelDay = new System.Windows.Forms.Label();
            this.comboBoxDayOfWeek = new System.Windows.Forms.ComboBox();
            this.comboBoxDayOfMonth = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Ежедневно",
            "Еженедельно",
            "Ежемесячно"});
            this.comboBoxType.Location = new System.Drawing.Point(0, 0);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 23);
            this.comboBoxType.TabIndex = 0;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker.Location = new System.Drawing.Point(127, 0);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.ShowUpDown = true;
            this.dateTimePicker.Size = new System.Drawing.Size(67, 23);
            this.dateTimePicker.TabIndex = 1;
            // 
            // labelDay
            // 
            this.labelDay.AutoSize = true;
            this.labelDay.Location = new System.Drawing.Point(0, 35);
            this.labelDay.Name = "labelDay";
            this.labelDay.Size = new System.Drawing.Size(37, 15);
            this.labelDay.TabIndex = 2;
            this.labelDay.Text = "День:";
            // 
            // comboBoxDayOfWeek
            // 
            this.comboBoxDayOfWeek.FormattingEnabled = true;
            this.comboBoxDayOfWeek.Items.AddRange(new object[] {
            "Понедельник",
            "Вторник",
            "Среда",
            "Четверг",
            "Пятница",
            "Суббота",
            "Воскресение"});
            this.comboBoxDayOfWeek.Location = new System.Drawing.Point(43, 29);
            this.comboBoxDayOfWeek.Name = "comboBoxDayOfWeek";
            this.comboBoxDayOfWeek.Size = new System.Drawing.Size(121, 23);
            this.comboBoxDayOfWeek.TabIndex = 3;
            this.comboBoxDayOfWeek.Visible = false;
            // 
            // comboBoxDayOfMonth
            // 
            this.comboBoxDayOfMonth.FormattingEnabled = true;
            this.comboBoxDayOfMonth.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.comboBoxDayOfMonth.Location = new System.Drawing.Point(43, 29);
            this.comboBoxDayOfMonth.Name = "comboBoxDayOfMonth";
            this.comboBoxDayOfMonth.Size = new System.Drawing.Size(48, 23);
            this.comboBoxDayOfMonth.TabIndex = 4;
            // 
            // ScheduleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxDayOfMonth);
            this.Controls.Add(this.comboBoxDayOfWeek);
            this.Controls.Add(this.labelDay);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.comboBoxType);
            this.Name = "ScheduleControl";
            this.Size = new System.Drawing.Size(197, 54);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox comboBoxType;
        private DateTimePicker dateTimePicker;
        private Label labelDay;
        private ComboBox comboBoxDayOfWeek;
        private ComboBox comboBoxDayOfMonth;
    }
}
