using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model.Services
{
    public interface IDataService : IDisposable
    {
        void Initialize();
        int RecordCount { get; }
        void Clear();
        void Add(Data data);
        void Save(Action progress = null);
        void Close();
        IEnumerable<Data> GetData();
        string DataPath { get; }
    }
}
