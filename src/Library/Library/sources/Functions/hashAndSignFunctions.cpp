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
	if (nullptr == pMechanism)
		return CKR_ARGUMENTS_BAD;

	auto signInitResult = Token->SignInit(hSession, hKey, pMechanism);

	return (CK_RV)signInitResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_Sign)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pData, CK_ULONG ulDataLen, CK_BYTE_PTR pSignature, CK_ULONG_PTR pulSignatureLen)
{
	if (nullptr == pData || ulDataLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto signResult = Token->Sign(hSession, pData, ulDataLen, nullptr == pSignature);

	*pulSignatureLen = signResult.GetValue().GetLength();

	if (nullptr != pSignature) {
		memcpy(pSignature, signResult.GetValue().GetBytes(), *pulSignatureLen);
	}

	return (CK_RV)signResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_SignUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pPart, CK_ULONG ulPartLen)
{
	if (nullptr == pPart || ulPartLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto signUpdateResult = Token->SignUpdate(hSession, pPart, ulPartLen);

	return (CK_RV)signUpdateResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_SignFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pSignature, CK_ULONG_PTR pulSignatureLen)
{
	auto signFinalResult = Token->SignFinal(hSession, nullptr == pSignature);

	*pulSignatureLen = signFinalResult.GetValue().GetLength();

	if (nullptr != pSignature) {
		memcpy(pSignature, signFinalResult.GetValue().GetBytes(), *pulSignatureLen);
	}

	return (CK_RV)signFinalResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_VerifyInit)(CK_SESSION_HANDLE hSession, CK_MECHANISM_PTR mechanismPointer, CK_OBJECT_HANDLE publicKeyHandler)
{
	if (nullptr == mechanismPointer)
		return CKR_ARGUMENTS_BAD;

	auto verifyInitResult = Token->VerifyInit(hSession, publicKeyHandler, mechanismPointer);

	return (CK_RV)verifyInitResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_Verify)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pData, CK_ULONG ulDataLen, CK_BYTE_PTR pSignature, CK_ULONG ulSignatureLen)
{
	if (nullptr == pData || ulDataLen == 0 || nullptr == pSignature || 0 == ulSignatureLen)
		return CKR_ARGUMENTS_BAD;

	auto verifyResult = Token->Verify(hSession, pData, ulDataLen, pSignature, ulSignatureLen);

	if (!verifyResult.GetValue())
		return CKR_SIGNATURE_INVALID;

	return (CK_RV)verifyResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_VerifyUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pPart, CK_ULONG ulPartLen)
{
	if (nullptr == pPart || ulPartLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto verifyUpdateResult = Token->VerifyUpdate(hSession, pPart, ulPartLen);

	return (CK_RV)verifyUpdateResult.GetCode();
}


CK_DEFINE_FUNCTION(CK_RV, C_VerifyFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pSignature, CK_ULONG ulSignatureLen)
{
	if (nullptr == pSignature || ulSignatureLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto verifyFinalResult = Token->VerifyFinal(hSession, pSignature, ulSignatureLen);
  
	return (CK_RV)verifyFinalResult.GetCode();
}