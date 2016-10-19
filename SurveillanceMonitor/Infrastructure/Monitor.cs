using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.ExtensionMethods;
using SurveillanceMonitor.Services;


namespace SurveillanceMonitor.Infrastructure
{
    class Monitor
    {
        private readonly ILogger _logger;        
        private readonly IVideoDumper _videoDumper;
        private readonly Infrastructure.Config.SurveillanceMonitor _settings;
        private List<CameraMonitor> _monitoredCameras;        
        private CancellationTokenSource _cancellationTokenSource;

        public Monitor(ILogger logger,  IVideoDumper videoDumper, Config.SurveillanceMonitor settings)
        {
            _logger = logger;
            
            _videoDumper = videoDumper;
            _settings = settings;            
            _monitoredCameras = new List<CameraMonitor>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            try
            {

                foreach (var camera in _settings.Cameras)
                {
                    var monitor = new CameraMonitor( camera, _settings, _logger);
                    monitor.Start(_cancellationTokenSource.Token);
                    _monitoredCameras.Add(monitor);
                }

                //var c = new CameraService(_settings, _logger);
                //await c.SubscribeToAlarmsAsync();
                //var webServer = new MiniWebServer(_logger,new MiniSocketService());
                //webServer.PageReceivedEvent += (sender, page) => {
                //    _logger.Write(LogEventLevel.Warning,$"ALARM {page}" );
                //};
                //webServer.Bind(8080);

            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error, e, "CameraService error");
                throw;
            }


            //_videoDumper.DumpVideo(TimeSpan.FromSeconds(10),
            //    _settings.VideoDumpDirectory + $"\\{DateTime.Now.ToFileDateTime()}.mpg",
            //    "").Wait();

        }

        public void Stop()
        {            
            foreach (var monitoredCamera in _monitoredCameras)
            {
                monitoredCamera.StopAsync().Wait(TimeSpan.FromSeconds(3));
            }
            _cancellationTokenSource.Cancel();
        }

    }
}
