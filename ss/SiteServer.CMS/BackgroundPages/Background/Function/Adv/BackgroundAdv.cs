using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;

using BaiRong.Model;


namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundAdv : BackgroundBasePage
    {
        public DropDownList AdAreaNameList;
        public DataGrid dgContents;

        public Button AddAdv;
        public Button Delete;

        private int adAreaID = 0;
        public string GetAdAreaName(int adAreaID)
        {
            string adAreaName=string.Empty;
            AdAreaInfo adAreaInfo = DataProvider.AdAreaDAO.GetAdAreaInfo(adAreaID,base.PublishmentSystemID);
            if (adAreaInfo != null)
            {
                adAreaName = adAreaInfo.AdAreaName;
            }
            return adAreaName;
         }

        public string GetIsEnabled(string isEnabledStr)
        {
            return StringUtils.GetTrueOrFalseImageHtml(isEnabledStr);
        }

        public string GetAdvInString(int advID)
        {
            StringBuilder builder = new StringBuilder();
            AdvInfo adInfo = DataProvider.AdvDAO.GetAdvInfo(advID, base.PublishmentSystemID);
            if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToChannel))
            {
                builder.Append("栏目：");
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToChannel);
                foreach (int nodeID in nodeIDArrayList)
                {
                    builder.Append(NodeManager.GetNodeName(base.PublishmentSystemID, nodeID));
                    builder.Append(",");
                }
                builder.Length--;
            }
            if (!string.IsNullOrEmpty(adInfo.NodeIDCollectionToContent))
            {
                if (builder.Length > 0)
                {
                    builder.Append("<br />");
                }
                builder.Append("内容：");
                ArrayList nodeIDArrayList = TranslateUtils.StringCollectionToIntArrayList(adInfo.NodeIDCollectionToContent);
                foreach (int nodeID in nodeIDArrayList)
                {
                    builder.Append(NodeManager.GetNodeName(base.PublishmentSystemID, nodeID));
                    builder.Append(",");
                }
                builder.Length--;
            }
            
            return builder.ToString();
        }

        public void Page_Load(object sender, EventArgs E)
        {
            PageUtils.CheckRequestParameter("PublishmentSystemID");
            this.adAreaID =TranslateUtils.ToInt(base.GetQueryString("AdAreaID"));

            if (base.GetQueryString("Delete") != null)
            {
                int advID = TranslateUtils.ToInt(base.GetQueryString("AdvID"));
                try
                {
                    DataProvider.AdvDAO.Delete(advID, base.PublishmentSystemID);

                     StringUtility.AddLog(base.PublishmentSystemID, "删除广告", string.Format("广告名称：{0}","" ));

                     base.SuccessDeleteMessage();
                }
                catch (Exception ex)
                {
                    base.FailDeleteMessage(ex);
                }
            }
         
            if (!Page.IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, "固定广告管理", AppManager.CMS.Permission.WebSite.Advertisement);
                ArrayList adAreaInfoArrayList = DataProvider.AdAreaDAO.GetAdAreaInfoArrayList(base.PublishmentSystemID);
                this.AdAreaNameList.Items.Add(new ListItem("<<所有广告位>>",string.Empty));
                if (adAreaInfoArrayList.Count > 0)
                {
                    foreach (AdAreaInfo adAreaInfo in adAreaInfoArrayList)
                    {
                        this.AdAreaNameList.Items.Add(new ListItem(adAreaInfo.AdAreaName,adAreaInfo.AdAreaID.ToString()));
                    }
                }
                this.Delete.Attributes.Add("onclick", "return confirm(\"此操作将删除所选广告，确定吗？\");");
               
                if (this.adAreaID > 0)
                {
                    this.AdAreaNameList.SelectedValue = this.adAreaID.ToString();
                }
            
            }
             BindGrid();

        }

        public void BindGrid()
        {
            try
            {
                if (string.IsNullOrEmpty(this.AdAreaNameList.SelectedValue))
                {
                    this.dgContents.DataSource = DataProvider.AdvDAO.GetDataSource(base.PublishmentSystemID);
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.AdAreaNameList.SelectedValue))
                    {
                        this.adAreaID = TranslateUtils.ToInt(this.AdAreaNameList.SelectedValue);
                    }

                    this.dgContents.DataSource = DataProvider.AdvDAO.GetDataSourceByAdAreaID(TranslateUtils.ToInt(this.AdAreaNameList.SelectedValue), base.PublishmentSystemID);
                }
                this.dgContents.DataBind();
            }
            catch (Exception ex)
            {
                PageUtils.RedirectToErrorPage(ex.Message);
            }
        }

        public void ReFresh(object sender, EventArgs E)
        {
            BindGrid();
        }


        public void AddAdv_OnClick(object sender, EventArgs E)
        {
            if(string.IsNullOrEmpty(this.AdAreaNameList.SelectedValue))
            {
                base.FailMessage("请选择广告位后进行操作！");
                return;
            }
            PageUtils.Redirect(PageUtils.GetCMSUrl(string.Format("background_advAdd.aspx?PublishmentSystemID={0}&AdAreaID={1}", base.PublishmentSystemID,this.adAreaID)));
        }

        public void Delete_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (Request.Form["AdvIDCollection"] != null)
                {
                    ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(Request.Form["AdvIDCollection"]);
                    try
                    {
                        foreach (string advID in arraylist)
                        {
                            DataProvider.AdvDAO.Delete(TranslateUtils.ToInt(advID), base.PublishmentSystemID);
                        }

                        StringUtility.AddLog(base.PublishmentSystemID, "删除广告", string.Format("广告名称：{0}", Request.Form["AdvIDCollection"]));

                        base.SuccessDeleteMessage();
                    }
                    catch (Exception ex)
                    {
                        base.FailDeleteMessage(ex);
                    }
                    BindGrid();
                }
                else
                {
                    base.FailMessage("请选择广告后进行操作！");
                }
            }
        }
    }
}
