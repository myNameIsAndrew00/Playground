#include "../include/ServiceProxy.h"
#include "../include/TlvStructure.h"

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

	return true;
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

			return CreateSessionResult((CreateSessionResult::Code)code, result);
		}
		);
}

EndSessionResult ServiceProxy::EndSession(const Handler sessionId)
{ 
	return this->InvokeServer<EndSessionResult>(
		ServiceActionCode::EndSession,
		[sessionId]() {
			return Bytes((const long long)sessionId);
		},
		[](auto reader, auto code) {

			return EndSessionResult((EndSessionResult::Code)code, true);
		}
		); 
}

CreateObjectResult ServiceProxy::CreateObject(const Handler sessionId, const std::list<TlvStructure>& attributes) {
	return this->InvokeServer<CreateObjectResult>(
		ServiceActionCode::CreateObject,
		[attributes]() {
			return Bytes(attributes);
		},
		[](auto reader, auto code) {
			unsigned long long result = reader->PeekLong();

			return CreateObjectResult((CreateObjectResult::Code)code, result);
		},
		sessionId
		);
}


#pragma region Private

BytesReader* ServiceProxy::executeRequest(Abstractions::ServiceActionCode serviceActionCode, unsigned long& resultCode, const unsigned char* data, const unsigned int dataLength) {
	if (!this->communicationInitialised) return nullptr;

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