using BaiRong.Core;
using SiteServer.WeiXin.BackgroundPages;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SiteServer.WeiXin.BackgroundPages
{
    public class BackgroundService : BackgroundBasePageWX
    {
        public const string TYPE_GetLoadingCategorys = "GetLoadingCategorys";

        public static string GetRedirectUrl(int publishmentSystemID, string type)
        {
            return PageUtils.GetWXUrl(string.Format("background_service.aspx?PublishmentSystemID={0}&type={1}", publishmentSystemID, type));
        }

        public void Page_Load(object sender, System.EventArgs e)
        {
            string type = base.Request.QueryString["type"];

            NameValueCollection retval = new NameValueCollection();
            string retString = string.Empty;

            if (type == TYPE_GetLoadingCategorys)
            {
                int parentID = TranslateUtils.ToInt(base.Request["parentID"]);
                string loadingType = base.Request["loadingType"];
                string additional = base.Request["additional"];
                retString = GetLoadingCategorys(parentID, loadingType, additional);
            }

            if (!string.IsNullOrEmpty(retString))
            {
                Page.Response.Write(retString);
                Page.Response.End();
            }
            else
            {
                string jsonString = TranslateUtils.NameValueCollectionToJsonString(retval);
                Page.Response.Write(jsonString);
                Page.Response.End();
            }
        }

        public string GetLoadingCategorys(int parentID, string loadingType, string additional)
        {
            ArrayList arraylist = new ArrayList();

            ECategoryLoadingType eLoadingType = ECategoryLoadingTypeUtils.GetEnumType(loadingType);

            List<int> categoryIDList = DataProviderWX.StoreCategoryDAO.GetCategoryIDListByParentID(base.PublishmentSystemID, parentID);
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(RuntimeUtils.DecryptStringByTranslate(additional));
            ArrayList allCategoryIDArrayList = new ArrayList();
            if (!string.IsNullOrEmpty(nameValueCollection["CategoryIDCollection"]))
            {
                allCategoryIDArrayList = TranslateUtils.StringCollectionToIntArrayList(nameValueCollection["CategoryIDCollection"]);
                nameValueCollection.Remove("CategoryIDCollection");
                foreach (int categotyID in categoryIDList)
                {
                    StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(categotyID);
                    if (categoryInfo.ParentID != 0 || allCategoryIDArrayList.Contains(categotyID))
                    {
                        arraylist.Add(BackgroundStoreCategory.GetCategoryRowHtml(base.PublishmentSystemID, categoryInfo, eLoadingType, nameValueCollection));
                    }
                }
            }
            else
            {
                foreach (int categotyID in categoryIDList)
                {
                    StoreCategoryInfo categoryInfo = DataProviderWX.StoreCategoryDAO.GetCategoryInfo(categotyID);
                    arraylist.Add(BackgroundStoreCategory.GetCategoryRowHtml(base.PublishmentSystemID, categoryInfo, eLoadingType, nameValueCollection));
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (string html in arraylist)
            {
                builder.Append(html);
            }
            return builder.ToString();
        }
    }
}
