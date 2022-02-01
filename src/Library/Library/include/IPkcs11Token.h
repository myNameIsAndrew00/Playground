#pragma once
#include <memory>
#include <string>

#include "TokenActionResult.h"

struct CK_ATTRIBUTE;
struct CK_MECHANISM;

namespace Abstractions {
	/*Constants*/
	static const char Manufacturer[] = "Pkcs11Playground";
	static const char Description[] = "Pkcs 11 playground";
	static const unsigned int Minor = 0x01;
	static const unsigned int Major = 0x01;
	
	/// <summary>
	///  Implements required methods for a pkcs11 token 
	/// </summary> 
	class IPkcs11Token abstract {
	public: 
		virtual InitialiseResult Initialise() = 0;
		virtual FinaliseResult Finalise() = 0;

		virtual GetIdentifierResult GetIdentifier() const = 0;
		virtual GetManufacturerResult GetManufacturer() const = 0;
		virtual CreateSessionResult CreateSession() const = 0;
		virtual EndSessionResult EndSession(const unsigned long long) const = 0;
		virtual CreateObjectResult CreateObject(const unsigned long long sessionId, CK_ATTRIBUTE* attributes, const int length) const = 0;
		virtual EncryptInitResult EncryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const = 0;
		virtual EncryptResult Encrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const = 0;
		virtual EncryptUpdateResult EncryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool encryptedDataLengthRequest) const = 0;
		virtual EncryptFinalResult EncryptFinal(const unsigned long long sessionId, bool encryptedDataLengthRequest) const = 0;
		virtual DecryptInitResult DecryptInit(const unsigned long long sessionId, const unsigned long long objectId, const CK_MECHANISM* mechanism) const = 0;
		virtual DecryptResult Decrypt(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const = 0;
		virtual DecryptUpdateResult DecryptUpdate(const unsigned long long sessionId, const unsigned char* data, const int length, bool decryptedDataLengthRequest) const = 0;
		virtual DecryptFinalResult DecryptFinal(const unsigned long long sessionId, bool decryptedDataLengthRequest) const = 0;
		virtual DigestInitResult DigestInit(const unsigned long long sessionId, const CK_MECHANISM* mechanism) const = 0;
		virtual DigestResult Digest(const unsigned long long sessionId, const unsigned char* data, const int length, bool digestLengthRequest) const = 0;
		virtual DigestUpdateResult DigestUpdate(const unsigned long long sessionId, const unsigned char* data, const int length) const = 0;
		virtual DigestFinalResult DigestFinal(const unsigned long long sessionId, bool digestLengthRequest) const = 0;

		virtual ~IPkcs11Token() { }
	};
	 

	using IPkcs11TokenReference = std::shared_ptr<IPkcs11Token>;	
}