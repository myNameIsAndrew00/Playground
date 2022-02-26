#include "../Library/include/pkcs11.h"
#include <string.h> 

//REMINDER: make sure to copy compile debug/release version of Library proj to generate dll file.

void encryptTest(CK_SESSION_HANDLE session) {
	// setup
	CK_BYTE iv[] = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
					 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
	CK_MECHANISM mechanismTemplate[] = {
		{CKM_AES_CBC, iv, sizeof(iv)}
	};

	CK_ULONG key_length_bytes = 128;
	CK_ULONG key_type = CKK_AES;
	CK_BBOOL true_val = 1;
	CK_BBOOL false_val = 0;

	CK_ATTRIBUTE _template[] = {
		{CKA_ENCRYPT,   &true_val,		   sizeof(CK_BBOOL)},
		{CKA_DECRYPT,   &true_val,		   sizeof(CK_BBOOL)},
		{CKA_SENSITIVE, &true_val,         sizeof(CK_BBOOL)},
		{CKA_TOKEN,     &false_val,        sizeof(CK_BBOOL)},
		{CKA_VALUE,		iv,				   sizeof(iv) },
		{CKA_KEY_TYPE,  &key_type,		   sizeof(CK_ULONG)},
		{CKA_VALUE_LEN, &key_length_bytes, sizeof(CK_ULONG)}
	};

	char encryptData[] = "This data will be encrypted";
	
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
	unsigned char* decryptedData = nullptr;
	CK_RV decryptResultCode = C_Decrypt(session, encryptedData, encryptedDataLength, nullptr, &decryptedDataLength);
	decryptedData = new unsigned char[decryptedDataLength];
	decryptResultCode = C_Decrypt(session, encryptedData, encryptedDataLength, decryptedData, &decryptedDataLength);
	
	// clean up
	delete[] decryptedData;
	delete[] encryptedData;
}

void hashTest(CK_SESSION_HANDLE session) {	
	CK_MECHANISM digestMechanismTemplate[] = {
		{CKM_SHA_1, 0, 0}
	};

	char hashData[] = "This data will be hashed";

	CK_RV digestInitResult = C_DigestInit(session, digestMechanismTemplate);
	unsigned int digestLength;
	unsigned char* digest;

	CK_RV digestResultCode = C_Digest(session, (unsigned char*)hashData, strlen(hashData) + 1, nullptr, &digestLength);
	digest = new unsigned char[digestLength];
	digestResultCode = C_Digest(session, (unsigned char*)hashData, strlen(hashData) + 1, digest, &digestLength);

	// clean up
	delete[] digest;
}

void signTest(CK_SESSION_HANDLE session) {
	CK_OBJECT_HANDLE privatekey, publickey;
	CK_BBOOL true_val = 1;
	CK_BBOOL false_val = 0;
	CK_BYTE modulusbits[] = { 0x00, 0x00, 0x04, 0x00};
	CK_BYTE public_exponent[] = { 3 };

	char signData[] = "This data will be signed";

	/* Set public key. */
	CK_ATTRIBUTE publickey_template[] = {
		{CKA_VERIFY, &true_val, sizeof(true_val)},
		{CKA_MODULUS_BITS, modulusbits, sizeof(modulusbits)},
		{CKA_PUBLIC_EXPONENT, &public_exponent, sizeof(public_exponent)}
	};

	/* Set private key. */
	CK_ATTRIBUTE privatekey_template[] = {
		{CKA_SIGN, &true_val, sizeof(true_val)},
		{CKA_TOKEN, &false_val, sizeof(false_val)},
		{CKA_SENSITIVE, &true_val, sizeof(true_val)},
		{CKA_EXTRACTABLE, &true_val, sizeof(true_val)}
	};

	/* Create sample message. */
	CK_ATTRIBUTE getattributes[] = {
		{CKA_MODULUS_BITS, NULL_PTR, 0},
		{CKA_MODULUS, NULL_PTR, 0},
		{CKA_PUBLIC_EXPONENT, NULL_PTR, 0}
	};

	CK_MECHANISM signMechanismTemplate[] = {
		{CKM_RSA_PKCS, nullptr, 0}
	};

	CK_MECHANISM generationMechanismTemplate[] = {
		{CKM_RSA_PKCS_KEY_PAIR_GEN, nullptr, 0}
	};

	CK_RV generateKeyPairResult = C_GenerateKeyPair(session,
		generationMechanismTemplate, 
		publickey_template,
		(sizeof(publickey_template) / sizeof(CK_ATTRIBUTE)),
		privatekey_template,
		(sizeof(privatekey_template) / sizeof(CK_ATTRIBUTE)),
		&publickey, &privatekey);

	CK_RV signInitResult = C_SignInit(session, signMechanismTemplate, privatekey);

	unsigned char* signature;
	unsigned int signatureLength;

	CK_RV signResult = C_Sign(session, (unsigned char*)signData, strlen(signData) + 1, nullptr, &signatureLength);
	signature = new unsigned char[signatureLength];
	signResult = C_Sign(session, (unsigned char*)signData, strlen(signData) + 1, signature, &signatureLength);

	CK_RV verifyInitResult = C_VerifyInit(session, signMechanismTemplate, publickey);
	
	CK_RV verifyResult = C_Verify(session, (unsigned char*)signData, strlen(signData) + 1, signature, signatureLength);
	
	delete[] signature;
}

int main() {

	CK_ULONG slotsCount;

	// ---------------- PKCS11 DEMO below ----------------

	// --> Initialisation demo
	CK_RV initResponseCode = C_Initialize(nullptr);

	CK_RV slotsRequestResponseCode = C_GetSlotList(false, nullptr, &slotsCount);
	CK_SLOT_ID_PTR slots = new CK_SLOT_ID[slotsCount];
	slotsRequestResponseCode = C_GetSlotList(false, slots, &slotsCount);

	CK_SESSION_HANDLE session;
	CK_RV createSessionResponseCode = C_OpenSession(*slots, 0, nullptr, nullptr, &session);


	// --> Encryption module demo
//	encryptTest(session);

	// --> Messaging module demo 
//	hashTest(session);

	// --> Sign and verify demo
	signTest(session);

	
	// -- Finalisation demo
	CK_RV endSessionResponseCode = C_CloseSession(session);
	CK_RV finaliseResponseCode = C_Finalize(nullptr);


	delete[] slots;
}