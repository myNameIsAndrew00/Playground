#pragma once
#include "IServiceProtocolDispatcher.h"
 

namespace Infrastructure {
	class AlphaProtocolDispatcher : public Abstractions::IServiceProtocolDispatcher {
	public:
		Abstractions::Bytes CreateClientRequest(Abstractions::ServiceActionCode code, const Abstractions::Bytes& payload);

		Abstractions::ServiceExecutionResult ParseServiceResponse(const Abstractions::Bytes& bytes);
	};
}