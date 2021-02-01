#pragma once
#include <memory>

/*Implements required methods for a pkcs11 token */
class IPkcs11Token abstract {
public:
	virtual unsigned int GetIdentifier() const = 0;
	virtual bool Initialise() = 0;
};


using IPkcs11TokenReference = std::shared_ptr<IPkcs11Token>;
