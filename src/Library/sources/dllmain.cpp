
#define WIN32_LEAN_AND_MEAN            

#include <windows.h> 
#include "../include/IPkcs11Token.h";
#include "../include/VirtualToken.h"
#include "../include/ServiceProxy.h"

IPkcs11TokenReference Token = std::make_shared<VirtualToken>(
    std::make_shared<ServiceProxy>()
    );

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{

    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

