#pragma once
#include "Bytes.h"

namespace Abstractions {
	class ServiceExecutionResult {
	public:
		ServiceExecutionResult(const Bytes& payload, unsigned long resultCode) : bytes(bytes), resultCode(resultCode) {
		}
	private:
		Bytes bytes;
		unsigned long resultCode;
	};
}