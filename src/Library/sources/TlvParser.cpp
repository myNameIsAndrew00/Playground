#include "..\include\TlvParser.h"
#include "..\include\pkcs11.h";
using namespace Infrastructure;


std::list<Abstractions::TlvStructure> TlvParser::ParsePkcs11Attributes(const CK_ATTRIBUTE* attributes, const int attributesCount)
{
	std::list<Abstractions::TlvStructure> result;

	if (attributes == nullptr) return result;

	int resultSize = 0;

	for (int i = 0; i < attributesCount; i++) {
		result.emplace_back(Abstractions::TlvStructure(attributes[i].type, (unsigned char*)attributes[i].pValue, attributes[i].ulValueLen));
	}
 
	return result;
}
