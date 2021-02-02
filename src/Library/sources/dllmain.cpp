
#define WIN32_LEAN_AND_MEAN            

#include <windows.h> 
#include "../include/VirtualToken.h"
#include "../include/ServiceProxy.h"
#include "../include/PipeCommunicationResolver.h"

using namespace Abstractions;
using namespace Infrastructure;

/*Create a token which use a proxy service which communicates via pipes*/
IPkcs11TokenReference Token = 
    std::make_shared<VirtualToken>(
        std::make_shared<ServiceProxy>( 
            std::make_shared<PipeCommunicationResolver>()
            )
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

