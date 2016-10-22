using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.Infrastructure.Config;

namespace SurveillanceMonitor.Services
{
    public class CameraMonitor
    {
        private readonly ICameraService _cameraService;
        private readonly SurveillanceMonitorConfig.SurveillanceMonitorCamera _cameraSettings;
        private readonly SurveillanceMonitorConfig _settings;
        private readonly IAlarmHandlers _alarmHandlers;
        private readonly ILogger _logger;
        private int _backOffSeconds = 1;

        public CameraMonitor(
            SurveillanceMonitorConfig.SurveillanceMonitorCamera cameraSettings,
            SurveillanceMonitorConfig settings, ILogger logger, IAlarmHandlers alarmHandlers)
        {
            _cameraService = new CameraService(cameraSettings);
            _cameraSettings = cameraSettings;
            _settings = settings;
            _logger = logger;
            _alarmHandlers = alarmHandlers;
        }

        public void Start(CancellationToken cancellationToken)
        {
            _logger.Information($"Start Monitoring {_cameraSettings.CameraHttpUrl}");
            _cameraService.SetAlarmCallbackUrlAsync(_settings.CallbackIp, _settings.CallbackPort);

            Task.Run(async () =>
                     {
                         while (!cancellationToken.IsCancellationRequested)
                         {
                             try
                             {                                 
                                 var webServer = new MiniWebServer(_logger, new MiniSocketService());
                                 webServer.PageReceivedEvent += (sender, page) => {
                                     _alarmHandlers.AlarmActivated(_cameraSettings);
                                 };
                                 webServer.Bind(8080);

                             }
                             catch (Exception e)
                             {
                                 _backOffSeconds *= 2;
                                 _logger.Error(e, $"CameraService error  {_cameraSettings.CameraHttpUrl}");                                 
                             }
                             await Task.Delay(TimeSpan.FromSeconds(_backOffSeconds), cancellationToken);

                         }
                     }, cancellationToken);
        }

        public async Task StopAsync()
        {
            await _cameraService.ClearCallbackUrlAsync();
        }
    }
}