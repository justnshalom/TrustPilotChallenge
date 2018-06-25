// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utilities.cs" company="TrustPilot">
//   Utilities
// </copyright>
// <summary>
//   Defines the Utilities type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BackEndChallengeAnagram
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>The utilities.</summary>
    public class Utilities
    {
        /// <summary>Get md5 hash value.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Md5Hash(string input)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            var utf8Encoding = new UTF8Encoding();
            byte[] data = md5Hasher.ComputeHash(utf8Encoding.GetBytes(input));
            var encryptedString = BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
            return encryptedString;
        }
    }
}
