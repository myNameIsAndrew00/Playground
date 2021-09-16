#pragma once

#include <string>
#include "pkcs11.h"
#include "Bytes.h"

namespace Abstractions {

	/*Encapsulates the response of a token action*/
	template<typename Object>
	class TokenActionResult {
	public:
		enum class Code {
            OK = CKR_OK,
            CANCEL = CKR_CANCEL,
            HOST_MEMORY = CKR_HOST_MEMORY,
            SLOT_ID_INVALID = CKR_SLOT_ID_INVALID,
            GENERAL_ERROR = CKR_GENERAL_ERROR,
            FUNCTION_FAILED = CKR_FUNCTION_FAILED,
            ARGUMENTS_BAD = CKR_ARGUMENTS_BAD,
            NO_EVENT = CKR_NO_EVENT,
            NEED_TO_CREATE_THREADS = CKR_NEED_TO_CREATE_THREADS,
            CANT_LOCK = CKR_CANT_LOCK,
            ATTRIBUTE_READ_ONLY = CKR_ATTRIBUTE_READ_ONLY,
            ATTRIBUTE_SENSITIVE = CKR_ATTRIBUTE_SENSITIVE,
            ATTRIBUTE_TYPE_INVALID = CKR_ATTRIBUTE_TYPE_INVALID,
            ATTRIBUTE_VALUE_INVALID = CKR_ATTRIBUTE_VALUE_INVALID,
            ACTION_PROHIBITED = CKR_ACTION_PROHIBITED,
            DATA_INVALID = CKR_DATA_INVALID,
            DATA_LEN_RANGE = CKR_DATA_LEN_RANGE,
            DEVICE_ERROR = CKR_DEVICE_ERROR,
            DEVICE_MEMORY = CKR_DEVICE_MEMORY,
            DEVICE_REMOVED = CKR_DEVICE_REMOVED,
            ENCRYPTED_DATA_INVALID = CKR_ENCRYPTED_DATA_INVALID,
            ENCRYPTED_DATA_LEN_RANGE = CKR_ENCRYPTED_DATA_LEN_RANGE,
            FUNCTION_CANCELED = CKR_FUNCTION_CANCELED,
            FUNCTION_NOT_PARALLEL = CKR_FUNCTION_NOT_PARALLEL,
            FUNCTION_NOT_SUPPORTED = CKR_FUNCTION_NOT_SUPPORTED,
            KEY_HANDLE_INVALID = CKR_KEY_HANDLE_INVALID,
            KEY_SIZE_RANGE = CKR_KEY_SIZE_RANGE,
            KEY_TYPE_INCONSISTENT = CKR_KEY_TYPE_INCONSISTENT,
            KEY_NOT_NEEDED = CKR_KEY_NOT_NEEDED,
            KEY_CHANGED = CKR_KEY_CHANGED,
            KEY_NEEDED = CKR_KEY_NEEDED,
            KEY_INDIGESTIBLE = CKR_KEY_INDIGESTIBLE,
            KEY_FUNCTION_NOT_PERMITTED = CKR_KEY_FUNCTION_NOT_PERMITTED,
            KEY_NOT_WRAPPABLE = CKR_KEY_NOT_WRAPPABLE,
            KEY_UNEXTRACTABLE = CKR_KEY_UNEXTRACTABLE,
            MECHANISM_INVALID = CKR_MECHANISM_INVALID,
            MECHANISM_PARAM_INVALID = CKR_MECHANISM_PARAM_INVALID,
            OBJECT_HANDLE_INVALID = CKR_OBJECT_HANDLE_INVALID,
            OPERATION_ACTIVE = CKR_OPERATION_ACTIVE,
            OPERATION_NOT_INITIALIZED = CKR_OPERATION_NOT_INITIALIZED,
            PIN_INCORRECT = CKR_PIN_INCORRECT,
            PIN_INVALID = CKR_PIN_INVALID,
            PIN_LEN_RANGE = CKR_PIN_LEN_RANGE,
            PIN_EXPIRED = CKR_PIN_EXPIRED,
            PIN_LOCKED = CKR_PIN_LOCKED,
            SESSION_CLOSED = CKR_SESSION_CLOSED,
            SESSION_COUNT = CKR_SESSION_COUNT,
            SESSION_HANDLE_INVALID = CKR_SESSION_HANDLE_INVALID,
            SESSION_PARALLEL_NOT_SUPPORTED = CKR_SESSION_PARALLEL_NOT_SUPPORTED,
            SESSION_READ_ONLY = CKR_SESSION_READ_ONLY,
            SESSION_EXISTS = CKR_SESSION_EXISTS,
            SESSION_READ_ONLY_EXISTS = CKR_SESSION_READ_ONLY_EXISTS,
            SESSION_READ_WRITE_SO_EXISTS = CKR_SESSION_READ_WRITE_SO_EXISTS,
            SIGNATURE_INVALID = CKR_SIGNATURE_INVALID,
            SIGNATURE_LEN_RANGE = CKR_SIGNATURE_LEN_RANGE,
            TEMPLATE_INCOMPLETE = CKR_TEMPLATE_INCOMPLETE,
            TEMPLATE_INCONSISTENT = CKR_TEMPLATE_INCONSISTENT,
            TOKEN_NOT_PRESENT = CKR_TOKEN_NOT_PRESENT,
            TOKEN_NOT_RECOGNIZED = CKR_TOKEN_NOT_RECOGNIZED,
            TOKEN_WRITE_PROTECTED = CKR_TOKEN_WRITE_PROTECTED,
            UNWRAPPING_KEY_HANDLE_INVALID = CKR_UNWRAPPING_KEY_HANDLE_INVALID,
            UNWRAPPING_KEY_SIZE_RANGE = CKR_UNWRAPPING_KEY_SIZE_RANGE,
            UNWRAPPING_KEY_TYPE_INCONSISTENT = CKR_UNWRAPPING_KEY_TYPE_INCONSISTENT,
            USER_ALREADY_LOGGED_IN = CKR_USER_ALREADY_LOGGED_IN,
            USER_NOT_LOGGED_IN = CKR_USER_NOT_LOGGED_IN,
            USER_PIN_NOT_INITIALIZED = CKR_USER_PIN_NOT_INITIALIZED,
            USER_TYPE_INVALID = CKR_USER_TYPE_INVALID,
            USER_ANOTHER_ALREADY_LOGGED_IN = CKR_USER_ANOTHER_ALREADY_LOGGED_IN,
            USER_TOO_MANY_TYPES = CKR_USER_TOO_MANY_TYPES,
            WRAPPED_KEY_INVALID = CKR_WRAPPED_KEY_INVALID,
            WRAPPED_KEY_LEN_RANGE = CKR_WRAPPED_KEY_LEN_RANGE,
            WRAPPING_KEY_HANDLE_INVALID = CKR_WRAPPING_KEY_HANDLE_INVALID,
            WRAPPING_KEY_SIZE_RANGE = CKR_WRAPPING_KEY_SIZE_RANGE,
            WRAPPING_KEY_TYPE_INCONSISTENT = CKR_WRAPPING_KEY_TYPE_INCONSISTENT,
            RANDOM_SEED_NOT_SUPPORTED = CKR_RANDOM_SEED_NOT_SUPPORTED,
            RANDOM_NO_RNG = CKR_RANDOM_NO_RNG,
            DOMAIN_PARAMS_INVALID = CKR_DOMAIN_PARAMS_INVALID,
            CURVE_NOT_SUPPORTED = CKR_CURVE_NOT_SUPPORTED,
            BUFFER_TOO_SMALL = CKR_BUFFER_TOO_SMALL,
            SAVED_STATE_INVALID = CKR_SAVED_STATE_INVALID,
            INFORMATION_SENSITIVE = CKR_INFORMATION_SENSITIVE,
            STATE_UNSAVEABLE = CKR_STATE_UNSAVEABLE,
            CRYPTOKI_NOT_INITIALIZED = CKR_CRYPTOKI_NOT_INITIALIZED,
            CRYPTOKI_ALREADY_INITIALIZED = CKR_CRYPTOKI_ALREADY_INITIALIZED,
            MUTEX_BAD = CKR_MUTEX_BAD,
            MUTEX_NOT_LOCKED = CKR_MUTEX_NOT_LOCKED,
            NEW_PIN_MODE = CKR_NEW_PIN_MODE,
            NEXT_OTP = CKR_NEXT_OTP,
            EXCEEDED_MAX_ITERATIONS = CKR_EXCEEDED_MAX_ITERATIONS,
            FIPS_SELF_TEST_FAILED = CKR_FIPS_SELF_TEST_FAILED,
            LIBRARY_LOAD_FAILED = CKR_LIBRARY_LOAD_FAILED,
            PIN_TOO_WEAK = CKR_PIN_TOO_WEAK,
            PUBLIC_KEY_INVALID = CKR_PUBLIC_KEY_INVALID,
            FUNCTION_REJECTED = CKR_FUNCTION_REJECTED,
            VENDOR_DEFINED = CKR_VENDOR_DEFINED
		};

        TokenActionResult(unsigned long code) {
            this->resultCode = (Code)code;
        }

		TokenActionResult(const Code code) {
			this->resultCode = code;
		}

		TokenActionResult(const Code code, const Object& value)  
            : resultCode(code), value(value){
			this->resultCode = code;
			this->value = value;
		}

        TokenActionResult(const Code code, Object&& value) 
           : resultCode(code), value(std::move(value)){
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
	using CreateSessionResult = TokenActionResult<unsigned long long>;
	using EndSessionResult = TokenActionResult<bool>;
	using CreateObjectResult = TokenActionResult<unsigned long long>;
	using EncryptInitResult = TokenActionResult<bool>;
	using EncryptResult = TokenActionResult<Bytes>;
    using EncryptUpdateResult = TokenActionResult<Bytes>;
    using EncryptFinalResult = TokenActionResult<Bytes>;
}
