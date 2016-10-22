using System.IO;
using System.Net;

namespace SurveillanceMonitor.Services
{
    public interface IMiniSocketService
    {
        void Listen(int port, int backlog);
        Stream GetNextRequest();
        void Stop();
        IPAddress IpAddress { get; }
    }
}