using SQLite;

namespace LittleLarry.Model
{
    public interface IConnection
    {
        string DataPath { get; }
        SQLiteConnection Initialize();
        SQLiteConnection SQLiteConnection { get; }
    }
}
