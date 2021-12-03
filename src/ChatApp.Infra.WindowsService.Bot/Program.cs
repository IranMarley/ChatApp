namespace ChatApp.Infra.WindowsService.Bot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {   
            #if (!DEBUG)

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Broadcast() 
            };
            ServiceBase.Run(ServicesToRun);
            
            #else

            var service = new Broadcast();

            // Call your method to Debug.
            service.InitializeSelfHosting();
            
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            #endif

        }
    }
}
