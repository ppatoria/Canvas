using System;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Models
{
    [Pure]
    public static class ContractChecks
    {
        // TODO try making this internal class
        public static bool IsPointOutOfBounds(this Point point, ICanvas canvas)
        {
            Contract.Requires<ArgumentNullException>(canvas != null);
            return point.X >= canvas.Width || point.Y >= canvas.Height;
        }
    }



    [ContractClassFor(typeof(ICanvas))]
    public abstract class CanvasContracts : ICanvas
    {
        public abstract CanvasCell[,] Cells { get; }
        public abstract int Height { get; }
        public abstract int Width { get; }

        public void BucketFill(IBucketFillParamerters fillParamerters)
        {
            //Contract.Requires<OutOfBoundsException>(!PointIsOutOfBounds(fillParameters.Point), "The target point is out of the canvas boundaries");
            Contract.Requires<OutOfBoundsException>(!fillParamerters.Point.IsPointOutOfBounds(this),
                "The target point is out of the canvas boundaries");

        }
        public void Delete(Point target)
        {
            Contract.Requires<OutOfBoundsException>(!target.IsPointOutOfBounds(this),
                "The target point is out of the canvas boundaries");
        }

        public void DrawLine(Line line)
        {
            Contract.Requires<InvalidLineException>(line.IsHorizontal || line.IsVertical, "Only horizontal and vertical lines are currently supported");
            Contract.Requires<OutOfBoundsException>(!line.Origin.IsPointOutOfBounds(this) && !line.End.IsPointOutOfBounds(this),
                "This item exceeds the canvas boundaries and cannot be drawn");
            // TODO try better solution either make a separate interface or this check or add method in ICanvas

            //Contract.Requires<OutOfBoundsException>(!PointIsOutOfBounds(line.Origin) && !PointIsOutOfBounds(line.End), 
            //    "This item exceeds the canvas boundaries and cannot be drawn");

            // TODO check how weather the method need to be abstract ot explicit and how it works.
        }
        public void DrawRectangle(Rectangle rectangle)
        {
            //Contract.Requires<OutOfBoundsException>(!PointIsOutOfBounds(rectangle.UpperLeft)
            // && !PointIsOutOfBounds(rectangle.UpperRight)
            // && !PointIsOutOfBounds(rectangle.LowerLeft)
            // && !PointIsOutOfBounds(rectangle.LowerRight)
            // ,"This item exceeds the canvas boundaries and cannot be drawn");
            Contract.Requires<OutOfBoundsException>(
                !rectangle.UpperLeft.IsPointOutOfBounds(this)
             && !rectangle.UpperRight.IsPointOutOfBounds(this)
             && !rectangle.LowerLeft.IsPointOutOfBounds(this)
             && !rectangle.LowerRight.IsPointOutOfBounds(this)
             ,"This item exceeds the canvas boundaries and cannot be drawn");
        }
        public abstract void Undo();
    }
    [ContractClass(typeof(CanvasContracts))]
    public interface ICanvas
    {
        CanvasCell[,] Cells { get; }
        int Height { get; }
        int Width { get; }

        void BucketFill(IBucketFillParamerters fillParamerters);

        void Delete(Point target);

        void DrawLine(Line line);

        void DrawRectangle(Rectangle rectangle);

        void Undo();
    }

}