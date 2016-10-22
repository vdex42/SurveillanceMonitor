# Surveillance Monitor

_Disclaimer: I have written this primarily for my own needs. I make no claims as to the stability of the software, and take no responsibility for any damage it may cause_

SurveillanceMonitor is a windows service that connects to IP Cameras that run the HIKVision software. This should also work for any OEMs that run the same camera software. e.g. Annke.

## Pre-Requisites
* Windows only. 
* [VideoLan](http://www.videolan.org/) should be installed 
* .NET Framework
* Open ports/Port forwarding so that this application can connect to the camera, and so that the camera can connect to this application

## Capabilities
* Windows service that runs in the background
* On start, connects to the camera, and instructs the camera to send alerts to this application's IP address
* Opens a port and listens for any alarms sent to the service's IP address.
* On an alarm, instructs VLC to capture and save video from the camera for the required duration
* Can monitor multiple Cameras

## Settings
During installation you'll be prompted to enter some XML settings, this will be saved as *MonitorSettings.xml* in the installed folder.

` VlcFolder="C:\Program Files (x86)\VideoLAN\VLC"`
`CallbackIp="192.168.0.102"` is the IP address of the machine running the Surveillance service
`CallbackPort="8080"` is the port that the service should listen on for alerts

For each camera
`RtspSource="rtsp://admin:password@192.168.0.103:554/ISAPI/Streaming/channels/101"` enter the RTSP url to your camera. (you can test this out directly in vlc:  Media > Open Network stream )
`CameraHttpUrl="http://192.168.0.103:80"` HTTP port of the camera. 
`CameraUserName="admin"`
 `CameraPassword="password"`

 And alert actions for the camera. At the moment only the VLC VideoDumper is supported
`<alarmAction Type="VideoDumper">
    <setting Key="videoDumpDirectory" Value=".\videos\" ></setting>
    <setting Key="recordForSeconds" Value="30" ></setting>
</alarmAction>`