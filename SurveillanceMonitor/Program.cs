using SurveillanceMonitor.Infrastructure;
using TinyIoC;
using Topshelf;

namespace SurveillanceMonitor
{
    internal class Program
    {        
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
                            {
                                IocSetup.Setup();
                                x.Service<Monitor>(s =>
                                                   {
                                                       s.ConstructUsing(name => TinyIoCContainer.Current.Resolve<Monitor>());
                                                       s.WhenStarted(tc => tc.Start());
                                                       s.WhenStopped(tc => tc.Stop());
                                                   });
                                x.RunAsLocalSystem();

                                x.SetDescription("Surveillance Monitor for alerting and reactive to alarms");
                                x.SetDisplayName("SurveillanceMonitor");
                                x.SetServiceName("SurveillanceMonitor");
                                x.UseSerilog();
                            });
        }
    }
}