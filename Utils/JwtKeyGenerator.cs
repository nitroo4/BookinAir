using System;
using System.Security.Cryptography;

namespace BOOKINGAPI.Utils
{
    public static class JwtKeyGenerator
    {
        // Génère une clé sécurisée en Base64
        public static string GenerateKey(int size = 64)
        {
            var keyBytes = RandomNumberGenerator.GetBytes(size);
            return Convert.ToBase64String(keyBytes);
        }
    }
}