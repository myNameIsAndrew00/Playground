#pragma once

#include <memory>

namespace Abstractions {
	/*Represents an instance which will be used to make the communication (send and receive bytes) with the server*/
	class IServiceCommunicationResolver abstract {
	public:

	};

	using IServiceCommunicationResolverReference = std::shared_ptr<IServiceCommunicationResolver>;
}
