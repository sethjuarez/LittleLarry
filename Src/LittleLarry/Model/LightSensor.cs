using GHIElectronics.UWP.Shields;
using Windows.UI;

namespace LittleLarry.Model
{
    public class LightSensor
    {
        private FEZHAT _hat;
        public LightSensor(FEZHAT hat) => _hat = hat;

        public bool Process()
        {
            if(_hat != null)
            {
                try
                {
                    Ain1 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain1);
                    Ain2 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain2);
                    Ain3 = _hat.ReadAnalog(FEZHAT.AnalogPin.Ain3);
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        private Color ToColor(double d, bool reverse = false)
        {
            byte b = reverse ? 
                        (byte)(d * 255) : 
                        (byte)(255 - (d * 255));

            return Color.FromArgb(255, b, b, b);
        }

        public double Ain1 { get; internal set; }
        public Color Ain1Color => ToColor(Ain1);
        public Color Ain1ReverseColor => ToColor(Ain1, true);
        public double Ain2 { get; internal set; }
        public Color Ain2Color => ToColor(Ain2);
        public Color Ain2ReverseColor => ToColor(Ain2, true);
        public double Ain3 { get; internal set; }
        public Color Ain3Color => ToColor(Ain3);
        public Color Ain3ReverseColor => ToColor(Ain3, true);
    }
}
