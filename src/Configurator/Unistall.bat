@echo OFF 

echo Unistaling service...

set ServiceName=.NET Pkcs11 Token Service

sc stop "%ServiceName%"
sc delete "%ServiceName%"

echo:
echo Done.