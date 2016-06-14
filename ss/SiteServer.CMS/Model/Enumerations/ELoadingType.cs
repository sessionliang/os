using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum ELoadingType
    {
        ContentTree,
        Channel,
        ChannelSelect,
        SiteAnalysis,
        TemplateFilePathRule,
        ConfigurationCreateDetails,
        ConfigurationCrossSiteTrans,
        ConfigurationSignin,
        GovPublicChannelAdd,
        GovPublicChannelTree,
        GovPublicChannel,
        GovInteractChannel,
        EvaluationNodeTree,
        TrialApplyNodeTree,
        TrialReportNodeTree,
        TrialAnalysisNodeTree,
        SurveyNodeTree,
        SurveyAnalysisNodeTree,
        CompareNodeTree,
    }

    public class ELoadingTypeUtils
    {
        public static string GetValue(ELoadingType type)
        {
            if (type == ELoadingType.ContentTree)
            {
                return "ContentTree";
            }
            else if (type == ELoadingType.Channel)
            {
                return "Channel";
            }
            else if (type == ELoadingType.ChannelSelect)
            {
                return "ChannelSelect";
            }
            else if (type == ELoadingType.SiteAnalysis)
            {
                return "SiteAnalysis";
            }
            else if (type == ELoadingType.TemplateFilePathRule)
            {
                return "TemplateFilePathRule";
            }
            else if (type == ELoadingType.ConfigurationCreateDetails)
            {
                return "ConfigurationCreateDetails";
            }
            else if (type == ELoadingType.ConfigurationCrossSiteTrans)
            {
                return "ConfigurationCrossSiteTrans";
            }
            else if (type == ELoadingType.ConfigurationSignin)
            {
                return "ConfigurationSignin";
            }
            else if (type == ELoadingType.GovPublicChannelAdd)
            {
                return "GovPublicChannelAdd";
            }
            else if (type == ELoadingType.GovPublicChannelTree)
            {
                return "GovPublicChannelTree";
            }
            else if (type == ELoadingType.GovPublicChannel)
            {
                return "GovPublicChannel";
            }
            else if (type == ELoadingType.GovInteractChannel)
            {
                return "GovInteractChannel";
            }
            else if (type == ELoadingType.EvaluationNodeTree)
            {
                return "EvaluationNodeTree";
            }
            else if (type == ELoadingType.TrialApplyNodeTree)
            {
                return "TrialApplyNodeTree";
            }
            else if (type == ELoadingType.TrialReportNodeTree)
            {
                return "TrialReportNodeTree";
            }
            else if (type == ELoadingType.SurveyNodeTree)
            {
                return "SurveyNodeTree";
            }
            else if (type == ELoadingType.TrialAnalysisNodeTree)
            {
                return "TrialAnalysisNodeTree";
            }
            else if (type == ELoadingType.SurveyAnalysisNodeTree)
            {
                return "SurveyAnalysisNodeTree";
            }
            else if (type == ELoadingType.CompareNodeTree)
            {
                return "CompareNodeTree";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ELoadingType GetEnumType(string typeStr)
        {
            ELoadingType retval = ELoadingType.Channel;

            if (Equals(ELoadingType.ContentTree, typeStr))
            {
                retval = ELoadingType.ContentTree;
            }
            else if (Equals(ELoadingType.Channel, typeStr))
            {
                retval = ELoadingType.Channel;
            }
            else if (Equals(ELoadingType.ChannelSelect, typeStr))
            {
                retval = ELoadingType.ChannelSelect;
            }
            else if (Equals(ELoadingType.SiteAnalysis, typeStr))
            {
                retval = ELoadingType.SiteAnalysis;
            }
            else if (Equals(ELoadingType.TemplateFilePathRule, typeStr))
            {
                retval = ELoadingType.TemplateFilePathRule;
            }
            else if (Equals(ELoadingType.ConfigurationCreateDetails, typeStr))
            {
                retval = ELoadingType.ConfigurationCreateDetails;
            }
            else if (Equals(ELoadingType.ConfigurationCrossSiteTrans, typeStr))
            {
                retval = ELoadingType.ConfigurationCrossSiteTrans;
            }
            else if (Equals(ELoadingType.ConfigurationSignin, typeStr))
            {
                retval = ELoadingType.ConfigurationSignin;
            }
            else if (Equals(ELoadingType.GovPublicChannelAdd, typeStr))
            {
                retval = ELoadingType.GovPublicChannelAdd;
            }
            else if (Equals(ELoadingType.GovPublicChannelTree, typeStr))
            {
                retval = ELoadingType.GovPublicChannelTree;
            }
            else if (Equals(ELoadingType.GovPublicChannel, typeStr))
            {
                retval = ELoadingType.GovPublicChannel;
            }
            else if (Equals(ELoadingType.GovInteractChannel, typeStr))
            {
                retval = ELoadingType.GovInteractChannel;
            }
            else if (Equals(ELoadingType.EvaluationNodeTree, typeStr))
            {
                retval = ELoadingType.EvaluationNodeTree;
            }
            else if (Equals(ELoadingType.TrialApplyNodeTree, typeStr))
            {
                retval = ELoadingType.TrialApplyNodeTree;
            }
            else if (Equals(ELoadingType.TrialReportNodeTree, typeStr))
            {
                retval = ELoadingType.TrialReportNodeTree;
            }
            else if (Equals(ELoadingType.SurveyNodeTree, typeStr))
            {
                retval = ELoadingType.SurveyNodeTree;
            }
            else if (Equals(ELoadingType.TrialAnalysisNodeTree, typeStr))
            {
                retval = ELoadingType.TrialAnalysisNodeTree;
            }
            else if (Equals(ELoadingType.SurveyAnalysisNodeTree, typeStr))
            {
                retval = ELoadingType.SurveyAnalysisNodeTree;
            }
            else if (Equals(ELoadingType.CompareNodeTree, typeStr))
            {
                retval = ELoadingType.CompareNodeTree;
            }

            return retval;
        }

        public static bool Equals(ELoadingType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ELoadingType type)
        {
            return Equals(type, typeStr);
        }
    }
}
