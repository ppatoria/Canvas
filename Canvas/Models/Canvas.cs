using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Models
{
    public interface IBucketFillParamerters
    {
        char Color { get; set; }
        Point Point { get; }
    }
    public class BucketFillParameters : IBucketFillParamerters, IEquatable<IBucketFillParamerters>
    {      
        public char Color { get; set; }

        public Point Point { get; set;}

        public bool Equals(IBucketFillParamerters other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Point == other.Point && Color == other.Color;
        }
        public override bool Equals(object obj) => Equals(obj as IBucketFillParamerters);
        public override int GetHashCode() => Point.GetHashCode() ^ Color.GetHashCode(); 

    }

    public interface ICanvasDimentions
    {
        uint Width { get; set; }     
        uint Height { get; set; }

    }
    public class CanvasDimentions : ICanvasDimentions
    {
        public uint Width { get; set; }     
        public uint Height { get; set; }
    }

    public class Canvas : ICanvas
    {
        private Stack<CanvasCell[,]> _previousStates = new Stack<CanvasCell[,]>();

        public CanvasCell[,] Cells { get; private set; }

        public int Width => Cells.GetUpperBound(0) + 1;
        public int Height => Cells.GetUpperBound(1) + 1;

        public Canvas(ICanvasDimentions canvasSize)
        {
            Initialize(canvasSize);
        }

        private void Initialize(ICanvasDimentions canvasSize)
        {
            Cells = new CanvasCell[canvasSize.Width, canvasSize.Height];

            for (var i = 0; i < canvasSize.Width; i++)
                for (var j = 0; j < canvasSize.Height; j++)
                    Cells[i, j] = new CanvasCell(CanvasCellContentType.Empty, ' ');
        }

        [Pure]
        //public bool IsEitherHorizontalOrVertical(Line line) => line.IsHorizontal || line.IsVertical; 
        public void DrawLine(Line line)
        {

            SaveState();

            InnerDrawLine(line);
        }

        private void InnerDrawLine(Line line)
        {
            var points = line.GetPoints();
            foreach (var point in points)
                DrawPoint(point);
        }

        public void DrawRectangle(Rectangle rectangle)
        {

            SaveState();

            var lines = rectangle.GetLines();

            foreach (var line in lines)
                InnerDrawLine(line);
        }

        [Pure]
        public bool PointIsOutOfBounds(Point point) =>  point.X >= Width || point.Y >= Height;

        private void DrawPoint(Point point)
        {
            var cellContent = new CanvasCell(CanvasCellContentType.Line, 'x');
            Cells[point.X, point.Y] = cellContent;
        }

        public void BucketFill(IBucketFillParamerters fillParameters)
        {

            SaveState();

            InnerBucketFill(fillParameters.Point, fillParameters.Color);
        }

        private void InnerBucketFill(Point target, char colour, bool deleteContentType = false)
        {

            var contentTypeToFill = Cells[target.X, target.Y].ContentType;

            var processed = new HashSet<Point>();
            var toProcess = new Queue<Point>();

            toProcess.Enqueue(target);

            Func<Point, bool> CanProcessCell = (c) => !processed.Contains(c) && !toProcess.Contains(c) && !PointIsOutOfBounds(c);

            while (toProcess.Count > 0)
            {
                var currentPoint = toProcess.Dequeue();

                processed.Add(currentPoint);

                if (Cells[currentPoint.X, currentPoint.Y].ContentType == contentTypeToFill)
                {
                    if (deleteContentType)
                        Cells[currentPoint.X, currentPoint.Y] = new CanvasCell(CanvasCellContentType.Empty, colour);
                    else
                        Cells[currentPoint.X, currentPoint.Y] = new CanvasCell(contentTypeToFill, colour);

                    var leftNeighbour = new Point(currentPoint.X - 1, currentPoint.Y);
                    if (CanProcessCell(leftNeighbour))
                        toProcess.Enqueue(leftNeighbour);

                    var rightNeighbour = new Point(currentPoint.X + 1, currentPoint.Y);
                    if (CanProcessCell(rightNeighbour))
                        toProcess.Enqueue(rightNeighbour);

                    var topNeighbour = new Point(currentPoint.X, currentPoint.Y - 1);
                    if (CanProcessCell(topNeighbour))
                        toProcess.Enqueue(topNeighbour);

                    var bottomNeighbour = new Point(currentPoint.X, currentPoint.Y + 1);
                    if (CanProcessCell(bottomNeighbour))
                        toProcess.Enqueue(bottomNeighbour);
                }
            }
        }

        public void Delete(Point target)
        {

            SaveState();

            InnerBucketFill(target, ' ', deleteContentType: true);
        }

        public void Undo()
        {
            if (_previousStates.Count > 0)
                Cells = _previousStates.Pop();
        }

        private void SaveState()
        {
            var state = new CanvasCell[Width, Height];
            Array.Copy(Cells, state, Cells.Length);
            _previousStates.Push(state);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(' ');
            HeaderLine(sb);

            sb.Append(Environment.NewLine);

            for (var j = 0; j < Height; j++)
            {
                sb.Append('|');
                for (var i = 0; i < Width; i++)
                {
                    sb.Append(Cells[i, j].Colour);
                }
                sb.Append('|');
                sb.Append(Environment.NewLine);
            }

            sb.Append(' ');
            FooterLine(sb);

            return sb.ToString();
        }

        private void FooterLine(StringBuilder sb)
        {
            HeaderLine(sb);
        }

        private void HeaderLine(StringBuilder sb)
        {
            for (var i = 0; i < Width; i++) sb.Append('-');
        }
    }

    public struct CanvasCell
    {
        public CanvasCellContentType ContentType { get; private set; }
        public char Colour { get; private set; }

        public CanvasCell(CanvasCellContentType contentType, char colour)
        {
            ContentType = contentType;
            Colour = colour;
        }
    }

    public enum CanvasCellContentType
    {
        Empty,
        Line
    }

    public class InvalidLineException : Exception
    {
        public InvalidLineException(string msg) : base(msg) { }
    }

    public class OutOfBoundsException : Exception
    {
        public OutOfBoundsException(string msg)
            : base(msg)
        { }
    }
}