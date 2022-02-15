#include "../Library/include/pkcs11.h"
#include <string.h> 

//REMINDER: make sure to copy compile debug/release version of Library proj to generate dll file.

int main() {
	
	// setup
	CK_BYTE iv[] = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
					 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };

	CK_BBOOL true_val = 1;
	CK_BBOOL false_val = 0;
	CK_ULONG key_length_bytes = 128;
	CK_ULONG key_type = CKK_AES;

	CK_MECHANISM mechanismTemplate[] = {
		{CKM_AES_CBC, iv, sizeof(iv)}
	};

	CK_MECHANISM digestMechanismTemplate[] = {
		{CKM_SHA_1, 0, 0}
	};

	CK_ATTRIBUTE _template[] = {
		{CKA_ENCRYPT,   &true_val,		   sizeof(CK_BBOOL)},
		{CKA_DECRYPT,   &true_val,		   sizeof(CK_BBOOL)},
		{CKA_SENSITIVE, &true_val,         sizeof(CK_BBOOL)},
		{CKA_TOKEN,     &false_val,        sizeof(CK_BBOOL)},
		{CKA_VALUE,		iv,				   sizeof(iv) },
		{CKA_KEY_TYPE,  &key_type,		   sizeof(CK_ULONG)},
		{CKA_VALUE_LEN, &key_length_bytes, sizeof(CK_ULONG)}
	};

	CK_ULONG slotsCount;
	char encryptData[] = "This data will be encrypted";

	// ---------------- PKCS11 DEMO below ----------------

	// --> Initialisation demo
	CK_RV initResponseCode = C_Initialize(nullptr);


	CK_RV slotsRequestResponseCode = C_GetSlotList(false, nullptr, &slotsCount);
	CK_SLOT_ID_PTR slots = new CK_SLOT_ID[slotsCount];
	slotsRequestResponseCode = C_GetSlotList(false, slots, &slotsCount);

	CK_SESSION_HANDLE session;
	CK_RV createSessionResponseCode = C_OpenSession(*slots, 0, nullptr, nullptr, &session);

	// --> Encryption module demo
	CK_OBJECT_HANDLE keyObject;
	CK_RV objectRequestResponseCode = C_CreateObject(session, _template, 7, &keyObject);

	CK_RV encryptInitResut = C_EncryptInit(session, mechanismTemplate, keyObject);

	unsigned int encryptedDataLength;
	unsigned char* encryptedData;
	CK_RV encryptResultCode = C_Encrypt(session, (unsigned char*)encryptData, strlen(encryptData) + 1, nullptr, &encryptedDataLength);
	encryptedData = new unsigned char[encryptedDataLength];
	encryptResultCode = C_Encrypt(session, (unsigned char*)encryptData, strlen(encryptData) + 1, encryptedData, &encryptedDataLength);

	CK_RV decryptInitResult = C_DecryptInit(session, mechanismTemplate, keyObject);

	unsigned int decryptedDataLength;
	unsigned char* decryptedData;
	CK_RV decryptResultCode = C_Decrypt(session, encryptedData, encryptedDataLength, nullptr, &decryptedDataLength);
	decryptedData = new unsigned char[decryptedDataLength];
	decryptResultCode = C_Decrypt(session, encryptedData, encryptedDataLength, decryptedData, &decryptedDataLength);

	// --> Messaging module demo 
	CK_RV digestInitResult = C_DigestInit(session, digestMechanismTemplate);
	unsigned int digestLength;
	unsigned char* digest;

	CK_RV digestResultCode = C_Digest(session, (unsigned char*)encryptData, strlen(encryptData) + 1, nullptr, &digestLength);
	digest = new unsigned char[digestLength];
	digestResultCode = C_Digest(session, (unsigned char*)encryptData, strlen(encryptData) + 1, digest, &digestLength);

	// -- Finalisation demo
	CK_RV endSessionResponseCode = C_CloseSession(session);
	CK_RV finaliseResponseCode = C_Finalize(nullptr);

	// clean up
	delete[] decryptedData;
	delete[] encryptedData;
	delete[] digest;
	delete[] slots;
}