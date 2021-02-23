#pragma once

#include <memory>

namespace Abstractions {
	/**Represents an instance which will be used to make the communication (send and receive bytes) with the server*/
	class IServiceCommunicationResolver abstract {
	public:
		/*Initialise the communication with the service*/
		virtual bool InitialiseCommunication() = 0;

		/** 
		* \brief Execute a request to the server
		*
		* \param payload: Payload which will be sent 
		* \param payloadLength: Length of the payload
		**/
		virtual unsigned char* ExecuteRequest(unsigned char* payload, unsigned int payloadLength) = 0;

		/*Stop the current communication instance */
		virtual bool FinaliseCurrentCommunication() = 0;

		virtual ~IServiceCommunicationResolver() { }
	};

	using IServiceCommunicationResolverReference = std::shared_ptr<IServiceCommunicationResolver>;
}
