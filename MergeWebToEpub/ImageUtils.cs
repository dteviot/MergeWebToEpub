using Ionic.Zip;
using MergeWebToEpub;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPWrapper;

namespace MergeWebToEpub
{
    static class ImageUtils
    {

        public static Image MakeThumbnail(this Image srcImage, int maxDimension)
        {
            double scaleFactor = CalcScaleFactor(srcImage, 96);
            var newWidth = (int)((srcImage.Width * scaleFactor) + 0.5);
            var newHeight = (int)((srcImage.Height * scaleFactor) + 0.5);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, newWidth, newHeight));
                return newImage;
            }
        }

        public static byte[] ConvertToJpeg(this Image srcImage)
        {
            using (var newImage = new Bitmap(srcImage.Width, srcImage.Height))
            using (var graphics = Graphics.FromImage(newImage))
            using (var outputStream = new MemoryStream())
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height));
                newImage.Save(outputStream, ImageFormat.Jpeg);
                return outputStream.ToArray();
            }
        }

        public static float CalcScaleFactor(Image img, int maxDimension)
        {
            float max = Math.Max(img.Width, img.Height);
            return ((float )maxDimension) / max;
        }

        public static Image ExtractImage(this ZipFile zip, string entryName, bool isWebp)
        {
            using (var ms = new MemoryStream(zip.ExtractBytes(entryName)))
            {
                return Image.FromStream(ms);
            }
        }

        public static Image ExtractImage(this byte[] rawBytes, bool isWebp)
        {
            if (rawBytes.Length == 0)
            {
                // if no image, return a dummy image
                return new Bitmap(1, 1);
            }
            if (isWebp)
            {
                using (var webp = new WebP())
                {
                    return webp.Decode(rawBytes);
                }
            }
            else
            {
                using (var ms = new MemoryStream(rawBytes))
                {
                    return Image.FromStream(ms);
                }
            }
        }

        public static bool IsImageFile(ZipEntry entry)
        {
            if (entry.IsDirectory)
            {
                return false;
            }
            string extension = Path.GetExtension(entry.FileName).ToLowerInvariant();
            switch (extension)
            {
                case ".jpeg":
                case ".jpg":
                case ".webp":
                case ".png":
                    return true;

                default:
                    return false;
            }
        }
    }
}
