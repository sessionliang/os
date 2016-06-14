using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;

using BaiRong.Core.IO;
using BaiRong.Core;
using BaiRong.Model;

namespace BaiRong.Core.Drawing
{
    public class ImageUtils
    {
        private ImageUtils() { }

        public static Bitmap GetBitmap(string imageFilePath)
        {
            FileStream fs = new FileStream(imageFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            MemoryStream ms = new MemoryStream(bytes);

            Bitmap bitmap = (Bitmap)Bitmap.FromStream(ms, false);

            return bitmap;
        }

        public static Image GetImage(string imageFilePath)
        {
            FileStream fs = new FileStream(imageFilePath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            MemoryStream ms = new MemoryStream(bytes);

            Image image = Image.FromStream(ms, false);

            return image;
        }

        public static ImageFormat GetImageFormat(string imagePath)
        {
            string extName = PathUtils.GetExtension(imagePath).ToLower();
            if (extName == ".bmp")
            {
                return ImageFormat.Bmp;
            }
            else if (extName == ".emf")
            {
                return ImageFormat.Emf;
            }
            else if (extName == ".exif")
            {
                return ImageFormat.Exif;
            }
            else if (extName == ".gif")
            {
                return ImageFormat.Gif;
            }
            else if (extName == ".ico")
            {
                return ImageFormat.Icon;
            }
            else if (extName == ".jpg" || extName == ".jpeg")
            {
                return ImageFormat.Jpeg;
            }
            else if (extName == ".png")
            {
                return ImageFormat.Png;
            }
            else if (extName == ".tiff")
            {
                return ImageFormat.Tiff;
            }
            else if (extName == ".wmf")
            {
                return ImageFormat.Wmf;
            }
            return ImageFormat.Png;
        }

        private static PointF GetWaterMarkPointF(Image image, int waterMarkPosition, float waterMarkWidth, float waterMarkHeight, bool textMark)
        {
            float x = 0;
            float y = 0;
            switch (waterMarkPosition)
            {
                case 1:
                    if (textMark)
                    {
                        x = waterMarkWidth / 2;
                    }
                    else
                    {
                        x = 0;
                    }
                    y = 0;
                    break;
                case 2:
                    if (textMark)
                    {
                        x = (image.Width / 2);
                    }
                    else
                    {
                        x = (image.Width / 2) - (waterMarkWidth / 2);
                    }
                    y = 0;
                    break;
                case 3:
                    if (textMark)
                    {
                        x = image.Width - waterMarkWidth / 2;
                    }
                    else
                    {
                        x = image.Width - waterMarkWidth;
                    }
                    y = 0;
                    break;
                case 4:
                    if (textMark)
                    {
                        x = waterMarkWidth / 2;
                    }
                    else
                    {
                        x = 0;
                    }
                    y = (image.Height / 2) - (waterMarkHeight / 2);
                    break;
                case 5:
                    if (textMark)
                    {
                        x = (image.Width / 2);
                    }
                    else
                    {
                        x = (image.Width / 2) - (waterMarkWidth / 2);
                    }
                    y = (image.Height / 2) - (waterMarkHeight / 2);
                    break;
                case 6:
                    if (textMark)
                    {
                        x = image.Width - waterMarkWidth / 2;
                    }
                    else
                    {
                        x = image.Width - waterMarkWidth;
                    }
                    y = (image.Height / 2) - (waterMarkHeight / 2);
                    break;
                case 7:
                    if (textMark)
                    {
                        x = waterMarkWidth / 2;
                    }
                    else
                    {
                        x = 0;
                    }
                    y = image.Height - waterMarkHeight;
                    break;
                case 8:
                    if (textMark)
                    {
                        x = (image.Width / 2);
                    }
                    else
                    {
                        x = (image.Width / 2) - (waterMarkWidth / 2);
                    }
                    y = image.Height - waterMarkHeight;
                    break;
                default:

                    if (textMark)
                    {
                        x = image.Width - waterMarkWidth / 2;
                    }
                    else
                    {
                        x = image.Width - waterMarkWidth;
                    }
                    y = image.Height - waterMarkHeight;
                    break;
            }
            return new PointF(x, y);
        }

        public static void AddTextWaterMark(string imagePath, string waterMarkText, string fontName, int fontSize, int waterMarkPosition, int waterMarkTransparency, int minWidth, int minHeight)
        {
            try
            {
                Image image = ImageUtils.GetImage(imagePath);

                if (minWidth > 0)
                {
                    if (image.Width < minWidth)
                    {
                        image.Dispose();
                        return;
                    }
                }
                if (minHeight > 0)
                {
                    if (image.Height < minHeight)
                    {
                        image.Dispose();
                        return;
                    }
                }

                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics picture = Graphics.FromImage(b);
                picture.Clear(Color.White);
                picture.SmoothingMode = SmoothingMode.HighQuality;
                picture.InterpolationMode = InterpolationMode.High;

                picture.DrawImage(image, 0, 0, image.Width, image.Height);

                int[] sizes = new int[] { fontSize, 16, 14, 12, 10, 8, 6, 4 };
                Font crFont = null;
                SizeF crSize = new SizeF();
                for (int i = 0; i < 8; i++)
                {
                    crFont = new Font(fontName, sizes[i], FontStyle.Bold);
                    crSize = picture.MeasureString(waterMarkText, crFont);

                    if ((ushort)crSize.Width < (ushort)image.Width && (ushort)crSize.Height < (ushort)image.Height) break;
                }

                if (image.Width <= Convert.ToInt32(crSize.Width) || image.Height <= Convert.ToInt32(crSize.Height)) return;
                bool textMark = true;
                PointF pointF = ImageUtils.GetWaterMarkPointF(image, waterMarkPosition, crSize.Width, crSize.Height, textMark);

                if (pointF.X < 0 || pointF.X >= image.Width || pointF.Y < 0 || pointF.Y >= image.Height) return;

                StringFormat strFormat = new StringFormat();
                strFormat.Alignment = StringAlignment.Center;

                int alphaRate = (255 * waterMarkTransparency) / 10;
                if (alphaRate <= 0 || alphaRate > 255) alphaRate = 153;

                SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(alphaRate, 0, 0, 0));
                picture.DrawString(waterMarkText, crFont, semiTransBrush2, pointF.X + 1, pointF.Y + 1, strFormat);

                SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(alphaRate, 255, 255, 255));
                //
                picture.DrawString(waterMarkText, crFont, semiTransBrush, pointF.X, pointF.Y, strFormat);

                semiTransBrush2.Dispose();
                semiTransBrush.Dispose();

                EFileSystemType fileType = EFileSystemTypeUtils.GetEnumType(PathUtils.GetExtension(imagePath));
                ImageFormat imageFormat = ImageFormat.Jpeg;
                if (fileType == EFileSystemType.Bmp)
                {
                    imageFormat = ImageFormat.Bmp;
                }
                else if (fileType == EFileSystemType.Gif)
                {
                    imageFormat = ImageFormat.Gif;
                }
                else if (fileType == EFileSystemType.Png)
                {
                    imageFormat = ImageFormat.Png;
                }

                b.Save(imagePath, imageFormat);
                b.Dispose();
                image.Dispose();

            }
            catch { }
        }

        public static void AddImageWaterMark(string imagePath, string waterMarkImagePath, int waterMarkPosition, int waterMarkTransparency, int minWidth, int minHeight, int qty)
        {
            try
            {
                Image image = ImageUtils.GetImage(imagePath);

                if (minWidth > 0)
                {
                    if (image.Width < minWidth)
                    {
                        image.Dispose();
                        return;
                    }
                }
                if (minHeight > 0)
                {
                    if (image.Height < minHeight)
                    {
                        image.Dispose();
                        return;
                    }
                }

                Bitmap b = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
                Graphics picture = Graphics.FromImage(b);
                picture.Clear(Color.White);
                picture.SmoothingMode = SmoothingMode.HighQuality;
                picture.InterpolationMode = InterpolationMode.High;

                picture.DrawImage(image, 0, 0, image.Width, image.Height);

                Image waterMark = ImageUtils.GetImage(waterMarkImagePath);

                if (image.Width <= waterMark.Width || image.Height <= waterMark.Height) return;
                bool testMark = false;
                PointF pointF = ImageUtils.GetWaterMarkPointF(image, waterMarkPosition, waterMark.Width, waterMark.Height, testMark);
                int xpos = Convert.ToInt32(pointF.X);
                int ypos = Convert.ToInt32(pointF.Y);

                if (xpos < 0 || xpos >= image.Width || ypos < 0 || ypos >= image.Height) return;

                int alphaRate = (255 * waterMarkTransparency) / 10;
                if (alphaRate <= 0 || alphaRate > 255) alphaRate = 153;

                Bitmap bmWaterMark = new Bitmap(waterMark);
                for (int ix = 0; ix < waterMark.Width; ix++)
                {
                    for (int iy = 0; iy < waterMark.Height; iy++)
                    {
                        int ir = bmWaterMark.GetPixel(ix, iy).R;
                        int ig = bmWaterMark.GetPixel(ix, iy).G;
                        int ib = bmWaterMark.GetPixel(ix, iy).B;

                        if (!(ir == 0 && ig == 0 && ib == 0))//水印图片中透明部分不显示，水印图片中纯黑也不显示
                        {
                            picture.DrawEllipse(new Pen(new SolidBrush(Color.FromArgb(alphaRate, ir, ig, ib))), xpos + ix, ypos + iy, 1, 1);
                        }
                    }
                }

                waterMark.Dispose();

                //压缩图片质量
                EncoderParameter p = new EncoderParameter(Encoder.Quality, qty * 10);
                EncoderParameters ps = new EncoderParameters(1);
                ps.Param[0] = p;
                b.Save(imagePath, GetCodecInfo("image/jpeg"), ps);

                b.Dispose();
                image.Dispose();

            }
            catch { }
        }

        /// <summary>
        /// 压缩图片质量时用
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns>得到指定mimeType的ImageCodecInfo</returns>
        private static ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        public static bool MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, bool isLessSizeNotThumb, out Size originalSize)
        {
            originalSize = new Size();

            if (width == 0 && height == 0)
            {
                FileUtils.CopyFile(originalImagePath, thumbnailPath);
                return true;
            }
            else
            {
                DirectoryUtils.CreateDirectoryIfNotExists(thumbnailPath);
                if (FileUtils.IsFileExists(originalImagePath))
                {
                    Image originalImage = Image.FromFile(originalImagePath);
                    originalSize = originalImage.Size;

                    if (width == 0)
                    {
                        if (isLessSizeNotThumb && originalImage.Height < height)
                        {
                            FileUtils.CopyFile(originalImagePath, thumbnailPath);
                            return true;
                        }
                        return MakeThumbnail(originalImage, originalImagePath, thumbnailPath, width, height, "H");
                    }
                    else if (height == 0)
                    {
                        if (isLessSizeNotThumb && originalImage.Width < width)
                        {
                            FileUtils.CopyFile(originalImagePath, thumbnailPath);
                            return true;
                        }
                        return MakeThumbnail(originalImage, originalImagePath, thumbnailPath, width, height, "W");
                    }
                    else
                    {
                        if (isLessSizeNotThumb && originalImage.Height < height && originalImage.Width < width)
                        {
                            FileUtils.CopyFile(originalImagePath, thumbnailPath);
                            return true;
                        }
                        return MakeThumbnail(originalImage, originalImagePath, thumbnailPath, width, height, "HW");
                    }
                }
                return false;
            }
        }

        public static bool MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, bool isLessSizeNotThumb)
        {
            Size originalSize = new Size();
            return MakeThumbnail(originalImagePath, thumbnailPath, width, height, isLessSizeNotThumb, out originalSize);
        }

        private static bool MakeThumbnail(Image originalImage, string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            bool created = false;

            if (FileUtils.IsFileExists(originalImagePath))
            {
                int towidth = width;
                int toheight = height;
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case "HW"://指定高宽缩放（可能变形）                                    
                        break;
                    case "W"://指定宽，高按比例                                        
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H"://指定高，宽按比例                    
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "Cut"://指定高宽裁减（不变形）
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                try
                {
                    bitmap.Save(thumbnailPath, GetImageFormat(originalImagePath));
                    created = true;
                }
                catch
                {
                    FileUtils.CopyFile(originalImagePath, thumbnailPath);
                    created = true;
                }
                finally
                {
                    originalImage.Dispose(); bitmap.Dispose(); g.Dispose();
                }
            }

            return created;
        }

        public static bool MakeThumbnailIfExceedWidth(string originalImagePath, string thumbnailPath, int width)
        {
            Size originalSize = new Size();
            Size thumbSize = new Size();
            return ImageUtils.MakeThumbnailIfExceedWidth(originalImagePath, thumbnailPath, width, out originalSize, out thumbSize);
        }

        public static bool MakeThumbnailIfExceedWidth(string originalImagePath, string thumbnailPath, int width, out Size originalSize, out Size thumbSize)
        {
            originalSize = new Size();
            thumbSize = new Size();

            bool created = false;

            DirectoryUtils.CreateDirectoryIfNotExists(thumbnailPath);
            if (FileUtils.IsFileExists(originalImagePath))
            {
                Image originalImage = Image.FromFile(originalImagePath);

                originalSize = originalImage.Size;
                thumbSize = originalImage.Size;

                if (originalImage.Width < width)
                {
                    return false;
                }

                int towidth = width;
                int toheight = toheight = originalImage.Height * width / originalImage.Width;//指定宽，高按比例
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
                try
                {
                    bitmap.Save(thumbnailPath, GetImageFormat(originalImagePath));
                    thumbSize = bitmap.Size;
                    created = true;
                }
                catch
                {
                    FileUtils.CopyFile(originalImagePath, thumbnailPath);
                    created = true;
                }
                finally
                {
                    originalImage.Dispose(); bitmap.Dispose(); g.Dispose();
                }
            }

            return created;
        }

        public static bool CropImage(string originalImagePath, string thumbnailPath, int xPosition, int yPosition, int width, int height)
        {
            bool created = false;

            DirectoryUtils.CreateDirectoryIfNotExists(thumbnailPath);
            if (FileUtils.IsFileExists(originalImagePath))
            {
                Image originalImage = Image.FromFile(originalImagePath);

                int towidth = width;
                int toheight = height;
                int x = xPosition;
                int y = yPosition;
                int ow = originalImage.Width;
                int oh = originalImage.Height;
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);

                Rectangle section = new Rectangle(new Point(x, y), new Size(width, height));
                g.DrawImage(originalImage, 0, 0, section, GraphicsUnit.Pixel);
                try
                {
                    bitmap.Save(thumbnailPath, GetImageFormat(originalImagePath));
                    created = true;
                }
                catch
                {
                    FileUtils.CopyFile(originalImagePath, thumbnailPath);
                    created = true;
                }
                finally
                {
                    originalImage.Dispose(); bitmap.Dispose(); g.Dispose();
                }
            }

            return created;
        }

        public static bool RotateFlipImage(string originalImagePath, string thumbnailPath, RotateFlipType rotateFlipType)
        {
            bool created = false;

            DirectoryUtils.CreateDirectoryIfNotExists(thumbnailPath);
            if (FileUtils.IsFileExists(originalImagePath))
            {
                Image originalImage = Image.FromFile(originalImagePath);

                originalImage.RotateFlip(rotateFlipType);

                try
                {
                    originalImage.Save(thumbnailPath, GetImageFormat(originalImagePath));
                    created = true;
                }
                catch
                {
                    FileUtils.CopyFile(originalImagePath, thumbnailPath);
                    created = true;
                }
                finally
                {
                    originalImage.Dispose();
                }
            }

            return created;
        }

        public static Image GetImageFromBytes(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                ms.Flush();
                return image;
            }
        }

        public static bool AddText(Image image, string imagePath, string topText, string middleText, string bottomText, int thumbImageHeight, int thumbFontSize)
        {
            bool isText = false;
            try
            {
                if (!string.IsNullOrEmpty(topText) || !string.IsNullOrEmpty(middleText) || !string.IsNullOrEmpty(bottomText))
                {
                    int fontSize = Convert.ToInt32(Convert.ToDouble(image.Height / thumbImageHeight) * thumbFontSize);
                    int lineHeight = fontSize + 20;
                    Font font = new Font("Microsoft YaHei", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

                    //http://tech.pro/tutorial/654/csharp-snippet-tutorial-how-to-draw-text-on-an-image

                    Graphics g = Graphics.FromImage(image);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    if (!string.IsNullOrEmpty(topText))
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        RectangleF rectangleF = new RectangleF(0, 20, image.Width, lineHeight);
                        g.DrawString(topText, font, Brushes.White, rectangleF, strFormat);
                    }
                    if (!string.IsNullOrEmpty(middleText))
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        RectangleF rectangleF = new RectangleF(0, Convert.ToInt64((image.Height - lineHeight) / 2), image.Width, lineHeight);
                        g.DrawString(middleText, font, Brushes.White, rectangleF, strFormat);
                    }
                    if (!string.IsNullOrEmpty(bottomText))
                    {
                        StringFormat strFormat = new StringFormat();
                        strFormat.Alignment = StringAlignment.Center;
                        RectangleF rectangleF = new RectangleF(0, image.Height - lineHeight - 30, image.Width, lineHeight);
                        g.DrawString(bottomText, font, Brushes.White, rectangleF, strFormat);
                    }

                    g.Dispose();

                    image.Save(imagePath);
                    image.Dispose();

                    isText = true;
                }
            }
            catch { }

            return isText;
        }
    }
}
