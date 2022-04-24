using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApplication
{
    internal class Program
    {
        private static void Main()
        {
            System.ServiceProcess.ServiceBase.Run(new WinService());
        }
    }
}