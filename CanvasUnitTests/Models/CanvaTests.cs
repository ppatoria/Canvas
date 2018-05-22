using CanvasApplication.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CanvasApplication.UnitTests.Models
{
    [TestFixture]
    public class Canvas_Should
    {
        [Test]
        public void Initializes_Correctly()
        {
            var width = 1280u;
            var height = 1024u;

            var canvas = new Canvas(new CanvasDimentions { Width = width, Height = height });

            canvas.Width.Should().Be((int)width);
            canvas.Height.Should().Be((int)height);

            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    canvas.Cells[i, j].ContentType.Should().Be(CanvasCellContentType.Empty);
                    canvas.Cells[i, j].Colour.Should().Be(' ');
                }
        }
    }

    [TestFixture]
    public class Canvas_DrawLine_Should
    {
        [Test]
        public void Throw_If_Line_Is_Not_Horizontal_Or_Vertical()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var origin = new Point(0, 0);
            var end = new Point(10, 10);
            var line = new Line(origin, end);

            Action test = () => canvas.DrawLine(line);

            test.Should().Throw<InvalidLineException>()
                .WithMessage("Only horizontal and vertical lines are currently supported");
        }

        
        [TestCase(1u, 1u, 1u, 50u)]
        [TestCase(1u, 50u, 25u, 50u)]
        public void Draw_An_Horizontal_Or_Vertical_Line(uint x1, uint y1, uint x2, uint y2)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var line = new Line(new Point(x1, y1), new Point(x2, y2));

            canvas.DrawLine(line);

            var points = line.GetPoints();
            foreach (var point in points)
            {
                canvas.Cells[point.X, point.Y].ContentType.Should().Be(CanvasCellContentType.Line);
                canvas.Cells[point.X, point.Y].Colour.Should().Be('x');
            }
        }

        [Test]
        public void Throw_When_A_Point_Is_Out_Of_Bound()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var line = new Line(new Point(0, 0), new Point(500, 0));

            Action test = () => canvas.DrawLine(line);

            test.Should().Throw<OutOfBoundsException>()
                .WithMessage("This item exceeds the canvas boundaries and cannot be drawn");
        }
    }

    [TestFixture]
    public class Canvas_DrawRectangle_Should
    {
        [Test]
        public void Throw_When_A_Point_Is_Out_Of_Bound()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var rectangle = new Rectangle(new Point(1, 1), new Point(5, 105));

            Action test = () => canvas.DrawRectangle(rectangle);

            test.Should().Throw<OutOfBoundsException>()
                .WithMessage("This item exceeds the canvas boundaries and cannot be drawn");
        }

        
        [TestCase(1u, 1u, 10u, 10u)]
        [TestCase(10u, 10u, 1u, 1u)]
        [TestCase(10u, 10u, 10u, 10u)]
        public void Draw_Correctly(uint upperLeftX, uint upperLeftY, uint lowerRightX, uint lowerRightY)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var rectangle = new Rectangle(new Point(1, 1), new Point(10, 10));

            canvas.DrawRectangle(rectangle);

            var lines = rectangle.GetLines();
            var points = new List<Point>();
            foreach (var line in lines)
                points.AddRange(line.GetPoints());

            foreach (var point in points)
            {
                canvas.Cells[point.X, point.Y].ContentType.Should().Be(CanvasCellContentType.Line);
                canvas.Cells[point.X, point.Y].Colour.Should().Be('x');
            }
        }
    }

    [TestFixture]
    public class Canvas_BucketFill_Should
    {
        [Test]
        public void Throw_When_Target_Point_Is_Out_Of_Bounds()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            Action test = () => canvas.BucketFill(new BucketFillParameters { Point = new Point(1,150), Color = 'f' });

            test.Should().Throw<OutOfBoundsException>()
                .WithMessage("The target point is out of the canvas boundaries");
        }

        [Test]
        public void Fill_The_Empty_Area_Connected_To_The_Target_Point()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 5, Height = 3 });

            //create this canvas
            //  -----
            // |  x  |
            // |  x  |
            // |  x  |
            //  -----

            canvas.Cells[2, 0] = new CanvasCell(CanvasCellContentType.Line, 'x');
            canvas.Cells[2, 1] = new CanvasCell(CanvasCellContentType.Line, 'x');
            canvas.Cells[2, 2] = new CanvasCell(CanvasCellContentType.Line, 'x');

            //fill the right side
            canvas.BucketFill(new BucketFillParameters { Point = new Point(1,1), Color = '0' });

            //check we have this canvas
            //  -----
            // |00x  |
            // |00x  |
            // |00x  |
            //  -----

            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 3; j++)
                    canvas.Cells[i, j].Should().Be(new CanvasCell(CanvasCellContentType.Empty, '0'));

            for (var j = 0; j < 3; j++)
                canvas.Cells[2, j].Should().Be(new CanvasCell(CanvasCellContentType.Line, 'x'));

            for (var i = 3; i < 5; i++)
                for (var j = 0; j < 3; j++)
                    canvas.Cells[i, j].Should().Be(new CanvasCell(CanvasCellContentType.Empty, ' '));
        }

        [Test]
        public void Fill_The_Shape_Connected_To_The_Target_Point()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 5, Height = 3 });

            //create this canvas
            //  -----
            // |  x  |
            // |  x  |
            // |  x  |
            //  -----

            canvas.Cells[2, 0] = new CanvasCell(CanvasCellContentType.Line, 'x');
            canvas.Cells[2, 1] = new CanvasCell(CanvasCellContentType.Line, 'x');
            canvas.Cells[2, 2] = new CanvasCell(CanvasCellContentType.Line, 'x');

            //fill the right side
            canvas.BucketFill(new BucketFillParameters { Point = new Point(2,1), Color = '@' });

            //check we have this canvas
            //  -----
            // |  @  |
            // |  @  |
            // |  @  |
            //  -----

            for (var i = 0; i < 2; i++)
                for (var j = 0; j < 3; j++)
                    canvas.Cells[i, j].Should().Be(new CanvasCell(CanvasCellContentType.Empty, ' '));

            for (var j = 0; j < 3; j++)
                canvas.Cells[2, j].Should().Be(new CanvasCell(CanvasCellContentType.Line, '@'));

            for (var i = 3; i < 5; i++)
                for (var j = 0; j < 3; j++)
                    canvas.Cells[i, j].Should().Be(new CanvasCell(CanvasCellContentType.Empty, ' '));
        }
    }

    [TestFixture]
    public class Canvas_Undo_Should
    {
        [Test]
        public void Do_Nothing_If_Action_History_Is_Empty()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            canvas.Undo();

            canvas.Should().BeEquivalentTo(new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }

        [Test]
        public void Undo_Drawn_Line()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var line = new Line(new Point(5, 5), new Point(5, 50));
            canvas.DrawLine(line);

            canvas.Undo();

            canvas.Should().BeEquivalentTo(new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }

        [Test]
        public void Undo_Drawn_Rectangle()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var rectangle = new Rectangle(new Point(5, 5), new Point(50, 50));
            canvas.DrawRectangle(rectangle);

            canvas.Undo();

            canvas.Should().BeEquivalentTo(new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }

        [Test]
        public void Undo_Bucket_Fill()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            canvas.BucketFill(new BucketFillParameters { Point = new Point(1,1), Color = 's' });

            canvas.Undo();

            canvas.Should().BeEquivalentTo( new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }

        [Test]
        public void Undo_Delete()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var rectangleTopLeft = new Point(5, 5);
            var rectangleBottomRight = new Point(50, 50);
            var rectangle = new Rectangle(rectangleTopLeft, rectangleBottomRight);

            canvas.DrawRectangle(rectangle);
            canvas.Delete(rectangleTopLeft);

            var expectedCanvasAfterUndoDelete = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });
            expectedCanvasAfterUndoDelete.DrawRectangle(rectangle);

            canvas.Undo();

            canvas.Should().BeEquivalentTo(expectedCanvasAfterUndoDelete);
        }

        [Test]
        public void Undo_Multiple_Last_Actions()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var rectangle = new Rectangle(new Point(5, 5), new Point(50, 50));
            canvas.DrawRectangle(rectangle);
            canvas.BucketFill(new BucketFillParameters { Point = new Point(10,10), Color = 's' });

            var expectedCanvasAfter1Undo = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });
            expectedCanvasAfter1Undo.DrawRectangle(rectangle);

            canvas.Undo();

            canvas.Should().BeEquivalentTo(expectedCanvasAfter1Undo);

            canvas.Undo();

            canvas.Should().BeEquivalentTo(new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }
    }

    [TestFixture]
    public class Canvas_Delete_Should
    {
        [Test]
        public void Throw_When_Target_Point_Is_Out_Of_Bound()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            Action test = () => canvas.Delete(new Point(1, 150));

            test.Should().Throw<OutOfBoundsException>()
                .WithMessage("The target point is out of the canvas boundaries");
        }

        [Test]
        public void Delete_A_Shape()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var line = new Line(new Point(10, 0), new Point(10, 99));
            canvas.DrawLine(line);

            canvas.Delete(new Point(10, 15));

            canvas.Should().BeEquivalentTo(new Canvas(new CanvasDimentions { Width = 100, Height = 100 }));
        }

        [Test]
        public void Delete_A_Colour()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var line = new Line(new Point(10, 0), new Point(10, 99));
            canvas.DrawLine(line);
            canvas.BucketFill(new BucketFillParameters { Point = new Point(5,50), Color = 'o' });

            var expectedCanvasAfterDelete = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });
            expectedCanvasAfterDelete.DrawLine(line);

            canvas.Delete(new Point(2, 25));

            canvas.Should().BeEquivalentTo(expectedCanvasAfterDelete);
        }
    }
}