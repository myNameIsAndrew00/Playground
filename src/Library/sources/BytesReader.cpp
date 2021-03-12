#include "..\include\BytesReader.h"

Abstractions::BytesReader::BytesReader(const BytesReference& bytes)
    : cursor(0), bytesLength(bytes->GetLength()), bufferPointer(bytes->GetBytes())
{
    this->bytes = bytes;
}

Abstractions::BytesReader::BytesReader(BytesReader&& bytesReader) noexcept
    : cursor(0), bytesLength(bytesReader.bytesLength), bufferPointer(bytesReader.bufferPointer)
{
    this->bytes = std::move(bytesReader.bytes);
}

char Abstractions::BytesReader::PeekChar()
{
    const char* valuePointer = this->peekType<char>();
    return *valuePointer;
}

int Abstractions::BytesReader::PeekInt()
{
    const int* valuePointer = this->peekType<int>();
    return *valuePointer;
}

long Abstractions::BytesReader::PeekLong()
{
    const long* valuePointer = this->peekType<long>();
    return *valuePointer;
}

Abstractions::Bytes Abstractions::BytesReader::PeekBytes(unsigned int length)
{
    const unsigned char* bytes = this->peekBytes(length);
    return Bytes(bytes, length);
}

void Abstractions::BytesReader::ResetCursor()
{
    this->cursor = 0;
}

#pragma region Private

const unsigned char* Abstractions::BytesReader::peekBytes(unsigned int& length)
{
    if (this->cursor >= this->bytesLength) {
        length = 0;
        return nullptr;
    }

    if (length > this->bytesLength - this->cursor) length = this->bytesLength - this->cursor;

    const unsigned char* result = this->bufferPointer + this->cursor;
    
    this->cursor = this->cursor + length;
    return result;
}

#pragma endregion
