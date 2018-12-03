using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Framework.Helper
{
    public class Utility
    {
        #region GenerateRandomCode
        /// <summary>
        /// Randoms the code by lenght.
        /// </summary>
        /// <param name="lenght">The lenght.</param>
        /// <returns>String</returns>
        public static string RandomCodeByLenght(int lenght)
        {
            Random random = new Random();
            const string _chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] buffer = new char[lenght];
            for (int i = 0; i < lenght; i++)
            {
                buffer[i] = _chars[random.Next(_chars.Length)];
            }
            return new string(buffer);
        }
        #endregion

        #region CookiesControl
        /// <summary>
        /// Saves the cookie.
        /// </summary>
        /// <param name="_cookieName">Name of the cookie.</param>
        /// <param name="_value">The value.</param>
        /// <param name="_expires">The expires.</param>
        public static void SaveCookie(string _cookieName, string _value, int _expires)
        {
            var _cookies = new HttpCookie(_cookieName) { Value = EncryptASE(_value), Expires = DateTime.Now.AddDays(_expires) };
            _cookies.HttpOnly = true;// cookie not available in js
            HttpContext.Current.Response.Cookies.Add(_cookies);//Save userID to cookie when remember checked.
        }

        /// <summary>
        /// Clears the cookie.
        /// </summary>
        /// <param name="_cookieName">Name of the cookie.</param>
        public static void ClearCookie(string _cookieName)
        {
            var _cookies = new HttpCookie(_cookieName) { Expires = DateTime.Now.AddDays(-365) };
            HttpContext.Current.Response.Cookies.Add(_cookies);
        }
        #endregion

        #region ASE256 Encrypt
        private static string AesIV = ConfigurationManager.AppSettings["AesIV"];
        private static string AesKey = ConfigurationManager.AppSettings["AesKey"];

        /// <summary>
        /// Encrypts the ASE.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>string</returns>
        public static string EncryptASE(string text)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Convert.FromBase64String(AesIV);
                aes.Key = Convert.FromBase64String(AesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Convert string to byte array
                byte[] src = Encoding.Unicode.GetBytes(text);

                // encryption
                using (ICryptoTransform encrypt = aes.CreateEncryptor())
                {
                    byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                    // Convert byte array to Base64 strings
                    return Convert.ToBase64String(dest);
                }
            }
            catch
            {
                return text;
            }
        }

        /// <summary>
        /// Decrypts the ASE.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string DecryptASE(string text)
        {
            try
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.IV = Convert.FromBase64String(AesIV);
                aes.Key = Convert.FromBase64String(AesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Convert Base64 strings to byte array
                byte[] src = System.Convert.FromBase64String(text);


                // decryption
                using (ICryptoTransform decrypt = aes.CreateDecryptor())
                {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    return Encoding.Unicode.GetString(dest);
                }
            }
            catch
            {
                return text;

            }
        }

        public static Stream EncryptFile(Stream inputStream)
        {
            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(AesIV, Encoding.ASCII.GetBytes(AesKey));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            try
            {
                return new CryptoStream(inputStream, algorithm.CreateEncryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                return inputStream;
            }
        }

        public static Stream DecryptFile(Stream inputStream)
        {
            var algorithm = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(AesIV, Encoding.ASCII.GetBytes(AesKey));

            algorithm.Key = key.GetBytes(algorithm.KeySize / 8);
            algorithm.IV = key.GetBytes(algorithm.BlockSize / 8);

            try
            {
                return new CryptoStream(inputStream, algorithm.CreateDecryptor(), CryptoStreamMode.Read);
            }
            catch
            {
                return inputStream;
            }
        }
        #endregion

        #region Error Message
        private static string strOutputHtml = string.Empty;

        public static string Error(string caption, string message)
        {
            strOutputHtml = "    <div id=\"message-error\" class=\"message message-error\">\n";
            strOutputHtml = strOutputHtml + "       <div class=\"image\">\n";
            strOutputHtml = strOutputHtml + "           <img src=\"/Resources/images/icons/error.png\" alt=\"" + caption + "\" height=\"32\" />\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"text\">\n";
            strOutputHtml = strOutputHtml + "           <h6>" + caption + "</h6>\n";
            strOutputHtml = strOutputHtml + "           <span>" + message + "</span>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"dismiss\">\n";
            strOutputHtml = strOutputHtml + "           <a href=\"#message-error\"></a>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "   </div>\n";
            return strOutputHtml;
        }

        public static string Notice(string caption, string message)
        {
            strOutputHtml = "    <div id=\"message-notice\" class=\"message message-notice\">\n";
            strOutputHtml = strOutputHtml + "       <div class=\"image\">\n";
            strOutputHtml = strOutputHtml + "           <img src=\"/Resources/images/icons/notice.png\" alt=\"" + caption + "\" height=\"32\" />\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"text\">\n";
            strOutputHtml = strOutputHtml + "           <h6>" + caption + "</h6>\n";
            strOutputHtml = strOutputHtml + "           <span>" + message + "</span>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"dismiss\">\n";
            strOutputHtml = strOutputHtml + "           <a href=\"#message-notice\"></a>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "   </div>\n";
            return strOutputHtml;
        }

        public static string Success(string caption, string message)
        {
            strOutputHtml = "    <div id=\"message-success\" class=\"message message-success\">\n";
            strOutputHtml = strOutputHtml + "       <div class=\"image\">\n";
            strOutputHtml = strOutputHtml + "           <img src=\"/Resources/images/icons/success.png\" alt=\"" + caption + "\" height=\"32\" />\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"text\">\n";
            strOutputHtml = strOutputHtml + "           <h6>" + caption + "</h6>\n";
            strOutputHtml = strOutputHtml + "           <span>" + message + "</span>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"dismiss\">\n";
            strOutputHtml = strOutputHtml + "           <a href=\"#message-success\"></a>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "   </div>\n";
            return strOutputHtml;
        }

        public static string Warning(string caption, string message)
        {
            strOutputHtml = "    <div id=\"message-warning\" class=\"message message-warning\">\n";
            strOutputHtml = strOutputHtml + "       <div class=\"image\">\n";
            strOutputHtml = strOutputHtml + "           <img src=\"/Resources/images/icons/warning.png\" alt=\"" + caption + "\" height=\"32\" />\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"text\">\n";
            strOutputHtml = strOutputHtml + "           <h6>" + caption + "</h6>\n";
            strOutputHtml = strOutputHtml + "           <span>" + message + "</span>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "       <div class=\"dismiss\">\n";
            strOutputHtml = strOutputHtml + "           <a href=\"#message-warning\"></a>\n";
            strOutputHtml = strOutputHtml + "       </div>\n";
            strOutputHtml = strOutputHtml + "   </div>\n";
            return strOutputHtml;
        }
        #endregion

        #region ConvertDatetime
        public static string ConvertToFormatDateTime(string sDateTime)
        {
            string sYear = sDateTime.Substring(0, 4);
            string sMonth = sDateTime.Substring(4, 2);
            string sDay = sDateTime.Substring(6, 2);
            string sHour = sDateTime.Substring(8, 2);
            string sMinute = sDateTime.Substring(10, 2);
            string sSecond = sDateTime.Substring(12, 2);
            return sDay + "-" + sMonth + "-" + sYear + " " + sHour + ":" + sMinute + ":" + sSecond;
        }
        /// <summary>
        /// ConvertToFormatDate
        /// </summary>
        /// <param name="sDate">ddMMyyyy</param>
        /// <returns>MM/dd/yyyy</returns>
        public static DateTime ConvertToFormatDate(string sDate)
        {
            string sYear = sDate.Substring(4, 4);
            string sMonth = sDate.Substring(2, 2);
            string sDay = sDate.Substring(0, 2);
            return DateTime.ParseExact(sDay + "/" + sMonth + "/" + sYear, "dd/MM/yyyy", new CultureInfo("fr-FR"));
        }
        public static string ConvertToFormatDateDMY(string sDate)
        {
            string sYear = sDate.Substring(4, 4);
            string sMonth = sDate.Substring(2, 2);
            string sDay = sDate.Substring(0, 2);
            return sDay + "/" + sMonth + "/" + sYear;
        }
        /// <summary>
        /// ConvertToRealFormatDateTime
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <returns>MM-dd-yyyy</returns>
        public static DateTime ConvertToRealFormatDateTime(string sDateTime, char format = '/')
        {
            var newDate = sDateTime.Substring(0, sDateTime.IndexOf(" ")).Split(format);
            var sYear = newDate[2];
            var sMonth = newDate[0];
            var sDay = newDate[1];
            if (int.Parse(sDay) < 10)
                sDay = "0" + sDay;
            if (int.Parse(sMonth) < 10)
                sMonth = "0" + sMonth;
            return DateTime.ParseExact(sDay + "/" + sMonth + "/" + sYear, "dd/MM/yyyy", new CultureInfo("fr-FR"));
        }
        public static DateTime ConvertToFormatHour(string sDateTime)
        {
            string sHour = sDateTime.Substring(8, 2);
            string sMinute = sDateTime.Substring(10, 2);
            return DateTime.Parse(sHour + ":" + sMinute);
        }

        public static DateTime FormatDateTimeYMD(string sDateTime)
        {
            sDateTime = sDateTime.Replace("-", "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            return DateTime.Parse(sYear + "-" + sMonth + "-" + sDay);// + " " + sHour + ":" + sMinute + ":" + sSecond);
        }
        /// <summary>
        /// Convert string to dateTime Type with input format (dd/MM/yyyy)
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <param name="sReplace"></param>
        /// <param name="sFormat"></param>
        /// <returns></returns>
        public static DateTime FormatDateTimeYMD(string sDateTime, string sReplace, string sFormat)
        {
            sDateTime = sDateTime.Replace(sReplace, "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            return DateTime.Parse(sYear + sFormat + sMonth + sFormat + sDay);// + " " + sHour + ":" + sMinute + ":" + sSecond);
        }

        public static DateTime FormatDateTimeWithReplaceYMD(string sDateTime, string sReplace, string sFormat)
        {
            sDateTime = sDateTime.Replace(sReplace, "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            return DateTime.Parse(sYear + sFormat + sMonth + sFormat + sDay);// + " " + sHour + ":" + sMinute + ":" + sSecond);
        }

        public static string FormatDateTimeDMY(string sDateTime)
        {
            sDateTime = sDateTime.Replace("/", "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            return sDay + "-" + sMonth + "-" + sYear;
        }

        public static DateTime FormatDateTimeYMDHMS(string sDateTime)
        {
            sDateTime = sDateTime.Replace("/", "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            string sHour = sDateTime.Substring(8, 2);
            string sMinute = sDateTime.Substring(10, 2);
            string sSecond = sDateTime.Substring(12, 2);
            return DateTime.Parse(sYear + "-" + sMonth + "-" + sDay + " " + sHour + ":" + sMinute + ":" + sSecond);
        }

        public static DateTime FormatDateTimeYMDHM(string sDateTime)
        {
            sDateTime = sDateTime.Replace("-", "");
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            string sHour = sDateTime.Substring(9, 2);
            string sMinute = sDateTime.Substring(12, 2);
            return DateTime.Parse(sYear + "-" + sMonth + "-" + sDay + " " + sHour + ":" + sMinute + ":00");
        }
        /// <summary>
        /// Convert string to DateTime with IFormatProvider
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string sDateTime, IFormatProvider culture)
        {
            DateTime date = DateTime.Parse(sDateTime, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            return date;
        }
        /// <summary>
        /// Format string Datetime to YMDHMS
        /// </summary>
        /// <param name="sDateTime"></param>
        /// <returns></returns>
        public static string ConvertFormatString(string sDateTime)
        {
            sDateTime = sDateTime.Replace("-", String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty);
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            string sHour = sDateTime.Substring(8, 2);
            string sMinute = sDateTime.Substring(10, 2);
            string sSecond = sDateTime.Substring(12, 2);
            return sYear + sMonth + sDay + sHour + sMinute + sSecond;
        }

        public static string ConvertFormatStringByFormat(string sDateTime, string strformat)
        {
            sDateTime = sDateTime.Replace(strformat, String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty);
            string sYear = sDateTime.Substring(4, 4);
            string sMonth = sDateTime.Substring(2, 2);
            string sDay = sDateTime.Substring(0, 2);
            string sHour = sDateTime.Substring(8, 2);
            string sMinute = sDateTime.Substring(10, 2);
            string sSecond = sDateTime.Substring(12, 2);
            return sDay + strformat + sMonth + strformat + sYear + " " + sHour + ":" + sMinute + ":" + sSecond;
        }
        #endregion

        #region RemoveHTML tag

        public static string removeHTML(string orgText)
        {
            if (string.IsNullOrEmpty(orgText))
                return string.Empty;
            for (int i = 33; i < 48; i++)
            {
                orgText = orgText.Replace(((char)i).ToString(CultureInfo.InvariantCulture), "");
            }
            for (int i = 58; i < 65; i++)
            {
                orgText = orgText.Replace(((char)i).ToString(CultureInfo.InvariantCulture), "");
            }
            for (int i = 91; i < 97; i++)
            {
                orgText = orgText.Replace(((char)i).ToString(CultureInfo.InvariantCulture), "");
            }

            for (int i = 123; i < 127; i++)
            {
                orgText = orgText.Replace(((char)i).ToString(CultureInfo.InvariantCulture), "");
            }

            orgText = orgText.Replace(" ", "-");

            var regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            var strFormD = orgText.Normalize(System.Text.NormalizationForm.FormD).ToLower();

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            //return System.Text.RegularExpressions.Regex.Replace(orgText, @"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Pc}]", string.Empty);
        }

        public static string RemoveHtmlTag(string str)
        {
            return Regex.Replace(str, "<.*?>", string.Empty);
        }
        #endregion

        #region trả về thứ trong tuần
        public static string ReturnThu(DayOfWeek dow)
        {
            string result;
            switch (dow)
            {
                case DayOfWeek.Monday: result = "2"; break;
                case DayOfWeek.Tuesday: result = "3"; break;
                case DayOfWeek.Wednesday: result = "4"; break;
                case DayOfWeek.Thursday: result = "5"; break;
                case DayOfWeek.Friday: result = "6"; break;
                case DayOfWeek.Saturday: result = "7"; break;
                case DayOfWeek.Sunday: result = "chu nhat"; break;
                default: result = ""; break;
            }
            return result;
        }
        #endregion

        #region Write log
        public static void WriteLogFile(string fileName, string logInfo, string filePath)
        {
            StreamWriter sw;
            DateTime dt = DateTime.Today;
            string strDate = String.Format("{0:ddMMyyyy}", dt);
            filePath += fileName + "_" + strDate + ".txt";

            if (File.Exists(filePath))
            {
                sw = File.AppendText(filePath);
                sw.WriteLine(logInfo);
                sw.Close();
            }
            else
            {
                sw = File.CreateText(filePath);
                sw.WriteLine(logInfo);
                sw.Close();
            }
        }
        #endregion
    }
    #region Pageing
    [ToolboxData("<{0}:Bjzj_2sGroup_Pager runat=\"server\"></{0}:Bjzj_2sGroup_Pager>")]
    public class Bjzj_2sGroup_Pager : WebControl, IPostBackEventHandler, INamingContainer
    {
        private bool _altEnabled = true;
        private string _BACK_TO_FIRST = "Đầu";
        private string _BACK_TO_PAGE = "Trước";
        private int _currentIndex = 1;
        private bool _enableSSC = true;

        private string _FIRST = "&lt;&lt;";
        private int _firstCompactedPageCount = 10;
        private string _FROM = "từ";
        private bool _generateGoToSection = false;
        private bool _generateHiddenHyperlinks = false;
        private string _GO = "go";
        private string _GO_TO_LAST = "Cuối";
        private bool _infoCellVisible = true;
        private double _itemCount;
        private string _LAST = "&gt;&gt;";
        private int _maxSmartShortCutCount = 6;
        private string _next = "&gt;";
        private string _NEXT_TO_PAGE = "Sau";
        private int _notCompactedPageCount = 15;
        private string _OF = "/";
        private string _PAGE = "Trang";
        private int _pageCount;
        private int _pageSize = 15;
        private string _previous = "&lt;";
        private string _queryStringParameterName = "pagerControlCurrentPageIndex";
        private bool _rightToLeft = false;
        private string _SHOW_RESULT = "Xem kết quả từ ";
        private bool _showFirstLast = false;
        private string _SHOWING_RESULT = "Đang xem kết quả từ ";
        private List<int> _smartShortCutList;
        private double _sscRatio = 3.0;
        private int _sscThreshold = 30;
        private string _TO = "tới";
        private static readonly object EventCommand = new object();

        public event CommandEventHandler Command
        {
            add
            {
                base.Events.AddHandler(EventCommand, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventCommand, value);
            }
        }

        private void CalculateSmartShortcutAndFillList()
        {
            this._smartShortCutList = new List<int>();
            double num = (this.PageCount * this.SmartShortCutRatio) / 100.0;
            double maxSmartShortCutCount = Math.Round(num, 0);
            if (maxSmartShortCutCount > this.MaxSmartShortCutCount)
            {
                maxSmartShortCutCount = this.MaxSmartShortCutCount;
            }
            if (maxSmartShortCutCount == 1.0)
            {
                maxSmartShortCutCount++;
            }
            for (int i = 1; i < (maxSmartShortCutCount + 1.0); i++)
            {
                int item = (int)(Math.Round((double)((((this.PageCount * (100.0 / maxSmartShortCutCount)) * i) / 100.0) * 0.1), 0) * 10.0);
                if (item >= this.PageCount)
                {
                    break;
                }
                this.SmartShortCutList.Add(item);
            }
        }

        private string GenerateAltMessage(int pageNumber)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append((pageNumber == this.CurrentIndex) ? this.ShowingResultClause : this.ShowResultClause);
            builder.Append(" ");
            builder.Append((int)(((pageNumber - 1) * this.PageSize) + 1));
            builder.Append(" ");
            builder.Append(this.ToClause);
            builder.Append(" ");
            builder.Append((pageNumber == this.PageCount) ? this.ItemCount : ((double)(pageNumber * this.PageSize)));
            builder.Append(" ");
            builder.Append(this.OfClause);
            builder.Append(" ");
            builder.Append(this.ItemCount);
            return builder.ToString();
        }

        private string GetAlternativeText(int pageNumber)
        {
            return (this.GenerateToolTips ? string.Format(" title=\"{0}\"", this.GenerateAltMessage(pageNumber)) : "");
        }

        private bool IsSmartShortCutAvailable()
        {
            return ((this.GenerateSmartShortCuts && (this.SmartShortCutList != null)) && (this.SmartShortCutList.Count != 0));
        }

        protected override void LoadControlState(object state)
        {
            object[] objArray = (object[])state;
            this.CurrentIndex = (int)objArray[0];
            this.PageSize = (int)objArray[1];
        }

        protected virtual void OnCommand(CommandEventArgs e)
        {
            CommandEventHandler handler = (CommandEventHandler)base.Events[EventCommand];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.RegisterRequiresControlState(this);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            int num;
            if (this.Page != null)
            {
                this.Page.VerifyRenderingInServerForm(this);
            }
            if ((this.PageCount > this.SmartShortCutThreshold) && this.GenerateSmartShortCuts)
            {
                this.CalculateSmartShortcutAndFillList();
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dataTables_paginate paging_simple_numbers");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "dataTables-example_paginate");
            if (this.RTL)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Dir, "rtl");
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "pagination");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            //if (this.GeneratePagerInfoSection)
            //{
            //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "dataTables_info");
            //    writer.RenderBeginTag(HtmlTextWriterTag.Div);
            //    writer.Write(this.PageClause + " " + this.CurrentIndex.ToString() + this.OfClause + this.PageCount.ToString());
            //    writer.RenderEndTag();
            //}
            if (this.GenerateFirstLastSection && (this.CurrentIndex != 1))
            {
                writer.Write(this.RenderFirst());
            }
            if (this.CurrentIndex != 1)
            {
                writer.Write(this.RenderBack());
            }
            if (this.CurrentIndex < this.CompactModePageCount)
            {
                if (this.CompactModePageCount > this.PageCount)
                {
                    this.CompactModePageCount = this.PageCount;
                }
                for (num = 1; num < (this.CompactModePageCount + 1); num++)
                {
                    if (num == this.CurrentIndex)
                    {
                        writer.Write(this.RenderCurrent());
                    }
                    else
                    {
                        writer.Write(this.RenderOther(num));
                    }
                }
                this.RenderSmartShortCutByCriteria(this.CompactModePageCount, true, writer);
            }
            else if ((this.CurrentIndex >= this.CompactModePageCount) && (this.CurrentIndex < this.NormalModePageCount))
            {
                if (this.NormalModePageCount > this.PageCount)
                {
                    this.NormalModePageCount = this.PageCount;
                }
                for (num = 1; num < (this.NormalModePageCount + 1); num++)
                {
                    if (num == this.CurrentIndex)
                    {
                        writer.Write(this.RenderCurrent());
                    }
                    else
                    {
                        writer.Write(this.RenderOther(num));
                    }
                }
                this.RenderSmartShortCutByCriteria(this.NormalModePageCount, true, writer);
            }
            else if (this.CurrentIndex >= this.NormalModePageCount)
            {
                int num2 = this.NormalModePageCount / 2;
                int basePageNumber = this.CurrentIndex - num2;
                int num4 = this.CurrentIndex + num2;
                this.RenderSmartShortCutByCriteria(basePageNumber, false, writer);
                for (num = basePageNumber; (num < (num4 + 1)) && (num < (this.PageCount + 1)); num++)
                {
                    if (num == this.CurrentIndex)
                    {
                        writer.Write(this.RenderCurrent());
                    }
                    else
                    {
                        writer.Write(this.RenderOther(num));
                    }
                }
                if (num4 < this.PageCount)
                {
                    this.RenderSmartShortCutByCriteria(num4, true, writer);
                }
            }
            if (this.CurrentIndex != this.PageCount)
            {
                writer.Write(this.RenderNext());
            }
            if (this.GenerateFirstLastSection && (this.CurrentIndex != this.PageCount))
            {
                writer.Write(this.RenderLast());
            }
            writer.RenderEndTag();
            writer.RenderEndTag();
            if (this.GenerateHiddenHyperlinks)
            {
                writer.Write(this.RenderHiddenDiv());
            }
        }

        /// <summary>
        /// Renders the back.
        /// </summary>
        /// <returns></returns>
        private string RenderBack()
        {
            //string[] strArray = new string[] { "<td class=\"page-left\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\" ", this.BackToPageClause, " ", (this.CurrentIndex - 1).ToString(), "\"> ", this.PreviousClause, " </a></td>" };
            string[] strArray = new string[] { "<li class=\"paginate_button previous\"><a href=\"{0}\" title=\" ", this.BackToPageClause, " ", (this.CurrentIndex - 1).ToString(), "\">Quay lại</a></li>" };
            int num = this.CurrentIndex - 1;
            return string.Format(string.Concat(strArray), this.Page.ClientScript.GetPostBackClientHyperlink(this, num.ToString()));
        }

        /// <summary>
        /// Renders the current.
        /// </summary>
        /// <returns></returns>
        private string RenderCurrent()
        {
            //return ("<td class=\"PagerCurrentPageCell\"><span class=\"PagerHyperlinkStyle\" " + this.GetAlternativeText(this.CurrentIndex) + " ><strong style=\"color:#000;font-weight:700;\">" + this.CurrentIndex.ToString() + "</strong></span></td>");
            return ("<li class=\"paginate_button active\" aria-controls=\"dataTables-example\"><span class=\"PagerHyperlinkStyle\" " + this.GetAlternativeText(this.CurrentIndex) + " ><strong style=\"color:#000;font-weight:700;\">" + this.CurrentIndex.ToString() + "</strong></span></li>");
        }

        /// <summary>
        /// Renders the first.
        /// </summary>
        /// <returns></returns>
        private string RenderFirst()
        {
            return string.Format("<td class=\"PagerOtherPageCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\" " + this.BackToFirstClause + " \"> " + this.FirstClause + " </a></td>", this.Page.ClientScript.GetPostBackClientHyperlink(this, "1"));
        }

        /// <summary>
        /// Renders the hidden div.
        /// </summary>
        /// <returns></returns>
        private string RenderHiddenDiv()
        {
            int num;
            Uri url = HttpContext.Current.Request.Url;
            bool flag = !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["QUERY_STRING"]);
            string format = "<a href=\"{0}\">page {1}</a>";
            string str2 = "<div style=\"display:none;\">{0}</div>";
            StringBuilder builder = new StringBuilder();
            if (flag && (HttpContext.Current.Request.QueryString[this.QueryStringParameterName] != null))
            {
                Regex regex = new Regex(this.QueryStringParameterName + @"\=\d*", RegexOptions.Singleline | RegexOptions.Compiled);
                for (num = 0; num < this.NormalModePageCount; num++)
                {
                    builder.Append(string.Format(format, regex.Replace(url.ToString(), this.QueryStringParameterName + "=" + (num + this.CurrentIndex)), num + this.CurrentIndex));
                }
            }
            else
            {
                string str3 = "";
                for (num = 0; num < this.NormalModePageCount; num++)
                {
                    str3 = string.Format("{0}={1}", this.QueryStringParameterName, num + this.CurrentIndex);
                    builder.Append(string.Format(format, flag ? (url.ToString() + "&" + str3) : (url.ToString() + "?" + str3), num + this.CurrentIndex));
                }
            }
            return string.Format(str2, builder.ToString());
        }

        /// <summary>
        /// Renders the last.
        /// </summary>
        /// <returns></returns>
        private string RenderLast()
        {
            return string.Format("<td class=\"PagerOtherPageCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" title=\" " + this.GoToLastClause + " \"> " + this.LastClause + " </a></td>", this.Page.ClientScript.GetPostBackClientHyperlink(this, this.PageCount.ToString()));
        }

        /// <summary>
        /// Renders the next.
        /// </summary>
        /// <returns></returns>
        private string RenderNext()
        {
            string[] strArray = new string[] { "<li class=\"paginate_button next\"><a href=\"{0}\" title=\" ", this.NextToPageClause, " ", (this.CurrentIndex + 1).ToString(), "\">Tiếp theo</a></li>" };
            int num = this.CurrentIndex + 1;
            return string.Format(string.Concat(strArray), this.Page.ClientScript.GetPostBackClientHyperlink(this, num.ToString()));
        }

        /// <summary>
        /// Renders the other.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns></returns>
        private string RenderOther(int pageNumber)
        {
            //return string.Format("<td class=\"PagerOtherPageCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" " + this.GetAlternativeText(pageNumber) + " > " + pageNumber.ToString() + " </a></td>", this.Page.ClientScript.GetPostBackClientHyperlink(this, pageNumber.ToString()));
            return string.Format("<li class=\"paginate_button\"><a href=\"{0}\" " + this.GetAlternativeText(pageNumber) + " > " + pageNumber.ToString() + " </a></li>", this.Page.ClientScript.GetPostBackClientHyperlink(this, pageNumber.ToString()));
        }

        private void RenderSmartShortCutByCriteria(int basePageNumber, bool getRightBand, HtmlTextWriter writer)
        {
            if (this.IsSmartShortCutAvailable())
            {
                int num2;
                List<int> smartShortCutList = this.SmartShortCutList;
                int num = -1;
                if (!getRightBand)
                {
                    if (!getRightBand)
                    {
                        for (num2 = 0; num2 < smartShortCutList.Count; num2++)
                        {
                            if (basePageNumber > smartShortCutList[num2])
                            {
                                num = num2;
                            }
                        }
                        if (num >= 0)
                        {
                            for (num2 = 0; num2 < (num + 1); num2++)
                            {
                                if (smartShortCutList[num2] != basePageNumber)
                                {
                                    writer.Write(this.RenderSSC(smartShortCutList[num2]));
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (num2 = 0; num2 < smartShortCutList.Count; num2++)
                    {
                        if (smartShortCutList[num2] > basePageNumber)
                        {
                            num = num2;
                            break;
                        }
                    }
                    if (num >= 0)
                    {
                        for (num2 = num; num2 < smartShortCutList.Count; num2++)
                        {
                            if (smartShortCutList[num2] != basePageNumber)
                            {
                                writer.Write(this.RenderSSC(smartShortCutList[num2]));
                            }
                        }
                    }
                }
            }
        }

        private string RenderSSC(int pageNumber)
        {
            return string.Format("<td class=\"PagerSSCCells\"><a class=\"PagerHyperlinkStyle\" href=\"{0}\" " + this.GetAlternativeText(pageNumber) + " > " + pageNumber.ToString() + " </a></td>", this.Page.ClientScript.GetPostBackClientHyperlink(this, pageNumber.ToString()));
        }

        protected override object SaveControlState()
        {
            return new object[] { this.CurrentIndex, this.PageSize };
        }

        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.OnCommand(new CommandEventArgs(this.UniqueID, Convert.ToInt32(eventArgument)));
        }

        // Properties
        [Category("Globalization")]
        public string BackToFirstClause
        {
            get
            {
                return this._BACK_TO_FIRST;
            }
            set
            {
                this._BACK_TO_FIRST = value;
            }
        }

        [Category("Globalization")]
        public string BackToPageClause
        {
            get
            {
                return this._BACK_TO_PAGE;
            }
            set
            {
                this._BACK_TO_PAGE = value;
            }
        }

        [Category("Behavioural")]
        public int CompactModePageCount
        {
            get
            {
                return this._firstCompactedPageCount;
            }
            set
            {
                this._firstCompactedPageCount = value;
            }
        }

        [Browsable(false)]
        public int CurrentIndex
        {
            get
            {
                return this._currentIndex;
            }
            set
            {
                this._currentIndex = value;
            }
        }

        [Category("Globalization")]
        public string FirstClause
        {
            get
            {
                return this._FIRST;
            }
            set
            {
                this._FIRST = value;
            }
        }

        [Category("Globalization")]
        public string FromClause
        {
            get
            {
                return this._FROM;
            }
            set
            {
                this._FROM = value;
            }
        }

        [Category("Behavioural")]
        public bool GenerateFirstLastSection
        {
            get
            {
                return this._showFirstLast;
            }
            set
            {
                this._showFirstLast = value;
            }
        }

        [Category("Behavioural")]
        public bool GenerateGoToSection
        {
            get
            {
                return this._generateGoToSection;
            }
            set
            {
                this._generateGoToSection = value;
            }
        }

        [Category("Behavioural")]
        public bool GenerateHiddenHyperlinks
        {
            get
            {
                return this._generateHiddenHyperlinks;
            }
            set
            {
                this._generateHiddenHyperlinks = value;
            }
        }

        [Category("Behavioural")]
        public bool GeneratePagerInfoSection
        {
            get
            {
                return this._infoCellVisible;
            }
            set
            {
                this._infoCellVisible = value;
            }
        }

        [Category("Behavioural")]
        public bool GenerateSmartShortCuts
        {
            get
            {
                return this._enableSSC;
            }
            set
            {
                this._enableSSC = value;
            }
        }

        [Category("Behavioural")]
        public bool GenerateToolTips
        {
            get
            {
                return this._altEnabled;
            }
            set
            {
                this._altEnabled = value;
            }
        }

        [Category("Globalization")]
        public string GoClause
        {
            get
            {
                return this._GO;
            }
            set
            {
                this._GO = value;
            }
        }

        [Category("Globalization")]
        public string GoToLastClause
        {
            get
            {
                return this._GO_TO_LAST;
            }
            set
            {
                this._GO_TO_LAST = value;
            }
        }

        [Browsable(false)]
        public double ItemCount
        {
            get
            {
                return this._itemCount;
            }
            set
            {
                this._itemCount = value;
                double a = this.ItemCount / ((double)this.PageSize);
                double num2 = Math.Ceiling(a);
                this.PageCount = Convert.ToInt32(num2);
            }
        }

        [Category("Globalization")]
        public string LastClause
        {
            get
            {
                return this._LAST;
            }
            set
            {
                this._LAST = value;
            }
        }

        [Category("Behavioural")]
        public int MaxSmartShortCutCount
        {
            get
            {
                return this._maxSmartShortCutCount;
            }
            set
            {
                this._maxSmartShortCutCount = value;
            }
        }

        [Category("Globalization")]
        public string NextClause
        {
            get
            {
                return this._next;
            }
            set
            {
                this._next = value;
            }
        }

        [Category("Globalization")]
        public string NextToPageClause
        {
            get
            {
                return this._NEXT_TO_PAGE;
            }
            set
            {
                this._NEXT_TO_PAGE = value;
            }
        }

        [Category("Behavioural")]
        public int NormalModePageCount
        {
            get
            {
                return this._notCompactedPageCount;
            }
            set
            {
                this._notCompactedPageCount = value;
            }
        }

        [Category("Globalization")]
        public string OfClause
        {
            get
            {
                return this._OF;
            }
            set
            {
                this._OF = value;
            }
        }

        [Category("Globalization")]
        public string PageClause
        {
            get
            {
                return this._PAGE;
            }
            set
            {
                this._PAGE = value;
            }
        }

        [Browsable(false)]
        private int PageCount
        {
            get
            {
                return this._pageCount;
            }
            set
            {
                this._pageCount = value;
            }
        }

        [Category("Behavioural")]
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                this._pageSize = value;
            }
        }

        [Category("Globalization")]
        public string PreviousClause
        {
            get
            {
                return this._previous;
            }
            set
            {
                this._previous = value;
            }
        }

        [Category("Behavioural")]
        public string QueryStringParameterName
        {
            get
            {
                return this._queryStringParameterName;
            }
            set
            {
                this._queryStringParameterName = value;
            }
        }

        [Category("Globalization")]
        public bool RTL
        {
            get
            {
                return this._rightToLeft;
            }
            set
            {
                this._rightToLeft = value;
            }
        }

        [Category("Globalization")]
        public string ShowingResultClause
        {
            get
            {
                return this._SHOWING_RESULT;
            }
            set
            {
                this._SHOWING_RESULT = value;
            }
        }

        [Category("Globalization")]
        public string ShowResultClause
        {
            get
            {
                return this._SHOW_RESULT;
            }
            set
            {
                this._SHOW_RESULT = value;
            }
        }

        private List<int> SmartShortCutList
        {
            get
            {
                return this._smartShortCutList;
            }
            set
            {
                this._smartShortCutList = value;
            }
        }

        [Category("Behavioural")]
        public double SmartShortCutRatio
        {
            get
            {
                return this._sscRatio;
            }
            set
            {
                this._sscRatio = value;
            }
        }

        [Category("Behavioural")]
        public int SmartShortCutThreshold
        {
            get
            {
                return this._sscThreshold;
            }
            set
            {
                this._sscThreshold = value;
            }
        }

        [Category("Globalization")]
        public string ToClause
        {
            get
            {
                return this._TO;
            }
            set
            {
                this._TO = value;
            }
        }
    }
    #endregion

    #region Show Image From byte[]
    public static class HtmlExtensions
    {
        public static MvcHtmlString Image(this HtmlHelper html, byte[] image)
        {
            var img = String.Format("data: image/jpg; base64, {0}", Convert.ToBase64String(image));
            return new MvcHtmlString("<image src='" + img + "' />");
        }
    }
    #endregion


}
