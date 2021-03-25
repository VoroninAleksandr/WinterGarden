using System;
using System.Windows.Forms;
using WinterGarden.Data;

namespace WinterGarden
{
    public partial class AddWorker : Form
    {
        Workers worker;
        bool index = false;
        public AddWorker()
        {
            InitializeComponent();
            worker = new Workers(WorkerDataGridView);
            worker.CreateTableAdapter();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (index)
            {
                if (MessageBox.Show("СОХРАНИТЬ?", "У вас имеются несохраненные данные", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    worker.InsertCommand();
                    index = false;
                }
                else
                {
                    DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            worker.InsertCommand();
            index = false;
        }

        private void WorkerDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            index = true;
        }
    }
}
