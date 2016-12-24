using GHIElectronics.UWP.Shields;
using LittleLarry.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using System.Linq;

namespace LittleLarry.Views
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        private FEZHAT _hat;
        private DispatcherTimer _timer;
        private Controller _controller;
        private LightSensor _lightSensor;
        private Mode _currentMode = Mode.Idle;

        public event PropertyChangedEventHandler PropertyChanged;
        internal void OnPropertyChanged([CallerMemberName] string name = null) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public StartPageViewModel()
        {
            Setup();
        }

        private async void Setup()
        {
            _hat = await FEZHAT.CreateAsync();
            _controller = new Controller();
            _lightSensor = new LightSensor(_hat);

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
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

        private int _x;
        public int X
        {
            get { return _x; }
            set
            {
                if (_x != value)
                {
                    _x = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _y;
        public int Y
        {
            get { return _y; }
            set
            {
                if (_y != value)
                {
                    _y = value;
                    OnPropertyChanged();
                }
            }
        }


        Queue<Mode> _buttons = new Queue<Mode>(10);
        private void OnTick(object sender, object e)
        {
            // handle button pushes
            if (_hat.IsDIO18Pressed() && _hat.IsDIO22Pressed()) _buttons.Enqueue(Mode.Auto);
            else if (_hat.IsDIO18Pressed()) _buttons.Enqueue(Mode.Learn);
            else if (_hat.IsDIO22Pressed()) _buttons.Enqueue(Mode.Model);
            else _buttons.Enqueue(Mode.Idle);
            if (_buttons.Count > 10) _buttons.Dequeue();

            Count = _buttons.Count;
            Arr = $"[{string.Join(", ", _buttons.ToArray())}]";

            if (Count > 9 && _buttons.All(m => m == Mode.Auto))
            {
                _currentMode = _currentMode == Mode.Auto ? Mode.Idle : Mode.Auto;
                _buttons.Clear();
            }
            else if (Count > 9 && _buttons.All(m => m == Mode.Learn))
            {
                _currentMode = _currentMode == Mode.Learn ? Mode.Idle : Mode.Learn;
                _buttons.Clear();
            }
            else if (Count > 9 && _buttons.All(m => m == Mode.Model))
            {
                _currentMode = _currentMode == Mode.Model ? Mode.Idle : Mode.Model;
                _buttons.Clear();
            }



            // current mode
            switch (_currentMode)
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



            // handle line tracking sensors
            (double ain1, double ain2, double ain3) = _lightSensor.GetValues();

            Ain1 = _lightSensor.ToColor(ain1);
            Ain2 = _lightSensor.ToColor(ain2);
            Ain3 = _lightSensor.ToColor(ain3);

            // handle accelerometer values;
            _hat.GetAcceleration(out double x, out double y, out double z);

            // handle controller values
            (X, Y) = _controller.GetValues();
            (var speedA, var speedB) = ConvertControllerToSpeed(X, Y);
            _hat.MotorA.Speed = speedA;
            _hat.MotorB.Speed = speedB;
        }

        private (double SpeedA, double SpeedB) ConvertControllerToSpeed(int x, int y)
        {
            double GetFloor(int num)
            {
                double n = (double)num / 10;
                if (n < -1)
                    return -1;
                else if (n > 1)
                    return 1;
                else
                    return n;
            }

            double speedA, speedB = 0;

            if (y < 0)
            {
                if (x < 0)
                {
                    speedA = GetFloor(y - x);
                    speedB = GetFloor(y);
                }
                else
                {
                    speedA = GetFloor(y);
                    speedB = GetFloor(y + x);
                }
            }
            else if (y > 0)
            {
                if (X < 0)
                {
                    speedA = GetFloor(y + x);
                    speedB = GetFloor(y);
                }
                else
                {
                    speedA = GetFloor(y);
                    speedB = GetFloor(y - x);
                }
            }
            else
            {
                speedA = 0;
                speedB = 0;
            }


            return (speedA, speedB);
        }
    }
}
