/***
 * Interface BCryptUtil, define method to EnCript and DeEnCript password
 * 
 * Created by : MINHDV
 * Created date : 01 04 2014
 */

namespace Framework.Security.Crypt
{
    public interface IBCriptUtil
    {
        string EnCrypt(string strNormal);
        string EnCrypt(string strNormal, int WorkFactor);
        bool CheckEnCrypt(string strNormal, string strEnCrypt);
    }
}
