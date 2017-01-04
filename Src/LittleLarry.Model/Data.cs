using SQLite;
using System;

namespace LittleLarry.Model
{

    public class Data
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Ain1 { get; set; }
        public double Ain2 { get; set; }
        public double Ain3 { get; set; }
        public double AccelerationX { get; set; }
        public double AccelerationY { get; set; }
        public double AccelerationZ { get; set; }

        public double Turn { get; set; }
        public double Speed { get; set; }

        public Turn Direction
        {
            get
            {
                if (Math.Abs(Turn) < 0.01)
                    return Model.Turn.Straight;
                else if (Turn >= -0.1 && Turn < -0.01)
                    return Model.Turn.Left;
                else if (Turn <= -0.1)
                    return Model.Turn.Left;
                else if (Turn >= 0.01 && Turn < 0.1)
                    return Model.Turn.Right;
                else if (Turn >= 0.1)
                    return Model.Turn.Right;
                return Model.Turn.Straight;
            }
        }

        public static double DirectionToTurn(Turn direction)
        {
            switch(direction)
            {
                case Model.Turn.Straight:
                    return 0;
                case Model.Turn.SmallLeft:
                    return -0.05;
                case Model.Turn.Left:
                    return -0.1;
                case Model.Turn.SmallRight:
                    return 0.5;
                case Model.Turn.Right:
                    return 0.1;
                default:
                    return 0;
            }
        }

        public Speed Forward
        {
            get
            {
                if (Speed < 0.3)
                    return Model.Speed.Stopped;
                else if (Speed >= .3 && Speed < 0.35)
                    return Model.Speed.Slower;
                else if (Speed >= 0.35 && Speed < 0.4)
                    return Model.Speed.Slow;
                else if (Speed >= 0.4 && Speed < 0.45)
                    return Model.Speed.Fast;
                else if (Speed >= 0.45)
                    return Model.Speed.Faster;
                else
                    return Model.Speed.Stopped;
            }
        }

        public static double ForwardToSpeed(Speed forward)
        {
            switch (forward)
            {
                case Model.Speed.Stopped:
                    return 0;
                case Model.Speed.Slower:
                    return 0.3;
                case Model.Speed.Slow:
                    return 0.35;
                case Model.Speed.Fast:
                    return 0.4;
                case Model.Speed.Faster:
                    return 0.45;
                default:
                    return 0;
            }
        }
    }
}
