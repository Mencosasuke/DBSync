using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace DBSync.Helper
{
    public class EncryptHelper
    {

        private static byte[] key = ASCIIEncoding.ASCII.GetBytes("M1d851nK");
        private static byte[] iv = ASCIIEncoding.ASCII.GetBytes("M1d851nK");

        /// <summary>
        /// Genera una cadena encriptada a partir de un String que se le envíe como parámetro
        /// </summary>
        /// <param name="originalString">String que contiene la cadena que se quiere encriptar.</param>
        /// <returns>String con la cadena original encriptada.</returns>
        public String Encrypt(string originalString)
        {
            if (!String.IsNullOrEmpty(originalString))
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                StreamWriter writer = new StreamWriter(cryptoStream);
                writer.Write(originalString);
                writer.Flush();
                cryptoStream.FlushFinalBlock();
                writer.Flush();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
            }
            
            return String.Empty;
        }

        /// <summary>
        /// Desencripta la cadena enviada como parametro.
        /// </summary>
        /// <param name="cryptedString">String con la cadena encriptada</param>
        /// <returns>String que contiene la cadena desencriptada</returns>
        public String Decrypt(string cryptedString)
        {
            if (!String.IsNullOrEmpty(cryptedString))
            {
                DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cryptoStream);

                return reader.ReadToEnd();
            }

            return String.Empty;
        }
    }
}