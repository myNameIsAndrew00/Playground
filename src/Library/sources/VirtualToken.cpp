#include "../include/VirtualToken.h"
#include "../include/TlvParser.h"

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
 
CreateObjectResult VirtualToken::CreateObject(const unsigned long long sessionId, CK_ATTRIBUTE* attributes, const int length) const {
	Infrastructure::TlvParser parser;

	std::list<TlvStructure> templateStructure = parser.ParsePkcs11Attributes(attributes, length);

	return this->serviceProxy->CreateObject(sessionId, templateStructure);
}

VirtualToken::~VirtualToken() {
	this->serviceProxy->Unregister(this);
}
