
[Setup]
AppName=SurveillanceMonitor
AppVersion=0.9
DefaultDirName={pf}\SurveillanceMonitor
DisableProgramGroupPage=yes
;UninstallDisplayIcon={app}\MyProg.exe

[Files]
Source: "bin/Release/Serilog.dll"; DestDir: "{app}"
Source: "bin/Release/Serilog.Sinks.ColoredConsole.dll"; DestDir: "{app}"
Source: "bin/Release/Serilog.Sinks.File.dll"; DestDir: "{app}"
Source: "bin/Release/SurveillanceMonitor.exe.config"; DestDir: "{app}"
Source: "bin/Release/SurveillanceMonitor.vshost.exe"; DestDir: "{app}"
Source: "bin/Release/SurveillanceMonitor.vshost.exe.config"; DestDir: "{app}"
Source: "bin/Release/SurveillanceMonitor.vshost.exe.manifest"; DestDir: "{app}"
Source: "bin/Release/Topshelf.dll"; DestDir: "{app}"
Source: "bin/Release/Topshelf.Serilog.dll"; DestDir: "{app}"
Source: "bin/Release/Topshelf.Serilog.xml"; DestDir: "{app}"
Source: "bin/Release/Vlc.DotNet.Core.dll"; DestDir: "{app}"
Source: "bin/Release/Vlc.DotNet.Core.Interops.dll"; DestDir: "{app}"
Source: "bin/Release/SurveillanceMonitor.exe"; DestDir: "{app}";  BeforeInstall: BeforeMyProgInstall()

[Run]
Filename: {app}\SurveillanceMonitor.exe; Flags: runhidden; Parameters: "install start"

[UninstallRun]
Filename: {app}\SurveillanceMonitor.exe; Flags: runhidden; Parameters: "stop uninstall"


[UninstallDelete]
Type: filesandordirs; Name: "{app}"

[Code]
var
  Page: TWizardPage;
  Memo: TNewMemo;    
  SettingsInfo : TNewStaticText;
  LightMsgPage: TOutputMsgWizardPage;
  KeyPage: TInputQueryWizardPage;
  ProgressPage: TOutputProgressWizardPage;
  vlcFolder: String;
      
procedure ExitProcess(uExitCode: Integer);
  external 'ExitProcess@kernel32.dll stdcall';

procedure InitializeWizard;
begin
  Page := CreateCustomPage(wpWelcome, 'Settings', 'Settings for your cameras and how they should react when an alarm is activated. ');  
  
  SettingsInfo := TNewStaticText.Create(Page);  
  SettingsInfo.Caption := 'To change these settings later, edit the ''MonitorSettings.xml'''#13#10' file in the installation folder and restart the SurveillanceMonitor service';
  SettingsInfo.AutoSize := True;
  SettingsInfo.Parent := Page.Surface;
    
  vlcFolder := 'C:\Program Files (x86)\VideoLAN\VLC';
  if(not BrowseForFolder('SurveillanceMonitor is dependant on VideoLAN'#13#10'Please select the VLC program folder'#13#10'Usually: "'+ vlcFolder +'"', vlcFolder, false)) then
     ExitProcess(0);  

  Memo := TNewMemo.Create(Page);
  Memo.Top := SettingsInfo.Top + SettingsInfo.Height + ScaleY(8);
  Memo.Width := Page.SurfaceWidth;
  Memo.Height := ScaleY(150);
  Memo.ScrollBars := ssVertical;
  Memo.Text := '<?xml version="1.0" encoding="utf-8"?>'#13#10'' +
  '<surveillanceMonitor'#13#10'' +
  '  VlcFolder="' +  vlcFolder +'"'#13#10'' +
  '  CallbackIp="192.168.0.102">'#13#10'' +
  '  <cameras>'#13#10'' +
  '    <camera'#13#10'' +
  '      RtspSource="rtsp://admin:password@192.168.0.103:554/ISAPI/Streaming/channels/101"'#13#10'' +
  '      CameraHttpUrl="http://192.168.0.103:8040"'#13#10'' +
  '      CameraUserName="admin"'#13#10'' +
  '      CameraPassword="password"'#13#10'' +
  '      CallbackPort="8080">'#13#10'' +
  '    </camera>'#13#10'' +
  '  </cameras>'#13#10'' +
  '  <alarmActions>'#13#10'' +
  '    <alarmAction Type="VideoDumper">'#13#10'' +
  '      <setting Key="videoDumpDirectory" Value=".\videos\" ></setting>'#13#10'' +
  '      <setting Key="recordForSeconds" Value="30" ></setting>'#13#10'' +
  '    </alarmAction>'#13#10'' +
  '  </alarmActions>'#13#10'' +  
  '</surveillanceMonitor>'#13#10'';
  Memo.Parent := Page.Surface;
    
end;

procedure BeforeMyProgInstall();
begin  
  SaveStringToFile(ExpandConstant('{app}')+ '\MonitorSettings.xml',Memo.Text, False);
end;




