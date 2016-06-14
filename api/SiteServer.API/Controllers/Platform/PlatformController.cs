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
        #region ��֤��ͼƬ����
        // ͼƬ������
        private int fineness = 100;
        // ͼƬ���
        private int imgWidth = 48;
        // ͼƬ�߶�
        private int imgHeight = 20;
        // �����������
        private string fontFamily = "Times New Roman";
        // �����С
        private int fontSize = 15;
        // ������ʽ
        private int fontStyle = 0;
        // ������ʼ���� X
        private int posX = 0;
        // ������ʼ���� Y
        private int posY = 0;

        private Bitmap validateimage;
        private Graphics g;
        #endregion

        #region ��֤��ͼƬ˽�з���
        //------------------------------------------------------------
        // ΪͼƬ���ø��ŵ�
        //------------------------------------------------------------
        private void DisturbBitmap(Bitmap bitmap)
        {
            // ͨ�����������
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
        // ������֤��ͼƬ
        //------------------------------------------------------------
        private void DrawValidateCode(Bitmap bitmap, string validateCode)
        {
            // ��ȡ����������
            Graphics g = Graphics.FromImage(bitmap);

            // ���û�������
            Font font = new Font(fontFamily, fontSize, GetFontStyle());

            // ������֤��ͼ��
            g.DrawString(validateCode, font, Brushes.Black, posX, posY);
        }

        //------------------------------------------------------------
        // ������֤��������ʽ��1 ���� 2 б�� 3 ��б�壬Ĭ��Ϊ��ͨ����
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
            g.FillRectangle(new SolidBrush(Color.FromArgb(240, 243, 248)), 0, 0, 200, 200); //���ο�
            g.DrawString(validateCode, new Font(FontFamily.GenericSerif, 28, FontStyle.Bold | FontStyle.Italic), new SolidBrush(RandomColor), new PointF(14, 3));//����/��ɫ

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
        /// ��ʾ��֤��
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
                HttpContext.Current.Response.BufferOutput = true;  //�ر�ע��
                HttpContext.Current.Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//�ر�ע��
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);//�ر�ע��
                HttpContext.Current.Response.AppendHeader("  Pragma", "No-Cache"); //�ر�ע��
                ValidateCode(validateCode);
            }
            else
            {
                // ����BITMAPͼ��
                Bitmap bitmap = new Bitmap(imgWidth, imgHeight);
                // ��ͼ�����ø���
                DisturbBitmap(bitmap);
                // ������֤��ͼ��
                DrawValidateCode(bitmap, validateCode);
                MemoryStream ms = new MemoryStream();
                // ������֤��ͼ�񣬵ȴ����
                bitmap.Save(ms, ImageFormat.Gif);
                response.Content = new ByteArrayContent(ms.ToArray());
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/gif");
            return response;

        }

    }
}
