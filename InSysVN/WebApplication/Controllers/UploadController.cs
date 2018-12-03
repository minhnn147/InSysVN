using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Code;
using LibCore.Helper;

namespace WebApplication.Controllers
{
    public class UploadController : BaseController
    {
        [HttpPost]
        private dynamic Upload(HttpPostedFileBase file, string folder = "")
        {
            //var a = int.Parse("ss");
            var fileName = Path.GetFileName(file.FileName);
            var pathRemote = GetFullRootUrl() + VirtualPathUtility.ToAbsolute(AppSettings.FolderUpload + "/" + folder);
            var rootPath = Server.MapPath(AppSettings.FolderUpload + "/" + folder);
            if (fileName != null)
            {
                var name = string.Format(Path.GetFileNameWithoutExtension(fileName) + "({0}-{1})", User.Id, DateTime.Now.ToString("MMddyyyy HHmmss"));
                //var name = Path.GetFileNameWithoutExtension(fileName);
                name = Utility.RejectMarks(name);

                var extension = Path.GetExtension(fileName);
                //extension = string.IsNullOrEmpty(extension) ? ".jpg" : extension;
                fileName = name + extension;

                bool isExists = Directory.Exists(rootPath);
                if (!isExists)
                    Directory.CreateDirectory(rootPath);

                var pathFile = Path.Combine(rootPath, fileName);
                file.SaveAs(pathFile);
                var file_link = pathRemote + "/" + fileName;
                return new
                {
                    Link = Utility.PathFormat(file_link),
                    URL = pathFile
                };
            }
            return null;
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, string folder = "")
        {
            try
            {
                var folderThum = Path.Combine("ImagesThumbnail", folder);
                folder = Path.Combine("/Images", folder);
                var upload = Upload(file, folder);
                if (upload != null)
                {
                    var URL = upload.URL as string;
                    folderThum = URL.Replace(folder, folderThum);
                    //var a = new System.IO.File();

                    //Common.Helpers.ImageHelper.ResizeWidth(new File(URL), folderThum, 200, Common.Helpers.ImageHelper.GetImageFormat(Path.GetFileName(URL));
                    return JsonResultSuccess(new
                    {
                        FileLink = upload.Link
                    });
                }
                return JsonResultError("");
            }
            catch (Exception ex)
            {
                return JsonResultError(ex);
            }
        }
        [HttpGet]
        public ActionResult UploadAvatar()
        {
            return PartialView("UploadAvatar");
        }
        
    }
}