using System;
using System.Windows.Forms;
using WinterGarden.Data;

namespace WinterGarden
{
    public partial class RW_clean : Form
    {
        public RW_clean()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
