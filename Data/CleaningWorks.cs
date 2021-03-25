using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinterGarden.Connection;

namespace WinterGarden.Data
{
    class CleaningWorks
    {
        private readonly WG_db _connect;
        private SqlDataAdapter _adapter;
        private DataTable _cleanTable;
        private readonly DataGridView _dataGridView;
        private readonly Label _cleansumm;
        private readonly Label _paysumm;
        private readonly Label _itogsumm;
        private readonly MaskedTextBox _maskedTextBox;
        private readonly DateTimePicker _dateTimePicker;
        private readonly TextBox _textBox;
        private readonly ComboBox _comboBox;
        public CleaningWorks(DataGridView dataGridView, Label clean, Label pay, Label itog, MaskedTextBox maskedTextBox,
            DateTimePicker dateTimePicker, TextBox textBox, ComboBox comboBox)
        {
            SingleConnection.NewConnection();
            _connect = new WG_db();
            _connect.cleaning_works.Load();
            _cleanTable = new DataTable();
            _dataGridView = dataGridView;
            _maskedTextBox = maskedTextBox;
            _dateTimePicker = dateTimePicker;
            _textBox = textBox;
            _comboBox = comboBox;
            _cleansumm = clean;
            _paysumm = pay;
            _itogsumm = itog;
            Summ();
        }
        public void CreateTableAdapter()
        {
            try
            {
                string tmp = @"SELECT C.id as [ID], C.date as [Дата платежа], C.cost as [Сумма], W.name as [Водитель], C.note as [Примечание]
                            FROM cleaning_works AS C, workers AS W WHERE C.fk=W.id;";
                _cleanTable.Clear();
                _adapter = new SqlDataAdapter(tmp, SingleConnection.NewConnection());
                _adapter.Fill(_cleanTable);
                _dataGridView.DataSource = _cleanTable;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Summ()
        {
            try
            {
                var obj = _connect.cleaning_works.Sum((x) => x.cost);
                _cleansumm.Text = obj.ToString();
                if (_cleansumm.Text == null) { _cleansumm.Text = "0"; }
                var obj1 = _connect.people.Sum((x) => x.summa);
                _paysumm.Text = obj1.ToString();
                if (_paysumm.Text == null) { _paysumm.Text = "0"; }
                _itogsumm.Text = (int.Parse(_paysumm.Text) - int.Parse(_cleansumm.Text)).ToString();
                _ = int.Parse(_itogsumm.Text) < 0 ? _itogsumm.ForeColor = Color.Red : _itogsumm.ForeColor = Color.Green;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void ShowDriver(ComboBox comboBox)
        {
            try
            {
                using (WG_db connect = new WG_db())
                {
                    comboBox.DataSource = connect.workers.Select((x) => x.name).ToList();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private cleaning_works VerifyDate(string date)
        {
            var obj = (from x in _connect.cleaning_works.Local
                       where x.date == date
                       select x).FirstOrDefault();
            return obj;
        }
        public void AddCleanWorks()
        {
            try
            {
                _connect.cleaning_works.Load();
                if (VerifyDate(_dateTimePicker.Value.ToShortDateString().ToString()) == null)
                {
                    cleaning_works obj = new cleaning_works();
                    if (_maskedTextBox.Text == "") _maskedTextBox.Text = "0";
                    obj.cost = int.Parse(_maskedTextBox.Text);
                    obj.date = _dateTimePicker.Value.ToShortDateString().ToString();
                    obj.note = _textBox.Text;
                    obj.fk = (_comboBox.SelectedIndex + 1);
                    _connect.cleaning_works.Add(obj);
                    _connect.SaveChanges();
                    Summ();
                    CreateTableAdapter();
                    MessageBox.Show("Запись успешно добавлена.");
                    _maskedTextBox.Text = "0";
                    _textBox.Clear();
                }
                else
                {
                    MessageBox.Show("Ошибка. На дату " + _dateTimePicker.Value.ToShortDateString().ToString() + " уже есть запись.");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void Edit()
        {
            RW_clean eForm = new RW_clean();
            try
            {
                int index = _dataGridView.SelectedRows[0].Index + 1;
                _connect.cleaning_works.Load();
                cleaning_works obj = _connect.cleaning_works.Find(index);
                if (obj != null)
                {
                    eForm.maskedTextBox1.Text = obj.cost.ToString();
                    eForm.dateTimePicker1.Text = obj.date;
                    eForm.textBox1.Text = obj.note;
                    ShowDriver(eForm.comboBox1);
                    eForm.comboBox1.SelectedIndex = (int)obj.fk - 1;
                    if (eForm.ShowDialog() == DialogResult.Cancel) { return; }
                    if (VerifyDate(eForm.dateTimePicker1.Value.ToShortDateString().ToString()) == null ||
                        VerifyDate(eForm.dateTimePicker1.Value.ToShortDateString().ToString()).id == obj.id)
                    {
                        obj.cost = int.Parse(eForm.maskedTextBox1.Text);
                        obj.date = eForm.dateTimePicker1.Value.ToShortDateString().ToString();
                        obj.note = eForm.textBox1.Text;
                        obj.fk = (eForm.comboBox1.SelectedIndex + 1);
                        _connect.SaveChanges();
                        CreateTableAdapter();
                        Summ();
                        MessageBox.Show("Запись обновлена успешно.");
                    }
                    else { MessageBox.Show("Ошибка. На дату " + eForm.dateTimePicker1.Value.ToShortDateString().ToString() + " уже есть запись."); }
                }
                else { MessageBox.Show("Ошибка. Запись не существует!"); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}