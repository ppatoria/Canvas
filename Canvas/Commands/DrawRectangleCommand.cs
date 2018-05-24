using CanvasApplication.Models;
using System;
using CommandLine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Commands
{
    [Verb("R", HelpText = "Draw rectangle")]
    public class DrawRectangleCommand : IDrawCommand
    {
        [Value(0)]
        public IEnumerable<string> Input { get; set; }
        public void Execute(ICanvas canvas)
        {
            var rectangeCoordinates = new CoordinateParser().Parse(Input);
            var rectangle = new Rectangle(rectangeCoordinates.StartPoint(), rectangeCoordinates.EndPoint());
            canvas.DrawRectangle(rectangle);
        }
    }
}