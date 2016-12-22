using System;
using Windows.Gaming.Input;

namespace LittleLarry.Model
{
    public class Controller
    {
        private Gamepad _gamepad = null;

        public Controller()
        {
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
            if (Gamepad.Gamepads.Count == 1)
                _gamepad = Gamepad.Gamepads[0];
        }

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

        public bool Process()
        {
            if (_gamepad != null)
            {
                var state = _gamepad.GetCurrentReading();
                Y = (int)(Math.Round(state.LeftThumbstickY * 10));
                X = (int)(Math.Round(state.RightThumbstickX * 10));
                return true;
            }

            return false;
        }

        public int Y { get; set; }
        public int X { get; set; }
    }
}
