#pragma once

#include <string>
#include "pkcs11.h"
#include "Bytes.h"
#include "ReturnCode.h"


namespace Abstractions {

	/*Encapsulates the response of a token action*/
	template<typename Object>
	class TokenActionResult {
	public:
		

        TokenActionResult(unsigned long code) {
            this->resultCode = (ReturnCode)code;
        }

		TokenActionResult(const ReturnCode code) {
			this->resultCode = code;
		}

		TokenActionResult(const ReturnCode code, const Object& value)
            : resultCode(code), value(value){
			this->resultCode = code;
			this->value = value;
		}

        TokenActionResult(const ReturnCode code, Object&& value)
           : resultCode(code), value(std::move(value)){
        }

		const ReturnCode& GetCode() const {
			return this->resultCode;
		}

		const Object& GetValue() const {
			return this->value;
		}

	private:
		Object value;
		ReturnCode resultCode;
	};


	/*Aliases*/
	using InitialiseResult = TokenActionResult<bool>;
	using FinaliseResult = TokenActionResult<bool>;
	using GetIdentifierResult = TokenActionResult<unsigned int>;
	using GetManufacturerResult = TokenActionResult<std::string>;
	using CreateSessionResult = TokenActionResult<unsigned long long>;
	using EndSessionResult = TokenActionResult<bool>;
	using CreateObjectResult = TokenActionResult<unsigned long long>;
	using EncryptInitResult = TokenActionResult<bool>;
	using EncryptResult = TokenActionResult<Bytes>;
    using EncryptUpdateResult = TokenActionResult<Bytes>;
    using EncryptFinalResult = TokenActionResult<Bytes>;
	using DecryptInitResult = TokenActionResult<bool>;
	using DecryptResult = TokenActionResult<Bytes>;
	using DecryptUpdateResult = TokenActionResult<Bytes>;
	using DecryptFinalResult = TokenActionResult<Bytes>;
	using DigestInitResult = TokenActionResult<bool>;
	using DigestResult = TokenActionResult<Bytes>;
	using DigestUpdateResult = TokenActionResult<bool>;
	using DigestFinalResult = TokenActionResult<Bytes>;
}
