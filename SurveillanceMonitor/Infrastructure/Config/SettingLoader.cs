using System.IO;
using System.Xml.Serialization;

namespace SurveillanceMonitor.Infrastructure.Config
{
    public static class SettingLoader
    {
        public static SurveillanceMonitorConfig Load()
        {
            var mySerializer = new XmlSerializer(typeof(SurveillanceMonitorConfig));
            string runningLocation = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName;
            using (var fileStream = new FileStream(Path.Combine(runningLocation,
                "./MonitorSettings.xml"), FileMode.Open))
            {
                var settings = (SurveillanceMonitorConfig)mySerializer.Deserialize(fileStream);
                for (int i = 0; i < settings.Cameras.Length; i++)
                {
                    settings.Cameras[i].Id = i+1;
                }
                return settings;
            }
        }
    }
}
