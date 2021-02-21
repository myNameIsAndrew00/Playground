#pragma once
#include "IServiceProtocolDispatcher.h"

namespace Infrastructure {
	class AlphaProtocolDispatcher : public Abstractions::IServiceProtocolDispatcher {
	public:
		unsigned char* CreateClientRequest(Abstractions::ServiceActionCode code, unsigned char* payload);

		Abstractions::ServiceExecutionResult ParseServiceResponse(unsigned char* payload);
	};
}