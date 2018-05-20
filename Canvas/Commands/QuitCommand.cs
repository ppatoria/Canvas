using System;
using CommandLine;
using CanvasApplication.Models;

namespace CanvasApplication.Commands
{
    [Verb("Q", HelpText = "Exit")]
    public class QuitCommand : IQuitCommand
    {
        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}