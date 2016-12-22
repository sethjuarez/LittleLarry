using GHIElectronics.UWP.Shields;
using LittleLarry.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace LittleLarry.Views
{
    public class StartPageViewModel : INotifyPropertyChanged
    {
        private FEZHAT _hat;
        private DispatcherTimer _timer;
        private Controller _controller;
        private LightSensor _lightSensor;

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

        private void OnTick(object sender, object e)
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

            _lightSensor.Process();
            Ain1 = _lightSensor.Ain1Color;
            Ain2 = _lightSensor.Ain2Color;
            Ain3 = _lightSensor.Ain3Color;

            _controller.Process();
            X = _controller.X;
            Y = _controller.Y;

            if (Y < 0)
            {
                if (X < 0)
                {
                    _hat.MotorA.Speed = GetFloor(Y - X);
                    _hat.MotorB.Speed = GetFloor(Y);
                }
                else
                {
                    _hat.MotorA.Speed = GetFloor(Y);
                    _hat.MotorB.Speed = GetFloor(Y + X);
                }
            }
            else if (Y > 0)
            {
                if (X < 0)
                {
                    _hat.MotorA.Speed = GetFloor(Y + X);
                    _hat.MotorB.Speed = GetFloor(Y);
                }
                else
                {
                    _hat.MotorA.Speed = GetFloor(Y);
                    _hat.MotorB.Speed = GetFloor(Y - X);
                }
            }
            else
            {
                _hat.MotorA.Speed = 0;
                _hat.MotorB.Speed = 0;
            }
        }
    }
}
