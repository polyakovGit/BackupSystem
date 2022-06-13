using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient
{
    public partial class Quota : Form
    {
        public Quota()
        {
            InitializeComponent();
            labelUsed.Text = $"{Globals.Tasks.UsedQuota/1024} кБайт /";
            textBoxMax.Text = $"{Globals.Tasks.MaxQuota/1024}";
        }
    }
}
