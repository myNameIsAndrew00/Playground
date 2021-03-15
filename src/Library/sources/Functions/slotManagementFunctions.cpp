#pragma once
#include "../../include/pkcs11.h" 
#include "../../include/VirtualToken.h"
 
extern Abstractions::IPkcs11TokenReference Token;

/*This functions returns a slot representing the virtual token (if SlotList is not null) or the number of virtual tokens
if slot list is null*/
CK_DEFINE_FUNCTION(CK_RV, C_GetSlotList)(CK_BBOOL tokenPresent, CK_SLOT_ID_PTR slotsPointer, CK_ULONG_PTR slotsCountPointer)
{ 
	CK_RV functionResult = CKR_OK;

	if (slotsPointer == NULL_PTR) *slotsCountPointer = 1;
	else {
		auto tokenResult = Token->GetIdentifier();

		*slotsPointer = tokenResult.GetValue();
		functionResult = (CK_RV)tokenResult.GetCode();
	}

	return functionResult;
}


CK_DEFINE_FUNCTION(CK_RV, C_GetSlotInfo)(CK_SLOT_ID slotID, CK_SLOT_INFO_PTR pInfo)
{ 
	return CKR_OK;
}


CK_DEFINE_FUNCTION(CK_RV, C_GetTokenInfo)(CK_SLOT_ID slotID, CK_TOKEN_INFO_PTR pInfo)
{
	return CKR_FUNCTION_NOT_SUPPORTED;
}

CK_DEFINE_FUNCTION(CK_RV, C_GetMechanismList)(CK_SLOT_ID slotID, CK_MECHANISM_TYPE_PTR pMechanismList, CK_ULONG_PTR pulCount)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}


CK_DEFINE_FUNCTION(CK_RV, C_GetMechanismInfo)(CK_SLOT_ID slotID, CK_MECHANISM_TYPE type, CK_MECHANISM_INFO_PTR pInfo)
{

	return CKR_FUNCTION_NOT_SUPPORTED;
}

