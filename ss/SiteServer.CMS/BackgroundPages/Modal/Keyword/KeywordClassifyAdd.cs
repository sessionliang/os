using System;
using System.Collections;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core.Security;
using System.Collections.Specialized;
using SiteServer.CMS.Controls;

namespace SiteServer.CMS.BackgroundPages.Modal
{
    public class KeywordClassifyAdd : BackgroundBasePage
    {
        public HtmlControl divSelectKeywordClassify;
        public Literal ltlSelectKeywordClassifyScript;

        public DropDownList ParentItemID;

        public TextBox ItemNames;

        private int itemID;
        private string returnUrl;

        public static string GetOpenWindowString(int publishmentSystemID, int itemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ItemID", itemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtility.GetOpenWindowString("添加分类", "modal_keywordClassifyAdd.aspx", arguments);
        }

        public static string GetRedirectUrl(int publishmentSystemID, int itemID, string returnUrl)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("ItemID", itemID.ToString());
            arguments.Add("ReturnUrl", StringUtils.ValueToUrl(returnUrl));
            return PageUtils.AddQueryString(PageUtils.GetCMSUrl("modal_keywordClassifyAdd.aspx"), arguments);
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ItemID", "ReturnUrl");

            this.itemID = base.GetIntQueryString("ItemID");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));

            if (!IsPostBack)
            {

                TreeManager.AddListItems(this.ParentItemID.Items, base.PublishmentSystemID, this.itemID, true, true, "KeywordClassify");
                ControlUtils.SelectListItems(this.ParentItemID, this.itemID.ToString());

            }
        }

        public override void Submit_OnClick(object sender, System.EventArgs e)
        {
            bool isChanged = false;
            int parentItemID = TranslateUtils.ToInt(base.Request.Form["parentItemID"]);

            try
            {
                if (string.IsNullOrEmpty(this.ItemNames.Text))
                {
                    base.FailMessage("请填写需要添加的分类名称");
                    return;
                }

                Hashtable insertedItemIDHashtable = new Hashtable();//key为分类的级别，1为第一级分类
                insertedItemIDHashtable[1] = parentItemID;
                ArrayList itemIndexNameList = null;
                string[] itemNameArray = this.ItemNames.Text.Split('\n');
                foreach (string item in itemNameArray)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        //count为分类的级别
                        int count = (StringUtils.GetStartCount('－', item) == 0) ? StringUtils.GetStartCount('-', item) : StringUtils.GetStartCount('－', item);
                        string itemName = item.Substring(count, item.Length - count);
                        string itemIndex = string.Empty;
                        count++;

                        if (!string.IsNullOrEmpty(itemName) && insertedItemIDHashtable.Contains(count))
                        {

                            if (StringUtils.Contains(itemName, "(") && StringUtils.Contains(itemName, ")"))
                            {
                                int length = itemName.IndexOf(')') - itemName.IndexOf('(');
                                if (length > 0)
                                {
                                    itemIndex = itemName.Substring(itemName.IndexOf('(') + 1, length);
                                    itemName = itemName.Substring(0, itemName.IndexOf('('));
                                }
                            }
                            itemName = itemName.Trim();
                            itemIndex = itemIndex.TrimEnd(')');
                            int parentID = (int)insertedItemIDHashtable[count];

                            if (!string.IsNullOrEmpty(itemIndex))
                            {
                                if (itemIndexNameList == null)
                                {
                                    itemIndexNameList = DataProvider.KeywordClassifyDAO.GetItemIndexNameArrayList(base.PublishmentSystemID);
                                }
                                if (itemIndexNameList.IndexOf(itemIndex) != -1)
                                {
                                    itemIndex = string.Empty;
                                }
                                else
                                {
                                    itemIndexNameList.Add(itemIndex);
                                }
                            }

                            int insertedItemID = DataProvider.KeywordClassifyDAO.InsertKeywordClassifyInfo(base.PublishmentSystemID, parentID, itemName, itemIndex);
                            insertedItemIDHashtable[count + 1] = insertedItemID;

                        }
                    }
                }

                isChanged = true;
            }
            catch (Exception ex)
            {
                isChanged = false;
                base.FailMessage(ex, ex.Message);
            }

            if (isChanged)
            {
                JsUtils.OpenWindow.CloseModalPageAndRedirect(Page, this.returnUrl);
            }
        }

        public void ParentItemID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theItemID = TranslateUtils.ToInt(this.ParentItemID.SelectedValue);
            if (theItemID == 0)
            {
                theItemID = this.itemID;
            }
            PageUtils.Redirect(KeywordClassifyAdd.GetRedirectUrl(base.PublishmentSystemID, theItemID, base.GetQueryString("ReturnUrl")));
        }
    }
}
