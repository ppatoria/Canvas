using System;
using CanvasApplication.Models;
using CanvasApplication.Commands;
using CommandLine;
using CanvasApplication.View;
namespace CanvasApplication.Handler
{
    public interface IController
    {
        void Run();
    }
    
    public class Controller : IController
    {
        private readonly IInteractiveView _view;
        public Controller(IInteractiveView view)
        {
            _view = view;
        }

        private ICanvas _canvas;


        public void Run()
        {
            Action DisplayCanvas= () => _view.Display(_canvas.ToString());
            while (true)
            {
                Parser.Default.ParseArguments<CreateCanvasCommand, BucketFillCommand, DeleteCommand, DrawLineCommand, DrawRectangleCommand, QuitCommand, UndoCommand>(_view.GetInput())
                            .WithParsed<ICreateCommand>(option => { _canvas = option.Execute(); DisplayCanvas(); })
                            .WithParsed<IDrawCommand>  (option => { option.Execute(_canvas); DisplayCanvas(); })
                            .WithParsed<IUndoCommand>  (option => { option.Execute(_canvas); DisplayCanvas(); })
                            .WithParsed<IQuitCommand>  (option =>   option.Execute())
                            .WithNotParsed(errs => Console.WriteLine("Incorrect command. Please refer help and try again."));
            }
        }
    }

}
