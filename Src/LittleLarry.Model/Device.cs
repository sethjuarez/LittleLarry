using LittleLarry.Model.Hardware;
using LittleLarry.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace LittleLarry.Model
{
    public class Device : IProcess
    {
        public State CurrentState { get; private set; }
        public Data CurrentData { get; private set; }

        private IFezHat _hat;
        private IDataService _dataService;
        public Controller Controls { get; private set; }
        private MachineLearningService _mlService;


        public Device(IFezHat hat, IDataService dataService)
        {
            _hat = hat;
            _dataService = dataService;
            Controls = new Controller();
            _mlService = new MachineLearningService(_dataService);
        }

        private State GetProcessedState()
        {
            // cool off period to prevent toggling
            if (Controls.TimeSinceMark.Milliseconds > 150)
            {
                Controls.MarkTime();

                if (Controls.IsButtonPushed(GamepadButtons.B) || _hat.DIO18Pressed)
                    return CurrentState == State.Learn ? State.Idle : State.Learn;
                else if (Controls.IsButtonPushed(GamepadButtons.Y) || _hat.DIO22Pressed)
                    return CurrentState == State.Auto ? State.Idle : State.Auto;
                else if (Controls.IsButtonPushed(GamepadButtons.X))
                {
                    _dataService.Clear();
                    return State.Idle;
                }
                else
                    return CurrentState;

            }
            else return CurrentState;
        }

        private void ExitLearnState()
        {
            bool on = false;
            _dataService.Save(() =>
            {
                _hat.D3Color = on ? LedColor.Red : LedColor.White;
                on = !on;
            });

            _hat.D3Color = LedColor.Blue;

            _mlService.Model();

            _hat.TurnOffD2();
            _hat.TurnOffD3();
        }

        public void Process()
        {
            _hat.Process();
            Controls.Process();

            var state = GetProcessedState();

            // ------- mode guards
            // can't do auto mode without models
            if (state == State.Auto && !_mlService.HasModel())
                state = State.Idle;

            // exit learn mode - persist and make models
            if (CurrentState == State.Learn && state != State.Learn)
                ExitLearnState();

            CurrentState = state;
            SetModeIndicators(CurrentState);

            // ------ process under current mode
            var data = GetSensorData();

            // handle controller values
            double speed = 0;
            double turn = 0;

            if (Controls.IsButtonPushed(GamepadButtons.A))
                speed = Motor.Speed;

            if (Controls.LeftTrigger > 0)
                turn = Motor.Left;
            else if (Controls.RightTrigger > 0)
                turn = Motor.Right;
            else
                turn = 0;

            data.Speed = speed;
            data.Turn = turn;

            // only learn on simple drive mechanism
            if (CurrentState == State.Learn)
                _dataService.Insert(data);
            if (CurrentState == State.Auto)
                (speed, turn) = _mlService.Predict(data);

            _hat.Drive(speed, turn);

            CurrentData = data;
        }

        private void SetModeIndicators(State state)
        {
            // current mode
            switch (state)
            {
                case State.Idle:
                    _hat.TurnOffD2();
                    _hat.TurnOffD3();
                    break;
                case State.Learn:
                    _hat.D2Color = LedColor.Red;
                    _hat.D3Color = LedColor.Red;
                    break;
                case State.Auto:
                    _hat.D2Color = LedColor.Yellow;
                    _hat.D3Color = LedColor.Yellow;
                    break;
            }
        }

        private Data GetSensorData()
        {
            Data data = new Data();

            // handle line tracking sensors
            data.Ain1 = _hat.Ain1;
            data.Ain2 = _hat.Ain2;
            data.Ain3 = _hat.Ain3;

            // handle accelerometer values;
            data.AccelerationX = _hat.AccelerationX;
            data.AccelerationY = _hat.AccelerationY;
            data.AccelerationZ = _hat.AccelerationZ;

            return data;
        }
    }
}
