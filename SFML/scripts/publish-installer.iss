[Setup]
AppName={#appName}
AppVersion=1.0
DefaultDirName={commonpf}\{#DefaultFolder}\{#appName}
DefaultGroupName={#appName}
OutputDir={#OutputPath}
OutputBaseFilename={#appName}-{#SO}-Installer
SetupIconFile={#IconPath}

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Files]
Source: "{#SourcePath}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Tasks]
Name: "desktopicon"; Description: "Criar um atalho na área de trabalho"; GroupDescription: "Adicionais";

[Icons]
Name: "{group}\{#appName}"; Filename: "{app}\{#appName}.exe"
Name: "{autodesktop}\{#appName}"; Filename: "{app}\{#appName}.exe"; Tasks: desktopicon
