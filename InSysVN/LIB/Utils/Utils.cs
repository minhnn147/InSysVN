using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LIB.Utils
{
    public class Utils
    {

        public static string GetVarible(string content, int _startIndex = 0, string start = "{{", string end = "}}")
        {
            var indexStart = content.IndexOf(start, _startIndex);
            if (indexStart == -1)
            {
                return "";
            }
            var indexEnd = content.IndexOf(end, _startIndex + indexStart + start.Length);
            if (indexEnd == -1)
            {
                return "";
            }
            return content.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length);
        }
        public static string MoneyToText(string value1)
        {
            Func<string, string> numToText = delegate (string num)
            {
                if (num == "0")
                {
                    return "không";
                }
                else if (num == "1")
                {
                    return "một";
                }
                else if (num == "2")
                {
                    return "hai";
                }
                else if (num == "3")
                {
                    return "ba";
                }
                else if (num == "4")
                {
                    return "bốn";
                }
                else if (num == "5")
                {
                    return "năm";
                }
                else if (num == "6")
                {
                    return "sáu";
                }
                else if (num == "7")
                {
                    return "bảy";
                }
                else if (num == "8")
                {
                    return "tám";
                }
                else if (num == "9")
                {
                    return "chín";
                }
                else if (num == "10")
                {
                    return "mười";
                }
                return "";
            };

            Func<string, string> docDonVi = null;
            docDonVi = (string value) =>
            {
                if (value.Length == 1)
                {
                    return numToText(value);
                }
                else if (value.Length == 2)
                {
                    if (value == "10")
                    {
                        return numToText(value);
                    }
                    else
                    {
                        if (value[0] == '1')
                        {
                            return "mười " + numToText(value[1].ToString());
                        }
                        var txt = numToText(value[0].ToString()) + " mươi";
                        if (value[1] != '0')
                        {
                            if (value[1] == '1')
                            {
                                return txt + " mốt";
                            }
                            return txt + " " + numToText(value[1].ToString());
                        }
                        return txt;
                    }
                }
                else
                {
                    var txt = "";
                    txt = numToText(value[0].ToString()) + " trăm";
                    if (value[1] == '0')
                    {
                        if (value[2] != '0')
                        {
                            txt += " linh " + numToText(value[2].ToString());
                        }
                    }
                    else
                    {
                        txt += " " + docDonVi(value[1].ToString() + value[2].ToString());
                    }
                    return txt;
                }
            };

            Func<string, int, List<string>> getDonvi = (string value, int boi) =>
            {
                List<string> donvi = new List<string>();
                var temp = "";
                for (var i = value.Length; i > 0; i--)
                {
                    var e = value[i - 1];
                    temp = e + temp;
                    if ((value.Length - i + 1) % boi == 0 || i == 1)
                    {
                        donvi.Insert(0, temp);
                        temp = "";
                    }
                }
                return donvi;
            };

            value1 = !string.IsNullOrWhiteSpace(value1) ? value1 : "0";
            value1 += "";
            var txt_return = "";

            var donvi1 = getDonvi(value1, 3);
            var hangTy = getDonvi(value1, 9);

            for (var i = 0; i < donvi1.Count; i++)
            {
                var txt1 = docDonVi(donvi1[i]);
                switch (donvi1.Count - i - 1)
                {
                    case 0:
                        break;
                    case 1:
                        txt1 += " nghìn";
                        break;
                    case 2:
                        txt1 += " triệu";
                        break;
                    case 3:
                        txt1 += " tỷ";
                        break;
                    case 4:
                        txt1 += " nghìn tỷ";
                        break;
                    case 5:
                        txt1 += " triệu tỷ";
                        break;
                    default:
                        txt1 += " tỷ tỷ";
                        break;
                }
                txt_return += " " + txt1;
                var conlai = donvi1.Skip(i + 1).Take(donvi1.Count - 1).ToList();
                var check = conlai.Count > 0 && int.Parse(string.Join("", conlai)) > 0;
                if (!check)
                {
                    break;
                }
                if (i != donvi1.Count - 1)
                {
                    txt_return += ",";
                }
            }
            txt_return = txt_return.Trim();
            txt_return = txt_return.Substring(0, 1).ToUpper() + txt_return.Substring(1);
            return txt_return;
        }
        public static string FileToText(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                return "";
            }
            return FileToText(file.Open(FileMode.Open, FileAccess.Read));
        }
        public static string FileToText(Stream file)
        {
            var htmlTemplate = "";
            //using (StreamReader reader = new StreamReader(file.Open(FileMode.Open, FileAccess.Read)))
            using (StreamReader reader = new StreamReader(file))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    htmlTemplate += line;
                }
            }
            return htmlTemplate;
        }
        public static string PathFormat(string path)
        {
            path = path.Replace("http:\\", "[http]").Replace("http://", "[http]").Replace("//", "/").Replace("\\", "/").Replace("[http]", "http://");
            return path;
        }
        public static string RejectMarks(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }
            text = text.Trim();
            text = text.ToLower();
            string[] pattern = new string[7];
            pattern[0] = "a|(á|ả|à|ạ|ã|ă|ắ|ẳ|ằ|ặ|ẵ|â|ấ|ẩ|ầ|ậ|ẫ)";
            pattern[1] = "o|(ó|ỏ|ò|ọ|õ|ô|ố|ổ|ồ|ộ|ỗ|ơ|ớ|ở|ờ|ợ|ỡ)";
            pattern[2] = "e|(é|è|ẻ|ẹ|ẽ|ê|ế|ề|ể|ệ|ễ)";
            pattern[3] = "u|(ú|ù|ủ|ụ|ũ|ư|ứ|ừ|ử|ự|ữ)";
            pattern[4] = "i|(í|ì|ỉ|ị|ĩ)";
            pattern[5] = "y|(ý|ỳ|ỷ|ỵ|ỹ)";
            pattern[6] = "d|đ";
            for (int i = 0; i < pattern.Length; i++)
            {
                char replaceChar = pattern[i][0];
                MatchCollection matchs = Regex.Matches(text, pattern[i]);
                foreach (Match m in matchs)
                {
                    text = text.Replace(m.Value[0], replaceChar);
                }
            }
            // remove entities
            text = Regex.Replace(text, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            //text = Regex.Replace(text, @"[^a-z0-9\-\s]", "");
            // replace spaces
            text = text.Replace(".", "");
            text = text.Replace(":", "");
            text = text.Replace("+", "");
            text = text.Replace("/", "");
            text = text.Replace(@"\", "");
            text = text.Replace(' ', '-');
            text = text.Replace("(", "");
            text = text.Replace(")", "");
            // collapse dashes
            text = Regex.Replace(text, @"-{2,}", "-");
            // trim excessive dashes at the beginning
            text = text.TrimStart(new[] { '-' });
            // remove trailing dashes
            text = text.TrimEnd(new[] { '-' });
            return text;
        }
        public static bool CheckCaptcha(string response, string secretKey)
        {
            try
            {
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");
                return status;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
