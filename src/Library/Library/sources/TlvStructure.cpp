#include "..\include\TlvStructure.h"

using namespace Abstractions;

TlvStructure::TlvStructure(const unsigned long type, const unsigned char* data, const int dataLength)
	: type(type), bytes(data, dataLength)
{
}

const Bytes& TlvStructure::GetBytes() const {
	return bytes;
}

long long TlvStructure::GetType() const {
	return type;
}

Bytes TlvStructure::GetRaw() const {
	return Bytes((long long)type).Append((int)bytes.GetLength()).Append(bytes);
}