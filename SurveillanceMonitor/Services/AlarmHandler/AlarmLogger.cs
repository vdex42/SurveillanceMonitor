using Serilog;
using SurveillanceMonitor.Infrastructure.Config;

namespace SurveillanceMonitor.Services.AlarmHandler
{
    public class AlarmLogger : IAlarmHandler
    {

        private readonly ILogger _logger;
        private SurveillanceMonitorConfig.SurveillanceMonitorCamera _camera;

        public AlarmLogger(ILogger logger, SurveillanceMonitorConfig.SurveillanceMonitorCamera camera)
        {
            _logger = logger;
            _camera = camera;
        }

        public void AlarmActivated()
        {
            _logger.Warning($"Alarm activated on {_camera.CameraHttpUrl}");
            IsHandlerBusy = false;
        }

        public void ExtendAlarm()
        {            
        }

        public bool IsHandlerBusy { get; private set; }
    }
}
