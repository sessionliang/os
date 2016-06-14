using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;
using BaiRong.Controls;
using SiteServer.CMS.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using SiteServer.CMS.Core;
using System.Web.UI.HtmlControls;
using SiteServer.CMS.Core.Office;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Collections.Generic;
using BaiRong.Core.IO;

namespace SiteServer.WeiXin.BackgroundPages.Modal
{
    public class CardEntitySNImport : BackgroundBasePage
    { 
        public HtmlInputFile hifUpload;

        private int cardID;
       
        public static string GetOpenUploadWindowString(int publishmentSystemID, int cardID )
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("publishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("cardID", cardID.ToString());
            
            return PageUtilityWX.GetOpenWindowString("导入实体卡", "modal_cardEntitySNImport.aspx", arguments, 400, 300);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            this.cardID = TranslateUtils.ToInt(base.GetQueryString("cardID"));
           
            if (!IsPostBack)
            {
                
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            bool isChanged = false;

            try
            {
                string filePath = PathUtils.GetTemporaryFilesPath("cardsn-example.csv");
                FileUtils.DeleteFileIfExists(filePath);
                this.hifUpload.PostedFile.SaveAs(filePath);

                try
                { 
                    ArrayList cardEntitySNInfoArrayList = GetCardEntitySNInfoArrayList(filePath);
                    if (cardEntitySNInfoArrayList.Count > 0)
                    {
                        isChanged = true;
                        string errorMessage = string.Empty;
                        for (int i = 0; i < cardEntitySNInfoArrayList.Count; i++)
                        {
                            CardEntitySNInfo cardEntitySNInfo = cardEntitySNInfoArrayList[i] as CardEntitySNInfo;

                            bool isExist=DataProviderWX.CardEntitySNDAO.IsExist(base.PublishmentSystemID,this.cardID,cardEntitySNInfo.SN);
                            if (!isExist)
                            {
                                DataProviderWX.CardEntitySNDAO.Insert(cardEntitySNInfo);
                            }
                       }
                    }
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "失败：" + ex.Message);
                }

            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "失败：" + ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPage(Page);
            }
        }

        public ArrayList GetCardEntitySNInfoArrayList(string filePath)
        {
            int index = 0;
            ArrayList cardEntitySNInfoArrayList = new ArrayList();

            List<string> cardEntitySNInfoList = CSVUtils.ReadCSVFile(filePath);
            if (cardEntitySNInfoList.Count > 0)
            {
                foreach (string info in cardEntitySNInfoList)
                {
                    index++;
                    if (index == 1) continue;
                    if (string.IsNullOrEmpty(info)) continue;
                    string[] value = info.Split(',');

                    CardEntitySNInfo cardEntitySNInfo = new CardEntitySNInfo();

                    cardEntitySNInfo.PublishmentSystemID = base.PublishmentSystemID;
                    cardEntitySNInfo.CardID = this.cardID;
                    cardEntitySNInfo.SN = value[0];
                    cardEntitySNInfo.UserName = value[1];
                    cardEntitySNInfo.Mobile = value[2];
                    cardEntitySNInfo.Email = value[3];
                    cardEntitySNInfo.Address = value[4];
                    cardEntitySNInfo.Amount = TranslateUtils.ToDecimal(value[5]);
                    cardEntitySNInfo.Credits = TranslateUtils.ToInt(value[6]);
                    cardEntitySNInfo.IsBinding = false;
                    cardEntitySNInfo.AddDate = DateTime.Now;
                    cardEntitySNInfoArrayList.Add(cardEntitySNInfo);
                 }
             }
          return cardEntitySNInfoArrayList;
        }
   }

}
