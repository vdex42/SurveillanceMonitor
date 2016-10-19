using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.ExtensionMethods;

namespace SurveillanceMonitor.Services
{
    public class TokenReader
    {
        private int _buffersize;
        private string _tokenStartString;
        private string _tokenEndString;
        private long bytesRead = 0;

        public TokenReader(int buffersize, string tokenStartString, string tokenEndString)
        {
            _buffersize = buffersize;
            _tokenStartString = tokenStartString;
            _tokenEndString = tokenEndString;
        }

        public static Action OnBeforeBufferReload { get; set; } = () => { };

        
        public IEnumerable<string> GetNextToken(Stream stream)
        {            
            var buffer1 = new byte[_buffersize];
            int tokenStart = 0;
            //_tokenStartString = "<EventNotificationAlert";
            //_tokenEndString = "</EventNotificationAlert>";
            var startMatcher = new TokenMatcher(_tokenStartString);
            var endMatcher = new TokenMatcher(_tokenEndString);
            var token = "";
            int len;
            var watch = new Stopwatch();
            watch.Start();
            Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
            while ((len = stream.Read(buffer1, 0, buffer1.Length) )> 0)
            {
                bytesRead += len;
                if(watch.ElapsedMilliseconds / 1000>0)
                Log.Write(LogEventLevel.Debug, $"bytes read {bytesRead}B  {bytesRead/1024}KB  {bytesRead / 1024/1024}MB {(bytesRead )/(watch.ElapsedMilliseconds/1000)}B/S ");
                Task.Delay(TimeSpan.FromMilliseconds(5000)).Wait();
                tokenStart = 0;
                for (int i = 0; i < len; i++)
                {

                    if (startMatcher.Process(buffer1[i]))
                    {
                        token = _tokenStartString;
                        tokenStart = i+1;
                        continue;
                    }

                    if (endMatcher.Process(buffer1[i]))
                    {
                        token += Encoding.ASCII.GetString(buffer1.SubArray(tokenStart, i));
                        yield return token;
                        token = "";
                        continue;
                    }

                    if (i == buffer1.Length - 1)
                    {
                        token += Encoding.ASCII.GetString(buffer1.SubArray(tokenStart, i));
                        tokenStart=0;
                        
                    }
                }
              //  OnBeforeBufferReload();

            }
        }


        private class TokenMatcher
        {
            private readonly byte[] _find;
            private int _lastMatch = 0;

            public TokenMatcher(string find)
            {
                _find = Encoding.ASCII.GetBytes(find);
            }

            public bool Process(byte character)
            {
                if (_find[_lastMatch] == character)
                {
                    _lastMatch++;
                    if (_lastMatch == _find.Length)
                    {
                        _lastMatch = 0;
                        return true;
                    }
                    return false;
                }

                _lastMatch = 0;
                return false;

            }
        }
    }
}
