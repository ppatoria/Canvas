using CanvasApplication.Models;
using System;
using CommandLine;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CanvasApplication.Commands
{
    public class PointParser
    {
        public Point Parse(IEnumerable<object> input)
        {
            Contract.Requires(input != null, "options can't be null");
            Contract.Requires(input.Count() < 2, "This command expects 2 arguments but received less than that");
            Contract.Ensures(Contract.Result<Point>() != null);

            //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
            return new Point((uint)input.ElementAt(0) - 1, (uint)input.ElementAt(1) - 1);
        }
    }

    [Verb("D", HelpText = "Delete")]
    public class DeleteCommand : IDrawCommand
    {
        [Value(0)]
        public IEnumerable<string> Input { get; set;}

        public void Execute(ICanvas canvas)
        {            
            Contract.Requires(canvas == null, "No canvas exist. Please create one then try again.");

            canvas.Delete(new PointParser().Parse(Input));
        }
    }
}