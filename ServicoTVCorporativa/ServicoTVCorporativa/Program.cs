using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            
            /*    ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            */
            
            
                Service1 service = new Service1();
                try
                {
                    Thread thread = new Thread(service.Motor);
                    thread.Start();
                }
                catch (Exception Ex)
                {
                }
            


        }
    }
}
