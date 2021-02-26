#pragma once

namespace Abstractions {
	enum class ServiceActionCode {
		BeginSession = 0x01,
		EndSession = 0x02,
		Authenticate = 0x03
	};
}