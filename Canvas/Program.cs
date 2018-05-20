using CanvasApplication.Models;
using CanvasApplication.Commands;
using System;
using CommandLine;
using System.Diagnostics.Contracts;

namespace CanvasApplication
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ICanvas _canvas = null;

            Action DisplayCanvas = () =>
            {
                Contract.Requires<ArgumentNullException>(_canvas != null);
                Console.WriteLine(_canvas);
            };

            while (true)
            { // TODO handle exception and dispay gracefully
                    Parser.Default.ParseArguments<CreateCanvasCommand, BucketFillCommand, DeleteCommand, DrawLineCommand, DrawRectangleCommand, QuitCommand, UndoCommand>(Console.ReadLine().Split(' '))
                        .WithParsed<ICreateCommand>(option => { _canvas = option.Execute(); DisplayCanvas(); })
                        .WithParsed<IDrawCommand>(option => { option.Execute(_canvas); DisplayCanvas(); })
                        .WithParsed<IUndoCommand>(option => { option.Execute(_canvas); DisplayCanvas(); })
                        .WithParsed<IQuitCommand>(option => option.Execute())
                        .WithNotParsed(errs => Console.WriteLine("Incorrect command. Please refer help and try again."));
            }
        }
    }
}