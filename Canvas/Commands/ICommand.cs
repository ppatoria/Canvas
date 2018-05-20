using CanvasApplication.Models;
using System.Collections.Generic;

namespace CanvasApplication.Commands
{
    public interface IUndoCommand
    {
        void Execute(ICanvas canvas);
    }
    public interface ICreateCommand : IInput
    {
        ICanvas Execute();
    }

    public interface IQuitCommand
    {
        void Execute();
    }

    public interface IDrawCommand : IInput
    {
        void Execute(ICanvas canvas);
    }

    public interface IInput
    {
        IEnumerable<string> Input { get; set; }
    }

    //public interface IInputHandler<TInput>
    //{        
    //    IEnumerable<TInput> Input { get; set; }
    //    TResult Parse<TResult>();
    //}


    //public interface IInputParser<TInput>
    //{
    //    IEnumerable<TInput> Input { get; set; }
        
    //}
    //public class SomeParser
    //{
    //    public SomeParser(IEnumerable<string> input)
    //    {

    //    }
    //}
    //public abstract class InputHanlder : IInputHandler<string>
    //{
    //    public IEnumerable<string> Input { get; set; }

    //    public TResult Parse<TResult>() where TResult : IInputParser<string>, new()
    //    {
    //        var obj = new TResult();
    //        obj.Input = Input;
            
    //    }
    //}


}