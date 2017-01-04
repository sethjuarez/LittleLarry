using LittleLarry.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using Windows.Storage;

namespace LittleLarryExperiments.Services
{
    public class Connection : IConnection
    {
        private const string DB_NAME = "LittleLarryData.db";

        public string DataPath => ApplicationData.Current.LocalFolder.Path;

        public SQLiteConnection SQLiteConnection { get; private set; }

        public SQLiteConnection Initialize()
        {
            SQLiteConnection = new SQLiteConnection(
                  Path.Combine(DataPath, DB_NAME),
                  SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
            return SQLiteConnection;
        }
    }
}
