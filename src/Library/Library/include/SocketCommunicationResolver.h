#include "IServiceCommunicationResolver.h"
#include <WinSock2.h>

namespace Infrastructure {
	class SocketCommunicationResolver : public Abstractions::IServiceCommunicationResolver {
	private: 
		sockaddr_in address;
		SOCKET socket;
		bool initialised;

		const unsigned int headerSize = 4;

		void initialiseAddress(const char* const ip, unsigned short port);
		
		/*Send data to the service. Return false if an error ocurs*/
		bool sendData(const unsigned char* payload, const int payloadLength);
		/*Wait to receive data from service*/
		unsigned int receiveData(unsigned char** receivedData);
	
	public: 

		SocketCommunicationResolver(const char* const ip, unsigned short port);
		
		bool InitialiseCommunication();
		Abstractions::Bytes ExecuteRequest(const Abstractions::Bytes& bytes);
		bool FinaliseCurrentCommunication();

		~SocketCommunicationResolver();
	};
}