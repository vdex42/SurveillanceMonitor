using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using SurveillanceMonitor.Services;
using Xunit;

namespace SurveillanceMonitor.Tests.Services
{
    public class TokenReaderTests
    {
        [Fact]
        public void GetNextTokenShouldReturnCleanToken()
        {
            for (int bufferSize = 1; bufferSize < 55; bufferSize++)
            {                
                string input = "---<Event></Event>\r\n--boundary\r\nNextBlock";
                var memStream = new MemoryStream();
                memStream.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
                memStream.Position = 0;
                var sut = new TokenReader(bufferSize, "<Event", "</Event>");
                var results = sut.GetNextToken(memStream).ToArray();
                results[0].Should().Be("<Event></Event>", $"buffer size {bufferSize} should work");
                results.Length.Should().Be(1, $"buffer size {bufferSize} should work");

            }

        }
    }
}
