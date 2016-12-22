using GHIElectronics.UWP.Shields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleLarry.Model
{
    public class Accelerometer
    {
        private FEZHAT _hat;
        public Accelerometer(FEZHAT hat) => _hat = hat;

        public bool Process()
        {
            return true;
        }

        (double, double, double) GetValues()
        {
            _hat.GetAcceleration(out var x, out var y, out var z);
             return (x, y, z);
        }
    }
}
