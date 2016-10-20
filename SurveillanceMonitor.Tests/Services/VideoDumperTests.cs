using System.IO;
using System.Linq;
using Castle.Core.Internal;
using FluentAssertions;
using Moq;
using Serilog;
using SurveillanceMonitor.Infrastructure.Config;
using SurveillanceMonitor.Services.AlarmHandler;
using Xunit;

namespace SurveillanceMonitor.Tests.Services
{
    public class VideoDumperTests
    {
        private const string TestresultDir = "./testResult";

        public VideoDumperTests()
        {
            var finfo = new DirectoryInfo(TestresultDir);
            if (finfo.Exists)
            {
                finfo.GetFiles("*").ForEach(f=>f.Delete());                
            }
        }

        [Fact]        
        public void ShouldDumpVideo()
        {
            var logMock = new Mock<ILogger>();
            var settings = SettingLoader.Load();
            var sut = new VideoDumper(logMock.Object, settings, settings.Cameras[0], TestresultDir,"4");
            sut.AlarmActivated();
            var finfo = new DirectoryInfo(TestresultDir);
            var file =finfo.GetFiles("*").First();            
            file.Length.Should().BeGreaterThan(0);

        }
    }
}
