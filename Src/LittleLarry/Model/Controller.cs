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
        
        public (int x, int y) GetValues()
        {
            if (_gamepad != null)
            {
                var state = _gamepad.GetCurrentReading();
                return
                (
                    (int)(Math.Round(state.LeftThumbstickY * 10)),
                    (int)(Math.Round(state.RightThumbstickX * 10))
                );
            }
            else return (0, 0);
        }
    }
}
