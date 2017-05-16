using System;
using System.Linq;
using Windows.Gaming.Input;

namespace LittleLarry.Model.Hardware
{
    public class Controller : IProcess
    {
        public TimeSpan TimeSinceMark { get; private set; }
        public GamepadButtons Buttons { get; private set; }
        public double LeftThumbStickX { get; private set; }
        public double LeftThumbStickY { get; private set; }
        public double LeftTrigger { get; private set; }
        public double RightThumbStickX { get; private set; }
        public double RightThumbStickY { get; private set; }
        public double RightTrigger { get; private set; }

        private Gamepad _gamepad = null;
        private DateTime _lastProcess;

        public Controller()
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            if (Gamepad.Gamepads.Count == 1)
                _gamepad = Gamepad.Gamepads[0];

            _lastProcess = DateTime.Now;
        }

        public void MarkTime()
        {
            _lastProcess = DateTime.Now;
        }

        public void Process()
        {
            // process gamepad
            if (_gamepad != null)
            {
                var state = _gamepad.GetCurrentReading();
                LeftThumbStickX = state.LeftThumbstickX;
                LeftThumbStickY = state.LeftThumbstickY;
                LeftTrigger = state.LeftTrigger;
                RightThumbStickX = state.RightThumbstickX;
                RightThumbStickY = state.RightThumbstickY;
                RightTrigger = state.RightTrigger;
                Buttons = state.Buttons;
            }
            else
            {
                LeftThumbStickX = 0;
                LeftThumbStickY = 0;
                LeftTrigger = 0;
                RightThumbStickX = 0;
                RightThumbStickY = 0;
                RightTrigger = 0;
                Buttons = GamepadButtons.None;
            }


            // set process time delta
            var now = DateTime.Now;
            TimeSinceMark = now.Subtract(_lastProcess);
        }

        public bool IsButtonPushed(GamepadButtons button) =>
            Buttons.HasFlag(button);

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count < 1)
                _gamepad = null;
            else
                _gamepad = Gamepad.Gamepads[0];
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            if (Gamepad.Gamepads.Count == 1)
                _gamepad = Gamepad.Gamepads[0];
        }
    }
}
