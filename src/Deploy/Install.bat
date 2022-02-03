@echo OFF

echo Checking build...

set ServiceName=.NET Pkcs11 Token Service
set ServiceConfiguratorInstallerPath=deploy\ConfigurationAPI
set ServiceInstallerPath=deploy\Service
set ServiceInstallDirectory=%USERPROFILE%\NETPkcs11TokenService
set ServiceConfigurationInstallDirectory=%ServiceInstallDirectory%\ConfiguratorAPI
set ServiceInstallFile=%ServiceInstallDirectory%\Service.Worker.exe


if not exist %ServiceInstallerPath% (
	echo Deploy directory doesn't exist. Run Build.bat first.
	goto final
)

:create_user_files
echo:

echo Installing required files into the system...

if exist %ServiceInstallDirectory% (
		rmdir /q /s %ServiceInstallDirectory%
)  

mkdir %ServiceInstallDirectory%
xcopy %ServiceInstallerPath% %ServiceInstallDirectory% /S/E/H/Y/Q
if exist %ServiceConfiguratorInstallerPath% (
	echo Installing configurator API...
	
	mkdir %ServiceConfigurationInstallDirectory%
	xcopy %ServiceConfiguratorInstallerPath% %ServiceConfigurationInstallDirectory% /S/E/H/Y/Q
)

echo Files installed...

:install_service
echo:


echo Instaling service... %ServiceName%


sc.exe create "%ServiceName%" start=auto binpath="%ServiceInstallFile%"
sc.exe description "%ServiceName%" "Represents the core processor of the pkcs11 C++ library deployed with this service."
sc.exe start "%ServiceName%"

echo:
echo Done.
 
:final