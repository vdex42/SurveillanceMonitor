using System;
using System.Threading.Tasks;

namespace SurveillanceMonitor.Services
{
    public interface IVideoDumper
    {
        Task DumpVideo(TimeSpan recordFor, string dumpFile, string stream);
    }
}