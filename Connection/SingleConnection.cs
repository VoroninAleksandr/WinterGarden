using System.Data.SqlClient;

namespace WinterGarden.Connection
{
    static class SingleConnection
    {
        private static SqlConnection _sConnection;
        public static SqlConnection NewConnection()
        {
            if(_sConnection == null)
            {
                _sConnection = new SqlConnection(@"Data Source=ALEX-MAIN\SQLEXPRESS; Initial catalog=WinterGarden; Integrated security=SSPI;");
            }
            return _sConnection;
        }
    }
}
