#include "..\include\TlvParser.h"
#include "..\include\pkcs11.h";
using namespace Infrastructure;


Abstractions::Bytes TlvParser::ParsePkcs11Attributes(const CK_ATTRIBUTE* attributes, const int attributesCount)
{
	Abstractions::Bytes result;
	if (attributes == nullptr) return result;

	int resultSize = 0;

	for (int i = 0; i < attributesCount; i++) {
		resultSize += 2 * sizeof(unsigned long);
		resultSize += attributes[i].ulValueLen;
	}

	unsigned char* resultData = new unsigned char[resultSize];
	int dataPointer = 0;

	for (int i = 0; i < attributesCount; i++) {
		memcpy(resultData + dataPointer, &attributes[i].type, sizeof(unsigned long));
		dataPointer += sizeof(unsigned long);
		memcpy(resultData + dataPointer, &attributes[i].ulValueLen, sizeof(unsigned long));
		dataPointer += sizeof(unsigned long);
		memcpy(resultData + dataPointer, attributes[i].pValue, attributes[i].ulValueLen);
		dataPointer += attributes[i].ulValueLen;		 
	}


	result.SetFromArray(resultData, resultSize);

	return std::move(result);
}
