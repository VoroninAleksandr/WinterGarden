using System;
using System.Windows.Forms;
using WinterGarden.Data;

namespace WinterGarden
{
    public partial class CleanWorkForm : Form
    {
        CleaningWorks cleanWork;
        public CleanWorkForm()
        {
            InitializeComponent();
            cleanWork = new CleaningWorks(CleanDataGridView, SumLabel, SumPayLabel, ItogLabel, CosteMaskedTextBox, dateTimePicker,
                NoteTextBox, DriverNameComboBox);
            cleanWork.CreateTableAdapter();
            cleanWork.ShowDriver(DriverNameComboBox);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            cleanWork.AddCleanWorks();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cleanWork.Edit();
        }
    }
}
