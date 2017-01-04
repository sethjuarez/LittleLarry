using GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace LittleLarry.Hardware
{
    public class Controls
    {
        public TimeSpan TimeSinceMark { get; private set; }
        public bool DIO18Pressed { get; private set; }
        public bool DIO22Pressed { get; private set; }
        public GamepadButtons Buttons { get; private set; }
        public double LeftThumbstickX { get; private set; }
        public double LeftThumbstickY { get; private set; }
        public double LeftTrigger { get; private set; }
        public double RightThumbstickX { get; private set; }
        public double RightThumbstickY { get; private set; }
        public double RightTrigger { get; private set; }

        private Gamepad _gamepad = null;
        private FEZHAT _hat;
        private DateTime _lastProcess;

        public Controls(FEZHAT hat)
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            if (Gamepad.Gamepads.Count == 1)
                _gamepad = Gamepad.Gamepads[0];

            _hat = hat;
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
                LeftThumbstickX = state.LeftThumbstickX;
                LeftThumbstickY = state.LeftThumbstickY;
                LeftTrigger = state.LeftTrigger;
                RightThumbstickX = state.RightThumbstickX;
                RightThumbstickY = state.RightThumbstickY;
                RightTrigger = state.RightTrigger;
                Buttons = state.Buttons;               
            }
            else
            {
                LeftThumbstickX = 0;
                LeftThumbstickY = 0;
                LeftTrigger = 0;
                RightThumbstickX = 0;
                RightThumbstickY = 0;
                RightTrigger = 0;
                Buttons = GamepadButtons.None;
            }
            
            // process hat
            DIO18Pressed = _hat.IsDIO18Pressed();
            DIO22Pressed = _hat.IsDIO22Pressed();
            
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
