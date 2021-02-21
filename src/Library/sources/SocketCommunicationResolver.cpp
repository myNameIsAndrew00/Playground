#include "../include/SocketCommunicationResolver.h"
#include <ws2tcpip.h>

Infrastructure::SocketCommunicationResolver::SocketCommunicationResolver(const char* const ip, unsigned short port)
	: initialised(false), socket(0)
{
	memset(&address, 0, sizeof(address));

	WSADATA tempWsaData;

	if (0 == WSAStartup(MAKEWORD(2, 2), (LPWSADATA)(&tempWsaData))) {
		initialiseAddress(ip, port);
		this->socket = ::socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	}
}

bool Infrastructure::SocketCommunicationResolver::InitialiseCommunication()
{
	if (!initialised) return false;
	
	return 0 == connect(this->socket, (struct sockaddr*)&address, sizeof(address));
}

unsigned char* Infrastructure::SocketCommunicationResolver::ExecuteRequest(unsigned char* payload)
{
	return nullptr;
}

bool Infrastructure::SocketCommunicationResolver::FinaliseCurrentCommunication()
{
	if(!initialised) return false;

	return 0 == closesocket(socket);
}

Infrastructure::SocketCommunicationResolver::~SocketCommunicationResolver()
{
	WSACleanup();
}

#pragma region Private


void Infrastructure::SocketCommunicationResolver::initialiseAddress(const char* const ip, unsigned short port)
{
	address.sin_family = AF_INET;
	address.sin_port = htons(port);

	addrinfo* addressInfo;
	if (0 == getaddrinfo(ip, nullptr, nullptr, &addressInfo)) {
		memcpy(&(address.sin_addr),
			&((sockaddr_in*)addressInfo->ai_addr)->sin_addr,
			sizeof(struct in_addr));


		initialised = true;
		freeaddrinfo(addressInfo); 
	}
}


#pragma endregion