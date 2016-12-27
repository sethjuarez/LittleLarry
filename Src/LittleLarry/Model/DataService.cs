using SQLite;
using System;
using System.IO;

namespace LittleLarry.Model
{

    public class DataService : IDisposable
    {
        private string _path;
        private SQLiteConnection _connection;
        public DataService()
        {
            _path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path,
                                 "LittleLarryData.db");
            _connection = new SQLiteConnection(_path, 
                                 SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
            _connection.CreateTable<Data>();
        }

        public void Add(Data d)
        {
            _connection.Insert(d);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
