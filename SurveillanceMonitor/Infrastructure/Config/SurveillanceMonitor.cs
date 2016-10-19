using System.Xml.Serialization;

namespace SurveillanceMonitor.Infrastructure.Config
{
    /// <remarks />
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false, ElementName = "surveillanceMonitor")]
    public class SurveillanceMonitor
    {
        //[XmlElement(ElementName = "cameras")]
        [XmlArray("cameras")]
        [XmlArrayItem("camera", IsNullable = false)]
        public SurveillanceMonitorCamera[] Cameras { get; set; }

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
            public string VideoDumpDirectory { get; set; }

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
        }
    }
}