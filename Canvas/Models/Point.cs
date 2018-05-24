using System;
namespace CanvasApplication.Models
{
    public struct Point : IEquatable<Point>
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

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }
        public override bool Equals(object obj)
        {
            return Equals((Point)obj);
        }
        public override int GetHashCode()
        {
            return (int)X ^ (int)Y;
        }
    }
}