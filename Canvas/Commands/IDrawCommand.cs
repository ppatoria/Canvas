using CanvasApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Commands
{
    [ContractClass(typeof(ModifyCanvasCommandContracts))]
    public interface IModifyCanvasCommand
    {
        void Execute(ICanvas canvas);
    }

    [ContractClassFor(typeof(IModifyCanvasCommand))]
    public abstract class ModifyCanvasCommandContracts : IModifyCanvasCommand
    {
        void IModifyCanvasCommand.Execute(ICanvas canvas)
        {
            Contract.Requires<ArgumentNullException>(canvas != null, "No canvas exist. Please create one then try again.");            
        }
    }


    [ContractClass(typeof(InputCommandContracts))]
    public interface IInput
    {
        IEnumerable<string> Input { get; set; }
    }
    [ContractClassFor(typeof(IInput))]
    public abstract class InputCommandContracts : IInput
    {
        IEnumerable<string> IInput.Input
        {
            get;set;
        }
    }

    public interface IDrawCommand : IModifyCanvasCommand, IInput
    {}

}
