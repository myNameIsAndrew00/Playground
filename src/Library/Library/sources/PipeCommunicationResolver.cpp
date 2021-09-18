#include "../include/PipeCommunicationResolver.h"

bool Infrastructure::PipeCommunicationResolver::InitialiseCommunication()
{
	return false;
}

Abstractions::Bytes Infrastructure::PipeCommunicationResolver::ExecuteRequest(const Abstractions::Bytes& bytes)
{
	Abstractions::Bytes result(nullptr, 0);

	return result;
}

bool Infrastructure::PipeCommunicationResolver::FinaliseCurrentCommunication()
{
	return false;
}
