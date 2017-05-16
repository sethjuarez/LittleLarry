using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace LittleLarry.Model.Hardware
{
    public enum LedColor
    {
        White,
        Blue,
        Red,
        Green,
        Yellow
    }
    public interface IFezHat : IProcess
    {
        bool DIO18Pressed { get; }
        bool DIO22Pressed { get; }

        double Ain1 { get; }
        double Ain2 { get; }
        double Ain3 { get; }

        double AccelerationX { get; }
        double AccelerationY { get; }
        double AccelerationZ { get; }

        LedColor D2Color { get; set; }
        LedColor D3Color { get; set; }

        void TurnOffD2();
        void TurnOffD3();

        //(double x, double y, double z) GetAccelerometer();
        //(double Ain1, double Ain2, double Ain3) GetLight();
        //(bool dio18Pressed, bool dio22Pressed) GetPressed();

        void Drive(double speed, double turn);
    }
}
