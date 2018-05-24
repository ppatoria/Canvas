using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanvasApplication.Models;
using CanvasApplication.Commands;
using CommandLine;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Controller
{
    interface IController
    {
        void Handle(IEnumerable<string> input);
    }
    public class Controller : IController
    {
        private ICanvas _canvas;

            void DisplayCanvas ()
            {
                Contract.Requires<ArgumentNullException>(_canvas != null);
                Console.WriteLine(_canvas);
            }

        public void Handle(IEnumerable<string> input)
        {
            Parser.Default.ParseArguments<CreateCanvasCommand, BucketFillCommand, DeleteCommand, DrawLineCommand, DrawRectangleCommand, QuitCommand, UndoCommand>(input)
                        .WithParsed<ICreateCommand>(option => { _canvas = option.Execute(); DisplayCanvas(); })
                        .WithParsed<IDrawCommand>(option => { option.Execute(_canvas); DisplayCanvas(); })
                        .WithParsed<IUndoCommand>(option => { option.Execute(_canvas); DisplayCanvas(); })
                        .WithParsed<IQuitCommand>(option => option.Execute())
                        .WithNotParsed(errs => Console.WriteLine("Incorrect command. Please refer help and try again."));
        }
    }
}
