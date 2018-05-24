using CanvasApplication.Models;
using CanvasApplication.Commands;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using System;

namespace CanvasApplication.UnitTests.Commands
{
    [TestFixture]
    public class DrawLineCommand_Should
    {

        [Test]
        public void Throw_When_Null_Argument()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DrawLineCommand();

            command.Input = null;
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Throw_When_Fewer_Params_Than_Expected_Are_Passed()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DrawLineCommand();

            command.Input = new string[] { "1", "2", "3" };

            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        
        [TestCase("hi", "2", "3", "4")]
        [TestCase("1", "hi", "3", "4")]
        [TestCase("1", "3", ".", "4")]
        [TestCase("1", "2", "3", "test")]
        public void Throw_When_An_Expected_Param_Is_Of_Invalid_Format(string arg1, string arg2, string arg3, string arg4)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DrawLineCommand();

            command.Input = new string[] { arg1, arg2, arg3, arg4 };
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Throw_When_No_Canvas_Exist()
        {
            var command = new DrawLineCommand();

            command.Input = new string[] { "1", "1", "1", "5" };
            Action test = () => command.Execute(null);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Call_Canvas_DrawLine_When_Params_Are_Fine()
        {
            var args = new string[] { "1", "1", "1", "20" };
            var expectedLine = new Line(new Point(1 - 1, 1 - 1), new Point(1 - 1, 20 - 1)); //adjust 1-based coordinates to 0-based

            var mockCanvas = new Mock<ICanvas>();

            var command = new DrawLineCommand();
            command.Input = args;
            command.Execute(mockCanvas.Object);

            mockCanvas.Verify(x => x.DrawLine(expectedLine), Times.Exactly(1), "DrawLineCommand doesn't call Canvas.DrawLine with correct args");
        }
    }
}