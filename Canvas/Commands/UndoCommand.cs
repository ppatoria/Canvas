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
            Contract.Requires(canvas != null,"No canvas exist. Please create one then try again.");
            canvas.Undo();
        }
    }
}
