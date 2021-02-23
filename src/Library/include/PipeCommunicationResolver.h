#include "IServiceCommunicationResolver.h"

namespace Infrastructure {
	class PipeCommunicationResolver : public Abstractions::IServiceCommunicationResolver {
	public: 
		bool InitialiseCommunication();

		unsigned char* ExecuteRequest(unsigned char* payload, const unsigned int payloadLengyh);

		bool FinaliseCurrentCommunication();
	};
}