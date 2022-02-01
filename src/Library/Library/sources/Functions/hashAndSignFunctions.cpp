#pragma once
#include "../../include/pkcs11.h" 
#include "../../include/IPkcs11Token.h"

extern Abstractions::IPkcs11TokenReference Token;

CK_DEFINE_FUNCTION(CK_RV, C_DigestInit)(CK_SESSION_HANDLE hSession, CK_MECHANISM_PTR pMechanism)
{
	if (nullptr == pMechanism)
		return CKR_ARGUMENTS_BAD;

	auto encryptInitResult = Token->DigestInit(hSession, pMechanism);

	return (CK_RV)encryptInitResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_Digest)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pData, CK_ULONG ulDataLen, CK_BYTE_PTR pDigest, CK_ULONG_PTR pulDigestLen)
{  
	if (nullptr == pData || ulDataLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto digestResult = Token->Digest(hSession, pData, ulDataLen, nullptr == pDigest);

	*pulDigestLen = digestResult.GetValue().GetLength();

	if (nullptr != pDigest) {
		memcpy(pDigest, digestResult.GetValue().GetBytes(), *pulDigestLen);
	}

	return (CK_RV)digestResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_DigestUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pPart, CK_ULONG ulPartLen)
{
	if (nullptr == pPart || ulPartLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto digestPartResult = Token->DigestUpdate(hSession, pPart, ulPartLen);

	return (CK_RV)digestPartResult.GetCode();
}
 

CK_DEFINE_FUNCTION(CK_RV, C_DigestFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pDigest, CK_ULONG_PTR pulDigestLen)
{ 
	auto digestFinalResult = Token->DigestFinal(hSession, nullptr == pDigest);

	*pulDigestLen = digestFinalResult.GetValue().GetLength();

	if (nullptr != pDigest) {
		memcpy(pDigest, digestFinalResult.GetValue().GetBytes(), *pulDigestLen);
	}

	return (CK_RV)digestFinalResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_SignInit)(CK_SESSION_HANDLE hSession, CK_MECHANISM_PTR pMechanism, CK_OBJECT_HANDLE hKey)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_Sign)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pData, CK_ULONG ulDataLen, CK_BYTE_PTR pSignature, CK_ULONG_PTR pulSignatureLen)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_SignUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pPart, CK_ULONG ulPartLen)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_SignFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pSignature, CK_ULONG_PTR pulSignatureLen)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


