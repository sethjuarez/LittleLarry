using numl;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using SQLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LittleLarry.Model
{
    public class DataService : IDisposable
    {
        private IList<Data> _data;
        private SQLiteConnection _connection;
        public DataService(IConnection connection)
        {
            _data = new List<Data>();
            _connection = connection.Initialize();
            _connection.CreateTable<Data>();
            RecordCount = _connection.Table<Data>().Count();
        }

        public void Add(Data d)
        {
            if (d.Speed > 0)
                _data.Add(d);
        }

        public void ClearData()
        {
            if (RecordCount > 0)
            {
                _connection.Table<Data>().Delete(d => d.Speed > 0);
                RecordCount = _connection.Table<Data>().Count();
            }
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

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public IEnumerable<Data> GetData(int total)
        {
            return _connection.Table<Data>()
                              .OrderByDescending(d => d.Id)
                              .Take(total);
        }

        public int RecordCount { get; private set; }
    }
}
