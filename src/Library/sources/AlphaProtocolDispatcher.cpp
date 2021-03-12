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
    unsigned char dataLength = payload.GetLength() - sizeof(unsigned long);
    unsigned char* data = new unsigned char[dataLength];
    unsigned long code = 0;

    memcpy(data, payload.GetBytes() + sizeof(unsigned long), dataLength);
    memcpy(&code, payload.GetBytes(), sizeof(unsigned long));
     
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