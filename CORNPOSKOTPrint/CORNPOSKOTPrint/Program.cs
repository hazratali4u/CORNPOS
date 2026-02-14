using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace CORNPOSKOTPrint
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new KOTPrint()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
