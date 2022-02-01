#include "../include/VirtualToken.h"
#include "../include/TlvParser.h"

using namespace Abstractions;

VirtualToken::VirtualToken(const ServiceProxyReference& serviceProxy)
{
	this->serviceProxy = serviceProxy;
}

GetIdentifierResult VirtualToken::GetIdentifier() const
{
	return GetIdentifierResult(ReturnCode::OK, VirtualToken::Identifier);
}

InitialiseResult VirtualToken::Initialise()
{
	bool registerResult = this->serviceProxy->Register(this);

	//todo: handle type of messages or do client side validations
	return InitialiseResult(registerResult ? ReturnCode::OK : ReturnCode::FUNCTION_FAILED, registerResult);
}

InitialiseResult VirtualToken::Finalise()
{
	bool detachResult = this->serviceProxy->DetachCurrentClient();
	//todo: handle type of messages or do client side validations
	return InitialiseResult(ReturnCode::OK, detachResult);
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
	return GetManufacturerResult(ReturnCode::OK, "Virtual token");
}
 
CreateObjectResult VirtualToken::CreateObject(const unsigned long long sessionId, CK_ATTRIBUTE* attributes, const int length) const {
	Infrastructure::TlvParser parser;

	std::list<TlvStructure> templateStructure = parser.ParsePkcs11Attributes(attributes, length);

	return this->serviceProxy->CreateObject(sessionId, templateStructure);
}

EncryptInitResult Abstractions::VirtualToken::EncryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->EncryptInit(sessionId, objectId, TlvStructure((unsigned long long)mechanism->mechanism, (const unsigned char*) mechanism->pParameter, mechanism->ulParameterLen));
}

EncryptResult Abstractions::VirtualToken::Encrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->Encrypt(sessionId, TlvStructure(0, data, length), encryptedDataLengthRequest);
}

EncryptUpdateResult Abstractions::VirtualToken::EncryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->EncryptUpdate(sessionId, TlvStructure(0, data, length), encryptedDataLengthRequest);
}

EncryptFinalResult Abstractions::VirtualToken::EncryptFinal(const unsigned long long sessionId, bool encryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->EncryptFinal(sessionId, encryptedDataLengthRequest);
}

DecryptInitResult Abstractions::VirtualToken::DecryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DecryptInit(sessionId, objectId, TlvStructure((unsigned long long)mechanism->mechanism, (const unsigned char*)mechanism->pParameter, mechanism->ulParameterLen));
}

DecryptResult Abstractions::VirtualToken::Decrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->Decrypt(sessionId, TlvStructure(0, data, length), decryptedDataLengthRequest);
}

DecryptUpdateResult Abstractions::VirtualToken::DecryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DecryptUpdate(sessionId, TlvStructure(0, data, length), decryptedDataLengthRequest);
}

DecryptFinalResult Abstractions::VirtualToken::DecryptFinal(const unsigned long long sessionId, bool decryptedDataLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DecryptFinal(sessionId, decryptedDataLengthRequest);
}

DigestInitResult Abstractions::VirtualToken::DigestInit(const unsigned long long sessionId, const CK_MECHANISM* mechanism) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DigestInit(sessionId, TlvStructure((unsigned long long)mechanism->mechanism, (const unsigned char*)mechanism->pParameter, mechanism->ulParameterLen));
}

DigestResult Abstractions::VirtualToken::Digest(const unsigned long long sessionId, const unsigned char* data, const int length, bool digestLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->Digest(sessionId, TlvStructure(0, data, length), digestLengthRequest);
}

DigestUpdateResult Abstractions::VirtualToken::DigestUpdate(const unsigned long long sessionId, const unsigned char* data, const int length) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DigestUpdate(sessionId, TlvStructure(0, data, length));
}

DigestFinalResult Abstractions::VirtualToken::DigestFinal(const unsigned long long sessionId, bool digestLengthRequest) const
{
	//todo: handle cases or do client side validations
	return this->serviceProxy->DigestFinal(sessionId, digestLengthRequest);
}
 


VirtualToken::~VirtualToken() {
	this->serviceProxy->Unregister(this);
}
