using GHIElectronics.UWP.Shields;
using Windows.UI;

namespace LittleLarry.Model
{
    public class LightSensor
    {
        private FEZHAT _hat;
        public LightSensor(FEZHAT hat) => _hat = hat;

        public (double Ain1, double Ain2, double Ain3) GetValues()
        {
            return
            (
                _hat.ReadAnalog(FEZHAT.AnalogPin.Ain1),
                _hat.ReadAnalog(FEZHAT.AnalogPin.Ain2),
                _hat.ReadAnalog(FEZHAT.AnalogPin.Ain3)
            );
        }

        public Color ToColor(double d, bool reverse = false)
        {
            byte b = reverse ?
                        (byte)(d * 255) :
                        (byte)(255 - (d * 255));

            return Color.FromArgb(255, b, b, b);
        }
        
    }
}
