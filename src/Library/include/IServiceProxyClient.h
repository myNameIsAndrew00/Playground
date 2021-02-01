#pragma once
#include <memory>

/*Implement methods used by a client to the service proxy.*/
class IServiceProxyClient abstract {
public:

};
 
using IServiceProxyClientReference = std::shared_ptr<IServiceProxyClient>;
