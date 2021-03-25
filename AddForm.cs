using System;
using System.Windows.Forms;
using WinterGarden.Data;

namespace WinterGarden
{
    public partial class AddForm : Form
    {
        Person person = new Person();
        public AddForm()
        {
            InitializeComponent();
            PayMaskedTextBox.Text = "0";
        }

        private void OKbutton_Click(object sender, EventArgs e)
        {
            person.AddInfoPerson(NameTextBox, SurnameTextBox, PhoneMaskedTextBox, NumberMaskedTextBox, InfoTextBox, PayMaskedTextBox);
            DialogResult = DialogResult.OK;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void AddForm_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                SelectNextControl(ActiveControl, true, true, true, true);
            }
        }

    }
}
