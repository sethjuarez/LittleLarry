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
using Windows.Gaming.Input;

namespace LittleLarry.Views
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        private FEZHAT _hat;
        private DispatcherTimer _timer;
        private Motor _motor;

        //private ButtonSensor _buttonSensor;
        //private Controller _controller;

        private Controls _controls;
        private LightSensor _lightSensor;
        
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

            _controls = new Controls(_hat);
            _lightSensor = new LightSensor(_hat);
            _motor = new Motor(_hat);

            _dataService = new DataService(connection);
            _mlService = new MachineLearningService(connection);

            CurrentMode = Mode.Idle;
            SetModeIndicators(CurrentMode);

            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += Process;
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

        private double _leftTrigger;
        public double LeftTrigger
        {
            get { return _leftTrigger; }
            set
            {
                if(_leftTrigger != value)
                {
                    _leftTrigger = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _rightTrigger;
        public double RightTrigger
        {
            get { return _rightTrigger; }
            set
            {
                if (_rightTrigger != value)
                {
                    _rightTrigger = value;
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


        private Mode GetProcessedMode()
        {
            // cool off period to prevent toggling
            if (_controls.TimeSinceMark.Milliseconds > 150)
            {
                _controls.MarkTime();

                if (_controls.IsButtonPushed(GamepadButtons.B) || _controls.DIO18Pressed)
                    return CurrentMode == Mode.Learn ? Mode.Idle : Mode.Learn;
                else if (_controls.IsButtonPushed(GamepadButtons.Y) || _controls.DIO22Pressed)
                    return CurrentMode == Mode.Auto ? Mode.Idle : Mode.Auto;
                else if (_controls.IsButtonPushed(GamepadButtons.X))
                {
                    _dataService.ClearData();
                    return Mode.Idle;
                }
                else
                    return CurrentMode;

            }
            else return CurrentMode;
        }

        private void ExitLearnMode()
        {
            bool on = false;
            _dataService.Save(() =>
            {
                _hat.D3.Color = on ? FEZHAT.Color.Red : FEZHAT.Color.White;
                on = !on;
            });

            _hat.D3.Color = FEZHAT.Color.Blue;

            _mlService.Model();

            _hat.D2.TurnOff();
            _hat.D3.TurnOff();
        }

        private void Process(object sender, object e)
        {
            // process peripherals
            _controls.Process();

            // get mode status changes
            var mode = GetProcessedMode();

            // ------- mode guards
            // can't do auto mode without models
            if (mode == Mode.Auto && !_mlService.HasModel())
                mode = Mode.Idle;

            // exit learn mode - persist and make models
            if (CurrentMode == Mode.Learn && mode != Mode.Learn)
                ExitLearnMode();

            Count = _dataService.RecordCount;
            CurrentMode = mode;
            SetModeIndicators(CurrentMode);

            // ------ process under current mode
            var data = GetSensorData();

            // handle controller values
            double speed = 0;
            double turn = 0;

            if (_controls.IsButtonPushed(GamepadButtons.A))
                speed = 0.4;

            if (_controls.LeftTrigger > 0)
                turn = -0.6;
            else if (_controls.RightTrigger > 0)
                turn = 0.6;
            else
                turn = 0;

            data.Speed = speed;
            data.Turn = turn;

            // only learn on simple drive mechanism
            if (CurrentMode == Mode.Learn)
                _dataService.Add(data);
            if (CurrentMode == Mode.Auto)
                (speed, turn) = _mlService.Predict(data);

            Speed = speed;
            Turn = turn;

            _motor.Drive(Speed, Turn);

            Ain1 = _lightSensor.ToColor(data.Ain1);
            Ain2 = _lightSensor.ToColor(data.Ain2);
            Ain3 = _lightSensor.ToColor(data.Ain3);

            RightTrigger = _controls.RightTrigger;
            LeftTrigger = _controls.LeftTrigger;
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
                case Mode.Auto:
                    _hat.D2.Color = FEZHAT.Color.Yellow;
                    _hat.D3.Color = FEZHAT.Color.Yellow;
                    break;
            }
        }

        private Data GetSensorData()
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

            return data;
        }

    }
}
