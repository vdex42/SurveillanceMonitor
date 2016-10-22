using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace SurveillanceMonitor.Services
{
    public class MiniSocketService : IMiniSocketService
    {
        private Socket _listener;
        public IPAddress IpAddress { get; private set; }

        public void Listen(int port, int backlog)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IpAddress = ipHostInfo.AddressList.First(a=>a.AddressFamily== AddressFamily.InterNetwork);
            IPEndPoint localEndPoint = new IPEndPoint(IpAddress, port);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _listener.Bind(localEndPoint);
            _listener.Listen(backlog);
        }

        public Stream GetNextRequest()
        {
            Socket handler = _listener.Accept();
            return new MiniSocketServiceStream(handler);
        }

        public void Stop()
        {
            try
            {
                _listener?.Close();
                _listener?.Shutdown(SocketShutdown.Both);
            }
            catch (Exception)
            {
                                
            }
            
        }


        public class MiniSocketServiceStream : Stream
        {
            private readonly Socket _socket;

            public MiniSocketServiceStream(Socket socket)
            {
                _socket = socket;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override void Close()
            {
                _socket.Close();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return _socket.Receive(buffer, buffer.Length, 0);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => 0;
            public override long Position { get; set; }
        }

    }
}
