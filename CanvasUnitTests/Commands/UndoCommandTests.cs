using CanvasApplication.Commands;
using NUnit.Framework;
using System;
using FluentAssertions;
using CanvasApplication.Models;
using Moq;

namespace CS.CanvaApp.UnitTests.Commands
{
    [TestFixture]
    public class UndoCommand_Should
    {
        [Test]
        public void Throws_On_Creation_If_Null_Arg()
        {
            Action test = () => new UndoCommand();

            test.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Throws_When_No_Canvas_Exist()
        {
            var command = new UndoCommand();

            Action test = () => command.Execute(null);

            test.Should().Throw<ArgumentException>()
                .WithMessage("No canvas exist. Please create one then try again.");
        }

        [Test]
        public void Calls_Canvas_Undo()
        {
            var mockCanvas = new Mock<ICanvas>();

            var command = new UndoCommand();
                        
            command.Execute(mockCanvas.Object);

            mockCanvas.Verify(x => x.Undo(), Times.Once);
        }
    }
}
