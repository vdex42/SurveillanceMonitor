using Serilog;
using SurveillanceMonitor.Infrastructure.Config;
using SurveillanceMonitor.Services;
using TinyIoC;

namespace SurveillanceMonitor.Infrastructure
{
    public static class IocSetup
    {
        public static void Setup()
        {
            TinyIoCContainer.Current.RegisterLogger();
            TinyIoCContainer.Current.Register<Monitor>().AsSingleton();
            TinyIoCContainer.Current.Register<IVideoDumper, VideoDumper>().AsSingleton();            
            TinyIoCContainer.Current.RegisterSettings();
            TinyIoCContainer.Current.RegisterLogger();
        }


        public static void RegisterSettings(this TinyIoCContainer current)
        {
            current.Register(SettingLoader.Load());
        }

        public static void RegisterLogger(this TinyIoCContainer current)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\SurveillanceMonitor.log")
                .WriteTo.ColoredConsole()
                .MinimumLevel.Debug()
                .CreateLogger();
            TinyIoCContainer.Current.Register(Log.Logger);
        }
    }
}