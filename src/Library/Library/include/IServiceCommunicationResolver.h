#pragma once

#include <memory>
#include <utility>
#include "Bytes.h"

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
		**/
		virtual Bytes ExecuteRequest(const Bytes& payload) = 0;

		/*Stop the current communication instance */
		virtual bool FinaliseCurrentCommunication() = 0;

		virtual ~IServiceCommunicationResolver() { }
	};

	using IServiceCommunicationResolverReference = std::shared_ptr<IServiceCommunicationResolver>;
}
