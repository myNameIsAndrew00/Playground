#include "../include/ServiceProxy.h"

ServiceProxy::ServiceProxy()
{ 
}

bool ServiceProxy::Register(const IServiceProxyClientReference& client) {

	this->clients.push_back(client);

	return true;
}