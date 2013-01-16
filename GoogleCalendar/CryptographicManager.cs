using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace GoogleCalendar
{
    /// <summary>
    /// Class used for encryption and decryption using AES algorithm
    /// </summary>
    public static class CryptographicManager
    {
        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="input">String to encrypt</param>
        /// <param name="password">Password to use for encryption</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string input, string password)
        {
            //make sure we have data to work with
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("input cannot be empty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password cannot be empty");
            // get IV, key and encrypt
            var iv = CreateInitializationVector(password);
            var key = CreateKey(password);
            var encryptedBuffer = CryptographicEngine.Encrypt(key, CryptographicBuffer.ConvertStringToBinary(input, BinaryStringEncoding.Utf8), iv);
            return CryptographicBuffer.EncodeToBase64String(encryptedBuffer);
        }
        /// <summary>
        /// Decrypt a string previously ecnrypted with Encrypt method and the same password
        /// </summary>
        /// <param name="input">String to decrypt</param>
        /// <param name="password">Password to use for decryption</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string input, string password)
        {
            //make sure we have data to work with
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("input cannot be empty");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("password cannot be empty");
            // get IV, key and decrypt
            var iv = CreateInitializationVector(password);
            var key = CreateKey(password);
            var decryptedBuffer = CryptographicEngine.Decrypt(key, CryptographicBuffer.DecodeFromBase64String(input), iv);
            return CryptographicBuffer.ConvertBinaryToString(
            BinaryStringEncoding.Utf8, decryptedBuffer);
        }
        /// <summary>
        /// Create initialization vector IV
        /// </summary>
        /// <param name="password">Password is used for random vector generation</param>
        /// <returns>Vector</returns>
        private static IBuffer CreateInitializationVector(string password)
        {
            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            var iv = CryptographicBuffer.CreateFromByteArray(UTF8Encoding.UTF8.GetBytes(password));
            return iv;
        }
        /// <summary>
        /// Create encryption key
        /// </summary>
        /// <param name="password">Password is used for random key generation</param>
        /// <returns></returns>
        private static CryptographicKey CreateKey(string password)
        {
            var provider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf8);
            if ((buffer.Length % provider.BlockLength) != 0)
            {
                throw new Exception("Message buffer length must be multiple of block length.");
            }
            var key = provider.CreateSymmetricKey(buffer);
            return key;
        }
    }
}