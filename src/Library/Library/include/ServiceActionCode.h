#pragma once

namespace Abstractions {
	enum class ServiceActionCode {
		BeginSession = 0x01,
		EndSession = 0x02,
		Authenticate = 0x03,   
        CreateObject = 0x04,    
        EncryptInit = 0x05,
        Encrypt = 0x06,
        EncryptFinal = 0x07,
        EncryptUpdate = 0x08,
		DecryptInit = 0x09,
		Decrypt = 0x0A,
		DecryptFinal = 0x0B,
		DecryptUpdate = 0x0C,
		DigestInit = 0x0D,
		Digest = 0x0E,
		DigestFinal = 0x0F,
		DigestUpdate = 0x10,
        GenerateKeyPair = 0x11,
        SignInit = 0x12,
        Sign = 0x13,
        SignUpdate = 0x14,
        SignFinal = 0x15,
        VerifyInit = 0x16,
        Verify = 0x17,
        VerifyUpdate = 0x18,
        VerifyFinal = 0x19
	};
}