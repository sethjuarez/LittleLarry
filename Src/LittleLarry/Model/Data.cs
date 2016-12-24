namespace LittleLarry.Model
{
    public enum Mode
    {
        Idle,
        Learn,
        Model,
        Auto
    }

    public class Data
    {
        public double Ain1 { get; set; }
        public double Ain2 { get; set; }
        public double Ain3 { get; set; }
        public double AccelerationX { get; set; }
        public double AccelerationY { get; set; }
        public double AccelerationZ { get; set; }

        public int Turn { get; set; }
        public int Speed { get; set; }
    }
}
