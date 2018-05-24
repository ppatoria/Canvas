using CanvasApplication.Models;
using System;
using CommandLine;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace CanvasApplication.Commands
{

    public class BucketFillParameterParser 
    {
        public IBucketFillParamerters Parse(IEnumerable<string> input)
        {
            Contract.Requires<ArgumentNullException>(input != null, "input can't be null");
            Contract.Requires<ArgumentException>(input.Count() == 3, "This command expects 3 arguments but only received less than that.");
            Contract.Ensures(Contract.Result<IBucketFillParamerters>() != null);

            try
            {
                return new BucketFillParameters
                {
                    //adjust as coordinates are passed 1-based but the underlying canvas expects them 0-based
                    Point = new Point(uint.Parse(input.ElementAt(0)) -1 ,uint.Parse(input.ElementAt(1)) -1),
                    Color = char.Parse(input.ElementAt(2))
                };
            }
            catch (FormatException)
            {
                throw new ArgumentException("There is some invalid arguments. First two arguments should be positive integers and other should be a character specifying color.");
            }
        }
    }

    [Verb("B", HelpText = "Bulk fill.")]
    public class BucketFillCommand : IDrawCommand
    {
        [Value(0)]
        public IEnumerable<string> Input { get; set; }

        public void Execute(ICanvas canvas)
        {
            var bucketFillParameters = new BucketFillParameterParser().Parse(Input);

            canvas.BucketFill(bucketFillParameters);
        }
    }
}