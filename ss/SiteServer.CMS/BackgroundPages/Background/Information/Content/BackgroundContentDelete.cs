using System;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Core;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Model;

using BaiRong.Core.Data.Provider;
using System.Text;

namespace SiteServer.CMS.BackgroundPages
{
    public class BackgroundContentDelete : BackgroundBasePage
    {
        public Literal ltlContents;
        public Control RetainRow;
        public RadioButtonList RetainFiles;
        public Button Submit;

        private Hashtable idsHashtable = new Hashtable();
        private bool isDeleteFromTrash = false;
        private string returnUrl;

        public static string GetRedirectClickStringForMultiChannels(int publishmentSystemID, bool isDeleteFromTrash, string returnUrl)
        {
            return JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_contentDelete.aspx?PublishmentSystemID={0}&IsDeleteFromTrash={1}&ReturnUrl={2}", publishmentSystemID, isDeleteFromTrash, StringUtils.ValueToUrl(returnUrl))), "IDsCollection", "IDsCollection", "请选择需要删除的内容！");
        }

        public static string GetRedirectClickStringForSingleChannel(int publishmentSystemID, int nodeID, bool isDeleteFromTrash, string returnUrl)
        {
            return JsUtils.GetRedirectStringWithCheckBoxValue(PageUtils.GetCMSUrl(string.Format("background_contentDelete.aspx?PublishmentSystemID={0}&NodeID={1}&IsDeleteFromTrash={2}&ReturnUrl={3}", publishmentSystemID, nodeID, isDeleteFromTrash, StringUtils.ValueToUrl(returnUrl))), "ContentIDCollection", "ContentIDCollection", "请选择需要删除的内容！");
        }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "ReturnUrl");
            this.returnUrl = StringUtils.ValueFromUrl(base.GetQueryString("ReturnUrl"));
            this.isDeleteFromTrash = TranslateUtils.ToBool(base.GetQueryString("IsDeleteFromTrash"));
            this.idsHashtable = ContentUtility.GetIDsHashtable(base.Request.QueryString);

            //if (this.nodeID > 0)
            //{
            //    this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, this.nodeID);
            //}
            //else
            //{
            //    this.nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, -this.nodeID);
            //}
            //if (this.nodeInfo != null)
            //{
            //    this.tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeInfo);
            //    this.tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeInfo);
            //}

            //if (this.contentID == 0)
            //{
            //    if (!base.HasChannelPermissions(Math.Abs(this.nodeID), AppManager.CMS.Permission.Channel.ContentDelete))
            //    {
            //        PageUtils.RedirectToErrorPage("您没有删除此栏目内容的权限！");
            //        return;
            //    }
            //}
            //else
            //{
            //    ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(this.tableStyle, this.tableName, this.contentID);

            //    if (contentInfo == null || !string.Equals(AdminManager.Current.UserName, contentInfo.AddUserName))
            //    {
            //        if (!base.HasChannelPermissions(Math.Abs(this.nodeID), AppManager.CMS.Permission.Channel.ContentDelete))
            //        {
            //            PageUtils.RedirectToErrorPage("您没有删除此栏目内容的权限！");
            //            return;
            //        }
            //    }
            //}

            if (!IsPostBack)
            {
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Content, "删除内容", string.Empty);

                StringBuilder builder = new StringBuilder();
                foreach (int nodeID in this.idsHashtable.Keys)
                {
                    ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                    string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                    ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;
                    foreach (int contentID in contentIDArrayList)
                    {
                        ContentInfo contentInfo = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);
                        if (contentInfo != null)
                        {
                            builder.AppendFormat(@"{0}<br />", WebUtils.GetContentTitle(base.PublishmentSystemInfo, contentInfo, this.returnUrl));
                        }
                    }
                }
                this.ltlContents.Text = builder.ToString();

                if (!this.isDeleteFromTrash)
                {
                    this.RetainRow.Visible = true;
                    base.InfoMessage(string.Format("此操作将把所选内容放入回收站，确定吗？"));
                }
                else
                {
                    this.RetainRow.Visible = false;
                    base.InfoMessage(string.Format("此操作将从回收站中彻底删除所选内容，确定吗？"));
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                try
                {
                    foreach (int nodeID in this.idsHashtable.Keys)
                    {
                        ETableStyle tableStyle = NodeManager.GetTableStyle(base.PublishmentSystemInfo, nodeID);
                        string tableName = NodeManager.GetTableName(base.PublishmentSystemInfo, nodeID);
                        ArrayList contentIDArrayList = this.idsHashtable[nodeID] as ArrayList;

                        if (!this.isDeleteFromTrash)
                        {
                            if (bool.Parse(this.RetainFiles.SelectedValue) == false)
                            {
                                DirectoryUtility.DeleteContents(base.PublishmentSystemInfo, nodeID, contentIDArrayList);
                                base.SuccessMessage("成功删除内容以及生成页面！");

                                #region 投稿删除 by 20160122 sofuny

                                foreach (int contentID in contentIDArrayList)
                                {
                                    ContentInfo info = DataProvider.ContentDAO.GetContentInfo(tableStyle, tableName, contentID);

                                    PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(info.PublishmentSystemID);
                                    //修改用户表中的投稿数量
                                    string memberName = info.MemberName;
                                    if (!string.IsNullOrEmpty(memberName))
                                    {
                                        UserInfo userInfo = UserManager.GetUserInfo(publishmentSystemInfo.GroupSN, memberName);
                                        if (userInfo.UserID > 0)
                                        {
                                            userInfo.MLibNum = userInfo.MLibNum - 1;
                                            BaiRongDataProvider.UserDAO.Update(userInfo);
                                        }
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                base.SuccessMessage("成功删除内容，生成页面未被删除！");
                            }

                            if (contentIDArrayList.Count == 1)
                            {
                                int contentID = (int)contentIDArrayList[0];
                                string contentTitle = BaiRongDataProvider.ContentDAO.GetValue(tableName, contentID, ContentAttribute.Title);
                                StringUtility.AddLog(base.PublishmentSystemID, nodeID, contentID, "删除内容", string.Format("栏目:{0},内容标题:{1}", NodeManager.GetNodeNameNavigation(this.PublishmentSystemID, nodeID), contentTitle));
                            }
                            else
                            {
                                StringUtility.AddLog(base.PublishmentSystemID, "批量删除内容", string.Format("栏目:{0},内容条数:{1}", NodeManager.GetNodeNameNavigation(this.PublishmentSystemID, nodeID), contentIDArrayList.Count));
                            }

                            DataProvider.ContentDAO.TrashContents(base.PublishmentSystemID, tableName, contentIDArrayList);

                            //引用内容，需要删除
                            ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent, EAuxiliaryTableType.JobContent, EAuxiliaryTableType.VoteContent, EAuxiliaryTableType.GoodsContent);
                            foreach (AuxiliaryTableInfo table in tableArrayList)
                            {
                                ArrayList targetContentIDList = BaiRongDataProvider.ContentDAO.GetReferenceIDArrayList(table.TableENName, contentIDArrayList);
                                if (targetContentIDList.Count > 0)
                                {
                                    ContentInfo targetContentInfo = DataProvider.ContentDAO.GetContentInfo(ETableStyleUtils.GetEnumType(table.AuxiliaryTableType.ToString()), table.TableENName, TranslateUtils.ToInt(targetContentIDList[0].ToString()));
                                    DataProvider.ContentDAO.DeleteContents(targetContentInfo.PublishmentSystemID, table.TableENName, targetContentIDList, targetContentInfo.NodeID);
                                }
                            }

                            string ajaxUrl = PageUtility.FSOAjaxUrl.GetUrlCreateImmediately(base.PublishmentSystemID, EChangedType.Delete, ETemplateType.ContentTemplate, nodeID, 0, 0);
                            AjaxUrlManager.AddAjaxUrl(ajaxUrl);
                        }
                        else
                        {
                            base.SuccessMessage("成功从回收站清空内容！");
                            DataProvider.ContentDAO.DeleteContents(base.PublishmentSystemID, tableName, contentIDArrayList, nodeID);

                            StringUtility.AddLog(base.PublishmentSystemID, "从回收站清空内容", string.Format("内容条数:{0}", contentIDArrayList.Count));
                        }

                    }


                    base.AddWaitAndRedirectScript(this.returnUrl);
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "删除内容失败！");

                    LogUtils.AddErrorLog(ex);
                }
            }
        }

        public void Return_OnClick(object sender, EventArgs E)
        {
            PageUtils.Redirect(this.returnUrl);
        }

    }
}
