using System;
using CanvasApplication.Models;
using CommandLine;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Commands
{
    [Verb("UndoOption", HelpText = "Undo")]
    public class UndoCommand : IUndoCommand
    {
        public void Execute(ICanvas canvas)
        {
            canvas.Undo();
        }
    }
}
