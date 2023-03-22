using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FormDesign
{
    internal class ConnectionService
    {
        public static MySqlConnection GetConnection()
        {
            MySqlConnection connection = null;

            try
            {
                string sConnstring = "server = localhost;database=db_sekolah;uid=root;passwordd=;";
                connection = new MySqlConnection(sConnstring);
            }
            catch (MySqlException sqlex)
            {
                throw new Exception(sqlex.Message.ToString());
            }
            return connection;
        }
    }
}
