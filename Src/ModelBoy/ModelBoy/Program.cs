using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBoy
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=LittleLarry.db");
            connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Data", connection);
            var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            
        }
    }
}
