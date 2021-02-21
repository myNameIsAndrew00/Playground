#pragma once
#include "ServiceActionCode.h"
#include "ServiceExecutionResult.h"

namespace Abstractions {
	/*Implements method to package data in a format used by service protocol*/
	class IServiceProtocolDispatcher abstract {
	public:
		virtual unsigned char* CreateClientRequest(ServiceActionCode code, unsigned char* payload) = 0;

		virtual ServiceExecutionResult ParseServiceResponse(unsigned char* payload) = 0;

		virtual ~IServiceProtocolDispatcher() { }
	};
}