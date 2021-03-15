#pragma once

#include <string>
#include "pkcs11.h"

namespace Abstractions {
 
	/*Encapsulates the response of a token action*/
	template<typename Object>
	class TokenActionResult {
	public:
		enum class Code {
			OkResult = CKR_OK,
			//handle more codes
		};

		TokenActionResult(const Code code) {
			this->resultCode = code;			
		}

		TokenActionResult(const Code code, const Object& value) {
			this->resultCode = code;
			this->value = std::move(value);
		}

		const Code& GetCode() const {
			return this->resultCode;
		}

		const Object& GetValue() const {
			return this->value;
		}

	private:
		Object value;
		Code resultCode;
	};


	/*Aliases*/
	using InitialiseResult = TokenActionResult<bool>;
	using FinaliseResult = TokenActionResult<bool>;
	using GetIdentifierResult = TokenActionResult<unsigned int>;
	using GetManufacturerResult = TokenActionResult<std::string>;
	using CreateSessionResult = TokenActionResult<unsigned char>;
	using EndSessionResult = TokenActionResult<bool>;
  
}
 