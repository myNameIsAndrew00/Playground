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

Abstractions::Bytes Infrastructure::SocketCommunicationResolver::ExecuteRequest(const Abstractions::Bytes& bytes)
{  
	  
	if (!sendData(bytes.GetBytes(), bytes.GetLength()))
		//todo: handle communication error here
		return Abstractions::Bytes(nullptr, 0);
	 
	unsigned char* receivedData;
	unsigned int receivedDataLength = receiveData(&receivedData);

	return Abstractions::Bytes(receivedData, receivedDataLength);
}

bool Infrastructure::SocketCommunicationResolver::FinaliseCurrentCommunication()
{
	if(!initialised) return false;

	sendData(nullptr, -1);
}

Infrastructure::SocketCommunicationResolver::~SocketCommunicationResolver()
{
	closesocket(socket);
	
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

bool Infrastructure::SocketCommunicationResolver::sendData(const unsigned char* payload, const int payloadLength)
{ 
	//send bytes to the server. Payload length can be negative to trigger connection close.

	unsigned int packetSize = max(0, payloadLength) + this->headerSize;
	unsigned char* packet = new unsigned char[packetSize];

	unsigned int networkPayloadLength = htonl(payloadLength);

	memcpy(packet, &networkPayloadLength, this->headerSize);
	if(payload != nullptr && payloadLength > 0)
		memcpy(packet + this->headerSize, payload, payloadLength);

	int sentBytesCount = 0;
	while (packetSize > 0)
	{
		sentBytesCount = send(this->socket, (const char*)(packet + sentBytesCount), packetSize, 0);
		if (sentBytesCount < 0) return false;
		packetSize -= sentBytesCount;
	} 

	return true;
}

unsigned int Infrastructure::SocketCommunicationResolver::receiveData(unsigned char** receivedData)
{
	if (receivedData == nullptr) return 0;

	unsigned int packetSize = 0;
	if (recv(this->socket, (char*)&packetSize, sizeof(int), 0) < 0) return 0;
	packetSize = ntohl(packetSize);
	const unsigned int packetSizeResult = packetSize;

	*receivedData = new unsigned char[packetSize];
	int receivedBytesCount = 0;

	while (packetSize > 0)
	{
		receivedBytesCount = recv(this->socket, (char*)(*receivedData + receivedBytesCount), packetSize, 0);
		if (receivedBytesCount < 0) return 0;
		packetSize -= receivedBytesCount;
	}

	return packetSizeResult;
}


#pragma endregion