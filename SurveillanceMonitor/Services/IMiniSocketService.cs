using System.IO;

namespace SurveillanceMonitor.Services
{
    public interface IMiniSocketService
    {
        void Listen(int port, int backlog);
        Stream GetNextRequest();
        void Stop();
    }
}