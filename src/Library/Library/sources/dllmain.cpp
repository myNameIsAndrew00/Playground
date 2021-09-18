 
#define WIN32_LEAN_AND_MEAN            

#include <windows.h> 
#include "../include/VirtualToken.h"
#include "../include/ServiceProxy.h"
#include "../include/PipeCommunicationResolver.h"
#include "../include/SocketCommunicationResolver.h"
#include "../include/AlphaProtocolDispatcher.h"

using namespace Abstractions;
using namespace Infrastructure;

/*Create a token which use a proxy service which communicates via pipes*/
IPkcs11TokenReference Token =
std::make_shared<VirtualToken>(
	std::make_shared<ServiceProxy>(
		std::make_shared<SocketCommunicationResolver>("127.0.0.1", 5123),
		std::make_shared<AlphaProtocolDispatcher>()
		)
	);

BOOL APIENTRY DllMain(HMODULE hModule,
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

int main() {

	//Tests setup...
	CK_BYTE iv[] = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
					 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };

	CK_BBOOL true_val = 1;
	CK_BBOOL false_val = 0;
	CK_ULONG key_length_bytes = 128;
	CK_ULONG key_type = CKK_AES;

	CK_MECHANISM mechanismTemplate[] = {
		{CKM_AES_CBC, iv, sizeof(iv)}
	};

	CK_ATTRIBUTE _template[] = {
		{CKA_ENCRYPT,   &true_val,		   sizeof(CK_BBOOL)},
		{CKA_SENSITIVE, &true_val,         sizeof(CK_BBOOL)},
		{CKA_TOKEN,     &false_val,        sizeof(CK_BBOOL)},
		{CKA_VALUE,		iv,				   sizeof(iv) },
		{CKA_KEY_TYPE,  &key_type,		   sizeof(CK_ULONG)},
		{CKA_VALUE_LEN, &key_length_bytes, sizeof(CK_ULONG)}
	};

	CK_ULONG slotsCount;
	char encryptData[] = "Ana are mere";
	 

	//Tests...

	auto initialiseResult = Token->Initialise();

	auto createSessionResult = Token->CreateSession();

	auto createObjectResult = Token->CreateObject(
		createSessionResult.GetValue(),
		_template,
		6
	);

	auto encryptInitResult = Token->EncryptInit(createSessionResult.GetValue(), createObjectResult.GetValue(), mechanismTemplate);
	auto encryptResult = Token->Encrypt(createSessionResult.GetValue(), (const unsigned char*)"ana are mere", 13, false);
	encryptResult = Token->Encrypt(createSessionResult.GetValue(), (const unsigned char*)"ana are mere", 13, false);
	
	auto endSessionResult = Token->EndSession(createSessionResult.GetValue());


	auto finaliseResult = Token->Finalise();


  	return 0;
}