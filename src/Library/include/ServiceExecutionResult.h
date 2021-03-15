#pragma once
#include "Bytes.h"

namespace Abstractions {
	class ServiceExecutionResult {
	public:
		ServiceExecutionResult(const Bytes& payload, unsigned long resultCode) : bytes(payload), resultCode(resultCode) {
		}

		const Bytes& GetBytes() const { return bytes; }

		Bytes& GetBytes() { return bytes; }

		unsigned long GetResultCode() const { return resultCode; }

	private:
		Bytes bytes;
		unsigned long resultCode;
	};
}