using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.BBS.Model 
{
    public enum ECreditRule
    {
        Digest,                 //��������
        Post,                   //��������
        Reply,                  //����ظ�
        UnDigest,               //ȡ������
        DeletePost,                 //ɾ������
        DeleteReply,                //ɾ���ظ�

        AttachUpload,           //�ϴ�����
        AttachDownload,         //���ظ���
        SendPM,                 //������Ϣ
        Search,                 //����
        SetAvatar,              //����ͷ��
        Login,                  //��¼
    }

    public class ECreditRuleUtils
    {
        public static string GetValue(ECreditRule type)
        {
            if (type == ECreditRule.Digest)
            {
                return "Digest";
            }
            else if (type == ECreditRule.Post)
            {
                return "Post";
            }
            else if (type == ECreditRule.Reply)
            {
                return "Reply";
            }
            else if (type == ECreditRule.UnDigest)
            {
                return "UnDigest";
            }
            else if (type == ECreditRule.DeletePost)
            {
                return "DeletePost";
            }
            else if (type == ECreditRule.DeleteReply)
            {
                return "DeleteReply";
            }
            else if (type == ECreditRule.AttachUpload)
            {
                return "AttachUpload";
            }
            else if (type == ECreditRule.AttachDownload)
            {
                return "AttachDownload";
            }
            else if (type == ECreditRule.SendPM)
            {
                return "SendPM";
            }
            else if (type == ECreditRule.Search)
            {
                return "Search";
            }
            else if (type == ECreditRule.SetAvatar)
            {
                return "SetAvatar";
            }
            else if (type == ECreditRule.Login)
            {
                return "Login";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ECreditRule type)
        {
            if (type == ECreditRule.Digest)
            {
                return "��������";
            }
            else if (type == ECreditRule.Post)
            {
                return "��������";
            }
            else if (type == ECreditRule.Reply)
            {
                return "����ظ�";
            }
            else if (type == ECreditRule.UnDigest)
            {
                return "ȡ������";
            }
            else if (type == ECreditRule.DeletePost)
            {
                return "ɾ������";
            }
            else if (type == ECreditRule.DeleteReply)
            {
                return "ɾ���ظ�";
            }
            else if (type == ECreditRule.AttachUpload)
            {
                return "�ϴ�����";
            }
            else if (type == ECreditRule.AttachDownload)
            {
                return "���ظ���";
            }
            else if (type == ECreditRule.SendPM)
            {
                return "������Ϣ";
            }
            else if (type == ECreditRule.Search)
            {
                return "����";
            }
            else if (type == ECreditRule.SetAvatar)
            {
                return "����ͷ��";
            }
            else if (type == ECreditRule.Login)
            {
                return "��¼";
            }
            else
            {
                throw new Exception();
            }
        }

        public static CreditRuleInfo GetCreditRuleInfo(int publishmentSystemID, ECreditRule type)
        {
            CreditRuleInfo ruleInfo = new CreditRuleInfo();
            ruleInfo.PublishmentSystemID = publishmentSystemID;
            ruleInfo.RuleType = type;

            if (type == ECreditRule.Digest)
            {
                ruleInfo.Prestige = 5;
            }
            else if (type == ECreditRule.Post)
            {
                ruleInfo.Prestige = 1;
                ruleInfo.Currency = 1;
            }
            else if (type == ECreditRule.Reply)
            {
                ruleInfo.Prestige = 1;
                ruleInfo.Currency = 1;
            }
            else if (type == ECreditRule.UnDigest)
            {
                ruleInfo.Prestige = -5;
            }
            else if (type == ECreditRule.DeletePost)
            {
                ruleInfo.Prestige = -1;
                ruleInfo.Currency = -1;
            }
            else if (type == ECreditRule.DeleteReply)
            {
                ruleInfo.Prestige = -1;
                ruleInfo.Currency = -1;
            }
            else if (type == ECreditRule.SetAvatar)
            {
                ruleInfo.PeriodType = EPeriodType.Once;
                ruleInfo.Prestige = 1;
                ruleInfo.Currency = 5;
            }
            else if (type == ECreditRule.Login)
            {
                ruleInfo.PeriodType = EPeriodType.Everyday;
                ruleInfo.MaxNum = 1;
                ruleInfo.Currency = 2;
            }
            return ruleInfo;
        }

        public static ECreditRule GetEnumType(string typeStr)
        {
            ECreditRule retval = ECreditRule.Digest;

            if (Equals(ECreditRule.Digest, typeStr))
            {
                retval = ECreditRule.Digest;
            }
            else if (Equals(ECreditRule.Post, typeStr))
            {
                retval = ECreditRule.Post;
            }
            else if (Equals(ECreditRule.Reply, typeStr))
            {
                retval = ECreditRule.Reply;
            }
            else if (Equals(ECreditRule.UnDigest, typeStr))
            {
                retval = ECreditRule.UnDigest;
            }
            else if (Equals(ECreditRule.DeletePost, typeStr))
            {
                retval = ECreditRule.DeletePost;
            }
            else if (Equals(ECreditRule.DeleteReply, typeStr))
            {
                retval = ECreditRule.DeleteReply;
            }
            else if (Equals(ECreditRule.AttachUpload, typeStr))
            {
                retval = ECreditRule.AttachUpload;
            }
            else if (Equals(ECreditRule.AttachDownload, typeStr))
            {
                retval = ECreditRule.AttachDownload;
            }
            else if (Equals(ECreditRule.SendPM, typeStr))
            {
                retval = ECreditRule.SendPM;
            }
            else if (Equals(ECreditRule.Search, typeStr))
            {
                retval = ECreditRule.Search;
            }
            else if (Equals(ECreditRule.SetAvatar, typeStr))
            {
                retval = ECreditRule.SetAvatar;
            }
            else if (Equals(ECreditRule.Login, typeStr))
            {
                retval = ECreditRule.Login;
            }

            return retval;
        }

        public static bool Equals(ECreditRule type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ECreditRule type)
        {
            return Equals(type, typeStr);
        }

        public static ArrayList GetValueArrayList()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.Digest));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.Post));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.Reply));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.UnDigest));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.DeletePost));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.DeleteReply));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.AttachUpload));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.AttachDownload));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.SendPM));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.Search));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.SetAvatar));
            arraylist.Add(ECreditRuleUtils.GetValue(ECreditRule.Login));

            return arraylist;
        }

        public static ArrayList GetArrayListOfForum()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(ECreditRule.Digest);
            arraylist.Add(ECreditRule.Post);
            arraylist.Add(ECreditRule.Reply);
            arraylist.Add(ECreditRule.UnDigest);
            arraylist.Add(ECreditRule.DeletePost);
            arraylist.Add(ECreditRule.DeleteReply);
            arraylist.Add(ECreditRule.AttachUpload);
            arraylist.Add(ECreditRule.AttachDownload);

            return arraylist;
        }
    }
}
