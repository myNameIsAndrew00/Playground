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
		CreateSessionResult CreateSession() const override;

	private:

		ServiceProxyReference serviceProxy;
	};
}
