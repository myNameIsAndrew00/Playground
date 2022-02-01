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
		
		~VirtualToken();
	private:

		ServiceProxyReference serviceProxy;
	};
}
