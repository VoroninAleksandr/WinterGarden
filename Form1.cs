using System;
using System.Windows.Forms;
using WinterGarden.Data;

namespace WinterGarden
{
    public partial class Form1 : Form
    {
        Person person;
        public Form1()
        {
            InitializeComponent();
            person = new Person(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                person.ShowAll();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddWorker workerForm = new AddWorker();
            workerForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CleanWorkForm cleanWork = new CleanWorkForm();
            cleanWork.ShowDialog();
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            person.EditInfo();
        }

        private void SortComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            person.Sort();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            person.EditInfo();
        }
    }
}
