
$appName = "Realms-Of-Storms"
$defaultFolder = "Games"
$outputPath = "C:\Publish\"
$sourcePath = "C:\Publish\$appName-x64"

# Script Publicação
dotnet publish -c Release -r win-x64 --self-contained -p:PublishReadyToRun=true -p:PublishSingleFile=true -p:PublishClickOnce=true -o $sourcePath


# Resolve o caminho do diretorio atual para pegar o script de instalação de o icone
$currentDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

$installerScript = Join-Path $currentDir "publish-installer.iss"

$iconPath = Join-Path -Path $currentDir -ChildPath "..\resources\icon\favicon.ico"


# Script Instalador
& "C:\Program Files (x86)\Inno Setup 6\ISCC.exe" $installerScript /dSO="x64" /dappName=$appName /dSourcePath=$sourcePath /dOutputPath=$outputPath /dDefaultFolder=$defaultFolder /dIconPath=$iconPath