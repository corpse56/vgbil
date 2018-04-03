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
    public partial class ViewFullSizePhoto : Form
    {
        public ViewFullSizePhoto(Image image)
        {
            InitializeComponent();
            pictureBox1.Image = image;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void ViewFullSizePhoto_Load(object sender, EventArgs e)
        {
            this.Size = pictureBox1.Image.Size;

        }
    }
}
