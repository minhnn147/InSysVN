/***
 * Class Md5Util, use to EnCrypt string
 * 
 * Created by : minhdv
 * Created date : 01 04 2014
 */

using System.Security.Cryptography;
using System.Text;

namespace Framework.Security.Crypt
{
    /// <summary>
    /// Class Md5 Encrypt
    /// </summary>
    public class Md5Util
    {
        //http://www.codeproject.com/Articles/463390/Password-Encryption-using-MD5-Hash-Algorithm-in-Cs
        /// <summary>
        /// Md5 hash.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string Md5EnCrypt(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));

            //get hash result after compute it
            var result = md5.Hash;

            var strBuilder = new StringBuilder();
            for (var i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
