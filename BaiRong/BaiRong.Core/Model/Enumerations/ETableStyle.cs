using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum ETableStyle
    {
        BackgroundContent,	    //����        
        GovPublicContent,	    //��Ϣ����
        GovInteractContent,	    //��������
        VoteContent,	        //ͶƱ
        JobContent,	            //��Ƹ
        GoodsContent,           //��Ʒ
        BrandContent,           //Ʒ��
        UserDefined,            //�Զ���
        Channel,			    //��Ŀ
        InputContent,           //�ύ��
        ClassifyInputContent,           //�����ύ�� by 20151028 sofuny
        User,                   //��Ա
        Comment,                //����
        Site,                   //Ӧ��
        ManuscriptContent,             //���
        WebsiteMessageContent,           //��վ����
        SpecialContent,           //ר��
        AdvImageContent,           //���
        EvaluationContent,         //����
        TrialApplyContent,         //��������
        TrialReportContent,         //���ñ���
        SurveyContent,         //�����ʾ�
        CompareContent,         //�ȽϷ���
    }

    public class ETableStyleUtils
    {
        public static string GetValue(ETableStyle type)
        {
            if (type == ETableStyle.BackgroundContent)
            {
                return "BackgroundContent";
            }
            else if (type == ETableStyle.GovPublicContent)
            {
                return "GovPublicContent";
            }
            else if (type == ETableStyle.GovInteractContent)
            {
                return "GovInteractContent";
            }
            else if (type == ETableStyle.VoteContent)
            {
                return "VoteContent";
            }
            else if (type == ETableStyle.JobContent)
            {
                return "JobContent";
            }
            else if (type == ETableStyle.GoodsContent)
            {
                return "GoodsContent";
            }
            else if (type == ETableStyle.BrandContent)
            {
                return "BrandContent";
            }
            else if (type == ETableStyle.UserDefined)
            {
                return "UserDefined";
            }
            else if (type == ETableStyle.Channel)
            {
                return "Channel";
            }
            else if (type == ETableStyle.InputContent)
            {
                return "InputContent";
            }
            else if (type == ETableStyle.ClassifyInputContent)// by 20151028 sofuny
            {
                return "ClassifyInputContent";
            }
            else if (type == ETableStyle.User)
            {
                return "User";
            }
            else if (type == ETableStyle.Comment)
            {
                return "Comment";
            }
            else if (type == ETableStyle.Site)
            {
                return "Site";
            }
            else if (type == ETableStyle.ManuscriptContent)
            {
                return "ManuscriptContent";
            }
            else if (type == ETableStyle.WebsiteMessageContent)
            {
                return "WebsiteMessageContent";
            }
            else if (type == ETableStyle.SpecialContent)
            {
                return "SpecialContent";
            }
            else if (type == ETableStyle.AdvImageContent)
            {
                return "AdvImageContent";
            }
            else if (type == ETableStyle.EvaluationContent)
            {
                return "EvaluationContent";
            }
            else if (type == ETableStyle.TrialApplyContent)
            {
                return "TrialApplyContent";
            }
            else if (type == ETableStyle.TrialReportContent)
            {
                return "TrialReportContent";
            }
            else if (type == ETableStyle.SurveyContent)
            {
                return "SurveyContent";
            }
            else if (type == ETableStyle.CompareContent)
            {
                return "CompareContent";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ETableStyle type)
        {
            if (type == ETableStyle.BackgroundContent)
            {
                return "����";
            }
            else if (type == ETableStyle.GovPublicContent)
            {
                return "��Ϣ����";
            }
            else if (type == ETableStyle.GovInteractContent)
            {
                return "��������";
            }
            else if (type == ETableStyle.VoteContent)
            {
                return "ͶƱ";
            }
            else if (type == ETableStyle.JobContent)
            {
                return "��Ƹ";
            }
            else if (type == ETableStyle.GoodsContent)
            {
                return "��Ʒ";
            }
            else if (type == ETableStyle.BrandContent)
            {
                return "Ʒ��";
            }
            else if (type == ETableStyle.UserDefined)
            {
                return "�Զ���";
            }
            else if (type == ETableStyle.Channel)
            {
                return "��Ŀ";
            }
            else if (type == ETableStyle.InputContent)
            {
                return "�ύ��";
            }
            else if (type == ETableStyle.ClassifyInputContent)
            {
                return "�ύ��";
            }
            else if (type == ETableStyle.User)
            {
                return "��Ա";
            }
            else if (type == ETableStyle.Comment)
            {
                return "����";
            }
            else if (type == ETableStyle.Site)
            {
                return "Ӧ��";
            }
            else if (type == ETableStyle.ManuscriptContent)
            {
                return "���";
            }
            else if (type == ETableStyle.WebsiteMessageContent)
            {
                return "��վ����";
            }
            else if (type == ETableStyle.SpecialContent)
            {
                return "ר��";
            }
            else if (type == ETableStyle.AdvImageContent)
            {
                return "���";
            }
            else if (type == ETableStyle.EvaluationContent)
            {
                return "����";
            }
            else if (type == ETableStyle.TrialApplyContent)
            {
                return "��������";
            }
            else if (type == ETableStyle.TrialReportContent)
            {
                return "���ñ���";
            }
            else if (type == ETableStyle.SurveyContent)
            {
                return "�����ʾ�";
            }
            else if (type == ETableStyle.CompareContent)
            {
                return "�ȽϷ���";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ETableStyle GetEnumType(string typeStr)
        {
            ETableStyle retval = ETableStyle.BackgroundContent;

            if (Equals(ETableStyle.BackgroundContent, typeStr))
            {
                retval = ETableStyle.BackgroundContent;
            }
            else if (Equals(ETableStyle.GovPublicContent, typeStr))
            {
                retval = ETableStyle.GovPublicContent;
            }
            else if (Equals(ETableStyle.GovInteractContent, typeStr))
            {
                retval = ETableStyle.GovInteractContent;
            }
            else if (Equals(ETableStyle.VoteContent, typeStr))
            {
                retval = ETableStyle.VoteContent;
            }
            else if (Equals(ETableStyle.JobContent, typeStr))
            {
                retval = ETableStyle.JobContent;
            }
            else if (Equals(ETableStyle.GoodsContent, typeStr))
            {
                retval = ETableStyle.GoodsContent;
            }
            else if (Equals(ETableStyle.BrandContent, typeStr))
            {
                retval = ETableStyle.BrandContent;
            }
            else if (Equals(ETableStyle.UserDefined, typeStr))
            {
                retval = ETableStyle.UserDefined;
            }
            else if (Equals(ETableStyle.Channel, typeStr))
            {
                retval = ETableStyle.Channel;
            }
            else if (Equals(ETableStyle.InputContent, typeStr))
            {
                retval = ETableStyle.InputContent;
            }
            else if (Equals(ETableStyle.ClassifyInputContent, typeStr))
            {
                retval = ETableStyle.ClassifyInputContent;
            }
            else if (Equals(ETableStyle.User, typeStr))
            {
                retval = ETableStyle.User;
            }
            else if (Equals(ETableStyle.Comment, typeStr))
            {
                retval = ETableStyle.Comment;
            }
            else if (Equals(ETableStyle.Site, typeStr))
            {
                retval = ETableStyle.Site;
            }
            else if (Equals(ETableStyle.ManuscriptContent, typeStr))
            {
                retval = ETableStyle.ManuscriptContent;
            }
            else if (Equals(ETableStyle.WebsiteMessageContent, typeStr))
            {
                retval = ETableStyle.WebsiteMessageContent;
            }
            else if (Equals(ETableStyle.SpecialContent, typeStr))
            {
                retval = ETableStyle.SpecialContent;
            }
            else if (Equals(ETableStyle.AdvImageContent, typeStr))
            {
                retval = ETableStyle.AdvImageContent;
            }
            else if (Equals(ETableStyle.EvaluationContent, typeStr))
            {
                retval = ETableStyle.EvaluationContent;
            }
            else if (Equals(ETableStyle.TrialApplyContent, typeStr))
            {
                retval = ETableStyle.TrialApplyContent;
            }
            else if (Equals(ETableStyle.TrialReportContent, typeStr))
            {
                retval = ETableStyle.TrialReportContent;
            }
            else if (Equals(ETableStyle.SurveyContent, typeStr))
            {
                retval = ETableStyle.SurveyContent;
            }
            else if (Equals(ETableStyle.CompareContent, typeStr))
            {
                retval = ETableStyle.CompareContent;
            }
            return retval;
        }

        public static bool Equals(ETableStyle type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ETableStyle type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(ETableStyle type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static bool IsNodeRelated(ETableStyle tableStyle)
        {
            if (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.GovPublicContent || tableStyle == ETableStyle.GovInteractContent || tableStyle == ETableStyle.VoteContent || tableStyle == ETableStyle.JobContent || tableStyle == ETableStyle.GoodsContent || tableStyle == ETableStyle.BrandContent || tableStyle == ETableStyle.UserDefined || tableStyle == ETableStyle.Channel)
            {
                return true;
            }
            return false;
        }

        public static bool IsContent(ETableStyle tableStyle)
        {
            if (tableStyle == ETableStyle.BackgroundContent || tableStyle == ETableStyle.GovPublicContent || tableStyle == ETableStyle.GovInteractContent || tableStyle == ETableStyle.VoteContent || tableStyle == ETableStyle.JobContent || tableStyle == ETableStyle.GoodsContent || tableStyle == ETableStyle.BrandContent || tableStyle == ETableStyle.UserDefined || tableStyle == ETableStyle.ManuscriptContent || tableStyle == ETableStyle.EvaluationContent || tableStyle == ETableStyle.TrialReportContent || tableStyle == ETableStyle.TrialApplyContent || tableStyle == ETableStyle.SurveyContent || tableStyle == ETableStyle.CompareContent)
            {
                return true;
            }
            return false;
        }

        public static EAuxiliaryTableType GetTableType(ETableStyle tableStyle)
        {
            if (tableStyle == ETableStyle.GovPublicContent)
            {
                return EAuxiliaryTableType.GovPublicContent;
            }
            else if (tableStyle == ETableStyle.GovInteractContent)
            {
                return EAuxiliaryTableType.GovInteractContent;
            }
            else if (tableStyle == ETableStyle.VoteContent)
            {
                return EAuxiliaryTableType.VoteContent;
            }
            else if (tableStyle == ETableStyle.JobContent)
            {
                return EAuxiliaryTableType.JobContent;
            }
            else if (tableStyle == ETableStyle.GoodsContent)
            {
                return EAuxiliaryTableType.GoodsContent;
            }
            else if (tableStyle == ETableStyle.BrandContent)
            {
                return EAuxiliaryTableType.BrandContent;
            }
            else if (tableStyle == ETableStyle.UserDefined)
            {
                return EAuxiliaryTableType.UserDefined;
            }
            else if (tableStyle == ETableStyle.ManuscriptContent)
            {
                return EAuxiliaryTableType.ManuscriptContent;
            } 
            return EAuxiliaryTableType.BackgroundContent;
        }
    }
}
