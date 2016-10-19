using System.Threading.Tasks;

namespace SurveillanceMonitor.Services
{
    public interface ICameraService
    {
        Task SetAlarmCallbackUrlAsync(string callbackHostIp, int callbackHostPort);
        Task ClearCallbackUrlAsync();
    }
}