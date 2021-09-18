#pragma once
#include "../../include/pkcs11.h" 
#include "../../include/IPkcs11Token.h"
#include <stdlib.h>
#include <string.h>

extern Abstractions::IPkcs11TokenReference Token;
 
CK_DEFINE_FUNCTION(CK_RV, C_OpenSession)(CK_SLOT_ID slotID, CK_FLAGS flags, CK_VOID_PTR pApplication, CK_NOTIFY Notify, CK_SESSION_HANDLE_PTR phSession)
{
	if (nullptr == phSession)
		return CKR_ARGUMENTS_BAD;

	if (slotID != Token->GetIdentifier().GetValue())
		return CKR_SLOT_ID_INVALID;

	auto createSessionResult = Token->CreateSession();

	*phSession = createSessionResult.GetValue();

	return (CK_RV)createSessionResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_CloseSession)(CK_SESSION_HANDLE hSession)
{
	auto endSessionResult = Token->EndSession(hSession);

	return (CK_RV)endSessionResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_CloseAllSessions)(CK_SLOT_ID slotID)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_GetSessionInfo)(CK_SESSION_HANDLE hSession, CK_SESSION_INFO_PTR pInfo)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}




CK_DEFINE_FUNCTION(CK_RV, C_Login)(CK_SESSION_HANDLE hSession, CK_USER_TYPE userType, CK_UTF8CHAR_PTR pPin, CK_ULONG ulPinLen)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_Logout)(CK_SESSION_HANDLE hSession)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
