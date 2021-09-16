#pragma once
#include "Bytes.h"
 

namespace Abstractions {
	class TlvStructure {
	public:
		TlvStructure(const unsigned long type, const unsigned char* data, const int dataLength);

		const Bytes& GetBytes() const;
		Bytes GetRaw() const;
		long long GetType() const;

	private:
		long long type;
		Bytes bytes;
	};
}