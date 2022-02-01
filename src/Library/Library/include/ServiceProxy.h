#pragma once  
#include "IServiceProxyClient.h"
#include "IServiceProtocolDispatcher.h"
#include "BytesReader.h"
#include "TokenActionResult.h"

#include <list>

class TlvStructure;
typedef unsigned long long Id;

namespace Abstractions {
	/*Represents a proxy class for the cryptographic service. It will be responsable for handling messages*/
	class ServiceProxy
	{
	public:
		ServiceProxy() : client(nullptr) {}
		ServiceProxy(const IServiceCommunicationResolverReference& communicationResolver,
			const IServiceProtocolDispatcherReference& protocolDispatcher);

		/*Add a new client to the registered clients list*/
		bool Register(IServiceProxyClient* client);
		bool Unregister(IServiceProxyClient* client);

		bool DetachCurrentClient();

		/// <summary>
		/// Trigger the begin session method on the server.
		/// </summary>
		/// <returns>An object containing the result of the process. An identifier of the session id will be stored in response.</returns>
		CreateSessionResult BeginSession();

		/// <summary>
		/// Trigger end session method on the server.
		/// </summary>
		/// <param name="sessionId">Id of session which is finished</param>
		/// <returns>An object containing the result of the process </returns>
		EndSessionResult EndSession(const Id sessionId);

		/// <summary>
		/// Trigger create object method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session where the object will be stored</param>
		/// <param name="attributes">A list of attributes which describes the object</param>
		/// <returns>An object containing the result of the process. An identifier of the object id will be stored in response.</returns>
		CreateObjectResult CreateObject(const Id sessionId, const std::list<TlvStructure>& attributes);

		/// <summary>
		/// Trigger encrypt initialisation method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which will contain the initialised encryption object</param>
		/// <param name="objectId">Id of encryption object which will be used for encryption</param>
		/// <param name="mechanism">Mechanism used for encryption</param>
		/// <returns>An object containing the result of the process.</returns>
		EncryptInitResult EncryptInit(const Id sessionId, const Id objectId, const TlvStructure& mechanism);

		/// <summary>
		/// Trigger encrypt method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised encryption object</param>
		/// <param name="dataToEncrypt">Data which will be encrypted. Type field of the tlv structure will be ignored.</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive encrypted data length</param>
		/// <returns>An object containing the result of the process. Encrypted data will be returned.</returns>
		EncryptResult Encrypt(const Id sessionId, TlvStructure dataToEncrypt, bool requestLength);

		/// <summary>
		/// Trigger encrypt update method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised encryption object</param>
		/// <param name="dataToEncrypt">Data which will be encrypted. Type field of the tlv structure will be ignored.</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive encrypted data length</param>
		/// <returns>An object containing the result of the process. Part encrypted data will be returned.</returns>
		EncryptUpdateResult EncryptUpdate(const Id sessionId, TlvStructure dataToEncrypt, bool requestLength);

		/// <summary>
		/// Trigger encrypt final method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised encryption object</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive encrypted data length</param>
		/// <returns>An object containing the result of the process. Final block of the previous encrypt update will be returned.</returns>
		EncryptFinalResult EncryptFinal(const Id sessionId, bool requestLength);

		/// <summary>
		/// Trigger dencrypt initialisation method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which will contain the initialised encryption object</param>
		/// <param name="objectId">Id of dencryption object which will be used for dencryption</param>
		/// <param name="mechanism">Mechanism used for dencryption</param>
		/// <returns>An object containing the result of the process.</returns>
		DecryptInitResult DecryptInit(const Id sessionId, const Id objectId, const TlvStructure& mechanism);

		/// <summary>
		/// Trigger dencrypt method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised dencryption object</param>
		/// <param name="dataToEncrypt">Data which will be dencrypted. Type field of the tlv structure will be ignored.</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive dencrypted data length</param>
		/// <returns>An object containing the result of the process. Dencrypted data will be returned.</returns>
		DecryptResult Decrypt(const Id sessionId, TlvStructure dataToDecrypt, bool requestLength);

		/// <summary>
		/// Trigger dencrypt update method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised dencryption object</param>
		/// <param name="dataToEncrypt">Data which will be dencrypted. Type field of the tlv structure will be ignored.</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive dencrypted data length</param>
		/// <returns>An object containing the result of the process. Part dencrypted data will be returned.</returns>
		DecryptUpdateResult DecryptUpdate(const Id sessionId, TlvStructure dataToDecrypt, bool requestLength);

		/// <summary>
		/// Trigger dencrypt final method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised dencryption object</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive dencrypted data length</param>
		/// <returns>An object containing the result of the process. Final block of the previous dencrypt update will be returned.</returns>
		DecryptFinalResult DecryptFinal(const Id sessionId, bool requestLength);

		/// <summary>
		/// Trigger digest initialisation method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which will contain the initialised mechanism</param>
		/// <param name="mechanism">Mechanism used for digest</param>
		/// <returns>An object containing the result of the process.</returns>
		DigestInitResult DigestInit(const Id sessionId, const TlvStructure& mechanism);

		/// <summary>
		/// Trigger digest method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised digest object</param>
		/// <param name="dataToDigest">Data which will be hashed. Type field of the tlv structure will be ignored.</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive hashed data length</param>
		/// <returns>An object containing the result of the process. Hashed data will be returned.</returns>
		DigestResult Digest(const Id sessionId, TlvStructure dataToDigest, bool requestLength);

		/// <summary>
		/// Trigger digest update method on the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised digest object</param>
		/// <param name="dataToDigest">Data which will be hashed. Type field of the tlv structure will be ignored.</param>
		/// <returns>An object containing the result of the process.</returns>
		DigestUpdateResult DigestUpdate(const Id sessionId, TlvStructure dataToDigest);

		/// <summary>
		/// Trigger digest finall method ont the server.
		/// </summary>
		/// <param name="sessionId">Id of the session which contains the initialised digest object</param>
		/// <param name="requestLength">A boolean which specify if request is made to receive hashed data length</param>
		/// <returns>An object containing the result of the process. Hashed data will be returned.</returns>
		DigestFinalResult DigestFinal(const Id sessionId, bool lengthRequest);

	private:
		bool communicationInitialised;

		/*Represents a client registered to the service*/
		IServiceProxyClient* client;
		IServiceCommunicationResolverReference communicationResolver;
		IServiceProtocolDispatcherReference protocolDispatcher;


		Abstractions::BytesReader* executeRequest(Abstractions::ServiceActionCode code, unsigned long& resultCode, const unsigned char* data, const unsigned int dataLength);


#pragma region Server Invoke Template 

		template<typename ActionResult, typename CreateRequestBytesFunctor, typename CreateResultFunctor>
		ActionResult InvokeServer(ServiceActionCode actionCode, CreateRequestBytesFunctor bytesCallback, CreateResultFunctor resultCallback, const Id sessionId = 0) {
			unsigned long resultCode;

			Bytes requestBytes = sessionId == 0 ?
				bytesCallback() :
				Bytes((long long)sessionId).Append(bytesCallback());

			BytesReader* reader = this->executeRequest(actionCode, resultCode, requestBytes.GetBytes(), requestBytes.GetLength());

			if (reader == nullptr) 
				return ActionResult(resultCode);
			 
			if (resultCode != (unsigned long)ReturnCode::OK) {
				delete reader;
				return ActionResult(resultCode);
			}


			ActionResult result = resultCallback(reader, resultCode);

			delete reader;

			return result;
		}

#pragma endregion

	};

	using ServiceProxyReference = std::shared_ptr<ServiceProxy>;
}