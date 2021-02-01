#pragma once
#include "IPkcs11Token.h"
#include "IServiceProxyClient.h"
#include "ServiceProxy.h"

/*Represents a layer of abstraction for token/slot*/
class VirtualToken : public IPkcs11Token, public IServiceProxyClient
{
public:
	static const unsigned int Identifier = 1; 
	
	VirtualToken(const ServiceProxyReference& proxyReference);

	unsigned int GetIdentifier() const override; 
	bool Initialise() override;

private:

	ServiceProxyReference serviceProxy;
};

