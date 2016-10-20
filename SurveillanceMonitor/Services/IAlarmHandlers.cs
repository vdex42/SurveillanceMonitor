namespace SurveillanceMonitor.Services
{
    public interface IAlarmHandlers
    {
        void Add(IAlarmHandler handler, Infrastructure.Config.SurveillanceMonitorConfig.SurveillanceMonitorCamera camera);
        void Delete(IAlarmHandler handler);
        void AlarmActivated(Infrastructure.Config.SurveillanceMonitorConfig.SurveillanceMonitorCamera camera);
    }
}