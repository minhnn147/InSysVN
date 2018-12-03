using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;


namespace Common.Helpers
{
    public class ImageHelper
    {
        public static bool CheckExtentionFile(string filename)
        {
            if (filename != null
                &&
                (
                       filename.ToLower().EndsWith(".jpg")
                    || filename.ToLower().EndsWith(".jpeg")
                    || filename.ToLower().EndsWith(".gif")
                    || filename.ToLower().EndsWith(".png")
                )
               )
                return true;
            return false;
        }
        public static ImageFormat GetImageFormat(string filename)
        {
            string ext = GetImageExtension(filename);
            ImageFormat imgFmt = ImageFormat.Jpeg;
            switch (ext)
            {
                case ".jpg":
                    imgFmt = ImageFormat.Jpeg;
                    break;
                case ".png":
                    imgFmt = ImageFormat.Png;
                    break;
                case ".gif":
                    imgFmt = ImageFormat.Gif;
                    break;
            }
            return imgFmt;
        }
        public static string GetImageExtension(string filename)
        {
            string ext = "";
            if (CheckExtentionFile(filename))
            {
                int i1 = filename.LastIndexOf('.');
                ext = filename.Substring(i1);

            }
            return ext;
        }
        public static float[] CaculatorSizeImage(float width, float height, float size = 800f)
        {
            //var size = 800f;
            var temp = width - height;

            if (temp > 0 && width > size)
            {
                height = height * (size / width);
                width = size;
            }
            else if (temp < 0 && height > size)
            {
                width = width * (size / height);
                height = size;
            }

            var _return = new float[] { width, height };
            return _return;
        }

        public static void ResizeWidth(byte[] imgByte, string path, int widthSize, int heightSize, ImageFormat imageFormat)
        {
            Stream stream = new MemoryStream(imgByte);
            ResizeWidth(stream, path, widthSize, heightSize, imageFormat);
        }
        public static void ResizeWidth(Stream stream, string path, ImageFormat imageFormat, int SizeResize)
        {
            Image image = Image.FromStream(stream);
            var sizeImage = CaculatorSizeImage(image.Width, image.Height, SizeResize);
            ResizeWidth(image, path, (int)sizeImage[0], (int)sizeImage[1], imageFormat);
        }
        public static void ResizeWidth(Stream stream, string path, int widthSize, int heightSize, ImageFormat imageFormat)
        {
            Image objImage = Image.FromStream(stream);
            ResizeWidth(objImage, path, widthSize, heightSize, imageFormat);
        }
        public static void ResizeWidth(Image objImage, string path, int widthSize, int heightSize, ImageFormat imageFormat)
        {
            int sWidth = objImage.Width;
            int sHeight = objImage.Height;
            int thumbWidth = widthSize;
            int thumbHeight = heightSize;
            Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.CompositingMode = CompositingMode.SourceCopy;
            Rectangle rectangle = new Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(objImage, rectangle, 0, 0, sWidth, sHeight, GraphicsUnit.Pixel);
            bmp.Save(path, imageFormat);
            bmp.Dispose();
        }
        public static void ResizeWidth(byte[] imgByte, string path, int widthSize, ImageFormat imageFormat)
        {
            Stream stream = new MemoryStream(imgByte);

            Image objImage = Image.FromStream(stream);
            int sWidth = objImage.Width;
            int sHeight = objImage.Height;
            int thumbWidth = sWidth;
            int thumbHeight = sHeight;
            if (sWidth > 300)
            {
                thumbWidth = widthSize;
                thumbHeight = (thumbWidth * sHeight) / sWidth;
            }
            Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            gr.SmoothingMode = SmoothingMode.HighQuality;
            gr.CompositingQuality = CompositingQuality.HighQuality;
            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
            gr.CompositingMode = CompositingMode.SourceCopy;
            Rectangle rectangle = new Rectangle(0, 0, thumbWidth, thumbHeight);
            gr.DrawImage(objImage, rectangle, 0, 0, sWidth, sHeight, GraphicsUnit.Pixel);
            bmp.Save(path, imageFormat);
            bmp.Dispose();
        }
    }
}
