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
	unsigned long resultCode = (unsigned long)Abstractions::CreateSessionResult::Code::OK;

	BytesReader* reader = this->executeRequest(Abstractions::ServiceActionCode::BeginSession, resultCode, nullptr, 0);
	if (reader == nullptr) return CreateSessionResult(Abstractions::CreateSessionResult::Code::GENERAL_ERROR, -1);

	unsigned long long result = reader->PeekLong();

	delete reader;
	return CreateSessionResult(Abstractions::CreateSessionResult::Code::OK, result);
}

EndSessionResult ServiceProxy::EndSession(const Handler sessionId)
{
	unsigned long resultCode = (unsigned long)Abstractions::CreateSessionResult::Code::OK;

	Bytes sessionIdBytes((const long long)sessionId);

	BytesReader* reader = this->executeRequest(Abstractions::ServiceActionCode::EndSession, resultCode, sessionIdBytes.GetBytes(), sessionIdBytes.GetLength());
	 
	if (reader == nullptr) return EndSessionResult(Abstractions::EndSessionResult::Code::GENERAL_ERROR, false);

	delete reader;
	return EndSessionResult(Abstractions::EndSessionResult::Code::OK, true);
}

CreateObjectResult ServiceProxy::CreateObject(const Handler sessionId, const std::list<TlvStructure>& attributes) {
	unsigned long resultCode = (unsigned long)Abstractions::CreateSessionResult::Code::OK;

	Bytes requestBytes = Bytes((const long long)sessionId).Append(Bytes(attributes));

	BytesReader* reader = this->executeRequest(Abstractions::ServiceActionCode::CreateObject, resultCode, requestBytes.GetBytes(), requestBytes.GetLength());
	 
	if (reader == nullptr) CreateObjectResult(Abstractions::CreateObjectResult::Code::GENERAL_ERROR, 0ll);

	unsigned long long result = reader->PeekLong();
	return CreateSessionResult(Abstractions::CreateSessionResult::Code::OK, result);
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