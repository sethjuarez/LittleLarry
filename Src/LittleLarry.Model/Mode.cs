using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model
{
    public enum State
    {
        Idle,
        Learn,
        Auto
    }

    public enum Speed
    {
        Stop = 0,
        Go = 1,
    }

    public enum Turn
    {
        Straight = 0,
        Left = 1,
        Right = 2
    }
}
