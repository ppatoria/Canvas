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
        public Point Parse(IEnumerable<string> input)
        {
            Contract.Requires<ArgumentNullException>(input != null, "options can't be null");
            Contract.Requires<ArgumentException>(input.Count() == 2, "This command expects 2 arguments but received less than that");

            try
            {
                //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
                return new Point(uint.Parse(input.ElementAt(0)) - 1, uint.Parse(input.ElementAt(1)) - 1);
            }
            catch(FormatException e)
            {
                throw new ArgumentException("There is some invalid arguments. Both arguments should be positive integers", e);
            }
        }
    }

    [Verb("D", HelpText = "Delete")]
    public class DeleteCommand : IDrawCommand
    {
        [Value(0)]
        public IEnumerable<string> Input { get; set;}

        public void Execute(ICanvas canvas)
        {            
            canvas.Delete(new PointParser().Parse(Input));
        }
    }
}