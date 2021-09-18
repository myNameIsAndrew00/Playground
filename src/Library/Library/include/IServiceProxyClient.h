#pragma once

#include "IServiceCommunicationResolver.h";

namespace Abstractions {
	/*Implement methods used by a client which use the service proxy class*/
	class IServiceProxyClient abstract { 
	public:
		virtual ~IServiceProxyClient() { }
	};

	using IServiceProxyClientReference = std::shared_ptr<IServiceProxyClient>;
}