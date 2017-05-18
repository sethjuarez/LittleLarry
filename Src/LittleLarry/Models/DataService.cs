using LittleLarry.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleLarry.Model;
using Windows.Storage;
using SQLite;
using System.IO;

namespace LittleLarry.Models
{
    public class DataService : IDataService
    {
        public string DataPath => ApplicationData.Current.LocalFolder.Path;
        public int RecordCount { get; private set; } = 0;

        private const string DB_NAME = "LittleLarryData.db";
        private IList<Data> _data;
        private SQLiteConnection _connection;
        public DataService()
        {
            _data = new List<Data>();
            Initialize();
            _connection.CreateTable<Data>();
            RecordCount = _connection.Table<Data>().Count();
        }

        public void Add(Data data)
        {
            if (data.Speed > 0)
                _data.Add(data);
        }

        public void Clear()
        {
            if (RecordCount > 0)
            {
                _connection.Table<Data>().Delete(d => d.Speed > 0);
                RecordCount = _connection.Table<Data>().Count();
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public IEnumerable<Data> GetData()
        {
            return _connection.Table<Data>()
                              .OrderByDescending(d => d.Id);
        }

        public void Initialize()
        {
            string database = Path.Combine(DataPath, DB_NAME);
            if (!File.Exists(database))
                _connection = new SQLiteConnection(
                    database,
                    SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite);
            else
                _connection = new SQLiteConnection(
                    database, SQLiteOpenFlags.ReadWrite);
        }

        public void Save(Action progress = null)
        {
            foreach (var d in _data)
            {
                _connection.Insert(d);
                progress?.Invoke();
            }
            _data.Clear();
            RecordCount = _connection.Table<Data>().Count();
        }
    }
}
