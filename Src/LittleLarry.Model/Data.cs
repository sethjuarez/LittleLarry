using SQLite;

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
    }
}
