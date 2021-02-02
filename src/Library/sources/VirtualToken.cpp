#include "../include/VirtualToken.h"
using namespace Abstractions;

VirtualToken::VirtualToken(const ServiceProxyReference& serviceProxy)
{
	this->serviceProxy = serviceProxy;
}

GetIdentifierResult VirtualToken::GetIdentifier() const
{
	return GetIdentifierResult(GetIdentifierResult::Code::OkResult, VirtualToken::Identifier);
}

InitialiseResult VirtualToken::Initialise()
{
	this->serviceProxy->Register(IServiceProxyClientReference(this));

	//todo: implement
	return InitialiseResult(InitialiseResult::Code::OkResult, true);
}
CreateSessionResult Abstractions::VirtualToken::CreateSession() const
{
	//todo: use the proxy
	return CreateSessionResult(CreateSessionResult::Code::OkResult, 1L);
}


GetManufacturerResult VirtualToken::GetManufacturer() const
{
	//todo: get from proxy?
	return GetManufacturerResult(GetManufacturerResult::Code::OkResult, "Virtual token");
}
 
