using BaiRong.Core;

namespace SiteServer.STL.Parser.Model
{
	
	public enum ETestType
	{
        ChannelName,
        ChannelIndex,
        TemplateName,
        TemplateType,
        TopLevel,
        UpChannel,
        UpChannelOrSelf,
        GroupChannel,
        GroupContent,
        AddDate,
        LastEditDate,
        ItemIndex,
        OddItem,
        IsAnonymous,
        IsCart,
        IsCurrent,
        Undefined
	}

    public class ETestTypeUtils
	{
        public static string GetValue(ETestType type)
		{
            if (type == ETestType.ChannelName)
			{
                return "ChannelName";
            }
            else if (type == ETestType.ChannelIndex)
            {
                return "ChannelIndex";
            }
            else if (type == ETestType.TemplateName)
			{
                return "TemplateName";
            }
            else if (type == ETestType.TemplateType)
            {
                return "TemplateType";
            }
            else if (type == ETestType.TopLevel)
			{
                return "TopLevel";
            }
            else if (type == ETestType.UpChannel)
            {
                return "UpChannel";
            }
            else if (type == ETestType.UpChannelOrSelf)
            {
                return "UpChannelOrSelf";
            }
            else if (type == ETestType.GroupChannel)
            {
                return "GroupChannel";
            }
            else if (type == ETestType.GroupContent)
            {
                return "GroupContent";
            }
            else if (type == ETestType.AddDate)
            {
                return "AddDate";
            }
            else if (type == ETestType.LastEditDate)
            {
                return "LastEditDate";
            }
            else if (type == ETestType.ItemIndex)
            {
                return "ItemIndex";
            }
            else if (type == ETestType.OddItem)
            {
                return "OddItem";
            }
            else if (type == ETestType.IsAnonymous)
            {
                return "IsAnonymous";
            }
            else if (type == ETestType.IsCart)
            {
                return "IsCart";
            }
            else if (type == ETestType.IsCurrent)
            {
                return "IsCurrent";
            }
			else
			{
                return "Undefined";
			}
		}

		public static bool Equals(ETestType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ETestType type)
        {
            return Equals(type, typeStr);
        }

        public static ETestType GetEnumType(string typeStr)
        {
            ETestType type = ETestType.Undefined;

            if (!string.IsNullOrEmpty(typeStr))
            {
                if (Equals(ETestType.ChannelName, typeStr))
                {
                    return ETestType.ChannelName;
                }
                else if (Equals(ETestType.ChannelIndex, typeStr))
                {
                    return ETestType.ChannelIndex;
                }
                else if (Equals(ETestType.TemplateName, typeStr))
                {
                    return ETestType.TemplateName;
                }
                else if (Equals(ETestType.TemplateType, typeStr))
                {
                    return ETestType.TemplateType;
                }
                else if (Equals(ETestType.TopLevel, typeStr))
                {
                    return ETestType.TopLevel;
                }
                else if (Equals(ETestType.UpChannel, typeStr))
                {
                    return ETestType.UpChannel;
                }
                else if (Equals(ETestType.UpChannelOrSelf, typeStr))
                {
                    return ETestType.UpChannelOrSelf;
                }
                else if (Equals(ETestType.GroupChannel, typeStr))
                {
                    return ETestType.GroupChannel;
                }
                else if (Equals(ETestType.GroupContent, typeStr))
                {
                    return ETestType.GroupContent;
                }
                else if (Equals(ETestType.AddDate, typeStr))
                {
                    return ETestType.AddDate;
                }
                else if (Equals(ETestType.LastEditDate, typeStr))
                {
                    return ETestType.LastEditDate;
                }
                else if (Equals(ETestType.ItemIndex, typeStr))
                {
                    return ETestType.ItemIndex;
                }
                else if (Equals(ETestType.OddItem, typeStr))
                {
                    return ETestType.OddItem;
                }
                else if (Equals(ETestType.IsAnonymous, typeStr))
                {
                    return ETestType.IsAnonymous;
                }
                else if (Equals(ETestType.IsCart, typeStr))
                {
                    return ETestType.IsCart;
                }
                else if (Equals(ETestType.IsCurrent, typeStr))
                {
                    return ETestType.IsCurrent;
                }
            }
            return type;
        }
	}
}
