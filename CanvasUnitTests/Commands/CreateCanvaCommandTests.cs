using CanvasApplication.Models;
using CanvasApplication.Commands;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace CanvasApplication.UnitTests.Commands
{
    [TestFixture]
    public class CreateCanvasCommand_Should
    {
        [Test]
        public void Throws_On_Creation_When_Null_Arg()
        {
            Action test = () => new CreateCanvasCommand().Execute();

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Throw_On_Execute_With_Null_Args()
        {
            var command = new CreateCanvasCommand();

            Action test = () => command.Execute();

            test.Should().Throw<ArgumentNullException>();
        }

        
        [TestCase("hi", "2")]
        [TestCase("1", "hi")]
        public void Throw_When_An_Expected_Param_Is_Of_Invalid_Format(string arg1, string arg2)
        {
            var canvas = new Canvas(new CanvasDimentions { Width = 100, Height = 100 });

            var command = new CreateCanvasCommand();

            command.Input = new string[] { arg1, arg2 };
            Action test = () => command.Execute();
            
            test.Should().Throw<ArgumentException>()
                .WithMessage("There is some invalid arguments. Both arguments should be positive integers");
        }

        [Test]
        public void Create_Canvas_With_Valid_Args()
        {
            var width = 100;
            var height = 100;
            ICanvas canvas = null;

            var command = new CreateCanvasCommand();

            command.Input = new string[] { width.ToString(), height.ToString() };
            canvas = command.Execute();

            canvas.Should().NotBeNull();
            canvas.Width.Should().Be(width);
            canvas.Height.Should().Be(height);
        }
    }
}