using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web.UI;
using System;
using System.Web;
using BaiRong.Core;

namespace BaiRong.Pages
{
    public class ValidateCode2 : Page
    {
        private Bitmap validateimage;
        private Graphics g;

        protected void Page_Load(object sender, EventArgs e)
        {
            string cookieName = base.Request.QueryString["cookieName"];
            bool isCrossDomain = TranslateUtils.ToBool(base.Request.QueryString["isCrossDomain"]);

            string validateCode = ValidateCodeManager.CreateValidateCode(true);

            if (isCrossDomain)
            {
                DbCacheManager.Insert(cookieName, validateCode);
            }
            else
            {
                CookieUtils.SetCookie(cookieName, validateCode, DateTime.Now.AddDays(1));
            }

            Response.BufferOutput = true;  //�ر�ע��
            Response.Cache.SetExpires(DateTime.Now.AddMilliseconds(-1));//�ر�ע��
            Response.Cache.SetCacheability(HttpCacheability.NoCache);//�ر�ע��
            Response.AppendHeader("  Pragma", "No-Cache"); //�ر�ע��
            ValidateCode(validateCode);
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
            Response.ClearContent();
            Response.BinaryWrite(ms.ToArray());
            Response.End();
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
    }
}
