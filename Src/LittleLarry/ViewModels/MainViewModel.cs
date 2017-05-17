using System;
using GalaSoft.MvvmLight;
using LittleLarry.Helpers;
using System.Collections.Generic;
using LittleLarry.Model;
using Windows.UI.Xaml;
using LittleLarry.Model.Hardware;

namespace LittleLarry.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private double _leftTrigger;
        public double LeftTrigger
        {
            get { return _leftTrigger; }
            set
            {
                if (_leftTrigger != value)
                {
                    _leftTrigger = value;
                    RaisePropertyChanged();
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
                    RaisePropertyChanged();
                }
            }
        }

        private State _currentState;
        public State CurrentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState != value)
                {
                    _currentState = value;
                    RaisePropertyChanged();
                }
            }
        }



        private string _buttons;
        public string Buttons
        {
            get { return _buttons; }
            set
            {
                if (_buttons != value)
                {
                    _buttons = value;
                    RaisePropertyChanged();
                }
            }
        }

        

        private Device _device;
        private DispatcherTimer _timer;
        public MainViewModel(Device device)
        {
            _device = device;
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(100) };
            _timer.Tick += (s, e) =>
            {
                _device.Process();
                CurrentState = _device.CurrentState;
                Ain1 = Motor.ToColor(_device.CurrentData.Ain1);
                Ain2 = Motor.ToColor(_device.CurrentData.Ain2);
                Ain3 = Motor.ToColor(_device.CurrentData.Ain3);
                LeftTrigger = _device.Controls.LeftTrigger;
                RightTrigger = _device.Controls.RightTrigger;
                Buttons = _device.Controls.GetGamePadButtons();
                Speed = _device.CurrentData.Speed;
                Turn = _device.CurrentData.Turn;
                Count = _device.RecordCount;
            };
            _timer.Start();
        }
    }
}
