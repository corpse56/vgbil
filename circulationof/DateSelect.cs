using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Circulation
{
    public partial class DateSelect : Form
    {
        public DateTime _selDate;
        
        
        public DateSelect()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _selDate = dateTimePicker1.Value;
            Close();
        }
    }
}
