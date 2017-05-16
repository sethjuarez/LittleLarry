using LittleLarry.Model.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Models
{
    public class FakeFezHat : IFezHat
    {
        public bool DIO18Pressed { get; private set; } = false;
        public bool DIO22Pressed { get; private set; } = false;

        public double Ain1 { get; private set; } = 0;
        public double Ain2 { get; private set; } = 0;
        public double Ain3 { get; private set; } = 0;

        public double AccelerationX { get; private set; } = 0;
        public double AccelerationY { get; private set; } = 0;
        public double AccelerationZ { get; private set; } = 0;

        public LedColor D2Color { get; set; } = LedColor.Black;
        public LedColor D3Color { get; set; } = LedColor.Black;

        public double MotorA { get; set; } = 0;
        public double MotorB { get; set; } = 0;

        public void Drive(double speed, double turn) =>
            (MotorA, MotorB) = Motor.Convert(speed, turn);

        private int _index = 0;
        public void Process()
        {
            _index = _index % FakeData.Array.Length;

            Ain1 = FakeData.Array[_index].Ain1;
            Ain2 = FakeData.Array[_index].Ain2;
            Ain3 = FakeData.Array[_index].Ain3;

            AccelerationX = FakeData.Array[_index].AccelerationX;
            AccelerationY = FakeData.Array[_index].AccelerationY;
            AccelerationZ = FakeData.Array[_index].AccelerationZ;
        }

        public void TurnOffD2() => D2Color = LedColor.Black;
        public void TurnOffD3() => D3Color = LedColor.Black;
    }
}
