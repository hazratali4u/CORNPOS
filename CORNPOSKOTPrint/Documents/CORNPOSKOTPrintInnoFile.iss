[Setup]
AppName=CORNPOSKOTPrint
AppVersion=1.0
DefaultDirName={pf}\CORNPOSKOTPrint
DefaultGroupName=CORNPOSKOTPrint
OutputDir=.
OutputBaseFilename=CORNPOSKOTPrintInstaller
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin


[Files]
Source: "D:\FS\Development\My Apps\CORN POS\Application\CORNPOSKOTPrint\Documents\Publish Folder\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\Uninstall CORNPOSKOTPrint"; Filename: "{uninstallexe}"

[Run]
Filename: "{cmd}"; Parameters: "/C sc create CORNPOSKOTPrint binPath= ""{app}\CORNPOSKOTPrint.exe"" start= auto"; Flags: runhidden runascurrentuser
Filename: "{cmd}"; Parameters: "/C sc start CORNPOSKOTPrint"; Flags: runhidden runascurrentuser

[UninstallRun]
Filename: "{cmd}"; Parameters: "/C sc stop CORNPOSKOTPrint"; Flags: runhidden
Filename: "{cmd}"; Parameters: "/C sc delete CORNPOSKOTPrint"; Flags: runhidden

[UninstallDelete]
Type: filesandordirs; Name: "{app}"
