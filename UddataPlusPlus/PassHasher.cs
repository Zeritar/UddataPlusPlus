using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UddataPlusPlus
{
    public class PassHasher
    {
        protected internal string Hash(string username, string password)
        {
            byte[] salt = Convert.FromBase64String("9fbGByUN5sRonNfscnoV2Q==");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
