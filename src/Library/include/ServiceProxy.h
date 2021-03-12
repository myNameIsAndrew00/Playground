#pragma once  
#include "IServiceProxyClient.h"
#include "IServiceProtocolDispatcher.h"
#include "BytesReader.h"
#include <list>

namespace Abstractions {
	/*Represents a proxy class for the cryptographic service. It will be responsable for handling messages*/
	class ServiceProxy
	{
	public: 
		ServiceProxy(){}
		ServiceProxy(const IServiceCommunicationResolverReference& communicationResolver,
				    const IServiceProtocolDispatcherReference& protocolDispatcher );

		/*Add a new client to the registered clients list*/
		bool Register(const IServiceProxyClientReference& client);
		int BeginSession();
		bool EndSession(const int sessionId);
	private:
		bool communicationInitialised;

		/*Represents a list of clients registered to the service*/
		IServiceProxyClientReference client;
		IServiceCommunicationResolverReference communicationResolver;
		IServiceProtocolDispatcherReference protocolDispatcher;


		Abstractions::BytesReader* executeRequest(Abstractions::ServiceActionCode code, const unsigned char* data, const unsigned int dataLength);
	};

	using ServiceProxyReference = std::shared_ptr<ServiceProxy>;
}