using System;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class GatherFileSet : BackgroundBasePage
	{		
        public TextBox GatherUrl;

        public PlaceHolder PlaceHolder_File;
        public TextBox FilePath;
        public RadioButtonList IsSaveRelatedFiles;
        public RadioButtonList IsRemoveScripts;
        public PlaceHolder PlaceHolder_File_Directory;
        public TextBox StyleDirectoryPath;
        public TextBox ScriptDirectoryPath;
        public TextBox ImageDirectoryPath;

        public PlaceHolder PlaceHolder_Content;
		protected DropDownList NodeIDDropDownList;
        public RadioButtonList IsSaveImage;

		private string gatherRuleName;

        public static string GetOpenWindowString(int publishmentSystemID, string gatherRuleName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherRuleName", gatherRuleName);
            return PageUtility.GetOpenWindowString("信息采集", "modal_gatherFileSet.aspx", arguments);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "GatherRuleName");

            this.gatherRuleName = base.GetQueryString("GatherRuleName");

			if (!IsPostBack)
			{
                base.InfoMessage("采集名称：" + this.gatherRuleName);

                GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.gatherRuleName, base.PublishmentSystemID);
                this.GatherUrl.Text = gatherFileRuleInfo.GatherUrl;
                if (gatherFileRuleInfo.IsToFile)
                {
                    this.PlaceHolder_File.Visible = true;
                    this.PlaceHolder_Content.Visible = false;

                    this.FilePath.Text = gatherFileRuleInfo.FilePath;
                    ControlUtils.SelectListItems(this.IsSaveRelatedFiles, gatherFileRuleInfo.IsSaveRelatedFiles.ToString());
                    ControlUtils.SelectListItems(this.IsRemoveScripts, gatherFileRuleInfo.IsRemoveScripts.ToString());
                    this.StyleDirectoryPath.Text = gatherFileRuleInfo.StyleDirectoryPath;
                    this.ScriptDirectoryPath.Text = gatherFileRuleInfo.ScriptDirectoryPath;
                    this.ImageDirectoryPath.Text = gatherFileRuleInfo.ImageDirectoryPath;
                }
                else
                {
                    this.PlaceHolder_File.Visible = false;
                    this.PlaceHolder_Content.Visible = true;

                    NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);
                    ControlUtils.SelectListItems(this.NodeIDDropDownList, gatherFileRuleInfo.NodeID.ToString());
                    ControlUtils.SelectListItems(this.IsSaveImage, gatherFileRuleInfo.IsSaveImage.ToString());
                }

				
			}
		}

        public void DropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.PlaceHolder_File.Visible)
            {
                this.PlaceHolder_File_Directory.Visible = TranslateUtils.ToBool(this.IsSaveRelatedFiles.SelectedValue);
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.GatherUrl.Text))
            {
                base.FailMessage("必须填写采集网页地址！");
                return;
            }

            if (this.PlaceHolder_File.Visible)
            {
                if (string.IsNullOrEmpty(this.FilePath.Text))
                {
                    base.FailMessage("必须填写采集到的文件地址！");
                    return;
                }
                else
                {
                    bool isOk = false;
                    if (StringUtils.StringStartsWith(this.FilePath.Text, '~') || StringUtils.StringStartsWith(this.FilePath.Text, '@'))
                    {
                        if (!PathUtils.IsDirectoryPath(this.FilePath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        base.FailMessage("采集到的文件地址不正确,必须填写有效的文件地址！");
                        return;
                    }
                }

                if (TranslateUtils.ToBool(this.IsSaveRelatedFiles.SelectedValue))
                {
                    bool isOk = false;
                    if (StringUtils.StringStartsWith(this.StyleDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.StyleDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.StyleDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        base.FailMessage("CSS样式保存地址不正确,必须填写有效的文件夹地址！");
                        return;
                    }
                    isOk = false;
                    if (StringUtils.StringStartsWith(this.ScriptDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.ScriptDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.ScriptDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        base.FailMessage("Js脚本保存地址不正确,必须填写有效的文件夹地址！");
                        return;
                    }
                    isOk = false;
                    if (StringUtils.StringStartsWith(this.ImageDirectoryPath.Text, '~') || StringUtils.StringStartsWith(this.ImageDirectoryPath.Text, '@'))
                    {
                        if (PathUtils.IsDirectoryPath(this.ImageDirectoryPath.Text))
                        {
                            isOk = true;
                        }
                    }
                    if (isOk == false)
                    {
                        base.FailMessage("图片保存地址不正确,必须填写有效的文件夹地址！");
                        return;
                    }
                }
            }

            GatherFileRuleInfo gatherFileRuleInfo = DataProvider.GatherFileRuleDAO.GetGatherFileRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

            gatherFileRuleInfo.GatherUrl = this.GatherUrl.Text;
            gatherFileRuleInfo.FilePath = this.FilePath.Text;
            gatherFileRuleInfo.IsSaveRelatedFiles = TranslateUtils.ToBool(this.IsSaveRelatedFiles.SelectedValue);
            gatherFileRuleInfo.IsRemoveScripts = TranslateUtils.ToBool(this.IsRemoveScripts.SelectedValue);
            gatherFileRuleInfo.StyleDirectoryPath = this.StyleDirectoryPath.Text;
            gatherFileRuleInfo.ScriptDirectoryPath = this.ScriptDirectoryPath.Text;
            gatherFileRuleInfo.ImageDirectoryPath = this.ImageDirectoryPath.Text;
            gatherFileRuleInfo.NodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
            gatherFileRuleInfo.IsSaveImage = TranslateUtils.ToBool(this.IsSaveImage.SelectedValue);

            DataProvider.GatherFileRuleDAO.Update(gatherFileRuleInfo);

            PageUtils.Redirect(ProgressBar.GetRedirectUrlStringWithGatherFile(base.PublishmentSystemID, this.gatherRuleName));
		}
	}
}
