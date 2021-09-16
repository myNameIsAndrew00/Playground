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
		EncryptResult Encrypt(const unsigned long long sessionId, const unsigned char* data, const int length) const override;
		EncryptUpdateResult EncryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length) const override;
		EncryptFinalResult EncryptFinal(const unsigned long long sessionId) const override;
		~VirtualToken();
	private:

		ServiceProxyReference serviceProxy;
	};
}
