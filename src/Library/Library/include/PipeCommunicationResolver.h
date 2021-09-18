#include "IServiceCommunicationResolver.h"

namespace Infrastructure {
	class PipeCommunicationResolver : public Abstractions::IServiceCommunicationResolver {
	public: 
		bool InitialiseCommunication();

		Abstractions::Bytes ExecuteRequest(const Abstractions::Bytes& bytes);

		bool FinaliseCurrentCommunication();
	};
}