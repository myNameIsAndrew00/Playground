#pragma once
#include "../../include/pkcs11.h" 
#include "../../include/IPkcs11Token.h"
#include <stdlib.h>
#include <string.h>

extern Abstractions::IPkcs11TokenReference Token;

CK_DEFINE_FUNCTION(CK_RV, C_FindObjectsInit)(CK_SESSION_HANDLE hSession, CK_ATTRIBUTE_PTR pTemplate, CK_ULONG ulCount)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}
 
CK_DEFINE_FUNCTION(CK_RV, C_FindObjects)(CK_SESSION_HANDLE hSession, CK_OBJECT_HANDLE_PTR phObject, CK_ULONG ulMaxObjectCount, CK_ULONG_PTR pulObjectCount)
{

	return CKR_FUNCTION_NOT_SUPPORTED;

}

CK_DEFINE_FUNCTION(CK_RV, C_FindObjectsFinal)(CK_SESSION_HANDLE hSession)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_GetAttributeValue)(CK_SESSION_HANDLE hSession, CK_OBJECT_HANDLE hObject, CK_ATTRIBUTE_PTR pTemplate, CK_ULONG ulCount)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}

CK_DEFINE_FUNCTION(CK_RV, C_SetAttributeValue)(CK_SESSION_HANDLE, CK_OBJECT_HANDLE, CK_ATTRIBUTE_PTR, CK_ULONG)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}

CK_DEFINE_FUNCTION(CK_RV, C_CreateObject)(CK_SESSION_HANDLE sessionHandle, CK_ATTRIBUTE_PTR attributes, CK_ULONG attributesLength, CK_OBJECT_HANDLE_PTR resultObject)
{
	if (nullptr == attributes || 0 == attributesLength || nullptr == resultObject)
		return CKR_ARGUMENTS_BAD;

	auto createObjectResult = Token->CreateObject(
		sessionHandle,
		attributes,
		attributesLength
	);

	*resultObject = createObjectResult.GetValue();

	return (CK_RV)createObjectResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_CopyObject)(CK_SESSION_HANDLE, CK_OBJECT_HANDLE, CK_ATTRIBUTE_PTR, CK_ULONG, CK_OBJECT_HANDLE_PTR)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_DestroyObject)(CK_SESSION_HANDLE, CK_OBJECT_HANDLE)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
CK_DEFINE_FUNCTION(CK_RV, C_GetObjectSize)(CK_SESSION_HANDLE, CK_OBJECT_HANDLE, CK_ULONG_PTR)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}
