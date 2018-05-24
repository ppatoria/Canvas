using CanvasApplication.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace CanvasApplication.Commands
{
    public interface IUndoCommand : IModifyCanvasCommand
    {}
}
