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

        [XmlAttribute]
        public ushort CallbackPort { get; set; }

        /// <remarks />
        [XmlType(AnonymousType = true)]
        public class SurveillanceMonitorCamera
        {
            /// <remarks />
            [XmlAttribute]
            public string RtspSource { get; set; }

            /// <remarks />
            [XmlAttribute]
            public string CameraHttpUrl { get; set; }

            /// <remarks />
            [XmlAttribute]
            public string CameraUserName { get; set; }

            /// <remarks />
            [XmlAttribute]
            public string CameraPassword { get; set; }

            public int Id { get; set; }
        }

        [XmlType(AnonymousType = true)]
        public class SurveillanceMonitorAlarmAction
        {
            //[XmlArray("settings")]
            //[XmlArrayItem("setting", IsNullable = false)]
            [XmlElement("setting")]
            public SurveillanceMonitorAlarmActionSetting[] Settings { get; set; }
                        
            [XmlAttribute]
            public string Type { get; set; }

            [XmlType(AnonymousType = true)]
            public class SurveillanceMonitorAlarmActionSetting
            {
                /// <remarks />
                [XmlAttribute]
                public string Key { get; set; }

                /// <remarks />
                [XmlAttribute]
                public string Value { get; set; }
            }
        }
    }
}