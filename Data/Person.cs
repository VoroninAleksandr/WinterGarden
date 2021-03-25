using System;
using System.Linq;
using System.Windows.Forms;


namespace WinterGarden.Data
{
    class Person
    {
        private readonly DataGridView _dataGridView;
        private readonly ComboBox _comboBox;
        public Person() { }
        public Person(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            ShowAll();
        }
        private void ColumnName()
        {
            _dataGridView.Columns[0].HeaderText = "ID";
            _dataGridView.Columns[1].HeaderText = "Имя";
            _dataGridView.Columns[2].HeaderText = "Фамилия";
            _dataGridView.Columns[3].HeaderText = "Телефон";
            _dataGridView.Columns[4].HeaderText = "Номер участка";
            _dataGridView.Columns[5].HeaderText = "Примечание";
            _dataGridView.Columns[6].HeaderText = "Сумма, руб.";
            _dataGridView.Columns[7].HeaderText = "Дата платежа";
        }
        public void ShowAll()
        {
            try
            {
                using (WG_db connect = new WG_db())
                {
                    _dataGridView.DataSource = connect.people.ToList();
                    ColumnName();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public void AddInfoPerson(TextBox name, TextBox surname, MaskedTextBox phone, MaskedTextBox number, TextBox info, MaskedTextBox pay)
        {
            if (name.Text != "" && surname.Text != "")
            {
                using (WG_db connect = new WG_db())
                {
                    var obj = connect.people.Where((x) => x.name == name.Text && x.surname == surname.Text).FirstOrDefault();
                    if (obj == null)
                    {
                        people people = new people();
                        people.name = name.Text;
                        people.surname = surname.Text;
                        people.phone = phone.Text;
                        people.namber = number.Text;
                        people.note = info.Text;
                        people.summa = int.Parse(pay.Text);
                        people.payment_date = DateTime.Now.ToShortDateString();
                        connect.people.Add(people);
                        connect.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("Запись есть в базе!!!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Поля Имя и Фамилия обязательны к заполнению");
            }

        }
        private people VerifyDate(string name, string surname)
        {
            using (WG_db connect = new WG_db())
            {
                var obj = (from x in connect.people
                           where x.name == name && x.surname == surname
                           select x).FirstOrDefault();
                return obj;
            }
        }
        public void EditInfo()
        {
            EditPersonWorm ePerson = new EditPersonWorm();
            try
            {
                int index = _dataGridView.SelectedRows[0].Index + 1;
                using (WG_db connect = new WG_db())
                {
                    people people = connect.people.Find(index);
                    if (people != null)
                    {
                        ePerson.textBox1.Text = people.name;
                        ePerson.textBox2.Text = people.surname;
                        ePerson.textBox3.Text = people.note;
                        ePerson.maskedTextBox1.Text = people.phone;
                        ePerson.maskedTextBox2.Text = people.namber;
                        ePerson.maskedTextBox3.Text = people.summa.ToString();
                        if (ePerson.ShowDialog() == DialogResult.Cancel) return;
                        if (VerifyDate(ePerson.textBox1.Text, ePerson.textBox2.Text) == null ||
                            VerifyDate(ePerson.textBox1.Text, ePerson.textBox2.Text).id == people.id)
                        {
                            people.name = ePerson.textBox1.Text;
                            people.surname = ePerson.textBox2.Text;
                            people.note = ePerson.textBox3.Text;
                            people.phone = ePerson.maskedTextBox1.Text;
                            people.namber = ePerson.maskedTextBox2.Text;
                            people.summa = int.Parse(ePerson.maskedTextBox3.Text);
                            connect.SaveChanges();
                            ShowAll();
                            MessageBox.Show("Запись успешно обновлена.");
                        }
                        else { MessageBox.Show("Ошибка. Такая запись уже есть в базе!!!."); }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Sort()
        {
            //using (WG_db connect = new WG_db())
            //{
            //    if (_comboBox.SelectedItem.Equals("1. по ID"))
            //    {
            //        _dataGridView.Columns.Clear();
            //        _dataGridView.DataSource = (connect.people.Select((x) => x)).OrderBy((y) => y.id).ToList();
            //    }
            //    else if (_comboBox.SelectedItem.Equals("2.по имени"))
            //    {
            //        _dataGridView.Columns.Clear();
            //        var obj = connect.people.OrderBy((x) => x.name);
            //        var obj2 = from x in obj
            //                   select x;
            //        connect.Entry(obj2).State = System.Data.Entity.EntityState.Modified;
            //        connect.SaveChanges();
            //        _dataGridView.DataSource = obj2.ToList();
            //    }
            //    else if (_comboBox.SelectedItem.Equals("3.по фамилии"))
            //    {
            //        _dataGridView.DataSource = (connect.people.Select((x) => x).ToList()).OrderBy((x) => x.name);
            //    }
            //    else if (_comboBox.SelectedItem.Equals("4.по телефону"))
            //    {
            //        _dataGridView.DataSource = (connect.people.Select((x) => x)).OrderBy((y) => y.phone).ToList();
            //    }
            //    else if (_comboBox.SelectedItem.Equals("5.по номеру участка"))
            //    {
            //        _dataGridView.DataSource = (connect.people.Select((x) => x)).OrderBy((y) => y.namber).ToList();
            //    }
            //    else
            //    {
            //        _dataGridView.DataSource = (connect.people.Select((x) => x)).OrderBy((y) => y.payment_date).ToList();
            //    }
            //}
        }
    }
}