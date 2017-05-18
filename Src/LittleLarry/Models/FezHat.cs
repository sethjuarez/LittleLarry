using LittleLarry.Model.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHIElectronics.UWP.Shields;

namespace LittleLarry.Models
{
    public class FezHat : IFezHat
    {
        private FEZHAT _hat;
        public FezHat()
        {
            SetupAsync();
        }

        private async void SetupAsync()
        {
            _hat = await FEZHAT.CreateAsync();
        }

        public bool DIO18Pressed { get; private set;  } = false; 

        public bool DIO22Pressed { get; private set; } = false;

        public double Ain1 { get; private set; } = 0;

        public double Ain2 { get; private set; } = 0;

        public double Ain3 { get; private set; } = 0;

        public double AccelerationX { get; private set; } = 0;

        public double AccelerationY { get; private set; } = 0;

        public double AccelerationZ { get; private set; } = 0;

        public double MotorA
        {
            get
            {
                return _hat.MotorA.Speed;
            }
            set
            {
                _hat.MotorA.Speed = value;
                if (Math.Abs(_hat.MotorA.Speed) < .01)
                {
                    _hat.MotorA.Speed = 0;
                    _hat.MotorA.Stop();
                }
            }
        }
        public double MotorB
        {
            get
            {
                return _hat.MotorB.Speed;
            }
            set
            {
                _hat.MotorB.Speed = value;
                if (Math.Abs(_hat.MotorB.Speed) < .01)
                {
                    _hat.MotorB.Speed = 0;
                    _hat.MotorB.Stop();
                }
            }
        }

        public LedColor D2Color
        {
            get
            {
                return ToLedColor(_hat.D2.Color);
            }
            set
            {
                _hat.D2.Color = ToFezHatColor(value);
            }
        }
        public LedColor D3Color
        {
            get
            {
                return ToLedColor(_hat.D3.Color);
            }
            set
            {
                _hat.D3.Color = ToFezHatColor(value);
            }
        }


        public void Drive(double speed, double turn)
        {
            (MotorA, MotorB) = Motor.Convert(speed, turn);
        }

        public void Process()
        {
            DIO18Pressed = _hat.IsDIO18Pressed();
            DIO22Pressed = _hat.IsDIO22Pressed();
            Ain1 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain1);
            Ain2 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain2);
            Ain3 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain3);

            _hat.GetAcceleration(out var x, out var y, out var z);
            AccelerationX = x;
            AccelerationY = y;
            AccelerationZ = z;
        }

        public void TurnOffD2()
        {
            _hat.D2.TurnOff();
        }

        public void TurnOffD3()
        {
            _hat.D3.TurnOff();
        }

        private FEZHAT.Color ToFezHatColor(LedColor color)
        {
            switch(color)
            {
                case LedColor.Red:
                    return FEZHAT.Color.Red;
                case LedColor.Green:
                    return FEZHAT.Color.Green;
                case LedColor.Blue:
                    return FEZHAT.Color.Blue;
                case LedColor.Cyan:
                    return FEZHAT.Color.Cyan;
                case LedColor.Magenta:
                    return FEZHAT.Color.Magneta;
                case LedColor.Yellow:
                    return FEZHAT.Color.Yellow;
                case LedColor.White:
                    return FEZHAT.Color.White;
                case LedColor.Black:
                    return FEZHAT.Color.Black;
                default:
                    return FEZHAT.Color.Black;
            }
        }

        private LedColor ToLedColor(FEZHAT.Color color)
        {
            if (color == FEZHAT.Color.Red)
                return LedColor.Red;
            else if(color == FEZHAT.Color.Green)
                return LedColor.Green;
            else if (color == FEZHAT.Color.Blue)
                return LedColor.Blue;
            else if (color == FEZHAT.Color.Cyan)
                return LedColor.Cyan;
            else if (color == FEZHAT.Color.Magneta)
                return LedColor.Magenta;
            else if (color == FEZHAT.Color.Yellow)
                return LedColor.Yellow;
            else if (color == FEZHAT.Color.White)
                return LedColor.White;
            else if (color == FEZHAT.Color.Black)
                return LedColor.Black;

            return LedColor.Black;
        }
    }
}
