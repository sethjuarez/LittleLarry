using LittleLarry.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LittleLarry.Model;
using Windows.Storage;

namespace LittleLarry.Models
{
    public class FakeDataService : IDataService
    {
        private List<Data> _data;
        public FakeDataService()
        {
            _data = new List<Data>();
        }

        public string DataPath => ApplicationData.Current.LocalFolder.Path;
        public int RecordCount => _data.Count;
        public void Initialize() => _data = new List<Data>();
        public IEnumerable<Data> GetData() => _data;
        public void Add(Data data) =>_data.Add(data);
        public void Save(Action progress = null)
        {
            return;
        }

        public void Clear() => _data.Clear();

        public void Close()
        {
            return;
        }

        public void Dispose()
        {
            return;
        }
    }
}
