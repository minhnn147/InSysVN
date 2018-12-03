/***
 * Class BCryptUtil, use to EnCript and DeEnCript password
 * 
 * Created by : minhdv
 * Created date : 01 04 2014
 */

namespace Framework.Security.Crypt
{
    public class BCryptUtil
    {
        /// <summary>
        /// EnCrypt string function with default workfactor
        /// </summary>
        /// <param name="strNormal">input string</param>
        /// <returns>Ouput: String EnCrypt</returns>
        /// <author>minhdv</author>
        /// <CreatedDate>01 04 2014</CreatedDate>
        public static string EnCrypt(string strNormal)
        {
            return BCrypt.Net.BCrypt.HashPassword(strNormal);
        }

        /// <summary>
        /// EnCrypt string function
        /// </summary>
        /// <param name="strNormal">input string</param>
        /// <param name="WorkFactor">option workfactor</param>
        /// <returns>Ouput: String EnCrypt</returns>
        /// <author>minhdv</author>
        /// <CreatedDate>01 04 2014</CreatedDate>
        public static string EnCrypt(string strNormal, int WorkFactor)
        {
            return BCrypt.Net.BCrypt.HashPassword(strNormal, BCrypt.Net.BCrypt.GenerateSalt(WorkFactor));
        }

        /// <summary>
        /// Check EnCript string function
        /// </summary>
        /// <param name="strNormal">string input</param>
        /// <param name="strEnCrypt">string EnCrypt</param>
        /// <returns>
        /// True if equal
        /// False if not equal
        /// </returns>
        /// <author>minhdv</author>
        /// <CreatedDate>01 04 2014</CreatedDate>
        public static bool CheckEnCrypt(string strNormal, string strEnCrypt)
        {
            return BCrypt.Net.BCrypt.Verify(strNormal, strEnCrypt);
        }

    }
}
