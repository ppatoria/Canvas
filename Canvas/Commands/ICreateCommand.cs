using CanvasApplication.Models;

namespace CanvasApplication.Commands
{
    public interface ICreateCommand : IInput
    {
        ICanvas Execute();
    }
}
