using System;
using System.Collections.Generic;
using System.Threading;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.Infrastructure.Config;
using SurveillanceMonitor.Services;


namespace SurveillanceMonitor.Infrastructure
{
    class Monitor
    {
        private readonly ILogger _logger;                
        private readonly SurveillanceMonitorConfig _settings;
        private readonly List<CameraMonitor> _monitoredCameras;        
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IAlarmHandlers _alarmHandlers;

        public Monitor(ILogger logger,  SurveillanceMonitorConfig settings, IAlarmHandlers alarmHandlers)
        {
            _logger = logger;                       
            _settings = settings;
            _alarmHandlers = alarmHandlers;
            _monitoredCameras = new List<CameraMonitor>();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            try
            {
                foreach (var camera in _settings.Cameras)
                {
                    var monitor = new CameraMonitor( camera, _settings, _logger,_alarmHandlers);
                    monitor.Start(_cancellationTokenSource.Token);
                    _monitoredCameras.Add(monitor);
                }
            }
            catch (Exception e)
            {
                _logger.Write(LogEventLevel.Error, e, "CameraService error");
                throw;
            }
            
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
