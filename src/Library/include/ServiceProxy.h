#pragma once  
#include "IServiceProxyClient.h"
#include "IServiceProtocolDispatcher.h"
#include "BytesReader.h"
#include "TokenActionResult.h"

#include <list>

class TlvStructure;
typedef unsigned long long Handler;

namespace Abstractions {
	/*Represents a proxy class for the cryptographic service. It will be responsable for handling messages*/
	class ServiceProxy
	{
	public: 
		ServiceProxy() : client(nullptr) {}
		ServiceProxy(const IServiceCommunicationResolverReference& communicationResolver,
				    const IServiceProtocolDispatcherReference& protocolDispatcher );

		/*Add a new client to the registered clients list*/
		bool Register( IServiceProxyClient* client);
		bool Unregister( IServiceProxyClient* client);

		bool DetachCurrentClient();

		CreateSessionResult BeginSession();
		EndSessionResult EndSession(const Handler sessionId);
		CreateObjectResult CreateObject(const Handler sessionId, const std::list<TlvStructure>& attributes);

	private:
		bool communicationInitialised;

		/*Represents a list of clients registered to the service*/
		IServiceProxyClient* client;
		IServiceCommunicationResolverReference communicationResolver;
		IServiceProtocolDispatcherReference protocolDispatcher;


		Abstractions::BytesReader* executeRequest(Abstractions::ServiceActionCode code, unsigned long& resultCode, const unsigned char* data, const unsigned int dataLength);
	 
	 
		#pragma region Server Invoke Template 
		
		template<typename ActionResult, typename CreateRequestBytesFunctor, typename CreateResultFunctor>
		ActionResult InvokeServer(ServiceActionCode actionCode, CreateRequestBytesFunctor bytesCallback, CreateResultFunctor resultCallback, const Handler sessionId = 0) {
			unsigned long resultCode;

			Bytes requestBytes = sessionId == 0 ? 				
				bytesCallback() : 
				Bytes((long long)sessionId).Append(bytesCallback());

			BytesReader* reader = this->executeRequest(actionCode, resultCode, requestBytes.GetBytes(), requestBytes.GetLength());

			if (reader == nullptr) return ActionResult(resultCode);

			ActionResult result = resultCallback(reader, resultCode);

			delete reader;

			return result;
		}

		#pragma endregion
		 
	};

	using ServiceProxyReference = std::shared_ptr<ServiceProxy>;
}