using System;
using System.Linq;
using Windows.UI;

namespace LittleLarry.Model.Hardware
{
    public static class Motor
    {
        public const double Speed = 0.5;
        public const double Left = -0.5;
        public const double Right = 0.5;

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

        public static Color ToColor(double d, bool reverse = false)
        {
            byte b = reverse ?
                        (byte)(d * 255) :
                        (byte)(255 - (d * 255));

            return Color.FromArgb(255, b, b, b);
        }
    }
}
