#include "../include/ServiceProxy.h"

using namespace Abstractions;

ServiceProxy::ServiceProxy(
	const IServiceCommunicationResolverReference& communicationResolver,
	const IServiceProtocolDispatcherReference& protocolDispatcher
	)
	: communicationInitialised(false)
{ 
	this->communicationResolver = communicationResolver;
	this->protocolDispatcher = protocolDispatcher;
}

bool ServiceProxy::Register(const IServiceProxyClientReference& client) {

	this->client = client; 

	if (this->communicationInitialised)
		this->communicationResolver->FinaliseCurrentCommunication();

	this->communicationInitialised = this->communicationResolver->InitialiseCommunication();

	return this->communicationInitialised;
}

int Abstractions::ServiceProxy::BeginSession()
{  
	BytesReader* reader = this->executeRequest(Abstractions::ServiceActionCode::BeginSession, nullptr, 0);
	if (reader == nullptr) return -1;

	int result = reader->PeekInt();
	
	delete reader;
	return result;
}

bool Abstractions::ServiceProxy::EndSession(const int sessionId)
{
	BytesReader* reader = this->executeRequest(Abstractions::ServiceActionCode::EndSession, nullptr, 0);

	if (reader == nullptr) return false;

	delete reader;
	return true;
}



#pragma region Private

BytesReader* ServiceProxy::executeRequest(Abstractions::ServiceActionCode code, const unsigned char* data, const unsigned int dataLength) {
	if (!this->communicationInitialised) return nullptr;

	Bytes clientRequest = data == nullptr ?
		this->protocolDispatcher->CreateClientRequest(code) :
		this->protocolDispatcher->CreateClientRequest(code, Bytes(data, dataLength));

	Abstractions::ServiceExecutionResult executionResult =
		this->protocolDispatcher->ParseServiceResponse(
			this->communicationResolver->ExecuteRequest(clientRequest));

	return new Abstractions::BytesReader(BytesReference(new Bytes(std::move(executionResult.GetBytes()))));
} 


#pragma endregion