using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;

using BaiRong.Core.Configuration;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.OracleClient;
using BaiRong.Core.Data;
using BaiRong.Core.Service;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System.Text;
using SiteServer.CMS.Core.Security;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundMLibManageScopeSite : BackgroundBasePage
    {
        public Literal ltlPublishmentSystems;
        public PlaceHolder phPublishmentSystem;

        public const string MLibPublishmentSystemArrayListKey = "MLibPublishmentSystemArrayListKey";

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            if (ConfigManager.Additional.IsUseMLib == false)
            {
                PageUtils.RedirectToErrorPage("投稿中心尚未开启.请在投稿设置启用投稿");
                return;
            }

            ArrayList hasInfoList = new ArrayList();

            if (base.GetQueryString("Return") == null)
            {
                ArrayList mLibScopeInfoList = DataProvider.MLibScopeDAO.GetInfoList();
                base.Session[MLibPublishmentSystemArrayListKey] = mLibScopeInfoList;

                hasInfoList = TranslateUtils.StringCollectionToArrayList(ConfigManager.Additional.MLibPublishmentSystemIDs);
            }

            if (!IsPostBack)
            {
                base.BreadCrumbForUserCenter(AppManager.User.LeftMenu.ID_MLibManage, "投稿范围设置", AppManager.User.Permission.Usercenter_MLibManageSetting);

                ArrayList mLibScopeInfoList = (ArrayList)base.Session[MLibPublishmentSystemArrayListKey];
                if (mLibScopeInfoList != null)
                {
                    ArrayList allPublishmentSystemIDArrayList = new ArrayList();
                    foreach (int itemForPSID in ProductPermissionsManager.Current.WebsitePermissionSortedList.Keys)
                    {
                        ArrayList arraylistOne = (ArrayList)ProductPermissionsManager.Current.ChannelPermissionSortedList[itemForPSID];
                        ArrayList arraylistTwo = (ArrayList)ProductPermissionsManager.Current.WebsitePermissionSortedList[itemForPSID];
                        if ((arraylistOne != null && arraylistOne.Count > 0) || (arraylistTwo != null && arraylistTwo.Count > 0))
                        {
                            PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(itemForPSID);
                            if (EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.CMS, publishmentSystemInfo.PublishmentSystemType) || EPublishmentSystemTypeUtils.Equals(EPublishmentSystemType.WCM, publishmentSystemInfo.PublishmentSystemType))
                                allPublishmentSystemIDArrayList.Add(itemForPSID);
                        }
                    }

                    ArrayList newmLibScopeInfoList = new ArrayList();
                    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoList)
                    {
                        foreach (int id in allPublishmentSystemIDArrayList)
                        {
                            if (mLibScopeInfo.PublishmentSystemID == id)
                            {
                                newmLibScopeInfoList.Add(mLibScopeInfo);
                            }
                        }
                        hasInfoList.Add(mLibScopeInfo.PublishmentSystemID);
                    }
                    base.Session[MLibPublishmentSystemArrayListKey] = newmLibScopeInfoList;
                    ltlPublishmentSystems.Text = this.GetPublishmentSystemsHtml(allPublishmentSystemIDArrayList, hasInfoList);
                }
                else
                {
                    PageUtils.RedirectToErrorPage("页面超时，请重新进入。");
                }
            }
        }


        public string GetPublishmentSystemsHtml(ArrayList allPublishmentSystemIDArrayList, ArrayList managedPublishmentSystemIDArrayList)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<table width='100%' cellpadding='4' cellspacing='0' border='0'>");
            int count = 1;
            foreach (int publishmentSystemID in allPublishmentSystemIDArrayList)
            {
                PublishmentSystemInfo psInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                string imageName = "cantedit";
                if (managedPublishmentSystemIDArrayList.Contains(publishmentSystemID))
                {
                    imageName = "canedit";
                }

                string space = "";
                if (count % 4 == 0)
                {
                    space = "<TR>";
                }

                string withRoleName = "";
                if (base.GetQueryString("RoleName") != null)
                {
                    withRoleName = "&RoleName=" + base.GetQueryString("RoleName");
                }

                string pageUrl = PageUtils.GetPlatformUrl(string.Format("background_mlibManageScope.aspx?PublishmentSystemID={0}{1}", publishmentSystemID, withRoleName));
                string content = string.Format(@"
					<td height=20>
                        <img id='PublishmentSystemImage_{0}' align='absmiddle' border='0' src='../pic/{1}.gif'/>
					    <a href='{2}'>{3}&nbsp;{4}</a>{5}
                    </td>
				", publishmentSystemID, imageName, pageUrl, psInfo.PublishmentSystemName, EPublishmentSystemTypeUtils.GetIconHtml(psInfo.PublishmentSystemType), space);
                htmlBuilder.Append(content);
                count++;
            }
            htmlBuilder.Append("</TABLE>");
            return htmlBuilder.ToString();
        }
        public override void Submit_OnClick(object sender, EventArgs E)
        {
            try
            {

                ArrayList mLibScopeInfoArrayList = (ArrayList)base.Session[MLibPublishmentSystemArrayListKey];
                if (mLibScopeInfoArrayList.Count > 0)
                {
                    ArrayList list = new ArrayList();
                    ArrayList nodelist = new ArrayList();
                    ArrayList newlist = new ArrayList();

                    foreach (MLibScopeInfo mLibScopeInfo in mLibScopeInfoArrayList)
                    {
                        if (!list.Contains(mLibScopeInfo.PublishmentSystemID))
                        {
                            list.Add(mLibScopeInfo.PublishmentSystemID);
                        }
                        if (!nodelist.Contains(mLibScopeInfo.NodeID))
                        {
                            nodelist.Add(mLibScopeInfo.NodeID);
                            newlist.Add(mLibScopeInfo);
                        }
                    }
                    DataProvider.MLibScopeDAO.Insert(newlist);

                    ConfigManager.Instance.Additional.MLibPublishmentSystemIDs = TranslateUtils.ObjectCollectionToSqlInStringWithoutQuote(list);
                }
                else
                {
                    base.FailMessage("保存失败：请设置投稿范围");
                    return;
                }

                BaiRongDataProvider.ConfigDAO.Update(ConfigManager.Instance);

                LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "投稿范围设置");
                base.SuccessMessage("投稿范围设置成功");
            }
            catch (Exception ex)
            {
                base.FailMessage(ex, ex.Message);
            }
        }
    }
}
