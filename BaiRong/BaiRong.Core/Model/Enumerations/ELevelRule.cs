using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiRong.Model
{

    public enum ELevelRule
    {
        //BBS
        BBSDigest,                 //精华主题
        BBSPost,                   //发表主题
        BBSReply,                  //发表回复
        BBSUnDigest,               //取消精华
        BBSDeletePost,             //删除主题
        BBSDeleteReply,            //删除回复
        //PlatForm
        PlatFormAttachUpload,           //上传附件
        PlatFormAttachDownload,         //下载附件
        PlatFormSendPM,                 //发短消息
        PlatFormSearch,                 //搜索
        PlatFormSetAvatar,              //设置头像
        PlatFormLogin,                  //登录
    }

    public class ELevelRuleUtils
    {
        public static string GetValue(ELevelRule type)
        {
            //BBS
            if (type == ELevelRule.BBSDigest)
            {
                return "BBSDigest";
            }
            else if (type == ELevelRule.BBSPost)
            {
                return "BBSPost";
            }
            else if (type == ELevelRule.BBSReply)
            {
                return "BBSReply";
            }
            else if (type == ELevelRule.BBSUnDigest)
            {
                return "BBSUnDigest";
            }
            else if (type == ELevelRule.BBSDeletePost)
            {
                return "BBSDeletePost";
            }
            else if (type == ELevelRule.BBSDeleteReply)
            {
                return "BBSDeleteReply";
            }
            //PlatForm
            else if (type == ELevelRule.PlatFormAttachUpload)
            {
                return "PlatFormAttachUpload";
            }
            else if (type == ELevelRule.PlatFormAttachDownload)
            {
                return "PlatFormAttachDownload";
            }
            else if (type == ELevelRule.PlatFormSendPM)
            {
                return "PlatFormSendPM";
            }
            else if (type == ELevelRule.PlatFormSearch)
            {
                return "PlatFormSearch";
            }
            else if (type == ELevelRule.PlatFormSetAvatar)
            {
                return "PlatFormSetAvatar";
            }
            else if (type == ELevelRule.PlatFormLogin)
            {
                return "PlatFormLogin";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ELevelRule type)
        {
            //BBS
            if (type == ELevelRule.BBSDigest)
            {
                return "精华主题";
            }
            else if (type == ELevelRule.BBSPost)
            {
                return "发表主题";
            }
            else if (type == ELevelRule.BBSReply)
            {
                return "发表回复";
            }
            else if (type == ELevelRule.BBSUnDigest)
            {
                return "取消精华";
            }
            else if (type == ELevelRule.BBSDeletePost)
            {
                return "删除主题";
            }
            else if (type == ELevelRule.BBSDeleteReply)
            {
                return "删除回复";
            }
            //PlatForm
            else if (type == ELevelRule.PlatFormAttachUpload)
            {
                return "上传附件";
            }
            else if (type == ELevelRule.PlatFormAttachDownload)
            {
                return "下载附件";
            }
            else if (type == ELevelRule.PlatFormSendPM)
            {
                return "发短消息";
            }
            else if (type == ELevelRule.PlatFormSearch)
            {
                return "搜索";
            }
            else if (type == ELevelRule.PlatFormSetAvatar)
            {
                return "设置头像";
            }
            else if (type == ELevelRule.PlatFormLogin)
            {
                return "登录";
            }
            else
            {
                throw new Exception();
            }
        }

        public static LevelRuleInfo GetLevelRuleInfo(ELevelRule type)
        {
            LevelRuleInfo ruleInfo = new LevelRuleInfo();
            ruleInfo.RuleType = type;
            //BBS
            if (type == ELevelRule.BBSDigest)
            {
                ruleInfo.CreditNum = 5;
            }
            else if (type == ELevelRule.BBSPost)
            {
                ruleInfo.CreditNum = 1;
                ruleInfo.CashNum = 1;
            }
            else if (type == ELevelRule.BBSReply)
            {
                ruleInfo.CreditNum = 1;
                ruleInfo.CashNum = 1;
            }
            else if (type == ELevelRule.BBSUnDigest)
            {
                ruleInfo.CreditNum = -5;
            }
            else if (type == ELevelRule.BBSDeletePost)
            {
                ruleInfo.CreditNum = -1;
                ruleInfo.CashNum = -1;
            }
            else if (type == ELevelRule.BBSDeleteReply)
            {
                ruleInfo.CreditNum = -1;
                ruleInfo.CashNum = -1;
            }
            //PlatForm
            else if (type == ELevelRule.PlatFormSetAvatar)
            {
                ruleInfo.PeriodType = ELevelPeriodType.Once;
                ruleInfo.CreditNum = 1;
                ruleInfo.CashNum = 5;
            }
            else if (type == ELevelRule.PlatFormLogin)
            {
                ruleInfo.PeriodType = ELevelPeriodType.Everyday;
                ruleInfo.MaxNum = 1;
                ruleInfo.CashNum = 2;
            }
            return ruleInfo;
        }

        public static ELevelRule GetEnumType(string typeStr)
        {
            //BBS
            ELevelRule retval = ELevelRule.BBSDigest;

            if (Equals(ELevelRule.BBSDigest, typeStr))
            {
                retval = ELevelRule.BBSDigest;
            }
            else if (Equals(ELevelRule.BBSPost, typeStr))
            {
                retval = ELevelRule.BBSPost;
            }
            else if (Equals(ELevelRule.BBSReply, typeStr))
            {
                retval = ELevelRule.BBSReply;
            }
            else if (Equals(ELevelRule.BBSUnDigest, typeStr))
            {
                retval = ELevelRule.BBSUnDigest;
            }
            else if (Equals(ELevelRule.BBSDeletePost, typeStr))
            {
                retval = ELevelRule.BBSDeletePost;
            }
            else if (Equals(ELevelRule.BBSDeleteReply, typeStr))
            {
                retval = ELevelRule.BBSDeleteReply;
            }
            //PlatForm
            else if (Equals(ELevelRule.PlatFormAttachUpload, typeStr))
            {
                retval = ELevelRule.PlatFormAttachUpload;
            }
            else if (Equals(ELevelRule.PlatFormAttachDownload, typeStr))
            {
                retval = ELevelRule.PlatFormAttachDownload;
            }
            else if (Equals(ELevelRule.PlatFormSendPM, typeStr))
            {
                retval = ELevelRule.PlatFormSendPM;
            }
            else if (Equals(ELevelRule.PlatFormSearch, typeStr))
            {
                retval = ELevelRule.PlatFormSearch;
            }
            else if (Equals(ELevelRule.PlatFormSetAvatar, typeStr))
            {
                retval = ELevelRule.PlatFormSetAvatar;
            }
            else if (Equals(ELevelRule.PlatFormLogin, typeStr))
            {
                retval = ELevelRule.PlatFormLogin;
            }

            return retval;
        }

        public static bool Equals(ELevelRule type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ELevelRule type)
        {
            return Equals(type, typeStr);
        }

        public static ArrayList GetValueArrayList()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSDigest));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSPost));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSReply));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSUnDigest));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSDeletePost));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.BBSDeleteReply));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormAttachUpload));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormAttachDownload));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormSendPM));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormSearch));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormSetAvatar));
            arraylist.Add(ELevelRuleUtils.GetValue(ELevelRule.PlatFormLogin));

            return arraylist;
        }

        public static ArrayList GetArrayListOfForum()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(ELevelRule.BBSDigest);
            arraylist.Add(ELevelRule.BBSPost);
            arraylist.Add(ELevelRule.BBSReply);
            arraylist.Add(ELevelRule.BBSUnDigest);
            arraylist.Add(ELevelRule.BBSDeletePost);
            arraylist.Add(ELevelRule.BBSDeleteReply);
            arraylist.Add(ELevelRule.PlatFormAttachUpload);
            arraylist.Add(ELevelRule.PlatFormAttachDownload);

            return arraylist;
        }
    }

}
