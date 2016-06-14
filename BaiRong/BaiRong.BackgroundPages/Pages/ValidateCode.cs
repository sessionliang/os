using System;
using System.Drawing;
using System.Drawing.Imaging;
using BaiRong.Core;
using System.Web.UI;

namespace BaiRong.Pages
{
    public class ValidateCode : Page
    {
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

            // ����BITMAPͼ��
            Bitmap bitmap = new Bitmap(imgWidth, imgHeight);

            // ��ͼ�����ø���
            DisturbBitmap(bitmap);

            // ������֤��ͼ��
            DrawValidateCode(bitmap, validateCode);

            // ������֤��ͼ�񣬵ȴ����
            bitmap.Save(Response.OutputStream, ImageFormat.Gif);
        }

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
    }
}
