﻿using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Drawing;
using SiteServer.CMS.Core;
using BaiRong.Model;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    //http://www.codeproject.com/Articles/83225/A-Simple-JPEG-Encoder-in-C
	public class CuttingImage : BackgroundBasePage
	{
        public Literal ltlScript;

		private string textBoxClientID;
        private string imageUrl;

        protected override bool IsSinglePage { get { return true; } }

        public static string GetOpenWindowStringWithTextBox(int publishmentSystemID, string textBoxClientID)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("textBoxClientID", textBoxClientID);
            return PageUtility.GetOpenWindowString("裁切图片", "modal_cuttingImage.aspx", arguments);
        }

        public static string GetOpenWindowStringToImageUrl(int publishmentSystemID, string imageUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("imageUrl", imageUrl);
            return PageUtility.GetOpenWindowString("裁切图片", "modal_cuttingImage.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.textBoxClientID = base.GetQueryString("TextBoxClientID");
            this.imageUrl = base.GetQueryString("imageUrl");

            if (!IsPostBack)
            {
                string virtualUrl = string.Empty;

                if (!string.IsNullOrEmpty(this.textBoxClientID))
                {
                    virtualUrl = string.Format("window.parent.document.getElementById('{0}').value", textBoxClientID);
                }
                else if (!string.IsNullOrEmpty(this.imageUrl))
                {
                    virtualUrl = "'" + this.imageUrl + "'";
                }

                this.ltlScript.Text = string.Format(@"
var rootUrl = '{0}';
var publishmentSystemUrl = '{1}';
var virtualUrl = {2};
var imageUrl = virtualUrl;
if(imageUrl && imageUrl.search(/\.bmp|\.jpg|\.jpeg|\.gif|\.png$/i) != -1){{
	if (imageUrl.charAt(0) == '~'){{
		imageUrl = imageUrl.replace('~', rootUrl);
	}}else if (imageUrl.charAt(0) == '@'){{
		imageUrl = imageUrl.replace('@', publishmentSystemUrl);
	}}
	if(imageUrl.substr(0,2)=='//'){{
		imageUrl = imageUrl.replace('//', '/');
	}}
}}
", PageUtils.GetRootUrl(string.Empty), PageUtility.GetPublishmentSystemUrl(base.PublishmentSystemInfo, string.Empty, true), virtualUrl);
            }
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            try
            {
                int rotate = TranslateUtils.ToIntWithNagetive(base.Request.Form["rotate"]);
                rotate = rotate % 4;
                string flip = base.Request.Form["flip"];
                string fileUrl = base.Request.Form["fileUrl"];
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    string filePath = PathUtility.MapPath(base.PublishmentSystemInfo, fileUrl);
                    if (FileUtils.IsFileExists(filePath))
                    {
                        string destImagePath = filePath.Substring(0, filePath.LastIndexOf('.')) + "_c" + filePath.Substring(filePath.LastIndexOf('.'));

                        if (rotate == 0 && string.IsNullOrEmpty(flip))
                        {
                            int x1 = TranslateUtils.ToIntWithNagetive(base.Request.Form["x1"]);
                            int y1 = TranslateUtils.ToIntWithNagetive(base.Request.Form["y1"]);
                            int w = TranslateUtils.ToIntWithNagetive(base.Request.Form["w"]);
                            int h = TranslateUtils.ToIntWithNagetive(base.Request.Form["h"]);

                            if (w > 0 && h > 0)
                            {
                                ImageUtils.CropImage(filePath, destImagePath, x1, y1, w, h);
                            }
                        }
                        else if (rotate != 0)
                        {
                            if (rotate == 1 || rotate == -3)
                            {
                                ImageUtils.RotateFlipImage(filePath, destImagePath, System.Drawing.RotateFlipType.Rotate90FlipNone);
                            }
                            else if (rotate == 2 || rotate == -2)
                            {
                                ImageUtils.RotateFlipImage(filePath, destImagePath, System.Drawing.RotateFlipType.Rotate180FlipNone);
                            }
                            else if (rotate == 3 || rotate == -1)
                            {
                                ImageUtils.RotateFlipImage(filePath, destImagePath, System.Drawing.RotateFlipType.Rotate270FlipNone);
                            }
                        }
                        else if (!string.IsNullOrEmpty(flip))
                        {
                            if (flip == "H")
                            {
                                ImageUtils.RotateFlipImage(filePath, destImagePath, System.Drawing.RotateFlipType.RotateNoneFlipX);
                            }
                            else if (flip == "V")
                            {
                                ImageUtils.RotateFlipImage(filePath, destImagePath, System.Drawing.RotateFlipType.RotateNoneFlipY);
                            }
                        }

                        string destUrl = PageUtility.GetVirtualUrl(base.PublishmentSystemInfo, PageUtility.GetPublishmentSystemUrlByPhysicalPath(base.PublishmentSystemInfo, destImagePath));

                        if (!string.IsNullOrEmpty(this.textBoxClientID))
                        {
                            this.ltlScript.Text = string.Format(@"
if (parent.document.getElementById('{0}'))
{{
    parent.document.getElementById('{0}').value = '{1}';
}}
", this.textBoxClientID, destUrl);
                        }
                        else
                        {
                            FileUtils.CopyFile(destImagePath, filePath, true);
                        }

                        this.ltlScript.Text += JsUtils.OpenWindow.HIDE_POP_WIN;
                    }
                }
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
		}
	}
}
