using System;
using System.Linq;
using Serilog;
using SurveillanceMonitor.Infrastructure.Config;
using SurveillanceMonitor.Services;
using SurveillanceMonitor.Services.AlarmHandler;
using TinyIoC;

namespace SurveillanceMonitor.Infrastructure
{
    public static class IocSetup
    {
        public static void Setup()
        {
            TinyIoCContainer.Current.RegisterLogger();
            TinyIoCContainer.Current.Register<Monitor>().AsSingleton();            
            TinyIoCContainer.Current.RegisterSettings();
            TinyIoCContainer.Current.RegisterLogger();
            TinyIoCContainer.Current.RegisterAlarmHandlers();
        }


        public static void RegisterSettings(this TinyIoCContainer current)
        {
            var settings = SettingLoader.Load();
            current.Register(settings);
        }

        public static void RegisterLogger(this TinyIoCContainer current)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs\\SurveillanceMonitorConfig.log")
                .WriteTo.ColoredConsole()
                .MinimumLevel.Debug()
                .CreateLogger();
            current.Register(Log.Logger);
        }

        public static void RegisterAlarmHandlers(this TinyIoCContainer current)
        {
            var alarmHandler = new AlarmHandlers();
            var settigs = current.Resolve<Config.SurveillanceMonitorConfig>();
            foreach (var alarmAction in settigs.AlarmActions)
            {
                
                foreach (var camera in settigs.Cameras)
                {
                    var paramDict = alarmAction.Settings.ToDictionary(k => k.Key, v => (object)v.Value);
                    paramDict.Add("camera", camera);
                    var handler = current.Resolve(Type.GetType($"SurveillanceMonitor.Services.AlarmHandler.{alarmAction.Type}"), new NamedParameterOverloads(
                        paramDict));

                    alarmHandler.Add((IAlarmHandler) handler, camera);

                    //Always register a logger
                    var loghandler = current.Resolve(typeof(AlarmLogger), new NamedParameterOverloads(
                        paramDict));
                    alarmHandler.Add((IAlarmHandler)loghandler, camera);

                }
            }
            

            current.Register<IAlarmHandlers>(alarmHandler);            
        }

    }
}