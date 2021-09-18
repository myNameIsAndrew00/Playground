#pragma once
#include "IServiceProtocolDispatcher.h"
 

namespace Infrastructure {
	class AlphaProtocolDispatcher : public Abstractions::IServiceProtocolDispatcher {
	public:
		Abstractions::Bytes CreateClientRequest(Abstractions::ServiceActionCode code, const Abstractions::Bytes& payload);
		Abstractions::Bytes CreateClientRequest(Abstractions::ServiceActionCode code);

		Abstractions::ServiceExecutionResult ParseServiceResponse(const Abstractions::Bytes& bytes);
	private:
		Abstractions::Bytes createClientRequest(Abstractions::ServiceActionCode code, const Abstractions::Bytes& payload);
	};
}