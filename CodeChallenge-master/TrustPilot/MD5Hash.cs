using System;
using System.Security.Cryptography;
using System.Text;

namespace TrustPilot
{
    public static class MD5Hash
    {
        private static UTF8Encoding UTF8 = new UTF8Encoding();
        private static MD5 md5Hasher = MD5.Create();

        public static string Calculate(string input)
        {
            byte[] data = md5Hasher.ComputeHash(UTF8.GetBytes(input));
            return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
        }
    }
}
