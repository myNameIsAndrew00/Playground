#include "..\include\Bytes.h"
#include <memory>

#include <type_traits>

using namespace Abstractions;

Bytes::Bytes()
	: byteArray(nullptr), length(0)
{
}

Bytes::Bytes(const unsigned char* byteArray, const unsigned int length)
	: byteArray(nullptr), length(0)
{
	this->copy(byteArray, length);
}

Bytes::Bytes(const char character)
	: byteArray(nullptr), length(0)
{
	this->copy((const unsigned char*)&character, sizeof(char));
}

Bytes::Bytes(const int integer)
{
	int swapedInteger = _byteswap_ulong(integer);
	this->copy((const unsigned char*)&swapedInteger, sizeof(int));
}

Bytes::Bytes(const long long int64)
{
	long long swapedLong = _byteswap_uint64(int64);
	this->copy((const unsigned char*)&swapedLong, sizeof(long long));
}

Bytes::Bytes(const Bytes& other)
	: byteArray(nullptr), length(0)
{ 
	this->copy(other.byteArray, other.length);
}

Bytes Bytes::operator=(const Bytes& other)
{
	return Bytes(other);
}

Bytes::Bytes(Bytes&& source) noexcept
{
	this->byteArray = source.byteArray;

	this->length = source.length;

	source.byteArray = nullptr;
	source.length = 0;
}

void Abstractions::Bytes::SetFromArray(unsigned char*& byteArray, const unsigned int length)
{
	if (this->byteArray != nullptr) delete this->byteArray;

	this->byteArray = byteArray;
	this->length = length;

	byteArray = nullptr;
}


Bytes::~Bytes()
{
	if (this->byteArray != nullptr) delete this->byteArray;
	this->length = 0;
}

const unsigned char* Bytes::GetBytes() const
{
	return this->byteArray;
}

const unsigned int Bytes::GetLength() const
{
	return this->length;
}


#pragma region Private


void Bytes::copy(const unsigned char* byteArray, const unsigned int length)
{
	if (byteArray == nullptr || length == 0) return;

	if (this->byteArray != nullptr) delete this->byteArray;
	this->byteArray = new unsigned char[length];

	memcpy(this->byteArray, byteArray, length);	 
	this->length = length;
}

#pragma endregion