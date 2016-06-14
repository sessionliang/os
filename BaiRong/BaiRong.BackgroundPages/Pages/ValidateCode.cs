using System;
using System.Drawing;
using System.Drawing.Imaging;
using BaiRong.Core;
using System.Web.UI;

namespace BaiRong.Pages
{
    public class ValidateCode : Page
    {
        // 图片清晰度
        private int fineness = 100;
        // 图片宽度
        private int imgWidth = 48;
        // 图片高度
        private int imgHeight = 20;
        // 字体家族名称
        private string fontFamily = "Times New Roman";
        // 字体大小
        private int fontSize = 15;
        // 字体样式
        private int fontStyle = 0;
        // 绘制起始坐标 X
        private int posX = 0;
        // 绘制起始坐标 Y
        private int posY = 0;

        public void Page_Load(object sender, System.EventArgs e)
        {
            string cookieName = PageUtils.FilterSqlAndXss(base.Request.QueryString["cookieName"]);
            bool isCrossDomain = TranslateUtils.ToBool(base.Request.QueryString["isCrossDomain"]);

            string validateCode = ValidateCodeManager.CreateValidateCode(false);

            if (isCrossDomain)
            {
                DbCacheManager.Insert(cookieName, validateCode);
            }
            else
            {
                CookieUtils.SetCookie(cookieName, validateCode, DateTime.Now.AddDays(1));
            }

            // 生成BITMAP图像
            Bitmap bitmap = new Bitmap(imgWidth, imgHeight);

            // 给图像设置干扰
            DisturbBitmap(bitmap);

            // 绘制验证码图像
            DrawValidateCode(bitmap, validateCode);

            // 保存验证码图像，等待输出
            bitmap.Save(Response.OutputStream, ImageFormat.Gif);
        }

        //------------------------------------------------------------
        // 为图片设置干扰点
        //------------------------------------------------------------
        private void DisturbBitmap(Bitmap bitmap)
        {
            // 通过随机数生成
            Random random = new Random();

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    if (random.Next(100) <= this.fineness)
                        bitmap.SetPixel(i, j, Color.White);
                }
            }
        }

        //------------------------------------------------------------
        // 绘制验证码图片
        //------------------------------------------------------------
        private void DrawValidateCode(Bitmap bitmap, string validateCode)
        {
            // 获取绘制器对象
            Graphics g = Graphics.FromImage(bitmap);

            // 设置绘制字体
            Font font = new Font(fontFamily, fontSize, GetFontStyle());

            // 绘制验证码图像
            g.DrawString(validateCode, font, Brushes.Black, posX, posY);
        }

        //------------------------------------------------------------
        // 换算验证码字体样式：1 粗体 2 斜体 3 粗斜体，默认为普通字体
        //------------------------------------------------------------
        private FontStyle GetFontStyle()
        {
            if (fontStyle == 1)
                return FontStyle.Bold;
            else if (fontStyle == 2)
                return FontStyle.Italic;
            else if (fontStyle == 3)
                return FontStyle.Bold | FontStyle.Italic;
            else
                return FontStyle.Regular;
        }
    }
}
