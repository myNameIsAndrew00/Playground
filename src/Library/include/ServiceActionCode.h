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
        EncryptUpdate = 0x08
	};
}