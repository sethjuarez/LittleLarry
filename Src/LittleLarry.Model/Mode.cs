namespace LittleLarry.Model
{
    public enum Mode
    {
        Idle,
        Learn,
        Model,
        Auto
    }

    public enum Speed
    {
        /// <summary>
        /// Speed 0
        /// </summary>
        Stopped = 0,
        /// <summary>
        /// Speed 0.3 - 0.35
        /// </summary>
        Slower = 1,
        /// <summary>
        /// Speed 0.35 - 0.4
        /// </summary>
        Slow = 2,
        /// <summary>
        /// Speed 0.4 - 0.45
        /// </summary>
        Fast = 3,
        /// <summary>
        /// Speed 0.45 - 0.5
        /// </summary>
        Faster = 4
    }

    public enum Turn
    {
        /// <summary>
        /// Turn at 0
        /// </summary>
        Straight = 0,
        /// <summary>
        /// -0.1 <= t < 0
        /// Snap to -0.5
        /// </summary>
        SmallLeft = 1,
        /// <summary>
        /// t < -0.1
        /// Snap to -0.1
        /// </summary>
        Left = 2,
        /// <summary>
        /// 0 < t < 0.1
        /// Snap to 0.5
        /// </summary>
        SmallRight = 3,
        /// <summary>
        /// t > 0.1
        /// Snap to 0.1
        /// </summary>
        Right = 4
    }
}
