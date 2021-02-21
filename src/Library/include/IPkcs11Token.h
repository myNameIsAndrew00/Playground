#pragma once
#include <memory>
#include <string>

#include "TokenActionResult.h"

namespace Abstractions {

	/*Constants*/
	static const char Manufacturer[] = "Pkcs11Playground";
	static const char Description[] = "Pkcs 11 playground";
	static const unsigned int Minor = 0x01;
	static const unsigned int Major = 0x01;

	/*Implements required methods for a pkcs11 token */
	class IPkcs11Token abstract {
	public: 
		virtual InitialiseResult Initialise() = 0;
		
		virtual GetIdentifierResult GetIdentifier() const = 0;
		virtual GetManufacturerResult GetManufacturer() const = 0;
		virtual CreateSessionResult CreateSession() const = 0;

		virtual ~IPkcs11Token() { }
	};
	 

	using IPkcs11TokenReference = std::shared_ptr<IPkcs11Token>;	
}