using System.IO;
using System.Xml.Serialization;

namespace SurveillanceMonitor.Infrastructure.Config
{
    public static class SettingLoader
    {
        public static SurveillanceMonitorConfig Load()
        {
            var mySerializer = new XmlSerializer(typeof(SurveillanceMonitorConfig));
            using (var fileStream = new FileStream("./MonitorSettings.xml", FileMode.Open))
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
