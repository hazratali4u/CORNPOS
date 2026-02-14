using System.ServiceProcess;

namespace CORNWeightScaleService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new CORNWeightScaleService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
