using System.IO;
using System.Xml.Serialization;

namespace SurveillanceMonitor.Infrastructure.Config
{
    public static class SettingLoader
    {
        public static SurveillanceMonitor Load()
        {
            var mySerializer = new XmlSerializer(typeof(SurveillanceMonitor));
            using (var fileStream = new FileStream("./MonitorSettings.xml", FileMode.Open))
            {
                return (SurveillanceMonitor)mySerializer.Deserialize(fileStream);
            }
        }
    }
}
