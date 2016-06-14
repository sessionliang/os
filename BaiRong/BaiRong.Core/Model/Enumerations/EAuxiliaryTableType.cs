using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model
{
	
	public enum EAuxiliaryTableType
	{
        BackgroundContent,	    //内容
        GovPublicContent,	    //信息公开
        GovInteractContent,     //互动交流
        VoteContent,            //投票
        JobContent,	            //招聘
        GoodsContent,	        //商品
        BrandContent,           //品牌
        ManuscriptContent,      //稿件
        UserDefined             //自定义
	}

	public class EAuxiliaryTableTypeUtils
	{
		public static string GetValue(EAuxiliaryTableType type)
		{
			if (type == EAuxiliaryTableType.BackgroundContent)
			{
				return "BackgroundContent";
            }
            else if (type == EAuxiliaryTableType.GovPublicContent)
            {
                return "GovPublicContent";
            }
            else if (type == EAuxiliaryTableType.GovInteractContent)
            {
                return "GovInteractContent";
            }
            else if (type == EAuxiliaryTableType.VoteContent)
            {
                return "VoteContent";
            }
            else if (type == EAuxiliaryTableType.JobContent)
            {
                return "JobContent";
            }
            else if (type == EAuxiliaryTableType.GoodsContent)
            {
                return "GoodsContent";
            }
            else if (type == EAuxiliaryTableType.BrandContent)
            {
                return "BrandContent";
            }
            else if (type == EAuxiliaryTableType.ManuscriptContent)
            {
                return "ManuscriptContent";
            }
            else if (type == EAuxiliaryTableType.UserDefined)
            {
                return "UserDefined";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EAuxiliaryTableType type)
		{
			if (type == EAuxiliaryTableType.BackgroundContent)
			{
                return "内容";
            }
            else if (type == EAuxiliaryTableType.GovPublicContent)
            {
                return "信息公开";
            }
            else if (type == EAuxiliaryTableType.GovInteractContent)
            {
                return "互动交流";
            }
            else if (type == EAuxiliaryTableType.VoteContent)
            {
                return "投票";
            }
            else if (type == EAuxiliaryTableType.JobContent)
            {
                return "招聘";
            }
            else if (type == EAuxiliaryTableType.GoodsContent)
            {
                return "商品";
            }
            else if (type == EAuxiliaryTableType.BrandContent)
            {
                return "品牌";
            }
            else if (type == EAuxiliaryTableType.ManuscriptContent)
            {
                return "稿件";
            }
            else if (type == EAuxiliaryTableType.UserDefined)
            {
                return "自定义";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EAuxiliaryTableType GetEnumType(string typeStr)
		{
            EAuxiliaryTableType retval = EAuxiliaryTableType.BackgroundContent;

			if (Equals(EAuxiliaryTableType.BackgroundContent, typeStr))
			{
				retval = EAuxiliaryTableType.BackgroundContent;
            }
            else if (Equals(EAuxiliaryTableType.GovPublicContent, typeStr))
            {
                retval = EAuxiliaryTableType.GovPublicContent;
            }
            else if (Equals(EAuxiliaryTableType.GovInteractContent, typeStr))
            {
                retval = EAuxiliaryTableType.GovInteractContent;
            }
            else if (Equals(EAuxiliaryTableType.VoteContent, typeStr))
            {
                retval = EAuxiliaryTableType.VoteContent;
            }
            else if (Equals(EAuxiliaryTableType.JobContent, typeStr))
            {
                retval = EAuxiliaryTableType.JobContent;
            }
            else if (Equals(EAuxiliaryTableType.GoodsContent, typeStr))
            {
                retval = EAuxiliaryTableType.GoodsContent;
            }
            else if (Equals(EAuxiliaryTableType.BrandContent, typeStr))
            {
                retval = EAuxiliaryTableType.BrandContent;
            }
            else if (Equals(EAuxiliaryTableType.ManuscriptContent, typeStr))
            {
                retval = EAuxiliaryTableType.ManuscriptContent;
            }
            else if (Equals(EAuxiliaryTableType.UserDefined, typeStr))
            {
                retval = EAuxiliaryTableType.UserDefined;
            }

			return retval;
		}

		public static bool Equals(EAuxiliaryTableType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EAuxiliaryTableType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(EAuxiliaryTableType type, bool selected)
		{
			ListItem item = new ListItem(GetText(type), GetValue(type));
			if (selected)
			{
				item.Selected = true;
			}
			return item;
		}

        public static ETableStyle GetTableStyle(EAuxiliaryTableType tableType)
        {
            if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                return ETableStyle.GovPublicContent;
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                return ETableStyle.GovInteractContent;
            }
            else if (tableType == EAuxiliaryTableType.VoteContent)
            {
                return ETableStyle.VoteContent;
            }
            else if (tableType == EAuxiliaryTableType.JobContent)
            {
                return ETableStyle.JobContent;
            }
            else if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                return ETableStyle.GoodsContent;
            }
            else if (tableType == EAuxiliaryTableType.BrandContent)
            {
                return ETableStyle.BrandContent;
            }
            else if (tableType == EAuxiliaryTableType.ManuscriptContent)
            {
                return ETableStyle.ManuscriptContent;
            }
            else if (tableType == EAuxiliaryTableType.UserDefined)
            {
                return ETableStyle.UserDefined;
            }
            return ETableStyle.BackgroundContent;
        }

        public static string GetDefaultTableName(EAuxiliaryTableType tableType)
        {
            if (tableType == EAuxiliaryTableType.GoodsContent)
            {
                return "model_B2C_Goods";
            }
            else if (tableType == EAuxiliaryTableType.BrandContent)
            {
                return "model_B2C_Brand";
            }
            else if (tableType == EAuxiliaryTableType.GovPublicContent)
            {
                return "model_WCM_GovPublic";
            }
            else if (tableType == EAuxiliaryTableType.GovInteractContent)
            {
                return "model_WCM_GovInteract";
            }
            else if (tableType == EAuxiliaryTableType.VoteContent)
            {
                return "model_Vote";
            }
            else if (tableType == EAuxiliaryTableType.JobContent)
            {
                return "model_Job";
            }
            else if (tableType == EAuxiliaryTableType.UserDefined)
            {
                return "model_UserDefined";
            }
            else if (tableType == EAuxiliaryTableType.ManuscriptContent)
            {
                return "ml_content";
            }
            return "model_Content";
        }
	}
}
