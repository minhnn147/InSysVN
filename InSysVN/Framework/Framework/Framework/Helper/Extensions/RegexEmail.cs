using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.Helper.Extensions
{
    /// <summary>
    /// Extension regex email
    /// </summary>
    /// <createdDate>01/04/2014</createdDate>
    public class RegexEmail
    {
        bool invalid = false;

        public bool IsValidEmail(string strIn)
        {
            string RegexMatch = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";
            if (String.IsNullOrEmpty(strIn))
                return false;
            //// Use IdnMapping class to convert Unicode domain names. 
            //try
            //{
            //    strIn = Regex.Replace(strIn, @"(@)(.+)$", RegexOptions.None, TimeSpan.FromMilliseconds(200));
            //}
            //catch (RegexMatchTimeoutException)
            //{
            //    return false;
            //}

            //if (invalid)
            //    return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                Regex IsSuccess = new Regex(RegexMatch,RegexOptions.IgnoreCase);
                if (!IsSuccess.IsMatch(strIn))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }
    }
}
