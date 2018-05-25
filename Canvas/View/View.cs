using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
namespace CanvasApplication.View
{

    [ContractClass(typeof(InteractiveViewContracts))]
    public interface IInteractiveView
    {
        IEnumerable<string> GetInput();
        void Display(string canvas);
    }

    [ContractClassFor(typeof(IInteractiveView))]
    public abstract class InteractiveViewContracts : IInteractiveView
    {
        void IInteractiveView.Display(string canvas)
        {
            Contract.Requires<ArgumentNullException>(!string.IsNullOrEmpty(canvas));
        }

        IEnumerable<string> IInteractiveView.GetInput()
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);            
            return default(IEnumerable<string>);
        }
    }

    public class ConsoleView : IInteractiveView
    {
        public IEnumerable<string> GetInput() => Console.ReadLine().Split(' ');
        public void Display(string canvas)
        {
            Console.WriteLine(canvas);
        }
    }
}
