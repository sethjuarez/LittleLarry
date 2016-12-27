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

        public (int speed, int turn) GetValues()
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

        public (double x, double y) GetRawValues()
        {
            if (_gamepad != null)
            {
                var state = _gamepad.GetCurrentReading();
                return (state.LeftThumbstickY, state.RightThumbstickX);
            }
            else return (0, 0);
        }

        public (double SpeedA, double SpeedB) Convert(int x, int y)
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
                if (x < 0)
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
