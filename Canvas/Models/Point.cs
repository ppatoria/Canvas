namespace CanvasApplication.Models
{
#pragma warning disable CS0660, CS0661 //no need to overrides Equals() and GetHasCode() as == and !== use their default implementation

    public struct Point
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public Point(uint x, uint y)
        {
            X = x;
            Y = y;
        }


        public static bool operator ==(Point p1, Point p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

    }

#pragma warning restore CS0660, CS0661
}