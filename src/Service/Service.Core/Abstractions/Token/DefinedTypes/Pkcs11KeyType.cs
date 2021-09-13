using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Token.DefinedTypes
{
    public enum Pkcs11KeyType : uint
    {
        RSA = 0x00000000,
        DSA = 0x00000001,
        DH = 0x00000002,
        ECDSA = 0x00000003,
        EC = 0x00000003,
        X9_42_DH = 0x00000004,
        KEA = 0x00000005,
        GENERIC_SECRET = 0x00000010,
        RC2 = 0x00000011,
        RC4 = 0x00000012,
        DES = 0x00000013,
        DES2 = 0x00000014,
        DES3 = 0x00000015,
        CAST = 0x00000016,
        CAST3 = 0x00000017,
        CAST5 = 0x00000018,
        CAST128 = 0x00000018,
        RC5 = 0x00000019,
        IDEA = 0x0000001A,
        SKIPJACK = 0x0000001B,
        BATON = 0x0000001C,
        JUNIPER = 0x0000001D,
        CDMF = 0x0000001E,
        AES = 0x0000001F,
        BLOWFISH = 0x00000020,
        TWOFISH = 0x00000021,
        SECURID = 0x00000022,
        HOTP = 0x00000023,
        ACTI = 0x00000024,
        CAMELLIA = 0x00000025,
        ARIA = 0x00000026,
        SHA512_224_HMAC = 0x00000027,
        SHA512_256_HMAC = 0x00000028,
        SHA512_T_HMAC = 0x00000029,
        SHA_1_HMAC = 0x00000040,
        SHA224_HMAC = 0x00000041,
        SHA256_HMAC = 0x00000042,
        SHA384_HMAC = 0x00000043,
        SHA512_HMAC = 0x00000044,
        SEED = 0x00000050,
        GOSTR3410 = 0x00000060,
        GOSTR3411 = 0x00000061,
        GOST28147 = 0x00000062,
        VENDOR_DEFINED = 0x80000000
    }
}
