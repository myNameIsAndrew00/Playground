#include "../include/ServiceProxy.h"
#include "../include/TlvStructure.h"
#include "../include/ReturnCode.h"

using namespace Abstractions;

ServiceProxy::ServiceProxy(
	const IServiceCommunicationResolverReference& communicationResolver,
	const IServiceProtocolDispatcherReference& protocolDispatcher
)
	: communicationInitialised(false), client(nullptr)
{
	this->communicationResolver = communicationResolver;
	this->protocolDispatcher = protocolDispatcher;
}

bool ServiceProxy::Register(IServiceProxyClient* client) {

	this->client = client;

	if (this->communicationInitialised)
		this->communicationResolver->FinaliseCurrentCommunication();

	this->communicationInitialised = this->communicationResolver->InitialiseCommunication();

	return this->communicationInitialised;
}

bool ServiceProxy::Unregister(IServiceProxyClient* client) {
	if (!this->DetachCurrentClient())
		return false;

	if (this->client != client) return false;

	this->client = nullptr;

	return true;
}


bool ServiceProxy::DetachCurrentClient() {
	if (!this->communicationInitialised) return true;

	return this->communicationResolver->FinaliseCurrentCommunication();
}

CreateSessionResult ServiceProxy::BeginSession()
{
	return this->InvokeServer<CreateSessionResult>(
			ServiceActionCode::BeginSession,
			[]() {
				return Bytes();
			},
			[](auto reader, auto code) {
				unsigned long long result = reader->PeekLong();

				return CreateSessionResult((ReturnCode)code, result);
			}
	);
}

EndSessionResult ServiceProxy::EndSession(const Id sessionId)
{ 
	return this->InvokeServer<EndSessionResult>(
			ServiceActionCode::EndSession,
			[sessionId]() {
				return Bytes(sessionId);
			},
			[](auto reader, auto code) {

				return EndSessionResult((ReturnCode)code, true);
			}
	); 
}

CreateObjectResult ServiceProxy::CreateObject(const Id sessionId, const std::list<TlvStructure>& attributes) {
	return this->InvokeServer<CreateObjectResult>(
			ServiceActionCode::CreateObject,
			[attributes]() {
				return Bytes(attributes);
			},
			[](auto reader, auto code) {
				unsigned long long result = reader->PeekLong();

				return CreateObjectResult((ReturnCode)code, result);
			},
			sessionId
	);
}

EncryptInitResult Abstractions::ServiceProxy::EncryptInit(const Id sessionId, const Id objectId, const TlvStructure& mechanism)
{
	return this->InvokeServer<EncryptInitResult>(
			ServiceActionCode::EncryptInit,
			[objectId, mechanism]() {
				return Bytes(objectId).Append(mechanism);
			},
			[](auto reader, auto code) {

				return EncryptInitResult((ReturnCode)code, true);
			},
			sessionId
	);
}

EncryptResult Abstractions::ServiceProxy::Encrypt(const Id sessionId, TlvStructure dataToEncrypt, bool requestLength)
{
	return this->InvokeServer<EncryptResult>(
			ServiceActionCode::Encrypt,
			[dataToEncrypt, requestLength]() {
				return Bytes(requestLength).Append(dataToEncrypt);
			},
			[](auto reader, auto code) {
				return EncryptResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
	); 
}

EncryptUpdateResult Abstractions::ServiceProxy::EncryptUpdate(const Id sessionId, TlvStructure dataToEncrypt, bool requestLength)
{
	return this->InvokeServer<EncryptUpdateResult>(
			ServiceActionCode::EncryptUpdate,
			[dataToEncrypt, requestLength]() {
				return Bytes(requestLength).Append(dataToEncrypt);
			},
			[](auto reader, auto code) {
				return EncryptUpdateResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		); 
}

EncryptFinalResult Abstractions::ServiceProxy::EncryptFinal(const Id sessionId, bool requestLength)
{
	return this->InvokeServer<EncryptFinalResult>(
			ServiceActionCode::EncryptUpdate,
			[requestLength]() {
				return Bytes(requestLength);
			},
			[](auto reader, auto code) {
				return EncryptFinalResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		); 
}

DecryptInitResult Abstractions::ServiceProxy::DecryptInit(const Id sessionId, const Id objectId, const TlvStructure& mechanism)
{
	return this->InvokeServer<DecryptInitResult>(
			ServiceActionCode::DecryptInit,
			[objectId, mechanism]() {
				return Bytes(objectId).Append(mechanism);
			},
			[](auto reader, auto code) {
				return DecryptInitResult((ReturnCode)code, true);
			},
			sessionId
		);
}

DecryptResult Abstractions::ServiceProxy::Decrypt(const Id sessionId, TlvStructure dataToDecrypt, bool requestLength)
{
	return this->InvokeServer<DecryptResult>(
			ServiceActionCode::Decrypt,
			[dataToDecrypt, requestLength]() {
				return Bytes(requestLength).Append(dataToDecrypt);
			},
			[](auto reader, auto code) {
				return DecryptResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

DecryptUpdateResult Abstractions::ServiceProxy::DecryptUpdate(const Id sessionId, TlvStructure dataToDecrypt, bool requestLength)
{
	return this->InvokeServer<EncryptUpdateResult>(
			ServiceActionCode::DecryptUpdate,
			[dataToDecrypt, requestLength]() {
				return Bytes(requestLength).Append(dataToDecrypt);
			},
			[](auto reader, auto code) {
				return DecryptUpdateResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

DecryptFinalResult Abstractions::ServiceProxy::DecryptFinal(const Id sessionId, bool requestLength)
{
	return this->InvokeServer<EncryptFinalResult>(
			ServiceActionCode::DecryptFinal,
			[requestLength]() {
				return Bytes(requestLength);
			},
			[](auto reader, auto code) {
				return DecryptFinalResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

DigestInitResult Abstractions::ServiceProxy::DigestInit(const Id sessionId, const TlvStructure& mechanism)
{
	return this->InvokeServer<DigestInitResult>(
			ServiceActionCode::DigestInit,
			[mechanism]() {
				return Bytes(mechanism);
			},
			[](auto reader, auto code) {
				return DigestInitResult((ReturnCode)code, true);
			},
			sessionId
		);
}

DigestResult Abstractions::ServiceProxy::Digest(const Id sessionId, TlvStructure dataToDigest, bool requestLength)
{
	return this->InvokeServer<DigestResult>(
			ServiceActionCode::Digest,
			[requestLength, dataToDigest]() {
				return Bytes(requestLength).Append(dataToDigest);
			},
			[](auto reader, auto code) {
				return DigestResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

DigestUpdateResult Abstractions::ServiceProxy::DigestUpdate(const Id sessionId, TlvStructure dataToDigest)
{
	return this->InvokeServer<DigestUpdateResult>(
			ServiceActionCode::DigestUpdate,
			[dataToDigest]() {
				return Bytes(dataToDigest);
			},
			[](auto reader, auto code) {
				return DigestUpdateResult((ReturnCode)code, true);
			},
			sessionId
		);
}

DigestFinalResult Abstractions::ServiceProxy::DigestFinal(const Id sessionId, bool lengthRequest)
{
	return this->InvokeServer<DigestFinalResult>(
			ServiceActionCode::DigestFinal,
			[lengthRequest]() {
				return Bytes(lengthRequest);
			},
			[](auto reader, auto code) {
				return DigestFinalResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

GenerateKeyPairResult Abstractions::ServiceProxy::GenerateKeyPair(const Id sessionId, const TlvStructure& mechanism, const std::list<TlvStructure>& publicKeyAttributes, const std::list<TlvStructure>& privateKeyAttributes)
{
	return this->InvokeServer<GenerateKeyPairResult>(
			ServiceActionCode::GenerateKeyPair,
			[mechanism, publicKeyAttributes, privateKeyAttributes]() {
				return Bytes(mechanism).Append(publicKeyAttributes).AppendListBreak().Append(privateKeyAttributes);
			},
			[](auto reader, auto code) {
				return GenerateKeyPairResult((ReturnCode)code, std::pair<unsigned long long, unsigned long long>( reader->PeekLong(), reader->PeekLong() ));
			},
			sessionId
		);
}

SignInitResult Abstractions::ServiceProxy::SignInit(const Id sessionId, const Id privateKeyId, const TlvStructure& mechanism)
{
	return this->InvokeServer<SignInitResult>(
			ServiceActionCode::SignInit,
			[privateKeyId, mechanism]() {
				return Bytes(privateKeyId).Append(mechanism);
			},
			[](auto reader, auto code) {
				return SignInitResult((ReturnCode)code, true);
			},
			sessionId
		);
}

SignResult Abstractions::ServiceProxy::Sign(const Id sessionId, const TlvStructure& dataToSign, bool requestLength)
{
	return this->InvokeServer<SignResult>(
			ServiceActionCode::Sign,
			[requestLength, dataToSign]() {
				return Bytes(requestLength).Append(dataToSign);
			},
			[](auto reader, auto code) {
				return SignResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

SignUpdateResult Abstractions::ServiceProxy::SignUpdate(const Id sessionId, const TlvStructure& dataToSign)
{
	return this->InvokeServer<SignUpdateResult>(
			ServiceActionCode::SignUpdate,
			[dataToSign]() {
				return Bytes(dataToSign);
			},
			[](auto reader, auto code) {
				return SignUpdateResult((ReturnCode)code, true);
			},
			sessionId
		);
}

SignFinalResult Abstractions::ServiceProxy::SignFinal(const Id sessionId, bool lengthRequest)
{
	return this->InvokeServer<SignFinalResult>(
			ServiceActionCode::SignFinal,
			[lengthRequest]() {
				return Bytes(lengthRequest);
			},
			[](auto reader, auto code) {
				return SignFinalResult((ReturnCode)code, reader->PeekBytes());
			},
			sessionId
		);
}

VerifyInitResult Abstractions::ServiceProxy::VerifyInit(const Id sessionId, const Id publicKeyId, const TlvStructure& mechanism)
{
	return this->InvokeServer<VerifyInitResult>(
		ServiceActionCode::VerifyInit,
		[publicKeyId, mechanism]() {
			return Bytes(publicKeyId).Append(mechanism);
		},
		[](auto reader, auto code) {
			return VerifyInitResult((ReturnCode)code, true);
		},
			sessionId
		);
}

VerifyResult Abstractions::ServiceProxy::Verify(const Id sessionId, const TlvStructure& dataToVerify, const TlvStructure& signedData)
{
	return this->InvokeServer<VerifyResult>(
			ServiceActionCode::Verify,
			[dataToVerify, signedData]() {
				return Bytes(dataToVerify).Append(signedData);
			},
			[](auto reader, auto code) {
				return VerifyResult((ReturnCode)code, reader->PeekBool());
			},
			sessionId
		);
}

VerifyUpdateResult Abstractions::ServiceProxy::VerifyUpdate(const Id sessionId, const TlvStructure& dataToVerify)
{
	return this->InvokeServer<VerifyUpdateResult>(
			ServiceActionCode::VerifyUpdate,
			[dataToVerify]() {
				return Bytes(dataToVerify);
			},
			[](auto reader, auto code) {
				return VerifyUpdateResult((ReturnCode)code, true);
			},
			sessionId
		);
}

VerifyFinalResult Abstractions::ServiceProxy::VerifyFinal(const Id sessionId, const TlvStructure& signedData)
{
	return this->InvokeServer<VerifyFinalResult>(
			ServiceActionCode::VerifyFinal,
			[signedData]() {
				return Bytes(signedData);
			},
			[](auto reader, auto code) {
				return VerifyFinalResult((ReturnCode)code, reader->PeekBool());
			},
			sessionId
		);
}
 


#pragma region Private

BytesReader* ServiceProxy::executeRequest(Abstractions::ServiceActionCode serviceActionCode, unsigned long& resultCode, const unsigned char* data, const unsigned int dataLength) {
	if (!this->communicationInitialised) {
		resultCode = (unsigned long)Abstractions::ReturnCode::CRYPTOKI_NOT_INITIALIZED;
		return nullptr;	
	}
	Bytes clientRequest = data == nullptr ?
		this->protocolDispatcher->CreateClientRequest(serviceActionCode) :
		this->protocolDispatcher->CreateClientRequest(serviceActionCode, Bytes(data, dataLength));

	Abstractions::ServiceExecutionResult executionResult =
		this->protocolDispatcher->ParseServiceResponse(
			this->communicationResolver->ExecuteRequest(clientRequest));

	resultCode = executionResult.GetResultCode();
	auto reader = new Abstractions::BytesReader(BytesReference(new Bytes(std::move(executionResult.GetBytes()))));

	return reader;
}


#pragma endregion