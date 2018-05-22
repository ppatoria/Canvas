using CanvasApplication.Models;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace CanvasApplication.UnitTests.Models
{
    [TestFixture]
    public class Line_IsHorizontal_Should
    {
        [Test]
        public void Return_True_When_Line_Is_Horizontal()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(0, 5);

            var result = new Line(p1, p2).IsHorizontal;

            result.Should().BeTrue();
        }

        [Test]
        public void Return_False_When_Line_Is_Not_Horizontal()
        {
            var p1 = new Point(0, 0);
            var p2 = new Point(1, 1);

            var result = new Line(p1, p2).IsHorizontal;

            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class Line_IsVertical_Should
    {
        [Test]
        public void Return_True_When_Line_Is_Vertical()
        {
            var p1 = new Point(0, 5);
            var p2 = new Point(100, 5);

            var result = new Line(p1, p2).IsVertical;

            result.Should().BeTrue();
        }

        [Test]
        public void Return_False_When_Line_Is_Not_Vertical()
        {
            var p1 = new Point(0, 5);
            var p2 = new Point(1, 500);

            var result = new Line(p1, p2).IsVertical;

            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class Line_GetPoints_Should
    {
        
        [TestCase(0u, 10u, 0u, 10u, 1)]
        [TestCase(0u, 10u, 0u, 60u, 51)]
        [TestCase(0u, 60u, 0u, 10u, 51)]
        [TestCase(5u, 5u, 10u, 5u, 6)]
        [TestCase(10u, 5u, 5u, 5u, 6)]
        public void Return_Correct_List_Of_Points(uint x1, uint y1, uint x2, uint y2, int nbPoints)
        {
            var line = new Line(new Point(x1, y1), new Point(x2, y2));

            var result = line.GetPoints();

            result.Count().Should().Be(nbPoints);

            if (line.IsHorizontal)
                result.Should().OnlyContain(point => point.X == x1, "all points should be aligned");

            if (line.IsVertical)
                result.Should().OnlyContain(point => point.Y == y1, "all points should be aligned");
        }

        [Test]
        public void Throw_If_Line_Is_Not_Horizontal_Or_Vertical()
        {
            var line = new Line(new Point(0, 10), new Point(5, 5));

            Action test = () => line.GetPoints();

            test.Should().Throw<NotImplementedException>()
                .WithMessage("Only implemented for horizontal and vertical lines");
        }
    }
}