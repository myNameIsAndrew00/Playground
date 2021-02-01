#include "../include/VirtualToken.h"

VirtualToken::VirtualToken(const ServiceProxyReference& serviceProxy)
{
	this->serviceProxy = serviceProxy;
}

unsigned int VirtualToken::GetIdentifier() const
{
	return VirtualToken::Identifier;
}

bool VirtualToken::Initialise()
{
	this->serviceProxy->Register(IServiceProxyClientReference(this));
	return true;
}
 
