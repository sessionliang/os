using BaiRong.Core;

namespace SiteServer.STL.Parser.Model
{
	
	public enum EStlEntityType
	{
		Stl,					    //通用实体
        StlElement,                 //STL元素实体
		Content,					//内容实体
		Channel,					//栏目实体
        Photo,                      //商品图片实体
        Comment,                    //评论实体
        Request,                    //参数获取实体
        Navigation,                 //导航地址
        Sql,                        //Sql实体
        User,                       //用户实体
        B2C,                       //B2C实体
        Unkown
	}

    public class EStlEntityTypeUtils
	{
        public const string REGEX_STRING_ALL = @"{stl\.[^{}]*}|{stl:[^{}]*}|{content\.[^{}]*}|{channel\.[^{}]*}|{comment\.[^{}]*}|{request\.[^{}]*}|{sql\.[^{}]*}|{user\.[^{}]*}|{navigation\.[^{}]*}|{photo\.[^{}]*}|{b2c\.[^{}]*}";

        public const string REGEX_STRING_SQL = @"{sql.[^{}]*}";

        public const string REGEX_STRING_USER = @"{user.[^{}]*}";

        public static string GetValue(EStlEntityType type)
		{
            if (type == EStlEntityType.Stl)
			{
                return "Stl";
            }
            else if (type == EStlEntityType.StlElement)
            {
                return "StlElement";
            }
            else if (type == EStlEntityType.Content)
			{
                return "Content";
			}
            else if (type == EStlEntityType.Channel)
			{
                return "Channel";
            }
            else if (type == EStlEntityType.Photo)
            {
                return "Photo";
            }
            else if (type == EStlEntityType.Comment)
            {
                return "Comment";
            }
            else if (type == EStlEntityType.Request)
            {
                return "Request";
            }
            else if (type == EStlEntityType.Navigation)
            {
                return "Navigation";
            }
            else if (type == EStlEntityType.Sql)
            {
                return "Sql";
            }
            else if (type == EStlEntityType.User)
            {
                return "User";
            }
            else if (type == EStlEntityType.B2C)
            {
                return "B2C";
            }
			else
			{
                return "Unkown";
			}
		}

		public static bool Equals(EStlEntityType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static EStlEntityType GetEntityType(string stlEntity)
        {
            EStlEntityType type = EStlEntityType.Unkown;
            if (!string.IsNullOrEmpty(stlEntity))
            {
                stlEntity = stlEntity.Trim().ToLower();

                if (stlEntity.StartsWith("{stl."))
                {
                    return EStlEntityType.Stl;
                }
                else if (stlEntity.StartsWith("{stl:"))
                {
                    return EStlEntityType.StlElement;
                }
                else if (stlEntity.StartsWith("{content."))
                {
                    return EStlEntityType.Content;
                }
                else if (stlEntity.StartsWith("{channel."))
                {
                    return EStlEntityType.Channel;
                }
                else if (stlEntity.StartsWith("{photo."))
                {
                    return EStlEntityType.Photo;
                }
                else if (stlEntity.StartsWith("{comment."))
                {
                    return EStlEntityType.Comment;
                }
                else if (stlEntity.StartsWith("{request."))
                {
                    return EStlEntityType.Request;
                }
                else if (stlEntity.StartsWith("{navigation."))
                {
                    return EStlEntityType.Navigation;
                }
                else if (stlEntity.StartsWith("{sql."))
                {
                    return EStlEntityType.Sql;
                }
                else if (stlEntity.StartsWith("{user."))
                {
                    return EStlEntityType.User;
                }
                else if (stlEntity.StartsWith("{b2c."))
                {
                    return EStlEntityType.B2C;
                }
            }
            return type;
        }
	}
}
