using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace SmsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                var parametr = string.Concat(args);
                switch (parametr)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new[]
                        {Assembly.GetExecutingAssembly().Location});
                        break;

                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new[] { "/u",
                        Assembly.GetExecutingAssembly().Location});
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new SmsErrorService()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
