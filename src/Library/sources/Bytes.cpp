#include "..\include\Bytes.h"
#include <memory>

using namespace Abstractions;

Bytes::Bytes()
	: byteArray(nullptr), length(0)
{
}

Bytes::Bytes(unsigned char* byteArray, const unsigned int length)
	: byteArray(nullptr), length(0)
{
	this->byteArray = byteArray;
	this->length = length;
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


void Bytes::copy(unsigned char* byteArray, const unsigned int length)
{
	if (byteArray == nullptr || length == 0) return;

	if (this->byteArray != nullptr) delete this->byteArray;
	this->byteArray = new unsigned char[length];

	memcpy(this->byteArray, byteArray, length);	 
	this->length = length;
}

#pragma endregion