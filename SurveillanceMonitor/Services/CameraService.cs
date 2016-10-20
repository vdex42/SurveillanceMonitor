using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SurveillanceMonitor.Services
{
    public class CameraService : ICameraService
    {
        private readonly Infrastructure.Config.SurveillanceMonitorConfig.SurveillanceMonitorCamera _cameraSettings;

        public CameraService(Infrastructure.Config.SurveillanceMonitorConfig.SurveillanceMonitorCamera cameraSettings)
        {
            _cameraSettings = cameraSettings;
        }

        public async Task SetAlarmCallbackUrlAsync(string callbackHostIp, int callbackHostPort)
        {
            await SetNotificationHttpHost( $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<HttpHostNotification version=""2.0"" xmlns=""http://www.std-cgi.com/ver20/XMLSchema"">
<id>1</id>
<url>/</url>
<protocolType>HTTP</protocolType>
<parameterFormatType>XML</parameterFormatType>
<addressingFormatType>ipaddress</addressingFormatType>
<ipAddress>{callbackHostIp}</ipAddress>
<portNo>{callbackHostPort}</portNo>
<userName></userName>
<httpAuthenticationMethod>none</httpAuthenticationMethod>
</HttpHostNotification>
");
        }

        public async Task ClearCallbackUrlAsync()
        {
            await SetNotificationHttpHost(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<HttpHostNotification version=""2.0"" xmlns=""http://www.std-cgi.com/ver20/XMLSchema"">
<id>1</id>
<url>/</url>
<protocolType>HTTP</protocolType>
<parameterFormatType>XML</parameterFormatType>
<addressingFormatType>hostname</addressingFormatType>
<hostName></hostName>
<portNo>1</portNo>
<userName></userName>
<httpAuthenticationMethod>none</httpAuthenticationMethod>
</HttpHostNotification>
");
        }

        private async Task SetNotificationHttpHost(string postContent)
        {
            var cameraHttpUrl = new Uri(_cameraSettings.CameraHttpUrl);
            var url = new Uri(cameraHttpUrl, "/ISAPI/Event/notification/httpHosts/1");
            var credCache = new System.Net.CredentialCache();
            credCache.Add(cameraHttpUrl, "Digest", new System.Net.NetworkCredential(_cameraSettings.CameraUserName, _cameraSettings.CameraPassword));
            using (var httpClient = new HttpClient(new HttpClientHandler {Credentials = credCache}))
            {
                var content =
                    new StringContent(
                        postContent);
                var response = await httpClient.PutAsync(url, content);


                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        $"Could not initialise camera {response.StatusCode} {await response.Content.ReadAsStringAsync()} ");
                }
            }
        }
    }
}
