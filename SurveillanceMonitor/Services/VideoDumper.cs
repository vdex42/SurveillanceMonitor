using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using SurveillanceMonitor.Infrastructure;
using Vlc.DotNet.Core;

namespace SurveillanceMonitor.Services
{
    public class VideoDumper : IVideoDumper
    {
        private readonly ILogger _logger;
        private readonly Infrastructure.Config.SurveillanceMonitor _settings;

        public VideoDumper(ILogger logger, Infrastructure.Config.SurveillanceMonitor settings)
        {
            _logger = logger;
            _settings = settings;
        }

        public async Task DumpVideo(TimeSpan recordFor, string dumpFile, string stream)
        {
            
            var saveTo = new FileInfo(dumpFile);
            if(!Directory.Exists(saveTo.DirectoryName))
                Directory.CreateDirectory(saveTo.DirectoryName);
            

            var player = new VlcMediaPlayer(new System.IO.DirectoryInfo(_settings.VlcFolder));
            player.SetMedia(stream,$":sout=#transcode{{acodec=mp3}}:duplicate{{dst=std{{access=file,mux=ts,dst=\'{saveTo.FullName}\'}}}}");
            player.EncounteredError += PlayerEncounteredError;
            _logger.Write(LogEventLevel.Information, $"Saving video to {saveTo.FullName} for {recordFor} ");
            player.Play();            
            await Task.Delay(recordFor);
            _logger.Write(LogEventLevel.Information, $"Stopping log write to {saveTo.FullName}");
            player.Stop();
        }

        private void PlayerEncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            _logger.Write(LogEventLevel.Warning, $"VLC enountetered an error {e}");
        }
    }
}
