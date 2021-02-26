#pragma once
#include "ServiceActionCode.h"
#include "ServiceExecutionResult.h"
#include "Bytes.h"

namespace Abstractions {
	/*Implements method to package data in a format used by service protocol*/
	class IServiceProtocolDispatcher abstract {
	public: 

		virtual Bytes CreateClientRequest(ServiceActionCode code, const Bytes& payload) = 0;

		virtual ServiceExecutionResult ParseServiceResponse(const Bytes& payload) = 0;

		virtual ~IServiceProtocolDispatcher() { }
	};
}