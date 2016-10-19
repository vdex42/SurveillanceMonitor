using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using SurveillanceMonitor.Services;
using Xunit;

namespace SurveillanceMonitor.Tests.Services
{
    
    public class MiniHttpStreamReaderTests
    {
        [Fact]
        public void ShouldReadHttpPacket()
        {
            for (int bufferSize = 51; bufferSize < 256; bufferSize++)
            {
                string input = @"Text received : POST / HTTP/1.1
                                    Content-Type: application/xml; charset=""UTF-8"" 
                                    Content-Length:38 

<?xml version=""1.0"" encoding=""UTF-8""?>";
                var memStream = new MemoryStream();
                memStream.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
                memStream.Position = 0;
                var sut = new MiniHttpStreamReader(memStream, bufferSize);
                string header = sut.ReadHeader();
                header.Should().Be(@"Text received : POST / HTTP/1.1
                                    Content-Type: application/xml; charset=""UTF-8"" 
                                    Content-Length:38 ", $"buffer size {bufferSize} should work");
                string body = sut.ReadBody();
                body.Should().Be(@"<?xml version=""1.0"" encoding=""UTF-8""?>", $"buffer size {bufferSize} should work");
            }
        }
    }
}
