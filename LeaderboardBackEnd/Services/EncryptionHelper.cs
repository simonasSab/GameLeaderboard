﻿using System.Security.Cryptography;
using System.Text;

namespace LeaderboardBackEnd.Services;

public static class EncryptionHelper
{
    private static byte[] RetrieveKey() // 32-byte key is stored in a limited-access file
    {
        using (StreamReader streamReader = new("key.txt"))
            return Encoding.UTF8.GetBytes(streamReader.ReadToEnd().Trim());
    }

    public static string Encrypt(string textString)
    {
        // Validate text
        if (string.IsNullOrEmpty(textString))
            return "";

        // Turn string into byte[] and save length for later use
        byte[] plaintext = Encoding.UTF8.GetBytes(textString);
        int plaintextLength = plaintext.Length;
        // Use RandomNumberGenerator to generate nonce (number used once)
        byte[] nonce = new byte[12];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(nonce);
        // Prepare empty byte arrays 
        byte[] ciphertext = new byte[plaintextLength];
        byte[] tag = new byte[16];

        // Encrypt plaintext and store in ciphertext
        using (AesGcm cipher = new(RetrieveKey(), 16))
            cipher.Encrypt(nonce, plaintext, ciphertext, tag);
        // Store nonce, tag and ciphertext in a single byte array
        byte[] result = new byte[28 + plaintextLength];
        Buffer.BlockCopy(nonce, 0, result, 0, 12);
        Buffer.BlockCopy(tag, 0, result, 12, 16);
        Buffer.BlockCopy(ciphertext, 0, result, 28, plaintextLength);

        return Convert.ToBase64String(result);
    }

    public static string Decrypt(string cipherMessageString)
    {
        // Validate text
        if (string.IsNullOrEmpty(cipherMessageString))
            return "";

        // Turn string into byte[] and save length for later use
        byte[] cipherMessage = Convert.FromBase64String(cipherMessageString);
        int cipherBytesLength = cipherMessage.Length;
        // Prepare empty byte arrays
        byte[] nonce = new byte[12];
        byte[] tag = new byte[16];
        byte[] plaintext = new byte[cipherBytesLength - 28];
        byte[] ciphertext = new byte[cipherBytesLength - 28];

        // Extract nonce, tag and ciphertext from cipherBytes
        Buffer.BlockCopy(cipherMessage, 0, nonce, 0, 12);
        Buffer.BlockCopy(cipherMessage, 12, tag, 0, 16);
        Buffer.BlockCopy(cipherMessage, 28, ciphertext, 0, ciphertext.Length);
        // Decrypt ciphertext and store in plaintext
        using (AesGcm cipher = new(RetrieveKey(), 16))
            cipher.Decrypt(nonce, ciphertext, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }
}
