using LibflClassLibrary.Readers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibflClassLibrary.Controls.Readers
{
    public partial class fReaderRegistrationAndRights : Form
    {
        //public fReaderRegistrationAndRights()
        //{
        //    InitializeComponent();
        //}
        public fReaderRegistrationAndRights(int NumberReader)
        {
            InitializeComponent();
            ReaderInfo reader = ReaderInfo.GetReader(NumberReader);
            label1.Text = "Регистрационные данные читателя №" + reader.NumberReader + ". " + reader.FamilyName + " " + reader.Name + " " + reader.FatherName;

        }

    }
}
