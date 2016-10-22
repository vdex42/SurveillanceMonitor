using System.Xml.Serialization;

namespace SurveillanceMonitor.Infrastructure.Config
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "surveillanceMonitor")]
    public class SurveillanceMonitorConfig
    {
        
        [XmlArray("cameras")]
        [XmlArrayItem("camera", IsNullable = false)]
        public SurveillanceMonitorCamera[] Cameras { get; set; }

        [XmlArray("alarmActions")]
        [XmlArrayItem("alarmAction", IsNullable = false)]
        public SurveillanceMonitorAlarmAction[] AlarmActions { get; set; }

        [XmlAttribute]
        public string VlcFolder { get; set; }

        [XmlAttribute]
        public string CallbackIp { get; set; }        

        /// <remarks />
        [XmlType(AnonymousType = true)]
        public class SurveillanceMonitorCamera
        {
            [XmlAttribute]
            public string RtspSource { get; set; }

            [XmlAttribute]
            public string CameraHttpUrl { get; set; }

            [XmlAttribute]
            public string CameraUserName { get; set; }

            [XmlAttribute]
            public string CameraPassword { get; set; }

            [XmlAttribute]
            public ushort CallbackPort { get; set; }

            public int Id { get; set; }
        }

        [XmlType(AnonymousType = true)]
        public class SurveillanceMonitorAlarmAction
        {
            [XmlElement("setting")]
            public SurveillanceMonitorAlarmActionSetting[] Settings { get; set; }
                        
            [XmlAttribute]
            public string Type { get; set; }

            [XmlType(AnonymousType = true)]
            public class SurveillanceMonitorAlarmActionSetting
            {
                [XmlAttribute]
                public string Key { get; set; }

                [XmlAttribute]
                public string Value { get; set; }
            }
        }
    }
}