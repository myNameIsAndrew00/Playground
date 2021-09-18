@echo OFF 
net.exe session 1>NUL 2>NUL || (Echo This script requires elevated rights. & Exit /b 1)

echo Unistaling service...

set ServiceName=.NET Pkcs11 Token Service
set ServiceInstallDirectory=%USERPROFILE%\NETPkcs11TokenService

sc stop "%ServiceName%"
sc delete "%ServiceName%"

echo Service unistalled...

echo Removing service files...

if exist %ServiceInstallDirectory% (
		rmdir /q /s %ServiceInstallDirectory%
)  

echo Files removed...

echo:
echo Done.