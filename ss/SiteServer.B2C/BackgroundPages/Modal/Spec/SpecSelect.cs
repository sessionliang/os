using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.B2C.Core;
using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using System.Collections.Specialized;
using System.Collections.Generic;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.BackgroundPages;

namespace SiteServer.B2C.BackgroundPages.Modal
{
    public class SpecSelect : BackgroundBasePage
    {
        public Literal ltlSpecItems;
        public Repeater rptSelected;

        private NodeInfo nodeInfo;
        private int contentID;
        private string sn;
        private SpecInfo specInfo;
        private int selectedCount;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int nodeID, int contentID, string sn, int specID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("NodeID", nodeID.ToString());
            arguments.Add("ContentID", contentID.ToString());
            arguments.Add("SN", sn);
            arguments.Add("specID", specID.ToString());
            arguments.Add("ReturnUrl", returnUrl);
            return PageUtilityB2C.GetOpenWindowString("设置规格项", "modal_specSelect.aspx", arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            int nodeID = base.GetIntQueryString("NodeID");
            this.contentID = base.GetIntQueryString("ContentID");
            this.sn = base.GetQueryString("SN");
            int specID = base.GetIntQueryString("specID");
            this.returnUrl = base.GetQueryString("ReturnUrl");

            this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, nodeID);
            this.specInfo = SpecManager.GetSpecInfo(base.PublishmentSystemID, specID);

            if (!IsPostBack)
            {
                ArrayList specItemIDArrayList = DataProviderB2C.SpecComboDAO.GetSpecItemIDArrayList(base.PublishmentSystemID, this.contentID, this.specInfo.SpecID);
                this.selectedCount = specItemIDArrayList.Count;
                this.ltlSpecItems.Text = SpecManager.GetSpecValuesToAdd(base.PublishmentSystemInfo, this.contentID, this.specInfo.SpecID, specItemIDArrayList);

                this.rptSelected.DataSource = DataProviderB2C.SpecComboDAO.GetDataSource(base.PublishmentSystemID, this.contentID, this.specInfo.SpecID);
                this.rptSelected.DataBind();
            }
        }

        public string ParseIconUrl(string iconUrl)
        {
            return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, iconUrl);
        }

        public string ParsePhotoIDCollection(string photoIDCollection)
        {
            if (!string.IsNullOrEmpty(photoIDCollection))
            {
                return photoIDCollection.Replace(',', '_');
            }
            return string.Empty;
        }

        public string ParsePhotoIDCollectionHTML(string photoIDCollection)
        {
            string imgHTML = string.Empty;
            if (!string.IsNullOrEmpty(photoIDCollection))
            {
                ArrayList photoIDArrayList = TranslateUtils.StringCollectionToIntArrayList(photoIDCollection);
                foreach (int photoID in photoIDArrayList)
                {
                    PhotoInfo photoInfo = DataProvider.PhotoDAO.GetPhotoInfo(photoID);
                    if (photoInfo != null)
                    {
                        imgHTML += string.Format("<img src='{0}' />", PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, photoInfo.SmallUrl));
                    }
                }
                if (!string.IsNullOrEmpty(imgHTML))
                {
                    imgHTML += "<br />";
                }
            }
            return imgHTML;
        }

        public string GetIsIcon()
        {
            return this.specInfo.IsIcon.ToString().ToLower();
        }

        public string GetSpecName()
        {
            return this.specInfo.SpecName;
        }

        public string GetSelectedCount()
        {
            return this.selectedCount.ToString();
        }

        public string GetSystemSpecItemHTML(int itemID)
        {
            SpecItemInfo itemInfo = SpecItemManager.GetSpecItemInfo(base.PublishmentSystemID, itemID);
            if (itemInfo != null)
            {
                if (!string.IsNullOrEmpty(itemInfo.IconUrl))
                {
                    return string.Format(@"<img alt=""{0}"" src=""{1}"" />", itemInfo.Title, PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, itemInfo.IconUrl));
                }
                else
                {
                    return itemInfo.Title;
                }
            }
            return string.Empty;
        }

        public string GetSystemSpecItemIconUrl(string iconUrl, int itemID)
        {
            if (!string.IsNullOrEmpty(iconUrl))
            {
                return iconUrl;
            }
            else
            {
                SpecItemInfo itemInfo = SpecItemManager.GetSpecItemInfo(base.PublishmentSystemID, itemID);
                if (itemInfo != null)
                {
                    if (!string.IsNullOrEmpty(itemInfo.IconUrl))
                    {
                        return itemInfo.IconUrl;
                    }
                }
            }
            return string.Empty;
        }

        public string GetIconClickString(string inputClientID, string imgClientID, string originalUrl)
        {
            return Modal.UploadOrUrlImage.GetOpenWindowString(base.PublishmentSystemID, inputClientID, imgClientID, originalUrl);
        }

        public string GetSelectPhotosClickString(string inputClientID, string photosClientID)
        {
            return Modal.GoodsPhotoSelect.GetOpenWindowString(base.PublishmentSystemID, this.contentID, inputClientID, photosClientID);
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            bool isChanged = false;

            ArrayList itemIDArrayList = TranslateUtils.StringCollectionToIntArrayList(base.Request.Form["selectedItemID"]);
            ArrayList iconUrlArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["selectedIconUrl"]);
            ArrayList photoIDCollectionArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["selectedPhotoIDCollection"]);

            ArrayList isHideArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["selectedIsHide"]);
            ArrayList titleArrayList = TranslateUtils.StringCollectionToArrayList(base.Request.Form["selectedTitle"]);

            int count = itemIDArrayList.Count;
            if (iconUrlArrayList.Count < count)
            {
                for (int i = 0; i < count - iconUrlArrayList.Count; i++)
                {
                    iconUrlArrayList.Add(string.Empty);
                }
            }
            if (photoIDCollectionArrayList.Count < count)
            {
                for (int i = 0; i < count - photoIDCollectionArrayList.Count; i++)
                {
                    photoIDCollectionArrayList.Add(string.Empty);
                }
            }
            if (isHideArrayList.Count < count)
            {
                for (int i = 0; i < count - isHideArrayList.Count; i++)
                {
                    isHideArrayList.Add(string.Empty);
                }
            }
            if (titleArrayList.Count < count)
            {
                for (int i = 0; i < count - titleArrayList.Count; i++)
                {
                    titleArrayList.Add(string.Empty);
                }
            }

            try
            {
                DataProviderB2C.SpecComboDAO.Delete(base.PublishmentSystemID, this.contentID, this.specInfo.SpecID);

                ArrayList comboInfoArrayList = new ArrayList();
                ArrayList selectedItemIDArrayList = new ArrayList();
                List<SpecComboInfo> comboInfoList = new List<SpecComboInfo>();
                for (int i = 0; i < itemIDArrayList.Count; i++)
                {
                    if (!TranslateUtils.ToBool(isHideArrayList[i] as string))
                    {
                        int itemID = (int)itemIDArrayList[i];
                        string title = (string)titleArrayList[i];

                        SpecComboInfo comboInfo = new SpecComboInfo(0, base.PublishmentSystemID, this.contentID, this.specInfo.SpecID, itemID, title, string.Empty, string.Empty, 0);

                        if (this.specInfo.IsIcon)
                        {
                            comboInfo.IconUrl = (string)iconUrlArrayList[i];
                            string photoIDCollection = (string)photoIDCollectionArrayList[i];
                            if (!string.IsNullOrEmpty(photoIDCollection))
                            {
                                photoIDCollection = photoIDCollection.Replace("_", ",");
                            }
                            comboInfo.PhotoIDCollection = photoIDCollection;
                        }

                        selectedItemIDArrayList.Add(itemID);

                        DataProviderB2C.SpecComboDAO.Insert(comboInfo);
                        comboInfoList.Add(comboInfo);
                    }
                }

                //string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, this.nodeInfo);

                //List<int> specIDList = new List<int>();
                //List<int> specItemIDList = new List<int>();

                //DataProvider.SpecComboDAO.GetContentSpec(base.PublishmentSystemID, this.contentID, out specIDList, out specItemIDList);

                //DataProvider.GoodsContentDAO.UpdateSpec(tableName, contentID, specIDList, specItemIDList);

                isChanged = true;

                #region 更新引用该商品内容的规格
                //更新引用该商品内容的规格
                ArrayList sourceContentIDList = new ArrayList();
                sourceContentIDList.Add(contentID);
                ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
                foreach (AuxiliaryTableInfo table in tableArrayList)
                {
                    ArrayList targetContentIDList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(table.TableENName, sourceContentIDList);
                    foreach (int targetContentID in targetContentIDList)
                    {
                        GoodsContentInfo targetGoodsContentInfo = DataProviderB2C.GoodsContentDAO.GetContentInfo(table.TableENName, targetContentID);
                        List<SpecInfo> targetSpecInfoList = DataProviderB2C.SpecDAO.GetSpecInfoList(targetGoodsContentInfo.PublishmentSystemID, targetGoodsContentInfo.NodeID);
                        Dictionary<int, int> comboIDDictionary = new Dictionary<int, int>();
                        foreach (SpecInfo targetSpecInfo in targetSpecInfoList)
                        {
                            if (targetSpecInfo.SpecName != specInfo.SpecName)
                            {
                                continue;
                            }

                            DataProviderB2C.SpecComboDAO.Delete(targetGoodsContentInfo.PublishmentSystemID, targetContentID, targetSpecInfo.SpecID);
                            foreach (SpecComboInfo comboInfo in comboInfoList)
                            {
                                comboInfo.PublishmentSystemID = targetGoodsContentInfo.PublishmentSystemID;
                                comboInfo.ContentID = targetContentID;
                                comboInfo.SpecID = targetSpecInfo.SpecID;
                                int targetComboID = DataProviderB2C.SpecComboDAO.Insert(comboInfo);
                                comboIDDictionary[comboInfo.ComboID] = targetComboID;
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, "设置规格项失败！");
            }

            if (isChanged)
            {
                string redirectUrl = BackgroundSpecContent.GetRedirectUrl(base.PublishmentSystemID, this.nodeInfo.NodeID, this.contentID, this.sn, this.returnUrl);
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, redirectUrl);
            }
        }
    }
}
