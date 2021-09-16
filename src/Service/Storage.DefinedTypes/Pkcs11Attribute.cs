﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.DefinedTypes
{
    /// <summary>
    /// Represents an enum which contains pkcs11 attributes 
    /// </summary>
    public enum Pkcs11Attribute : ulong
    {
       CLASS                                   = 0x00000000,
       TOKEN                                   = 0x00000001,
       PRIVATE                                 = 0x00000002,
       LABEL                                   = 0x00000003,
       APPLICATION                             = 0x00000010,
       VALUE                                   = 0x00000011,
       OBJECT_ID                               = 0x00000012,
       CERTIFICATE_TYPE                        = 0x00000080,
       ISSUER                                  = 0x00000081,
       SERIAL_NUMBER                           = 0x00000082,
       AC_ISSUER                               = 0x00000083,
       OWNER                                   = 0x00000084,
       ATTR_TYPES                              = 0x00000085,
       TRUSTED                                 = 0x00000086,
       CERTIFICATE_CATEGORY                    = 0x00000087,
       JAVA_MIDP_SECURITY_DOMAIN               = 0x00000088,
       URL                                     = 0x00000089,
       HASH_OF_SUBJECT_PUBLIC_KEY              = 0x0000008A,
       HASH_OF_ISSUER_PUBLIC_KEY               = 0x0000008B,
       NAME_HASH_ALGORITHM                     = 0x0000008C,
       CHECK_VALUE                             = 0x00000090,
       KEY_TYPE                                = 0x00000100,
       SUBJECT                                 = 0x00000101,
       ID                                      = 0x00000102,
       SENSITIVE                               = 0x00000103,
       ENCRYPT                                 = 0x00000104,
       DECRYPT                                 = 0x00000105,
       WRAP                                    = 0x00000106,
       UNWRAP                                  = 0x00000107,
       SIGN                                    = 0x00000108,
       SIGN_RECOVER                            = 0x00000109,
       VERIFY                                  = 0x0000010A,
       VERIFY_RECOVER                          = 0x0000010B,
       DERIVE                                  = 0x0000010C,
       START_DATE                              = 0x00000110,
       END_DATE                                = 0x00000111,
       MODULUS                                 = 0x00000120,
       MODULUS_BITS                            = 0x00000121,
       PUBLIC_EXPONENT                         = 0x00000122,
       PRIVATE_EXPONENT                        = 0x00000123,
       PRIME_1                                 = 0x00000124,
       PRIME_2                                 = 0x00000125,
       EXPONENT_1                              = 0x00000126,
       EXPONENT_2                              = 0x00000127,
       COEFFICIENT                             = 0x00000128,
       PUBLIC_KEY_INFO                         = 0x00000129,
       PRIME                                   = 0x00000130,
       SUBPRIME                                = 0x00000131,
       BASE                                    = 0x00000132,
       PRIME_BITS                          = 0x00000133,
       SUBPRIME_BITS                       = 0x00000134,
       VALUE_BITS                          = 0x00000160,
       VALUE_LEN                           = 0x00000161,
       EXTRACTABLE                         = 0x00000162,
       LOCAL                               = 0x00000163,
       NEVER_EXTRACTABLE                   = 0x00000164,
       ALWAYS_SENSITIVE                    = 0x00000165,
       KEY_GEN_MECHANISM                   = 0x00000166,
       MODIFIABLE                          = 0x00000170,
       COPYABLE                            = 0x00000171,
       DESTROYABLE                             = 0x00000172, 
       ECDSA_PARAMS                            = 0x00000180,
       EC_PARAMS                               = 0x00000180,
       EC_POINT                                = 0x00000181, 
       SECONDARY_AUTH                          = 0x00000200,  
       AUTH_PIN_FLAGS                          = 0x00000201,  
       ALWAYS_AUTHENTICATE                     = 0x00000202,
       WRAP_WITH_TRUSTED                       = 0x00000210,
       WRAP_TEMPLATE                           = (0x40000000|0x00000211),
       UNWRAP_TEMPLATE                         = (0x40000000|0x00000212),
       OTP_FORMAT                              = 0x00000220,
       OTP_LENGTH                              = 0x00000221,
       OTP_TIME_INTERVAL                       = 0x00000222,
       OTP_USER_FRIENDLY_MODE                  = 0x00000223,
       OTP_CHALLENGE_REQUIREMENT               = 0x00000224,
       OTP_TIME_REQUIREMENT                    = 0x00000225,
       OTP_COUNTER_REQUIREMENT                 = 0x00000226,
       OTP_PIN_REQUIREMENT                     = 0x00000227,
       OTP_COUNTER                             = 0x0000022E,
       OTP_TIME                                = 0x0000022F,
       OTP_USER_IDENTIFIER                     = 0x0000022A,
       OTP_SERVICE_IDENTIFIER                  = 0x0000022B,
       OTP_SERVICE_LOGO                        = 0x0000022C,
       OTP_SERVICE_LOGO_TYPE                   = 0x0000022D,
       GOSTR3410_PARAMS                        = 0x00000250,
       GOSTR3411_PARAMS                        = 0x00000251,
       GOST28147_PARAMS                        = 0x00000252,
       HW_FEATURE_TYPE	                       = 0x00000300,
       RESET_ON_INIT                           = 0x00000301,
       HAS_RESET                               = 0x00000302,
       PIXEL_X                                 = 0x00000400,
       PIXEL_Y                                 = 0x00000401,
       RESOLUTION                              = 0x00000402,
       CHAR_ROWS                               = 0x00000403,
       CHAR_COLUMNS                            = 0x00000404,
       COLOR                                   = 0x00000405,
       BITS_PER_PIXEL                          = 0x00000406,
       CHAR_SETS                               = 0x00000480,
       ENCODING_METHODS                        = 0x00000481,
       MIME_TYPES                              = 0x00000482,
       MECHANISM_TYPE                          = 0x00000500,
       REQUIRED_CMS_ATTRIBUTES                 = 0x00000501,
       DEFAULT_CMS_ATTRIBUTES                  = 0x00000502,
       SUPPORTED_CMS_ATTRIBUTES                = 0x00000503,
       ALLOWED_MECHANISMS                      = (0x40000000|0x00000600),
       VENDOR_DEFINED                          = 0x80000000
    }
}