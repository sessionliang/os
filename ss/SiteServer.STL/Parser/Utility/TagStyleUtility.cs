using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages.Modal;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using SiteServer.CMS.Core;
using SiteServer.STL.StlTemplate;

namespace SiteServer.STL.Parser
{
    public class TagStyleUtility
    {
        public static string GetTagStyleTitle(string elementName)
        {
            if (StringUtils.EqualsIgnoreCase(elementName, StlComments.ElementName))
            {
                return "评论列表";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlSearchInput.ElementName))
            {
                return "搜索框";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlDigg.ElementName))
            {
                return "掘客(digg)";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlStar.ElementName))
            {
                return "评分";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlResume.ElementName))
            {
                return "简历";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicApply.ElementName))
            {
                return "依申请公开提交";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicQuery.ElementName))
            {
                return "依申请公开查询";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractApply.ElementName))
            {
                return "互动交流提交";
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractQuery.ElementName))
            {
                return "互动交流查询";
            }
            return string.Empty;
        }

        public static bool IsStyleVisible(string elementName)
        {
            if (StringUtils.EqualsIgnoreCase(StlSearchInput.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlComments.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlResume.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovInteractApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovInteractQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlCommentInput.ElementName, elementName))
            {
                return false;
            }
            return true;
        }

        public static bool IsScriptVisible(string elementName)
        {
            if (StringUtils.EqualsIgnoreCase(StlSearchInput.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlComments.ElementName, elementName))
            {
                return false;
            }
            return true;
        }

        public static bool IsSuccessVisible(string elementName)
        {
            if (StringUtils.EqualsIgnoreCase(StlGovInteractApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovInteractQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlCommentInput.ElementName, elementName))
            {
                return true;
            }
            return false;
        }

        public static bool IsFailureVisible(string elementName)
        {
            if (StringUtils.EqualsIgnoreCase(StlGovInteractApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovInteractQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicApply.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlGovPublicQuery.ElementName, elementName) || StringUtils.EqualsIgnoreCase(StlCommentInput.ElementName, elementName))
            {
                return true;
            }
            return false;
        }

        public static string GetOpenWindowStringToAdd(string elementName, int publishmentSystemID)
        {
            if (StringUtils.EqualsIgnoreCase(elementName, StlSearchInput.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleSearchInputAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlDigg.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleDiggAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlStar.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleStarAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlResume.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleResumeAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicApply.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovPublicApplyAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicQuery.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovPublicQueryAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }
            //else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractApply.ElementName))
            //{
            //    return TagStyleGovInteractApplyAdd.GetOpenWindowStringToAdd(publishmentSystemID);
            //}
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractQuery.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovInteractQueryAdd_GetOpenWindowStringToAdd(publishmentSystemID);
            }

            return string.Empty;
        }

        public static string GetOpenWindowStringToEdit(string elementName, int publishmentSystemID, int styleID)
        {
            if (StringUtils.EqualsIgnoreCase(elementName, StlCommentInput.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleCommentInputAdd_GetOpenWindowString(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlComments.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleCommentsAdd_GetOpenWindowString(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlSearchInput.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleSearchInputAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlDigg.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleDiggAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlStar.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleStarAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlResume.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleResumeAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicApply.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovPublicApplyAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovPublicQuery.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovPublicQueryAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractApply.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovInteractApplyAdd_GetOpenWindowString(publishmentSystemID, styleID);
            }
            else if (StringUtils.EqualsIgnoreCase(elementName, StlGovInteractQuery.ElementName))
            {
                return PageUtility.ModalSTL.TagStyleGovInteractQueryAdd_GetOpenWindowStringToEdit(publishmentSystemID, styleID);
            }
            return string.Empty;
        }

        public static void IsCreateTemplate_CheckedChanged(TagStyleInfo styleInfo, PublishmentSystemInfo publishmentSystemInfo, TextBox tbContent, TextBox tbSuccess, TextBox tbFailure, TextBox tbStyle, TextBox tbScript)
        {
            if (StringUtils.EqualsIgnoreCase(StlContentInput.ElementName, styleInfo.ElementName))
            {
                TagStyleContentInputInfo inputInfo = new TagStyleContentInputInfo(styleInfo.SettingsXML);
                ContentInputTemplate contentInputTemplate = new ContentInputTemplate(publishmentSystemInfo, styleInfo, inputInfo);
                tbContent.Text = contentInputTemplate.GetContent();
                tbStyle.Text = contentInputTemplate.GetStyle(ETableStyle.BackgroundContent);
                tbScript.Text = contentInputTemplate.GetScript();
            }
            else if (StringUtils.EqualsIgnoreCase(StlSearchInput.ElementName, styleInfo.ElementName))
            {
                TagStyleSearchInputInfo inputInfo = new TagStyleSearchInputInfo(styleInfo.SettingsXML);
                SearchInputTemplate searchInputTemplate = new SearchInputTemplate(publishmentSystemInfo, styleInfo, inputInfo, string.Empty, string.Empty);
                tbContent.Text = searchInputTemplate.GetContent();
            }
            else if (StringUtils.EqualsIgnoreCase(StlResume.ElementName, styleInfo.ElementName))
            {
                TagStyleResumeInfo resumeInfo = new TagStyleResumeInfo(styleInfo.SettingsXML);
                ResumeTemplate resumeTemplate = new ResumeTemplate(publishmentSystemInfo, styleInfo, resumeInfo);
                tbContent.Text = resumeTemplate.GetContent();
                tbScript.Text = resumeTemplate.GetScript();
            }
            else if (StringUtils.EqualsIgnoreCase(StlGovPublicApply.ElementName, styleInfo.ElementName))
            {
                TagStyleGovPublicApplyInfo applyInfo = new TagStyleGovPublicApplyInfo(styleInfo.SettingsXML);
                GovPublicApplyTemplate applyTemplate = new GovPublicApplyTemplate(publishmentSystemInfo, styleInfo, applyInfo);
                tbContent.Text = applyTemplate.GetFileInputTemplate();
                tbSuccess.Text = applyTemplate.GetFileSuccessTemplate();
                tbFailure.Text = applyTemplate.GetFileFailureTemplate();
                tbScript.Text = applyTemplate.GetScript();
            }
            else if (StringUtils.EqualsIgnoreCase(StlGovPublicQuery.ElementName, styleInfo.ElementName))
            {
                GovPublicQueryTemplate queryTemplate = new GovPublicQueryTemplate(publishmentSystemInfo, styleInfo);
                tbContent.Text = queryTemplate.GetFileInputTemplate();
                tbSuccess.Text = queryTemplate.GetFileSuccessTemplate();
                tbFailure.Text = queryTemplate.GetFileFailureTemplate();
                tbScript.Text = queryTemplate.GetScript();
            }
            else if (StringUtils.EqualsIgnoreCase(StlGovInteractApply.ElementName, styleInfo.ElementName))
            {
                TagStyleGovInteractApplyInfo applyInfo = new TagStyleGovInteractApplyInfo(styleInfo.SettingsXML);
                int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByApplyStyleID(styleInfo.StyleID);
                GovInteractApplyTemplate applyTemplate = new GovInteractApplyTemplate(publishmentSystemInfo, nodeID, styleInfo, applyInfo);
                tbContent.Text = applyTemplate.GetFileInputTemplate();
                tbSuccess.Text = applyTemplate.GetFileSuccessTemplate();
                tbFailure.Text = applyTemplate.GetFileFailureTemplate();
                tbScript.Text = applyTemplate.GetScript();
            }
            else if (StringUtils.EqualsIgnoreCase(StlGovInteractQuery.ElementName, styleInfo.ElementName))
            {
                int nodeID = DataProvider.GovInteractChannelDAO.GetNodeIDByQueryStyleID(styleInfo.StyleID);
                GovInteractQueryTemplate queryTemplate = new GovInteractQueryTemplate(publishmentSystemInfo, nodeID, styleInfo);
                tbContent.Text = queryTemplate.GetFileInputTemplate();
                tbSuccess.Text = queryTemplate.GetFileSuccessTemplate();
                tbFailure.Text = queryTemplate.GetFileFailureTemplate();
                tbScript.Text = queryTemplate.GetScript();
            }
        }
    }
}
