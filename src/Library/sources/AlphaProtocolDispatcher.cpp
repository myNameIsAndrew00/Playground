#include "..\include\AlphaProtocolDispatcher.h"

unsigned char* Infrastructure::AlphaProtocolDispatcher::CreateClientRequest(Abstractions::ServiceActionCode code, unsigned char* payload)
{
    //todo: implement
    return nullptr;
}

Abstractions::ServiceExecutionResult Infrastructure::AlphaProtocolDispatcher::ParseServiceResponse(unsigned char* payload)
{
    //todo: implement
    return Abstractions::ServiceExecutionResult(nullptr, 0);
}
