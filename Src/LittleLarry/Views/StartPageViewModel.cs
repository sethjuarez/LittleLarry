using GHIElectronics.UWP.Shields;
using LittleLarry.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using System.Linq;
using LittleLarry.Hardware;

namespace LittleLarry.Views
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        private FEZHAT _hat;
        private DispatcherTimer _timer;
        private Controller _controller;
        private LightSensor _lightSensor;
        private ButtonSensor _buttonSensor;
        private DataService _dataService;
        private MachineLearningService _mlService;

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged([CallerMemberName] string name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public StartPageViewModel()
        {
            SetupAsync();
        }

        private async void SetupAsync()
        {
            CurrentMode = Mode.Idle;
            _hat = await FEZHAT.CreateAsync();
            _controller = new Controller();
            _lightSensor = new LightSensor(_hat);
            _buttonSensor = new ButtonSensor(_hat);
            _dataService = new DataService();
            _mlService = new MachineLearningService();

            _timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            _timer.Tick += OnTick;
            _timer.Start();
        }

        private int _sum;
        public int Sum
        {
            get { return _sum; }
            set
            {
                if (_sum != value)
                {
                    _sum = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _arr;
        public string Arr
        {
            get { return _arr; }
            set
            {
                if (_arr != value)
                {
                    _arr = value;
                    OnPropertyChanged();
                }
            }
        }


        private Windows.UI.Color _ain1;
        public Windows.UI.Color Ain1
        {
            get { return _ain1; }
            set
            {
                if (_ain1 != value)
                {
                    _ain1 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Windows.UI.Color _ain2;
        public Windows.UI.Color Ain2
        {
            get { return _ain2; }
            set
            {
                if (_ain2 != value)
                {
                    _ain2 = value;
                    OnPropertyChanged();
                }
            }
        }

        private Windows.UI.Color _ain3;
        public Windows.UI.Color Ain3
        {
            get { return _ain3; }
            set
            {
                if (_ain3 != value)
                {
                    _ain3 = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _speed;
        public int Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                {
                    _speed = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _turn;
        public int Turn
        {
            get { return _turn; }
            set
            {
                if (_turn != value)
                {
                    _turn = value;
                    OnPropertyChanged();
                }
            }
        }

        private Mode _currentMode;
        private Mode CurrentMode
        {
            get { return _currentMode; }
            set
            {
                if (_currentMode != value)
                {
                    _currentMode = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnTick(object sender, object e)
        {
            // get mode
            _buttonSensor.Process();
            if (CurrentMode != _buttonSensor.Mode)
            {
                CurrentMode = _buttonSensor.Mode;
                SetModeIndicators(CurrentMode);
            }

            var data = GetData();

            Ain1 = _lightSensor.ToColor(data.Ain1);
            Ain2 = _lightSensor.ToColor(data.Ain2);
            Ain3 = _lightSensor.ToColor(data.Ain3);

            Speed = data.Speed;
            Turn = data.Turn;


            if (CurrentMode == Mode.Model)
            {
                _mlService.Model(_dataService.GetData(100000).ToArray());
                _buttonSensor.SetIdle();
            }
            else if (CurrentMode == Mode.Auto)
            {
                if (_mlService.HasModel())
                    data = _mlService.Predict(data);
                else
                    _buttonSensor.SetIdle();
            }
            else
                Drive(data);
        }

        private void Drive(Data data)
        {
            (var speedA, var speedB) = _controller.Convert(data.Speed, data.Turn);
            _hat.MotorA.Speed = speedA;
            _hat.MotorB.Speed = speedB;
        }

        private void SetModeIndicators(Mode mode)
        {
            // current mode
            switch (mode)
            {
                case Mode.Idle:
                    _hat.D2.TurnOff();
                    _hat.D3.TurnOff();
                    break;
                case Mode.Learn:
                    _hat.D2.Color = FEZHAT.Color.Red;
                    _hat.D3.Color = FEZHAT.Color.Red;
                    break;
                case Mode.Model:
                    _hat.D2.Color = FEZHAT.Color.Blue;
                    _hat.D3.Color = FEZHAT.Color.Blue;
                    break;
                case Mode.Auto:
                    _hat.D2.Color = FEZHAT.Color.Green;
                    _hat.D3.Color = FEZHAT.Color.Green;
                    break;
            }
        }

        private Data GetData()
        {
            Data data = new Data();

            // handle line tracking sensors
            (double ain1, double ain2, double ain3) = _lightSensor.GetValues();
            data.Ain1 = ain1;
            data.Ain2 = ain2;
            data.Ain3 = ain3;

            // handle accelerometer values;
            _hat.GetAcceleration(out double x, out double y, out double z);
            data.AccelerationX = x;
            data.AccelerationY = y;
            data.AccelerationZ = z;

            // handle controller values
            (int speed, int turn) = _controller.GetValues();
            data.Speed = speed;
            data.Turn = turn;

            return data;
        }

    }
}
