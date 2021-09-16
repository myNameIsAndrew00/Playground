#include "..\include\TlvParser.h"
#include "..\include\pkcs11.h";
#include <algorithm>

using namespace Infrastructure;


TlvParser::TlvParser() 
	: reversedTypes(
		{
			CKA_VALUE_LEN,
			CKA_KEY_TYPE
		})
{ 
}


std::list<Abstractions::TlvStructure> TlvParser::ParsePkcs11Attributes(const CK_ATTRIBUTE* attributes, const int attributesCount)
{
	std::list<Abstractions::TlvStructure> result;

	if (attributes == nullptr) return result;

	int resultSize = 0;

	for (int i = 0; i < attributesCount; i++) {
		bool releaseData = false;
		unsigned char* data = (unsigned char*)attributes[i].pValue;
		 
		if (reversedTypes.find(attributes[i].type) != reversedTypes.end()) {
			data = new unsigned char[attributes[i].ulValueLen];
			memcpy(data, attributes[i].pValue, attributes[i].ulValueLen);

			std::reverse(data, data + attributes[i].ulValueLen);
			
			releaseData = true;
		}
		result.emplace_back(Abstractions::TlvStructure(attributes[i].type, data, attributes[i].ulValueLen));
	
		if (releaseData) delete[] data;
	}
 
	return result;
}
