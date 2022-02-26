#pragma once
#include "IPkcs11Token.h"
#include "IServiceProxyClient.h"
#include "ServiceProxy.h"

namespace Abstractions {
	/*Represents a layer of abstraction for token/slot*/
	class VirtualToken : public IPkcs11Token, public IServiceProxyClient
	{
	public:
		static const unsigned int Identifier = 1;

		VirtualToken(const ServiceProxyReference& proxyReference);

		GetIdentifierResult GetIdentifier() const override; 
		GetManufacturerResult GetManufacturer() const override;
		InitialiseResult Initialise() override;
		FinaliseResult Finalise() override;
		CreateSessionResult CreateSession() const override;
		EndSessionResult EndSession(const unsigned long long sessionId) const override;
		CreateObjectResult CreateObject(const unsigned long long sessionId, CK_ATTRIBUTE* attributes, const int length) const override;
		EncryptInitResult EncryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const override;
		EncryptResult Encrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const override;
		EncryptUpdateResult EncryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const override;
		EncryptFinalResult EncryptFinal(const unsigned long long sessionId, bool encryptedDataLengthRequest) const override;
		DecryptInitResult DecryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const override;
		DecryptResult Decrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const override;
		DecryptUpdateResult DecryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const override;
		DecryptFinalResult DecryptFinal(const unsigned long long sessionId, bool decryptedDataLengthRequest) const override;
		DigestInitResult DigestInit(const unsigned long long sessionId, const CK_MECHANISM* mechanism) const override;
		DigestResult Digest(const unsigned long long sessionId, const unsigned char* data, const int length, bool digestLengthRequest) const override;
		DigestUpdateResult DigestUpdate(const unsigned long long sessionId, const unsigned char* data, const int length) const override;
		DigestFinalResult DigestFinal(const unsigned long long sessionId, bool digestLengthRequest) const override;
		GenerateKeyPairResult GenerateKeyPair(const  unsigned long long  sessionId, const CK_MECHANISM* mechanism, CK_ATTRIBUTE* publicKeyAttributes, const int publicKeyLength, CK_ATTRIBUTE* privateKeyAttributes, const int privateKeyLength) const override;
		SignInitResult SignInit(const  unsigned long long  sessionId, const unsigned long long privateKeyId, const CK_MECHANISM* mechanism) const override;
		SignResult Sign(const  unsigned long long  sessionId, const unsigned char* dataToSign, const int length, bool requestLength) const override;
		SignUpdateResult SignUpdate(const  unsigned long long  sessionId, const unsigned char* dataToSign, const int length) const override;
		SignFinalResult SignFinal(const  unsigned long long  sessionId, bool lengthRequest) const override;
		VerifyInitResult VerifyInit(const  unsigned long long  sessionId, const unsigned long long publicKeyId, const CK_MECHANISM* mechanism) const override;
		VerifyResult Verify(const  unsigned long long  sessionId, const unsigned char* dataToVerify, const int dataLength, const unsigned char* signedData, const int signedDataLength) const override;
		VerifyUpdateResult VerifyUpdate(const  unsigned long long  sessionId, const unsigned char* dataToVerify, const int dataLength) const override;
		VerifyFinalResult VerifyFinal(const  unsigned long long  sessionId, const unsigned char* signedData, const int signedDataLength) const override;
		~VirtualToken();
	private:

		ServiceProxyReference serviceProxy;
	};
}
