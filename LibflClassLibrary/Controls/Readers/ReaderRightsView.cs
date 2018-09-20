using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LibflClassLibrary.Readers.ReadersRight;

namespace LibflClassLibrary.Controls.Readers
{
    public partial class ReaderRightsView : UserControl
    {
        public ReaderRightsView()
        {
            InitializeComponent();
        }

        public void Init(int NumberReader)
        {
            lvRights.Items.Clear();
            ReaderRightsInfo rights = ReaderRightsInfo.GetReaderRights(NumberReader);
            lvRights.Items.Clear();
            foreach (var right in rights.RightsList)
            {
                lvRights.Items.Add(right.ToString());
            }

        }
    }
}
