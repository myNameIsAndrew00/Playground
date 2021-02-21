#include "../include/ServiceProxy.h"

using namespace Abstractions;

ServiceProxy::ServiceProxy(const IServiceCommunicationResolverReference& communicationResolver)
{ 
	this->communicationResolver = communicationResolver;
}

bool ServiceProxy::Register(const IServiceProxyClientReference& client) {

	this->client = client;

	return true;
}