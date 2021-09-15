#pragma once
#include "TlvStructure.h"

struct CK_ATTRIBUTE;

namespace Infrastructure {
	/// <summary>
	/// Use this class to parse data to a tlv byte structure
	/// </summary>
	class TlvParser {
	public:
		std::list<Abstractions::TlvStructure> ParsePkcs11Attributes(const CK_ATTRIBUTE* attributes, const int attributesCount);
	};
}