using CanvasApplication.Models;
using CanvasApplication.Commands;
using FluentAssertions;
using NUnit.Framework;
using Moq;
using System;

namespace CanvasApplication.UnitTests.Commands
{
    [TestFixture]
    public class BucketFillCommand_Should
    {

        [Test]
        public void Throw_When_Null_Argument()
        {
            var canvas = new Canvas( new CanvasDimentions { Width = 100, Height = 100 });

            var command = new BucketFillCommand();

            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Throw_When_Fewer_Params_Than_Expected_Are_Passed()
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new BucketFillCommand();

            command.Input = new string[] { "1", "2" };
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        
        [TestCase("hi", "2", "c")]
        [TestCase("1", "hi", "c")]
        [TestCase("1", "3", "string")]
        public void Throw_When_An_Expected_Param_Is_Of_Invalid_Format(string arg1, string arg2, string arg3)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new BucketFillCommand();

            command.Input = new string[] { arg1, arg2, arg3 };
            Action test = () => command.Execute(canvas);

            test.Should().Throw<ArgumentException>();
        }

        [Test]
        public void Throw_When_No_Canvas_Exist()
        {
            var command = new BucketFillCommand();

            command.Input = new string[] { "1", "1", "c" };
            Action test = () => command.Execute(null);

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Call_Canvas_BucketFill_When_Params_Are_Fine()
        {
            uint expected_x = 1;
            uint expected_y = 1;
            var expected_colour = 'c';            

            var args = new string[] 
            {
                expected_x.ToString(),
                expected_y.ToString(),
                expected_colour.ToString()
            };

            var mockCanvas = new Mock<ICanvas>();
            mockCanvas.Setup(m => m.BucketFill(It.IsAny<BucketFillParameters>()));
            var command = new BucketFillCommand();
            command.Input = args;
            command.Execute(mockCanvas.Object);

            var expected_bulk_fill_parameters = new BucketFillParameters
            {
                //adjust 1-based coordinates to 0-based
                Point = new Point(expected_x -1 , expected_y -1),
                Color = expected_colour
            };

            mockCanvas.Verify(x => x.BucketFill(expected_bulk_fill_parameters),
                                Times.Exactly(1),
                                "BucketFillCommand doesn't call Canvas.BucketFill with correct args");
        }
    }
}