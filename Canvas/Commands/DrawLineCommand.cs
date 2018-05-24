using CanvasApplication.Models;
using System;
using CommandLine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CanvasApplication.Commands
{
    public class Coordinates
    {
        public uint X1 { get; set; }
        public uint Y1 { get; set; }
        public uint X2 { get; set; }
        public uint Y2 { get; set; }

        //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
        public Point StartPoint() => new Point(X1 - 1, Y1 - 1);
        public Point EndPoint() => new Point(X2 - 1, Y2 - 1);
    }

    public class CoordinateParser
    {
        public Coordinates Parse(IEnumerable<string> input)
        {
            Contract.Requires<ArgumentNullException>(input != null, "options can't be null");
            Contract.Requires<ArgumentException>(input.Count() == 4, "This command expects 4 arguments but received less than that.");
            Contract.Ensures(Contract.Result<Coordinates>() != null);

            try
            {
                return new Coordinates
                {
                    X1 = (uint.Parse(input.ElementAt(0))),
                    Y1 = (uint.Parse(input.ElementAt(1))),
                    X2 = (uint.Parse(input.ElementAt(2))),
                    Y2 = (uint.Parse(input.ElementAt(3)))
                }; 
            }
            catch(Exception e)
            {
                throw new ArgumentException("There is some invalid arguments.All 4 arguments should be positive integers", e);
            }
        }
    }
    
    [Verb("L", HelpText = "Draw line.")]
    public class DrawLineCommand : IDrawCommand
    {
        [Value(0)]
        public IEnumerable<string> Input { get; set; }

        public void Execute(ICanvas canvas)
        {
            var lineCoordinates = new CoordinateParser().Parse(Input);
            var line = new Line(lineCoordinates.StartPoint(), lineCoordinates.EndPoint());
            canvas.DrawLine(line);
        }
    }
}