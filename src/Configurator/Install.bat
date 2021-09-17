@echo OFF

echo Checking build...

set ServiceName=.NET Pkcs11 Token Service
set ServicePath=deploy\Service\Service.Worker.exe

if not exist %ServicePath% (
	echo Deploy directory doesn't exist. Run Build.bat first.
	goto final
)

echo Instaling service... %ServiceName%


sc.exe create "%ServiceName%" start=auto binpath="%CD%\%ServicePath%"
sc.exe description "%ServiceName%" "Represents the core processor of the pkcs11 C++ library deployed with this service."
sc.exe start "%ServiceName%"

echo:
echo Done.
:final