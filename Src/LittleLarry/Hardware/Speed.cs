using System;

namespace LittleLarry.Hardware
{
    public class Speed
    {
        public static (double a, double b) Convert(double speed, double turn)
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
