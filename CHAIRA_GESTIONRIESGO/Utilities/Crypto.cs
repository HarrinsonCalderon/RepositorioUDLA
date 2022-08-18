using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CHAIRA_GESTIONRIESGO.Utilities
{
    public class Crypto
    {
        static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("SenorSuO");
        Random random = new Random();

        /// <summary>
        /// añade un numero aleatorio antes del numero de documento el cual va seguido de un -
        /// </summary>
        /// <param name="cadena">numero de documento de la persona</param>
        /// <returns></returns>

        public string Concat(string cadena)
        {
            return random.Next().ToString() + "-" + cadena;
        }
        /// <summary>
        /// Elimina caracres especiales a una cadena 
        /// </summary>
        /// <param name="cadena">string cadena</param>
        /// <returns>una cadena sin caracteres especiales</returns>        
        public string ClearString(string cadena)
        {
            return Regex.Replace(cadena, @"[^\w\.@-]", "");
        }

        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the original string is null or empty.</exception>
        public string _Encrypt(string cadena)
        {
            return Encrypt(cadena);
        }

        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException("El texto a encriptar es Nulo, metodo Encrypt(string).");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        /// <param name="cryptedString">The crypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the crypted string is null or empty.</exception>
        public string _Decrypt(string cadena)
        {
            return Decrypt(cadena);
        }

        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException("El texto a desencriptar es Nulo, metodo Decrypt(string);");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Obtiene el md5 de una cadena
        /// </summary>
        /// <param name="Cadena">cadena a enciptar</param>
        /// <returns>cadena encriptada con md5</returns>
        public string Md5(string Cadena)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] dataMd5 = md5.ComputeHash(Encoding.Default.GetBytes(Cadena));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
                sb.AppendFormat("{0:x2}", dataMd5[i]);
            return sb.ToString();
        }

        public static string MD5(string cadena)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(cadena);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        public string Md5_php(string cadena)
        {
            return MD5(cadena);
        }
    }
}