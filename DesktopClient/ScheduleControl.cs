using SharedData;

namespace DesktopClient
{
    public partial class ScheduleControl : UserControl
    {
        public ScheduleControl()
        {
            InitializeComponent();
            comboBoxType.SelectedIndex = 0;
            comboBoxDayOfWeek.SelectedIndex = 0;
            comboBoxDayOfMonth.SelectedIndex = 0;
            labelDay.Visible = false;
            comboBoxDayOfWeek.Visible = false;
            comboBoxDayOfMonth.Visible = false;
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedIndex == 1)
            {
                labelDay.Visible = true;
                comboBoxDayOfWeek.Visible = true;
                comboBoxDayOfMonth.Visible = false;
            }
            else if (comboBoxType.SelectedIndex == 2)
            {
                labelDay.Visible = true;
                comboBoxDayOfWeek.Visible = false;
                comboBoxDayOfMonth.Visible = true;
            }
            else
            {
                labelDay.Visible = false;
                comboBoxDayOfWeek.Visible = false;
                comboBoxDayOfMonth.Visible = false;
            }
        }

        public Schedule GetSchedule()
        {
            int type = comboBoxType.SelectedIndex;
            if (type < 0 || type > 2)
                type = 0;
            int dayOfWeek = comboBoxDayOfWeek.SelectedIndex;
            if (dayOfWeek < 0 || dayOfWeek > 6)
                dayOfWeek = 0;
            if (dayOfWeek == 6)
                dayOfWeek = 0;
            else
                dayOfWeek += 1;
            int dayOfMonth = comboBoxDayOfMonth.SelectedIndex;
            dayOfMonth += 1;
            if (dayOfMonth < 1 || dayOfMonth > 31)
                dayOfMonth = 1;

            return new Schedule
            {
                Type = (ScheduleType)type,
                Time = dateTimePicker.Value.TimeOfDay,
                DayOfWeek = (DayOfWeek)dayOfWeek,
                DayOfMonth = dayOfMonth
            };
        }

        public void SetSchedule(Schedule schedule)
        {
            int type = (int)schedule.Type;
            if (type < 0 || type > 2)
                type = 0;
            comboBoxType.SelectedIndex = type;

            dateTimePicker.Value = DateTime.Today + schedule.Time;

            int dayOfWeek = (int)schedule.DayOfWeek;
            if (dayOfWeek < 0 || dayOfWeek > 6)
                dayOfWeek = 0;
            if (dayOfWeek == 0)
                dayOfWeek = 6;
            else
                dayOfWeek -= 1;
            comboBoxDayOfWeek.SelectedIndex = dayOfWeek;

            int dayOfMonth = schedule.DayOfMonth;
            if (dayOfMonth < 1 || dayOfMonth > 31)
                dayOfMonth = 1;
            dayOfMonth -= 1;
            comboBoxDayOfMonth.SelectedIndex = dayOfMonth;
        }
    }
}
