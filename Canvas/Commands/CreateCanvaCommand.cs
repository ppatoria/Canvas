using System;
using CanvasApplication.Models;
using CommandLine;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Contracts;


namespace CanvasApplication.Commands
{
    public class CanvasSizeParser        
    {
        public ICanvasDimentions Parse(IEnumerable<string> input) 
        {
            Contract.Requires<ArgumentNullException>(input != null);
            Contract.Requires<ArgumentException>(input.Count() < 2, $"This command expects 2 arguments but received less than expected.");

            Contract.Ensures(Contract.Result<ICanvasDimentions>() != null);
            Contract.Ensures(Contract.Result<ICanvasDimentions>().Width >= 0);
            Contract.Ensures(Contract.Result<ICanvasDimentions>().Height >= 0);

            try
            {
                return new CanvasDimentions { Width = uint.Parse(input.ElementAt(0)), Height = uint.Parse(input.ElementAt(1)) };
            }
            catch(FormatException)
            {
                throw new ArgumentException("There is some invalid arguments. Both arguments should be positive integers");
            }
        }
    }

    [Verb("C", HelpText = "Create canvas")]
    public class CreateCanvasCommand : ICreateCommand
    {
        [Value(0, Required = true)]
        public IEnumerable<string> Input { get; set; }
        public ICanvas Execute() => new CanvasApplication.Models.Canvas( new CanvasSizeParser().Parse(Input));
    }
}