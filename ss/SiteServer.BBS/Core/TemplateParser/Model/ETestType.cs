using BaiRong.Core;

namespace SiteServer.BBS.Core.TemplateParser.Model
{
    public enum ETestType
	{
        AddDate,
        ItemIndex,
        Undefined
	}

    public class ETestTypeUtils
	{
        public static string GetValue(ETestType type)
		{
            if (type == ETestType.AddDate)
            {
                return "AddDate";
            }
            else if (type == ETestType.ItemIndex)
            {
                return "ItemIndex";
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
                if (Equals(ETestType.AddDate, typeStr))
                {
                    return ETestType.AddDate;
                }
                else if (Equals(ETestType.ItemIndex, typeStr))
                {
                    return ETestType.ItemIndex;
                }
            }
            return type;
        }
	}
}
