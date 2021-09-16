#include "..\include\AlphaProtocolDispatcher.h"
#include <memory>

Abstractions::Bytes Infrastructure::AlphaProtocolDispatcher::CreateClientRequest(Abstractions::ServiceActionCode code, const Abstractions::Bytes& payload)
{    
    return this->createClientRequest(code, payload);
}

Abstractions::Bytes Infrastructure::AlphaProtocolDispatcher::CreateClientRequest(Abstractions::ServiceActionCode code)
{
    Abstractions::Bytes bytes;
    return this->createClientRequest(code, bytes);
}

Abstractions::ServiceExecutionResult Infrastructure::AlphaProtocolDispatcher::ParseServiceResponse(const Abstractions::Bytes& payload)
{
    const int codeSize = 8;
    unsigned char dataLength = payload.GetLength() - codeSize; //sizeof(long) 64bit
    unsigned char* data = new unsigned char[dataLength];
    unsigned long long code = 0;

    memcpy(data, payload.GetBytes() + codeSize, dataLength);
    memcpy(&code, payload.GetBytes(), codeSize);

    code = _byteswap_uint64(code);

    return Abstractions::ServiceExecutionResult(Abstractions::Bytes(data, dataLength), code);
}


#pragma region Private

Abstractions::Bytes Infrastructure::AlphaProtocolDispatcher::createClientRequest(Abstractions::ServiceActionCode code, const Abstractions::Bytes& payload)
{
    unsigned char resultLength = payload.GetLength() + 1;
    unsigned char* result = new unsigned char[resultLength];

    memcpy(result, (char*)&code, 1);

    if (payload.GetLength())
        memcpy(result + 1, payload.GetBytes(), payload.GetLength());

    return Abstractions::Bytes(result, resultLength);
}

#pragma endregion