using GHIElectronics.UWP.Shields;
using LittleLarry.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using System.Linq;
using LittleLarry.Hardware;
using LittleLarry.Services;

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
            _hat = await FEZHAT.CreateAsync();

            var connection = new Connection();
            _controller = new Controller();
            _lightSensor = new LightSensor(_hat);
            _buttonSensor = new ButtonSensor(_hat);
            _dataService = new DataService(connection);
            _mlService = new MachineLearningService(connection);

            CurrentMode = Mode.Idle;
            SetModeIndicators(CurrentMode);

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

        private double _speed;
        public double Speed
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

        private double _turn;
        public double Turn
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
        public Mode CurrentMode
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

        bool _on = true;
        private void OnTick(object sender, object e)
        {
            // get mode
            _buttonSensor.Process();
            if (CurrentMode != _buttonSensor.Mode)
            {
                // before we go out of Learn state,
                // persist data to a file
                if (CurrentMode == Mode.Learn)
                {
                    _dataService.Save(() =>
                    {
                        _hat.D3.Color = _on ? FEZHAT.Color.Red : FEZHAT.Color.White;
                        _on = !_on;
                    });

                    _buttonSensor.SetMode(Mode.Model);
                }

                CurrentMode = _buttonSensor.Mode;
                SetModeIndicators(CurrentMode);
            }

            var data = GetData();

            switch (CurrentMode)
            {
                case Mode.Learn:
                    _dataService.Add(data);
                    Drive(data);
                    break;
                case Mode.Model:
                    _mlService.Model();
                    _buttonSensor.SetIdle();
                    break;
                case Mode.Auto:
                    if (_mlService.HasModel())
                    {
                        (double speed, double turn) = _mlService.Predict(data);
                        Speed = speed;
                        Turn = turn;
                        Drive(speed, turn);
                    }
                    else
                        _buttonSensor.SetIdle();
                    break;
                case Mode.Idle:
                    Drive(data);
                    break;

            }

            Ain1 = _lightSensor.ToColor(data.Ain1);
            Ain2 = _lightSensor.ToColor(data.Ain2);
            Ain3 = _lightSensor.ToColor(data.Ain3);

            Speed = data.Speed;
            Turn = data.Turn;

        }
        
        private void Drive(Data data)
        {
            Drive(data.Speed, data.Turn);
        }

        private void Drive(double speed, double turn)
        {
            (var speedA, var speedB) = _controller.Convert(speed, turn);
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
            (double speed, double turn) = _controller.GetValues();
            data.Speed = speed;
            data.Turn = turn;

            return data;
        }

    }
}
