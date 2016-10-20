using System;
using System.IO;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.ExtensionMethods;
using SurveillanceMonitor.Infrastructure.Config;
using Vlc.DotNet.Core;

namespace SurveillanceMonitor.Services.AlarmHandler
{
    public class VideoDumper : IAlarmHandler
    {
        private readonly ILogger _logger;
        private readonly SurveillanceMonitorConfig _settings;
        private readonly SurveillanceMonitorConfig.SurveillanceMonitorCamera _camera;
        private readonly string _videoDumpDirectory;
        private readonly int _recordForSeconds;
        private int _sleepFor;

        public void ExtendAlarm()
        {
            if (_sleepFor <= 1) _sleepFor = _recordForSeconds;
        }

        public bool IsHandlerBusy { get; private set; }

        public VideoDumper(ILogger logger, SurveillanceMonitorConfig settings, SurveillanceMonitorConfig.SurveillanceMonitorCamera camera, string videoDumpDirectory, string recordForSeconds )
        {
            _logger = logger;
            _settings = settings;
            _camera = camera;
            _videoDumpDirectory = videoDumpDirectory;
            _recordForSeconds = Convert.ToInt32(recordForSeconds);
            IsHandlerBusy = false;
        }

        public void AlarmActivated( )
        {
            IsHandlerBusy = true;
            _sleepFor = _recordForSeconds;
            var saveTo = new FileInfo( Path.Combine(_videoDumpDirectory,$"{DateTime.Now.ToFileDateTime()}.mpg"));
            if(!Directory.Exists(saveTo.DirectoryName))
                Directory.CreateDirectory(saveTo.DirectoryName);
            

            var player = new VlcMediaPlayer(new System.IO.DirectoryInfo(_settings.VlcFolder));
            player.SetMedia(_camera.RtspSource, $":sout=#transcode{{acodec=mp3}}:duplicate{{dst=std{{access=file,mux=ts,dst=\'{saveTo.FullName}\'}}}}");
            player.EncounteredError += PlayerEncounteredError;
            _logger.Write(LogEventLevel.Information, $"Saving video to {saveTo.FullName} for {_recordForSeconds} ");
            player.Play();
            while (_sleepFor>0)
            {
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                _sleepFor--;
            }
            
            _logger.Write(LogEventLevel.Information, $"Stopping log write to {saveTo.FullName}");
            player.Stop();
            IsHandlerBusy = false;
        }

        private void PlayerEncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            _logger.Write(LogEventLevel.Warning, $"VLC enountetered an error {e}");
        }

      
    }
}
