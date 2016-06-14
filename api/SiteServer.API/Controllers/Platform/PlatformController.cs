using BaiRong.Core;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.API.Model;
using SiteServer.CMS.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace SiteServer.API.Controllers.CMS
{
    public class PlatformController : ApiController
    {
        #region 验证码图片属性
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

        private Bitmap validateimage;
        private Graphics g;
        #endregion

        #region 验证码图片私有方法
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

        public void ValidateCode(string validateCode)
        {
            validateimage = new Bitmap(130, 53, PixelFormat.Format32bppRgb);
            g = Graphics.FromImage(validateimage);
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 243, 248)), 0, 0, 200, 200); //矩形框
            g.DrawString(validateCode, new Font(FontFamily.GenericSerif, 28, FontStyle.Bold | FontStyle.Italic), new SolidBrush(RandomColor), new PointF(14, 3));//字体/颜色

            g.Save();
            MemoryStream ms = new MemoryStream();
            validateimage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            HttpContext.Current.Response.End();
        }

        private static Color[] colors = new Color[] { Color.FromArgb(37, 72, 91), Color.FromArgb(68, 24, 25), Color.FromArgb(17, 46, 2), Color.FromArgb(70, 16, 100), Color.FromArgb(24, 88, 74) };
        private Color RandomColor
        {
            get
            {
                Random r = new Random();
                return colors[r.Next(0, 5)];
            }
        }
        #endregion

        /// <summary>
        /// 显示验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("ValidateCode")]
        public HttpResponseMessage ValidateCode()
        {
            string cookieName = PageUtils.FilterSqlAndXss(HttpContext.Current.Request.QueryString["cookieName"]);
            bool isCorsCross = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isCorsCross"]);
            bool isBigImage = TranslateUtils.ToBool(HttpContext.Current.Request.QueryString["isBigImage"]);

            string validateCode = ValidateCodeManager.CreateValidateCode(false);

            if (isCorsCross)
            {
                DbCacheManager.Insert(cookieName, validateCode);
            }
            else
            {
                CookieUtils.SetCookie(cookieName, validateCode, DateTime.Now.AddDays(1));
            }


            HttpResponseMessage response = new HttpResponseMessage();
            if (isBigImage)
            {
                HttpContext.Current.Response.BufferOutput = true;  //特别注意
                HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//特别注意
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);//特别注意
                HttpContext.Current.Response.AppendHeader("  Pragma", "No-Cache"); //特别注意
                ValidateCode(validateCode);
            }
            else
            {
                // 生成BITMAP图像
                Bitmap bitmap = new Bitmap(imgWidth, imgHeight);
                // 给图像设置干扰
                DisturbBitmap(bitmap);
                // 绘制验证码图像
                DrawValidateCode(bitmap, validateCode);
                MemoryStream ms = new MemoryStream();
                // 保存验证码图像，等待输出
                bitmap.Save(ms, ImageFormat.Gif);
                response.Content = new ByteArrayContent(ms.ToArray());
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
            return response;

        }

    }
}
