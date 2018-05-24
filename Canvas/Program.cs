using CanvasApplication.Models;
using CanvasApplication.Commands;
using System;
using CommandLine;
using System.Diagnostics.Contracts;
using CanvasApplication.Controller;
namespace CanvasApplication
{
    public class Program
    {
        private static void Main()
        {
            var controller = new Controller.Controller();
            while (true)
            { 
                var input = Console.ReadLine().Split(' ');
                controller.Handle(input);
            }
        }
    }

}