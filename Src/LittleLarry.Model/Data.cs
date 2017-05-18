using LittleLarry.Model.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model
{
    public class Data
    {
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
                if (Turn < 0)
                    return Model.Turn.Left;
                else if (Turn > 0)
                    return Model.Turn.Right;
                else
                    return Model.Turn.Straight;
            }
        }

        public static double DirectionToTurn(Turn direction)
        {
            switch (direction)
            {
                case Model.Turn.Straight:
                    return 0;
                case Model.Turn.Left:
                    return Motor.Left;
                case Model.Turn.Right:
                    return Motor.Right;
                default:
                    return 0;
            }
        }

        public Speed Forward
        {
            get
            {
                if (Speed > 0)
                    return Model.Speed.Go;
                else
                    return Model.Speed.Stop;
            }
        }

        public static double ForwardToSpeed(Speed forward)
        {
            switch (forward)
            {
                case Model.Speed.Stop:
                    return 0;
                case Model.Speed.Go:
                    return Motor.Speed;
                default:
                    return 0;
            }
        }
    }

}
