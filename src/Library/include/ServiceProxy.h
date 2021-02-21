#pragma once  
#include "IServiceProxyClient.h"
#include <list>

namespace Abstractions {
	/*Represents a proxy class for the cryptographic service. It will be responsable for handling messages*/
	class ServiceProxy
	{
	public: 
		ServiceProxy(){}
		ServiceProxy(const IServiceCommunicationResolverReference& communicationResolver);

		/*Add a new client to the registered clients list*/
		bool Register(const IServiceProxyClientReference& client);

	private:

		/*Represents a list of clients registered to the service*/
		IServiceProxyClientReference client;
		IServiceCommunicationResolverReference communicationResolver;
	};

	using ServiceProxyReference = std::shared_ptr<ServiceProxy>;
}