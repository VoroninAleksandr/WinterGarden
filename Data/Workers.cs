using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using WinterGarden.Connection;

namespace WinterGarden.Data
{
    class Workers
    {
        private SqlDataAdapter _adapter;
        private DataTable _workerTable;
        private DataGridView _dataGridView;
        public Workers(DataGridView dataGridView)
        {
            SingleConnection.NewConnection();
            _workerTable = new DataTable();
            _dataGridView = dataGridView;
        }
        public void CreateTableAdapter()
        {
            _workerTable.Clear();
            _adapter = new SqlDataAdapter(@"SELECT* FROM workers", SingleConnection.NewConnection());
            _adapter.Fill(_workerTable);
            _dataGridView.DataSource = _workerTable;
        }
        public void InsertCommand()
        {
            try
            {
                SqlCommand _insert = new SqlCommand(@"INSERT INTO workers (name, phone ) VALUES (@name, @phone);", SingleConnection.NewConnection());
                _insert.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100));
                _insert.Parameters.Add(new SqlParameter("@phone", SqlDbType.VarChar, 16));
                _insert.Parameters["@name"].SourceVersion = DataRowVersion.Current;
                _insert.Parameters["@phone"].SourceVersion = DataRowVersion.Current;
                _insert.Parameters["@name"].SourceColumn = "name";
                _insert.Parameters["@phone"].SourceColumn = "phone";
                _adapter.InsertCommand = _insert;
                SqlCommandBuilder _builder = new SqlCommandBuilder(_adapter);
                _adapter.Update(_workerTable);
                CreateTableAdapter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
