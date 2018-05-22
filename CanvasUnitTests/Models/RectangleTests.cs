using CanvasApplication.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CanvasApplication.Tests.UnitTests.Models
{
    [TestFixture]
    public class Rectangle_Should
    {
        [Test]
        public void Initialize_Correctly()
        {
            var rectangle = new Rectangle(new Point(1, 1), new Point(5, 5));

            rectangle.UpperLeft.Should().Be(new Point(1, 1));
            rectangle.UpperRight.Should().Be(new Point(1, 5));
            rectangle.LowerLeft.Should().Be(new Point(5, 1));
            rectangle.LowerRight.Should().Be(new Point(5, 5));
        }

        [Test]
        public void Return_Correct_Lines()
        {
            var rectangle = new Rectangle(new Point(1, 1), new Point(5, 5));

            var result = rectangle.GetLines();

            result.Count().Should().Be(4);
            result.Should().BeEquivalentTo(new List<Line>
            {
                new Line(new Point(1, 1), new Point(1, 5)),
                new Line(new Point(1, 5), new Point(5, 5)),
                new Line(new Point(5, 1), new Point(5, 5)),
                new Line(new Point(5, 1), new Point(1, 1))
            });
        }
    }
}