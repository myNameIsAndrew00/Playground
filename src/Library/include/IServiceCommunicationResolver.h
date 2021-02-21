#pragma once

#include <memory>

namespace Abstractions {
	/*Represents an instance which will be used to make the communication (send and receive bytes) with the server*/
	class IServiceCommunicationResolver abstract {
	public:
		/*Initialise the communication to the service*/
		virtual bool InitialiseCommunication() = 0;

		/*Execute a request to the server*/
		virtual unsigned char* ExecuteRequest(unsigned char* payload) = 0;

		/*Stop the current communication instance */
		virtual bool FinaliseCurrentCommunication() = 0;

		virtual ~IServiceCommunicationResolver() { }
	};

	using IServiceCommunicationResolverReference = std::shared_ptr<IServiceCommunicationResolver>;
}
