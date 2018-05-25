using System;
using System.Collections.Generic;
using CanvasApplication.Handler;
using CanvasApplication.View;
namespace CanvasApplication
{
    public class Program
    {
        private static void Main()
        {
            try
            {
                new Controller(new ConsoleView()).Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Unexpected error.", ex);
            }

        }
    }


}