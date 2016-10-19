using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SurveillanceMonitor.Services
{
    public class MiniHttpStreamReader
    {
        private readonly Stream _contentStream;
        private readonly int _buffer;
        private string _header;
        private string _body;
        private bool _headerCaller;
        private int _bodyLength;

        public MiniHttpStreamReader(Stream contentStream, int buffer)
        {
            _contentStream = contentStream;
            _buffer = buffer;
        }

        public string ReadHeader()
        {
            _headerCaller = true;
            _header = "";
            _body = "";
            string data = "";
            byte[] bytes = new byte[_buffer];
            int indexOfEnd;
            int read = -1;
            while ((indexOfEnd = data.IndexOf("\r\n\r\n", StringComparison.Ordinal)) < 0 && read != 0)
            {
                read = _contentStream.Read(bytes, 0, bytes.Length);
                data += Encoding.ASCII.GetString(bytes, 0, read);
            }
            if (indexOfEnd < -1)
                throw new Exception("No header found in content " + data);
            _header = data.Substring(0, indexOfEnd);
            _body = data.Substring(indexOfEnd+4);
            _bodyLength = Convert.ToInt32(Regex.Match(_header, "Content-Length:([^\r]*)").Groups[1].Value);
            return _header;
        }

        public string ReadBody()
        {            
            if(!_headerCaller)throw new Exception("Can't read body without calling ReadHeader()");            
            byte[] bytes = new byte[_buffer];            
            int read = -1;
            int contentRead = _body.Length;

            while (contentRead < _bodyLength && read != 0)
            {
                read = _contentStream.Read(bytes, 0, bytes.Length);
                contentRead += read;
                _body += Encoding.ASCII.GetString(bytes, 0, read);
            }
            if (contentRead != _bodyLength)
                throw new Exception($"Content length did not matcher header declared length. Expected {_bodyLength} got {contentRead}. Read {_body} ");
            
            return _body;
        }

    }
}