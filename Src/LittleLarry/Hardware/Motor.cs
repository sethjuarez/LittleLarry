using GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Hardware
{
    public class Motor
    {
        public const double Speed = 0.5;
        public const double Left = -0.5;
        public const double Right = 0.5;

        private FEZHAT _hat;
        public Motor(FEZHAT hat)
        {
            _hat = hat;
        }

        public void Drive(double speed, double turn)
        {
            (var a, var b) = Convert(speed, turn);
            _hat.MotorA.Speed = a;
            _hat.MotorB.Speed = b;
        }

        private static (double a, double b) Convert(double speed, double turn)
        {
            double GetFloor(double num)
            {
                double n = num;
                if (n < -1)
                    return -1;
                else if (n > 1)
                    return 1;
                else
                    return n;
            }

            double s = speed / 2d;
            double t = Math.Abs(turn / 2d);

            if (speed >= 0)
            {
                if (turn > 0)
                    return (GetFloor(s + t), GetFloor(s));
                else
                    return (GetFloor(s), GetFloor(s + t));
            }
            else
            {
                if (turn > 0)
                    return (GetFloor(s - t), GetFloor(s));
                else
                    return (GetFloor(s), GetFloor(s - t));
            }
        }
    }
}
