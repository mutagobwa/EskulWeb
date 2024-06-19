using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ASEEncryptorDecryptorTool
{
    public class Crypto
    {
        #region Class Constructor
        public Crypto()
        {
            string key = "Fintech";
            TripleDes.Key = TruncateHash(key, TripleDes.KeySize / 8);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize / 8);
        }
        #endregion

        #region SHA256 Hashing
        /// <summary>
        /// Computes a SHA256 hash code for the string parameter provided.
        /// </summary>
        /// <param name="Message">The string for which a SHA256 hash code is to be generated.</param>
        /// <returns>Returns a SHA256 hash code for the string parameter provided.</returns>
        public static string EncryptSHA256Managed(string Message)
        {
            SHA256 sha256 = new SHA256Managed();
            byte[] sha256Bytes = Encoding.Default.GetBytes(Message);
            byte[] cryString = sha256.ComputeHash(sha256Bytes);
            string sha256Str = string.Empty;
            for (int i = 0; i < cryString.Length; i++)
            {
                sha256Str += cryString[i].ToString("x");
            }
            return sha256Str;
        }
        #endregion

        #region TrippleDES Cryptography
        /// <summary>
        /// Performs a TrippleDES encryption on the provided string.
        /// </summary>
        /// <param name="Message">The string parameter for which a TrippleDES encryption will be returned.</param>
        /// <param name="Passphrase">The optional string key to be used in TrippleDES encryption.</param>
        /// <returns>Returns the TrippleDES representation of the provided string.</returns>
        public static string EncryptString(string Message)
        {
            try
            {
                string Passphrase = "Fintech";
                byte[] Results;
                UTF8Encoding UTF8 = new UTF8Encoding();

                /* Step 1. We hash the passphrase using MD5 We use the MD5 hash generator as the result is a 128 bit byte array which is a valid 
                 * length for the TripleDES encoder we use below. */

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the encoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToEncrypt = UTF8.GetBytes(Message);

                // Step 5. Attempt to encrypt the string
                try
                {
                    ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                    Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the encrypted string as a base64 encoded string
                return Convert.ToBase64String(Results);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Performs a TrippleDES decryption on the provided encrypted string.
        /// </summary>
        /// <param name="Message">The TrippleDES string parameter for which a decryption will be returned.</param>
        /// <param name="Passphrase">The optional string key to be used in TrippleDES decryption.</param>
        /// <returns>Returns the plain text of the TrippleDES string provided.</returns>
        public static string DecryptString(string Message)
        {
            try
            {
                string Passphrase = "Fintech";
                byte[] Results;
                UTF8Encoding UTF8 = new UTF8Encoding();

                /* Step 1. We hash the passphrase using MD5 We use the MD5 hash generator as the result is a 128 bit byte array which is a valid 
                 * length for the TripleDES encoder we use below. */

                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

                // Step 3. Setup the decoder
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;

                // Step 4. Convert the input string to a byte[]
                byte[] DataToDecrypt = Convert.FromBase64String(Message);

                // Step 5. Attempt to decrypt the string
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    // Clear the TripleDes and Hashprovider services of any sensitive information
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }

                // Step 6. Return the decrypted string in UTF8 format
                return UTF8.GetString(Results);
            }
            catch
            {
                return "";
            }
        }

        private TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        private byte[] TruncateHash(string key, int length)
        {

            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            // Hash the key.
            byte[] keyBytes = Encoding.Unicode.GetBytes(key);
            byte[] hash = sha1.ComputeHash(keyBytes);

            // Truncate or pad the hash.
            Array.Resize(ref hash, length);
            return hash;
        }

        public string EncryptData(string Plaintext)
        {
            try
            {
                // Convert the plaintext string to a byte array.
                byte[] plaintextBytes = Encoding.Unicode.GetBytes(Plaintext);

                // Create the stream.
                MemoryStream ms = new MemoryStream();
                // Create the encoder to write to the stream.
                CryptoStream encStream = new CryptoStream(ms, TripleDes.CreateEncryptor(), CryptoStreamMode.Write);

                // Use the crypto stream to write the byte array to the stream.
                encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
                encStream.FlushFinalBlock();

                // Convert the encrypted stream to a printable string.
                return Convert.ToBase64String(ms.ToArray());

            }
            catch
            {
                //errorLog(ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "") + Environment.NewLine + ex.StackTrace, "99", "Functions", "TestConnection", Now(), "Encripting text")
                return "";
            }
        }

        public string DecryptData(string encryptedtext)
        {

            // Convert the encrypted text string to a byte array.
            byte[] encryptedBytes = Convert.FromBase64String(encryptedtext);

            // Create the stream.
            MemoryStream ms = new MemoryStream();
            // Create the decoder to write to the stream.
            CryptoStream decStream = new CryptoStream(ms, TripleDes.CreateDecryptor(), CryptoStreamMode.Write);

            // Use the crypto stream to write the byte array to the stream.
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();

            // Convert the plaintext stream to a string.
            return Encoding.Unicode.GetString(ms.ToArray());
        }
        #endregion

        #region MD5 Hashing
        /// <summary>
        /// Computes an MD5 hash code for the string parameter provided.
        /// </summary>
        /// <param name="Message">The string for which an MD5 hash code is to be generated.</param>
        /// <returns>Returns an MD5 hash code for the string parameter provided.</returns>
        public static string MD5Hash(string Message)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(Message));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        /// <summary>
        /// Verifies a generated MD5 hash against the input.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="hash">The input string's MD5 hash</param>
        /// <returns>Return true if the MD5 checks out, false otherwise.</returns>
        private static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = MD5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            if (0 == comparer.Compare(hashOfInput, hash))
                return true;
            else
                return false;
        }
        #endregion

        #region AES 256
        public static string EncryptStringAES(string text, string password = "")
        {
            string Password = "";
            Password = password == "" ? "Fintech" : password;

            byte[] baPwd = Encoding.UTF8.GetBytes(Password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);


            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt1(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string EncryptStringAESWithoutSalt(string text, string password = "")
        {
            string Password = "";
            Password = password == "" ? "Fintech" : password;

            byte[] baPwd = Encoding.UTF8.GetBytes(Password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);


            //byte[] baSalt = GetRandomBytes();
            //byte[] baEncrypted = new byte[baSalt.Length + baText.Length];


            byte[] baEncrypted = AES_Encrypt1(baText, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string EncryptStringAESWithoutSalt(string text)
        {
            string Password = "";
            Password = "Fintech";

            byte[] baPwd = Encoding.UTF8.GetBytes(Password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);


            //byte[] baSalt = GetRandomBytes();
            //byte[] baEncrypted = new byte[baSalt.Length + baText.Length];


            byte[] baEncrypted = AES_Encrypt1(baText, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }
        private static Aes CreateAes()
        {
            var aes = Aes.Create();
            aes.Mode = CipherMode.CFB;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;


            // You can change the salt item to suit your needs
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            //If this doesnt work Citi wajipange.. Jirongo 20201013

            //var aes = Aes.Create();
            //aes.Mode = CipherMode.CBC;
            //aes.Padding = PaddingMode.PKCS7;


            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = CreateAes())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    //AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Encrypt1(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };



            //var aes = Aes.Create();
            //aes.Mode = CipherMode.CBC;
            //aes.Padding = PaddingMode.PKCS7;


            using (MemoryStream ms = new MemoryStream())
            {
                using (var AES = CreateAes())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    //AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        public static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }

        public static int GetSaltLength()
        {
            return 8;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static byte[] AES_Decrypt1(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {

                using (var AES = CreateAes())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 10000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);


                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        public static string DecryptStringAES(string text, string password = "")
        {
            string Password = "";
            Password = password == "" ? "Fintech" : password;

            byte[] baPwd = Encoding.UTF8.GetBytes(Password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = AES_Decrypt1(baText, baPwdHash);

            // Remove salt
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];


            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        public static string DecryptStringAESWithoutSalt(string text, string password = "")
        {
            string Password = "";
            Password = password == "" ? "Fintech" : password;

            byte[] baPwd = Encoding.UTF8.GetBytes(Password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = AES_Decrypt1(baText, baPwdHash);


            string result = Encoding.UTF8.GetString(baDecrypted);
            return result;
        }
        #endregion
    }
}
