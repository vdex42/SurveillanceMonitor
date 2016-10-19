using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace SurveillanceMonitor.Services
{
    public class CameraMonitor
    {
        private readonly ICameraService _cameraService;
        private readonly Infrastructure.Config.SurveillanceMonitor.SurveillanceMonitorCamera _cameraSettings;
        private readonly Infrastructure.Config.SurveillanceMonitor _settings;
        private readonly ILogger _logger;
        private int _backOffSeconds = 1;

        public CameraMonitor(
            Infrastructure.Config.SurveillanceMonitor.SurveillanceMonitorCamera cameraSettings,
            Infrastructure.Config.SurveillanceMonitor settings, ILogger logger)
        {
            _cameraService = new CameraService(cameraSettings);
            _cameraSettings = cameraSettings;
            _settings = settings;
            _logger = logger;
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
                                     _logger.Write(LogEventLevel.Warning, $"ALARM {page}");
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