using CanvasApplication.Models;
using CanvasApplication.Commands;
using NUnit.Framework;
using System;
using FluentAssertions;
using Moq;

namespace CanvasApplication.UnitTests.Commands
{
    [TestFixture]
    public class DeleteCommand_Should
    {

        [Test]
        public void Throw_When_Null_Argument()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DeleteCommand();

            Action test = () => command.Execute(null);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Throw_When_Fewer_Params_Than_Expected_Are_Passed()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DeleteCommand();

            command.Input = new string[] { "1" }; 
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        
        [TestCase("hi", "2")]
        [TestCase("1", "hi")]
        public void Throw_When_An_Expected_Param_Is_Of_Invalid_Format(string arg1, string arg2)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new DeleteCommand();

            command.Input = new string[] { arg1, arg2 };
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Throw_When_No_Canvas_Exist()
        {
            var command = new DeleteCommand();

            command.Input = new string[] { "1", "1" };
            Action test = () => command.Execute(null);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Call_Canvas_BucketFill_When_Params_Are_Fine()
        {
            var expectedTarget = new Point(1, 1);
            var expectedAdjustedTarget = new Point(1 - 1, 1 - 1); //adjust 1-based coordinates to 0-based
            var args = new string[] { expectedTarget.X.ToString(), expectedTarget.Y.ToString() };

            var mockCanvas = new Mock<ICanvas>();

            var command = new DeleteCommand();

            command.Input = args;
            command.Execute(mockCanvas.Object);

            mockCanvas.Verify(x => x.Delete(expectedAdjustedTarget), Times.Exactly(1), "DeleteCommand doesn't call Canvas.Delete with correct args");
        }
    }
}
