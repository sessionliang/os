using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	
	public enum ENodeType
	{
		BackgroundPublishNode,	//Ó¦ÓÃ
        BackgroundNormalNode,	//À¸Ä¿
	}

	public class ENodeTypeUtils
	{
		public static string GetValue(ENodeType type)
		{
			if (type == ENodeType.BackgroundPublishNode)
			{
				return "BackgroundPublishNode";
			}
			else if (type == ENodeType.BackgroundNormalNode)
			{
				return "BackgroundNormalNode";
            }
			else
			{
				throw new Exception();
			}
		}

		public static ENodeType GetEnumType(string typeStr)
		{
			ENodeType retval = ENodeType.BackgroundNormalNode;

			if (Equals(ENodeType.BackgroundPublishNode, typeStr))
			{
				retval = ENodeType.BackgroundPublishNode;
			}
			else if (Equals(ENodeType.BackgroundNormalNode, typeStr))
			{
				retval = ENodeType.BackgroundNormalNode;
            }

			return retval;
		}

		public static bool Equals(ENodeType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, ENodeType type)
		{
			return Equals(type, typeStr);
		}
	}
}
