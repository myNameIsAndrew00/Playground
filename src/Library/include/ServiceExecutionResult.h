#pragma once

namespace Abstractions {
	class ServiceExecutionResult {
	public:
		ServiceExecutionResult(unsigned char* const bytes, unsigned long resultCode) : bytes(bytes), resultCode(resultCode) {		
		}
	private:
		unsigned char* bytes;
		unsigned long resultCode;
	};
}