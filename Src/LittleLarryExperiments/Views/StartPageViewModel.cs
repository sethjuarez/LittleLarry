using LittleLarry.Model;
using LittleLarryExperiments.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LittleLarryExperiments.Views
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged([CallerMemberName] string name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private Connection _connection;
        public StartPageViewModel()
        {
            Sync = new DelegateCommand(LoadData);
            Model = new DelegateCommand(ModelData);
            DataCollection = new ObservableCollection<Data>();
            _connection = new Connection();
            _connection.Initialize();
        }
        

        public ICommand Sync { get; set; }
        public ICommand Model { get; set; }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Data> DataCollection { get; set; }

        private void LoadData(object o)
        {
            Status = "Loading Data...";
            DataCollection.Clear();
            DataService service = new DataService(_connection);
            foreach (var d in service.GetData(10000000))
                DataCollection.Add(d);
            OnPropertyChanged(nameof(DataCollection));
            Status = "Idle...";
        }


        private void ModelData(object obj)
        {
            var ml = new MachineLearningService(_connection);
            ml.Model();

            foreach(var data in DataCollection.Where(d => d.Speed >= 0))
            {
                (double speed, double turn) = ml.Predict(data);
                Console.WriteLine($"Speed: {speed}, Turn: {turn}");
            }
            
        }
    }
}

