@echo OFF 

echo -----------------Build started-----------------
:prerequeries
	echo:
	set output=.\deploy
	set serviceOutputPath=.\deploy\Service
	set serviceConfiguratorOutputPath=.\deploy\ConfigurationAPI
	set libraryOutputPath=.\deploy\Library
	set configuratorOutputPath=.\deploy\Configurator
	
	set ServiceLibraryPath=..\Service\Service.Worker
	set ServiceConfiguratorAPIPath=..\Service\Service.ConfigurationAPI
	set LibraryPath=..\Library\Library\sources
	set ConfiguratorPath=..\Configurator\Configurator
	
	echo Pkcs11 Service output directory: %serviceOutputPath%
	echo Pkcs11 C++ Library output directory: %libraryOutputPath%

	if exist %output% (
		rmdir /q /s %output%
	)  
	mkdir %output%
	mkdir %serviceOutputPath%
	mkdir %libraryOutputPath%
	mkdir %configuratorOutputPath%
	
	echo Successufuly created directories...
	echo:
:service
	echo -----------------Building service-----------------
	echo:

	:: Step 1: check .net
	echo [1]Checking dotnet version...

	FOR /F "tokens=*" %%x in ('dotnet --version') do set DotNetVersion=%%x

	set DotNetVersion=%DotNetVersion:~0,1%

	if %DotNetVersion% lss 6 ( 
		echo "Dot Net Version must be >=6. Install .net 6 sdk."
		goto final
	)

	:: Step 2: publish service
	echo:
	
	echo [2]Building service...
	dotnet publish %ServiceLibraryPath% --output %serviceOutputPath%
	
	:: Step 3: publish configuration API
	echo [3]Building service configuration API
	dotnet publish %ServiceConfiguratorAPIPath% --output %serviceConfiguratorOutputPath%
	
	echo:
	echo Service built with success...
	echo:
:configurator
	echo -----------------Building configuration application-----------------
	
	:: Step 4: publish configurator
	echo [4]Building configurator
	dotnet publish %ConfiguratorPath% --output %configuratorOutputPath%
	
	echo Configurator built with success...
	
:library  
	echo -----------------Building library-----------------
	echo:
	
	::Step 5: check cl.exe
	echo [5]Checking msvc compiler...
	WHERE cl.exe
	echo %ERRORLEVEL%
	
	IF %ERRORLEVEL% NEQ 0 (
		echo Msvc compiler not found. Install msvc compiler to build pkcs 11 library. 
		echo "If you installed the compiler, please make sure that cl.exe exists on your PATH and the compile environment is valid. Run vcvars32.bat, usually found at C:\Program Files\Microsoft Visual Studio\2022\Community\VC\Auxiliary\Build to setup environment."

		goto final
	)

	echo [6]Building C library...
	set sourceFiles="%LibraryPath%\dllmain.cpp"^
 "%LibraryPath%\Bytes.cpp"^
 "%LibraryPath%\BytesReader.cpp"^
 "%LibraryPath%\TlvStructure.cpp"^
 "%LibraryPath%\ServiceProxy.cpp"^
 "%LibraryPath%\VirtualToken.cpp"^
 "%LibraryPath%\AlphaProtocolDispatcher.cpp"^
 "%LibraryPath%\PipeCommunicationResolver.cpp"^
 "%LibraryPath%\SocketCommunicationResolver.cpp"^
 "%LibraryPath%\TlvParser.cpp"^
 "%LibraryPath%\Functions\dencryptionFunctions.cpp"^
 "%LibraryPath%\Functions\generalFunctions.cpp"^
 "%LibraryPath%\Functions\hashAndSignFunctions.cpp"^
 "%LibraryPath%\Functions\objectManagementFunctions.cpp"^
 "%LibraryPath%\Functions\sessionManagementFunctions.cpp"^
 "%LibraryPath%\Functions\slotManagementFunctions.cpp"
	set linkFiles="kernel32.lib" "ws2_32.lib"
	  
	cl.exe /Fo%libraryOutputPath%\ /D_USRDLL /D_WINDLL %sourceFiles% %linkFiles% /link /DLL /OUT:"%libraryOutputPath%\pkcs11.dll" 
	
	echo Cleanup...	
	del *.obj /s /q
	
	
	echo Library built with success...
	 
	echo:
	echo Done!
:final
