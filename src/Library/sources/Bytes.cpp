#include "..\include\Bytes.h"
#include "..\include\TlvStructure.h"

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
	: byteArray(nullptr), length(0)
{
	int swapedInteger = _byteswap_ulong(integer);
	this->copy((const unsigned char*)&swapedInteger, sizeof(int));
}

Bytes::Bytes(const long long int64)
	: byteArray(nullptr), length(0)
{
	long long swapedLong = _byteswap_uint64(int64);
	this->copy((const unsigned char*)&swapedLong, sizeof(long long));
}

Bytes::Bytes(const Bytes& other)
	: byteArray(nullptr), length(0)
{
	this->copy(other.byteArray, other.length);
}

Bytes::Bytes(const TlvStructure& other)
	: byteArray(nullptr), length(0)
{
	Bytes bytes = other.GetRaw();
	this->copy(bytes.byteArray, bytes.length);
}


Bytes::Bytes(const std::list<TlvStructure>& tlvList)
	: byteArray(nullptr), length(0)
{
	std::list<TlvStructure>::const_iterator iterator;
	for (iterator = tlvList.begin(); iterator != tlvList.end(); ++iterator) {
		this->Append(iterator->GetRaw());
	}
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

Bytes& Bytes::Append(const unsigned char* byteArray, const unsigned int length)
{
	this->append(byteArray, length);
	
	return *this;
}

Bytes& Bytes::Append(const Bytes& bytes) {
	this->append(bytes.byteArray, bytes.length);

	return *this;
}


Bytes& Bytes::Append(const char character) {
	this->append((const unsigned char*)&character, sizeof(char));

	return *this;
}


Bytes& Bytes::Append(const int integer) {
	int swapedInteger = _byteswap_ulong(integer);

	this->append((const unsigned char*)&swapedInteger, sizeof(int));

	return *this;
}


Bytes& Bytes::Append(const long long int64) {
	long long swapedLong = _byteswap_uint64(int64);
	this->append((const unsigned char*)&swapedLong, sizeof(long long));

	return *this;
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

void Bytes::append(const unsigned char* byteArray, const unsigned int length)
{
	if (byteArray == nullptr || length == 0) return;

	const int newBufferLength = this->length + length;
	unsigned char* newBuffer = new unsigned char[newBufferLength];

	if (this->byteArray != nullptr)
		memcpy(newBuffer, this->byteArray, this->length);

	memcpy(newBuffer + this->length, byteArray, length);

	if (this->byteArray != nullptr) delete this->byteArray;

	this->length = newBufferLength;
	this->byteArray = newBuffer;
}

#pragma endregion