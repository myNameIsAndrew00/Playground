
#pragma once
#include "../../include/pkcs11.h"  
#include "../../include/IPkcs11Token.h"
#include <stdlib.h>
#include <string.h>

extern Abstractions::IPkcs11TokenReference Token;

CK_FUNCTION_LIST functionList = {
	{ 2,40 },
	&C_Initialize,
	&C_Finalize,
	&C_GetInfo,
	&C_GetFunctionList,
	&C_GetSlotList,
	&C_GetSlotInfo,
	&C_GetTokenInfo,
	&C_GetMechanismList,
	&C_GetMechanismInfo,
	&C_InitToken,
	&C_InitPIN,
	&C_SetPIN,
	&C_OpenSession,
	&C_CloseSession,
	&C_CloseAllSessions,
	&C_GetSessionInfo,
	&C_GetOperationState,
	&C_SetOperationState,
	&C_Login,
	&C_Logout,
	&C_CreateObject,
	&C_CopyObject,
	&C_DestroyObject,
	&C_GetObjectSize,
	&C_GetAttributeValue,
	&C_SetAttributeValue,
	&C_FindObjectsInit,
	&C_FindObjects,
	&C_FindObjectsFinal,
	&C_EncryptInit,
	&C_Encrypt,
	&C_EncryptUpdate,
	&C_EncryptFinal,
	&C_DecryptInit,
	&C_Decrypt,
	&C_DecryptUpdate,
	&C_DecryptFinal,
	&C_DigestInit,
	&C_Digest,
	&C_DigestUpdate,
	&C_DigestKey, 
	&C_DigestFinal,
	&C_SignInit,
	&C_Sign,
	&C_SignUpdate,
	&C_SignFinal,
	&C_SignRecoverInit, 
	&C_SignRecover, 
	&C_VerifyInit, 
	&C_Verify, 
	&C_VerifyUpdate,
	&C_VerifyFinal,
	&C_VerifyRecoverInit,
	&C_VerifyRecover,
	&C_DigestEncryptUpdate,
	&C_DecryptDigestUpdate,
	&C_SignEncryptUpdate,
	&C_DecryptVerifyUpdate,
	&C_GenerateKey,
	&C_GenerateKeyPair,
	&C_WrapKey,
	&C_UnwrapKey,
	&C_DeriveKey,
	&C_SeedRandom,
	&C_GenerateRandom,
	&C_GetFunctionStatus,
	&C_CancelFunction,
	&C_WaitForSlotEvent
}; 

CK_DEFINE_FUNCTION(CK_RV, C_GetFunctionList)(CK_FUNCTION_LIST_PTR_PTR ppFunctionList)
{
	if (nullptr == ppFunctionList)
		return CKR_ARGUMENTS_BAD;

	*ppFunctionList = &functionList;
	return CKR_OK;
}

CK_DEFINE_FUNCTION(CK_RV, C_Initialize)(CK_VOID_PTR pInitArgs)
{
	auto initialiseResult = Token->Initialise();

	return (CK_RV) initialiseResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_Finalize)(CK_VOID_PTR pReserved)
{
	auto finaliseResult = Token->Finalise();

	return (CK_RV)finaliseResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_GetInfo)(CK_INFO_PTR pInfo)
{
	if (nullptr == pInfo) return CKR_ARGUMENTS_BAD;
	 
	pInfo->cryptokiVersion.major = Abstractions::Major;
	pInfo->cryptokiVersion.minor = Abstractions::Minor;
	pInfo->libraryVersion.major = Abstractions::Major;
	pInfo->libraryVersion.minor = Abstractions::Minor;

	strcpy_s((char*)pInfo->manufacturerID, sizeof(Abstractions::Manufacturer), Abstractions::Manufacturer);
	strcpy_s((char*)pInfo->libraryDescription, sizeof(Abstractions::Description), Abstractions::Description);

	return CKR_OK;
}



CK_DEFINE_FUNCTION(CK_RV, C_InitToken)(CK_SLOT_ID , CK_UTF8CHAR_PTR , CK_ULONG , CK_UTF8CHAR_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_InitPIN)(CK_SESSION_HANDLE , CK_UTF8CHAR_PTR , CK_ULONG )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SetPIN)(CK_SESSION_HANDLE , CK_UTF8CHAR_PTR , CK_ULONG , CK_UTF8CHAR_PTR , CK_ULONG )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_GetOperationState)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SetOperationState)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_OBJECT_HANDLE , CK_OBJECT_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}

CK_DEFINE_FUNCTION(CK_RV, C_DigestKey)(CK_SESSION_HANDLE , CK_OBJECT_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SignRecoverInit)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_OBJECT_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SignRecover)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_VerifyRecoverInit)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_OBJECT_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_VerifyRecover)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_DigestEncryptUpdate)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_DecryptDigestUpdate)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR pPart, CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SignEncryptUpdate)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_DecryptVerifyUpdate)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_GenerateKey)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_ATTRIBUTE_PTR , CK_ULONG , CK_OBJECT_HANDLE_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}

CK_DEFINE_FUNCTION(CK_RV, C_WrapKey)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_OBJECT_HANDLE , CK_OBJECT_HANDLE , CK_BYTE_PTR , CK_ULONG_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_UnwrapKey)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_OBJECT_HANDLE , CK_BYTE_PTR , CK_ULONG , CK_ATTRIBUTE_PTR , CK_ULONG , CK_OBJECT_HANDLE_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_DeriveKey)(CK_SESSION_HANDLE , CK_MECHANISM_PTR , CK_OBJECT_HANDLE , CK_ATTRIBUTE_PTR , CK_ULONG , CK_OBJECT_HANDLE_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_SeedRandom)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_GenerateRandom)(CK_SESSION_HANDLE , CK_BYTE_PTR , CK_ULONG )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_GetFunctionStatus)(CK_SESSION_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_CancelFunction)(CK_SESSION_HANDLE )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_WaitForSlotEvent)(CK_FLAGS , CK_SLOT_ID_PTR , CK_VOID_PTR )
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
