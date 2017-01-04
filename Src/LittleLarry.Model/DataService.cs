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
        }

        public void Add(Data d)
        {
            if (d.Speed > 0)
                _data.Add(d);
        }

        public void Save(Action progress = null)
        {
            foreach (var d in _data)
            {
                _connection.Insert(d);
                progress?.Invoke();
            }
            _data.Clear();
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
    }
}
