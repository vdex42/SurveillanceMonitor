using System;
using System.IO;
using FluentAssertions;
using Moq;
using Serilog;
using SurveillanceMonitor.Infrastructure;
using SurveillanceMonitor.Infrastructure.Config;
using SurveillanceMonitor.Services;
using Xunit;

namespace SurveillanceMonitor.Tests.Services
{
    public class VideoDumperTests
    {
        private const string TestresultMpg = "./testResult.mpg";

        public VideoDumperTests()
        {
            var finfo = new FileInfo(TestresultMpg);
            if(finfo.Exists)
                finfo.Delete();
        }

        [Fact]        
        public void ShouldDumpVideo()
        {
            var logMock = new Mock<ILogger>();
            var settings = SettingLoader.Load();
            var sut = new VideoDumper(logMock.Object, settings);
            sut.DumpVideo(TimeSpan.FromSeconds(4), TestresultMpg,settings.Cameras[0].RtspSource).Wait();
            var finfo = new FileInfo(TestresultMpg);
            finfo.Exists.Should().BeTrue();
            finfo.Length.Should().BeGreaterThan(0);

        }
    }
}
