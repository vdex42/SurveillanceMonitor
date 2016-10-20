namespace SurveillanceMonitor.Services
{
    public interface IAlarmHandler
    {
        void AlarmActivated();
        void ExtendAlarm();
        bool IsHandlerBusy { get; }
    }
}