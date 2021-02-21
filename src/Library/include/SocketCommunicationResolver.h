#include "IServiceCommunicationResolver.h"
#include <WinSock2.h>

namespace Infrastructure {
	class SocketCommunicationResolver : public Abstractions::IServiceCommunicationResolver {
	private: 
		sockaddr_in address;
		SOCKET socket;
		bool initialised;

		void initialiseAddress(const char* const ip, unsigned short port);

	public: 

		SocketCommunicationResolver(const char* const ip, unsigned short port);
		
		bool InitialiseCommunication();
		unsigned char* ExecuteRequest(unsigned char* payload);
		bool FinaliseCurrentCommunication();

		~SocketCommunicationResolver();
	};
}