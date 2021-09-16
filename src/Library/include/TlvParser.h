#pragma once
#include "TlvStructure.h"

#include <set>

struct CK_ATTRIBUTE; 

namespace Infrastructure {
	/// <summary>
	/// Use this class to parse data to a tlv byte structure
	/// </summary>
	class TlvParser {
	private:
		//contains a list of attributes of which data bytes should be reversed
		//when they are stored in a tlv structure (little endianes cases)
		std::set<unsigned long> reversedTypes;
	public:
		TlvParser();
		std::list<Abstractions::TlvStructure> ParsePkcs11Attributes(const CK_ATTRIBUTE* attributes, const int attributesCount);
	};
}