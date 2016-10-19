using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace SurveillanceMonitor.Services
{
    public class MiniWebServer
    {
        private const int BufferSize = 1024;

        public delegate void OnPageReceived(object sender, string page);
        public event OnPageReceived PageReceivedEvent;
        private readonly ILogger _logger;
        private readonly IMiniSocketService _miniSocketService;
        private bool _bound;

        public MiniWebServer(ILogger logger, IMiniSocketService miniSocketService)
        {
            _logger = logger;
            _miniSocketService = miniSocketService;
        }


        public void Bind(int port)
        {                                
            _miniSocketService.Listen(port, 10);
            _bound = true;
            while (true)
            {
                _logger.Write(LogEventLevel.Information, $"Waiting for a connection... port:{port}");
                // Program is suspended while waiting for an incoming connection.

                try
                {
                    using (Stream requestStream = _miniSocketService.GetNextRequest())
                    {
                        MiniHttpStreamReader streamReader = new MiniHttpStreamReader(requestStream, BufferSize);
                        streamReader.ReadHeader();
                        string body = streamReader.ReadBody();
                        PageReceivedEvent?.Invoke(this, body);
                    }                                        
                }
                catch (Exception exception)
                {
                    _logger.Write(LogEventLevel.Error, $"MiniWebServer Error", exception);                    
                }                                

            }
        }


        public void Stop()
        {
            _bound = false;
            _miniSocketService.Stop();
        }        
    }
}
