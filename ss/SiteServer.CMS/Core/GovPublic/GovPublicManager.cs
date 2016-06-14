using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using SiteServer.CMS.Model;
using System.Text;

using System;

namespace SiteServer.CMS.Core
{
	public class GovPublicManager
	{
        public static void Initialize(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (publishmentSystemInfo.Additional.GovPublicNodeID > 0)
            {
                if (!DataProvider.NodeDAO.IsExists(publishmentSystemInfo.Additional.GovPublicNodeID))
                {
                    publishmentSystemInfo.Additional.GovPublicNodeID = 0;
                }
            }
            if (publishmentSystemInfo.Additional.GovPublicNodeID == 0)
            {
                int govPublicNodeID = DataProvider.NodeDAO.GetNodeIDByContentModelType(publishmentSystemInfo.PublishmentSystemID, EContentModelType.GovPublic);
                if (govPublicNodeID == 0)
                {
                    govPublicNodeID = DataProvider.NodeDAO.InsertNodeInfo(publishmentSystemInfo.PublishmentSystemID, publishmentSystemInfo.PublishmentSystemID, "信息公开", string.Empty, EContentModelTypeUtils.GetValue(EContentModelType.GovPublic));
                }
                publishmentSystemInfo.Additional.GovPublicNodeID = govPublicNodeID;
                DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);
            }

            GovPublicCategoryManager.Initialize(publishmentSystemInfo);

            if (DataProvider.GovPublicIdentifierRuleDAO.GetCount(publishmentSystemInfo.PublishmentSystemID) == 0)
            {
                ArrayList ruleInfoArrayList = new ArrayList();
                ruleInfoArrayList.Add(new GovPublicIdentifierRuleInfo(0, "机构分类代码", publishmentSystemInfo.PublishmentSystemID, EGovPublicIdentifierType.Department, 5, "-", string.Empty, string.Empty, 0, 0, string.Empty));
                ruleInfoArrayList.Add(new GovPublicIdentifierRuleInfo(0, "主题分类代码", publishmentSystemInfo.PublishmentSystemID, EGovPublicIdentifierType.Channel, 5, "-", string.Empty, string.Empty, 0, 0, string.Empty));
                ruleInfoArrayList.Add(new GovPublicIdentifierRuleInfo(0, "生效日期", publishmentSystemInfo.PublishmentSystemID, EGovPublicIdentifierType.Attribute, 0, "-", "yyyy", GovPublicContentAttribute.EffectDate, 0, 0, string.Empty));
                ruleInfoArrayList.Add(new GovPublicIdentifierRuleInfo(0, "顺序号", publishmentSystemInfo.PublishmentSystemID, EGovPublicIdentifierType.Sequence, 5, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty));

                foreach (GovPublicIdentifierRuleInfo ruleInfo in ruleInfoArrayList)
                {
                    DataProvider.GovPublicIdentifierRuleDAO.Insert(ruleInfo);
                }
            }
        }

        public static string GetPreviewIdentifier(int publishmentSystemID)
        {
            StringBuilder builder = new StringBuilder();

            ArrayList ruleInfoArrayList = DataProvider.GovPublicIdentifierRuleDAO.GetRuleInfoArrayList(publishmentSystemID);
            foreach (GovPublicIdentifierRuleInfo ruleInfo in ruleInfoArrayList)
            {
                if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Department)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("D123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("D123").Append(ruleInfo.Suffix);
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Channel)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("C123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("C123").Append(ruleInfo.Suffix);
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Attribute)
                {
                    if (ruleInfo.AttributeName == GovPublicContentAttribute.AbolitionDate || ruleInfo.AttributeName == GovPublicContentAttribute.EffectDate || ruleInfo.AttributeName == GovPublicContentAttribute.PublishDate || ruleInfo.AttributeName == ContentAttribute.AddDate)
                    {
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(DateTime.Now.ToString(ruleInfo.FormatString).PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(DateTime.Now.ToString(ruleInfo.FormatString)).Append(ruleInfo.Suffix);
                        }
                    }
                    else
                    {
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append("A123".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append("A123").Append(ruleInfo.Suffix);
                        }
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Sequence)
                {
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append("1".PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append("1").Append(ruleInfo.Suffix);
                    }
                }
            }

            return builder.ToString();
        }

        public static bool IsIdentifierChanged(int channelID, int departmentID, DateTime effectDate, GovPublicContentInfo contentInfo)
        {
            bool isIdentifierChanged = false;
            ArrayList ruleInfoArrayList = DataProvider.GovPublicIdentifierRuleDAO.GetRuleInfoArrayList(contentInfo.PublishmentSystemID);
            foreach (GovPublicIdentifierRuleInfo ruleInfo in ruleInfoArrayList)
            {
                if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Department)
                {
                    if (contentInfo.DepartmentID != departmentID)
                    {
                        isIdentifierChanged = true;
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Channel)
                {
                    if (contentInfo.NodeID != channelID)
                    {
                        isIdentifierChanged = true;
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Attribute)
                {
                    if (StringUtils.EqualsIgnoreCase(ruleInfo.AttributeName, GovPublicContentAttribute.EffectDate) && TranslateUtils.ToDateTime(contentInfo.GetExtendedAttribute(ruleInfo.AttributeName)) != effectDate)
                    {
                        isIdentifierChanged = true;
                    }
                }
            }
            return isIdentifierChanged;
        }

        public static string GetIdentifier(PublishmentSystemInfo publishmentSystemInfo, int channelID, int departmentID, GovPublicContentInfo contentInfo)
        {
            StringBuilder builder = new StringBuilder();
            NodeInfo nodeInfo = NodeManager.GetNodeInfo(publishmentSystemInfo.PublishmentSystemID, channelID);

            ArrayList ruleInfoArrayList = DataProvider.GovPublicIdentifierRuleDAO.GetRuleInfoArrayList(publishmentSystemInfo.PublishmentSystemID);
            foreach (GovPublicIdentifierRuleInfo ruleInfo in ruleInfoArrayList)
            {
                if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Department)
                {
                    string departmentCode = DepartmentManager.GetDepartmentCode(departmentID);
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(departmentCode.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(departmentCode).Append(ruleInfo.Suffix);
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Channel)
                {
                    string channelCode = DataProvider.GovPublicChannelDAO.GetCode(nodeInfo.NodeID);
                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(channelCode.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(channelCode).Append(ruleInfo.Suffix);
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Attribute)
                {
                    if (ruleInfo.AttributeName == GovPublicContentAttribute.AbolitionDate || ruleInfo.AttributeName == GovPublicContentAttribute.EffectDate || ruleInfo.AttributeName == GovPublicContentAttribute.PublishDate || ruleInfo.AttributeName == ContentAttribute.AddDate)
                    {
                        DateTime dateTime = TranslateUtils.ToDateTime(contentInfo.GetExtendedAttribute(ruleInfo.AttributeName));
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(dateTime.ToString(ruleInfo.FormatString).PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(dateTime.ToString(ruleInfo.FormatString)).Append(ruleInfo.Suffix);
                        }
                    }
                    else
                    {
                        string attributeValue = contentInfo.GetExtendedAttribute(ruleInfo.AttributeName);
                        if (ruleInfo.MinLength > 0)
                        {
                            builder.Append(attributeValue.PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                        }
                        else
                        {
                            builder.Append(attributeValue).Append(ruleInfo.Suffix);
                        }
                    }
                }
                else if (ruleInfo.IdentifierType == EGovPublicIdentifierType.Sequence)
                {
                    int targetPublishmentSystemID = publishmentSystemInfo.PublishmentSystemID;
                    int targetNodeID = 0;
                    if (ruleInfo.Additional.IsSequenceChannelZero)
                    {
                        targetNodeID = nodeInfo.NodeID;
                    }
                    int targetDepartmentID = 0;
                    if (ruleInfo.Additional.IsSequenceDepartmentZero)
                    {
                        targetDepartmentID = departmentID;
                    }
                    int targetAddYear = 0;
                    if (ruleInfo.Additional.IsSequenceYearZero)
                    {
                        targetAddYear = contentInfo.AddDate.Year;
                    }

                    int sequence = DataProvider.GovPublicIdentifierSeqDAO.GetSequence(targetPublishmentSystemID, targetNodeID, targetDepartmentID, targetAddYear, ruleInfo.Sequence);

                    if (ruleInfo.MinLength > 0)
                    {
                        builder.Append(sequence.ToString().PadLeft(ruleInfo.MinLength, '0')).Append(ruleInfo.Suffix);
                    }
                    else
                    {
                        builder.Append(sequence.ToString()).Append(ruleInfo.Suffix);
                    }
                }
            }

            return builder.ToString();
        }

        public static ArrayList GetFirstDepartmentIDArrayList(PublishmentSystemInfo publishmentSystemInfo)
        {
            if (string.IsNullOrEmpty(publishmentSystemInfo.Additional.GovPublicDepartmentIDCollection))
            {
                return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByParentID(0);
            }
            else
            {
                return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByDepartmentIDCollection(publishmentSystemInfo.Additional.GovPublicDepartmentIDCollection);
            }
        }

        public static ArrayList GetAllDepartmentIDArrayList(PublishmentSystemInfo publishmentSystemInfo)
        {
            ArrayList firstIDArrayList = GovPublicManager.GetFirstDepartmentIDArrayList(publishmentSystemInfo);
            return BaiRongDataProvider.DepartmentDAO.GetDepartmentIDArrayListByFirstDepartmentIDArrayList(firstIDArrayList);
        }
	}
}
