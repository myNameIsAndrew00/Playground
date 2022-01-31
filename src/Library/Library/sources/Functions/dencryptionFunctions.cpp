 
#include "../../include/pkcs11.h" 
#include "../../include/IPkcs11Token.h"
#include <stdlib.h>
#include <string.h>

extern Abstractions::IPkcs11TokenReference Token;

CK_DEFINE_FUNCTION(CK_RV, C_EncryptInit)(CK_SESSION_HANDLE hSession, CK_MECHANISM_PTR pMechanism, CK_OBJECT_HANDLE hKey)
{
	if (nullptr == pMechanism)
		return CKR_ARGUMENTS_BAD;

	auto encryptInitResult = Token->EncryptInit(hSession, hKey, pMechanism);

	return (CK_RV)encryptInitResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_Encrypt)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pData, CK_ULONG ulDataLen, CK_BYTE_PTR pEncryptedData, CK_ULONG_PTR pulEncryptedDataLen)
{
	if (nullptr == pData || ulDataLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto encrypResult = Token->Encrypt(hSession, pData, ulDataLen, nullptr == pEncryptedData);

	*pulEncryptedDataLen = encrypResult.GetValue().GetLength();

	if (nullptr != pEncryptedData) {
		memcpy(pEncryptedData, encrypResult.GetValue().GetBytes(), *pulEncryptedDataLen);
	}

	return (CK_RV)encrypResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_EncryptUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pPart, CK_ULONG ulPartLen, CK_BYTE_PTR pEncryptedPart, CK_ULONG_PTR pulEncryptedPartLen)
{
	if (nullptr == pPart || ulPartLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto encrypResult = Token->EncryptUpdate(hSession, pPart, ulPartLen, nullptr == pEncryptedPart);

	*pulEncryptedPartLen = encrypResult.GetValue().GetLength();

	if (nullptr != pEncryptedPart) {
		memcpy(pEncryptedPart, encrypResult.GetValue().GetBytes(), *pulEncryptedPartLen);
	}

	return (CK_RV)encrypResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_EncryptFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pLastEncryptedPart, CK_ULONG_PTR pulLastEncryptedPartLen)
{
 
	auto encrypResult = Token->EncryptFinal(hSession, nullptr == pLastEncryptedPart);

	*pulLastEncryptedPartLen = encrypResult.GetValue().GetLength();

	if (nullptr != pLastEncryptedPart) {
		memcpy(pLastEncryptedPart, encrypResult.GetValue().GetBytes(), *pulLastEncryptedPartLen);
	}

	return (CK_RV)encrypResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_DecryptInit)(CK_SESSION_HANDLE hSession, CK_MECHANISM_PTR pMechanism, CK_OBJECT_HANDLE hKey)
{
	if (nullptr == pMechanism)
		return CKR_ARGUMENTS_BAD;

	auto encryptInitResult = Token->DecryptInit(hSession, hKey, pMechanism);

	return (CK_RV)encryptInitResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_DecryptUpdate)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pEncryptedPart, CK_ULONG ulEncryptedPartLen, CK_BYTE_PTR pPart, CK_ULONG_PTR pulPartLen)
{
	if (nullptr == pPart || ulEncryptedPartLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto encrypResult = Token->DecryptUpdate(hSession, pPart, ulEncryptedPartLen, nullptr == pEncryptedPart);

	*pulPartLen = encrypResult.GetValue().GetLength();

	if (nullptr != pPart) {
		memcpy(pPart, encrypResult.GetValue().GetBytes(), *pulPartLen);
	}

	return (CK_RV)encrypResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_DecryptFinal)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pLastPart, CK_ULONG_PTR pulLastPartLen)
{
	auto encrypResult = Token->DecryptFinal(hSession, nullptr == pLastPart);

	*pulLastPartLen = encrypResult.GetValue().GetLength();

	if (nullptr != pLastPart) {
		memcpy(pLastPart, encrypResult.GetValue().GetBytes(), *pulLastPartLen);
	}

	return (CK_RV)encrypResult.GetCode();
}

CK_DEFINE_FUNCTION(CK_RV, C_Decrypt)(CK_SESSION_HANDLE hSession, CK_BYTE_PTR pEncryptedData, CK_ULONG ulEncryptedDataLen, CK_BYTE_PTR pData, CK_ULONG_PTR pulDataLen)
{
	if (nullptr == pEncryptedData || ulEncryptedDataLen == 0)
		return CKR_ARGUMENTS_BAD;

	auto encrypResult = Token->DecryptUpdate(hSession, pEncryptedData, ulEncryptedDataLen, nullptr == pEncryptedData);

	*pulDataLen = encrypResult.GetValue().GetLength();

	if (nullptr != pData) {
		memcpy(pData, encrypResult.GetValue().GetBytes(), *pulDataLen);
	}

	return (CK_RV)encrypResult.GetCode();
}