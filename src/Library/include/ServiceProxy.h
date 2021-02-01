#pragma once  
#include "IServiceProxyClient.h"
#include <list>

/*Represents a proxy class for the cryptographic service*/
class ServiceProxy
{
public:
	ServiceProxy();

	/*Add a new client to the registered clients list*/
	bool Register(const IServiceProxyClientReference& client);

private:
	
	/*Represents a list of clients registered to the service*/
	std::list<IServiceProxyClientReference> clients;
};
 
using ServiceProxyReference = std::shared_ptr<ServiceProxy>;