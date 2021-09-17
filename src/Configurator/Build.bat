@echo OFF 

echo -----------------Build started...
:prerequeries
	echo:
	set output=.\deploy
	set serviceOutputPath=.\deploy\Service
	set libraryOutputPath=.\deploy\Library	
	set ServiceLibraryPath=..\Service\Service.Worker
	set LibraryPath=..\Library\sources
	
goto library
	echo Pkcs11 Service output directory: %serviceOutputPath%
	echo Pkcs11 C++ Library output directory: %libraryOutputPath%

	if exist %output% (
		rmdir /q /s %output%
	)  
	mkdir %output%
	mkdir %serviceOutputPath%
	mkdir %libraryOutputPath%

	echo Successufuly created directories...
	echo:
echo -----------------Building service...
:service
	echo:

	:: Step 1: check .net
	echo [1]Checking dotnet version...

	FOR /F "tokens=*" %%x in ('dotnet --version') do set DotNetVersion=%%x

	set DotNetVersion=%DotNetVersion:~0,1%

	if %DotNetVersion% lss 5 ( 
		echo "Dot Net Version must be >=5"
		goto final
	)

:: Step 2: publish service
	echo:
	
	echo [2]Building service...
	dotnet publish %ServiceLibraryPath% --output %serviceOutputPath%
	
	echo:
	echo Service built with success...
	echo:
:library  
	set sourceFiles="%LibraryPath%\dllmain.cpp"^
 "%LibraryPath%\Bytes.cpp"^
 "%LibraryPath%\BytesReader.cpp"^
 "%LibraryPath%\TlvStructure.cpp"^
 "%LibraryPath%\ServiceProxy.cpp"^
 "%LibraryPath%\VirtualToken.cpp"^
 "%LibraryPath%\AlphaProtoclDispatcher.cpp"^
 "%LibraryPath%\PipeCommunicationResolver.cpp"^
 "%LibraryPath%\SocketCommunicationResolver.cpp"^
 "%LibraryPath%\TlvParser.cpp"^
 "%LibraryPath%\Functions\dencryptionFunctions.cpp"^
 "%LibraryPath%\Functions\generalFunctions.cpp"^
 "%LibraryPath%\Functions\hashAndSignFunctions.cpp"^
 "%LibraryPath%\Functions\objectManagementFunctions.cpp"^
 "%LibraryPath%\Functions\sessionManagementFunctions.cpp"^
 "%LibraryPath%\Functions\slotManagementFunctions.cpp"
 
 cl.exe %sourceFiles%^
 /OUT:"%libraryOutputPath%\pkcs11_x32.dll"^
 "kernel32.lib" "user32.lib" "gdi32.lib" "winspool.lib" "comdlg32.lib" "advapi32.lib" "shell32.lib" "ole32.lib" "oleaut32.lib" "uuid.lib" "odbc32.lib" "odbccp32.lib" "ws2_32.lib"^
 /IMPLIB:"%libraryOutputPath%\pkcs11_x32.lib"^
 /LD  
:final
