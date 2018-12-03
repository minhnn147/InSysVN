using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB
{
    public class UploadFile
    {
        public static void UploadImageWithBase64(string base64Image, string PathServer, string PathFile)
        {
            try
            {
                byte[] data = Convert.FromBase64String(base64Image.Split(',')[1]);
                MemoryStream ms = new MemoryStream(data);
                FileStream fs = new FileStream(PathServer + PathFile, FileMode.Create, System.IO.FileAccess.Write);
                ms.CopyTo(fs);
                fs.Close();
            }
            catch
            {
                throw;
            }
        }
    }
}
