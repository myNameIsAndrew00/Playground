#include "../include/VirtualToken.h"
using namespace Abstractions;

VirtualToken::VirtualToken(const ServiceProxyReference& serviceProxy)
{
	this->serviceProxy = serviceProxy;
}

GetIdentifierResult VirtualToken::GetIdentifier() const
{
	return GetIdentifierResult(GetIdentifierResult::Code::OK, VirtualToken::Identifier);
}

InitialiseResult VirtualToken::Initialise()
{
	bool registerResult = this->serviceProxy->Register(this);

	//todo: handle type of messages or do client side validations
	return InitialiseResult(InitialiseResult::Code::OK, registerResult);
}

InitialiseResult VirtualToken::Finalise()
{
	bool detachResult = this->serviceProxy->DetachCurrentClient();
	//todo: handle type of messages or do client side validations
	return InitialiseResult(InitialiseResult::Code::OK, detachResult);
}


CreateSessionResult Abstractions::VirtualToken::CreateSession() const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->BeginSession();
}

EndSessionResult Abstractions::VirtualToken::EndSession(const unsigned long long sessionId) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->EndSession(sessionId);
}


GetManufacturerResult VirtualToken::GetManufacturer() const
{
	//todo: get from proxy?
	return GetManufacturerResult(GetManufacturerResult::Code::OK, "Virtual token");
}
 

VirtualToken::~VirtualToken() {
	this->serviceProxy->Unregister(this);
}
