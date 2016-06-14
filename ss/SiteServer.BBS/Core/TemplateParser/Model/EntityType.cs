using BaiRong.Core;

namespace SiteServer.BBS.Core.TemplateParser.Model
{

    public enum EntityType
	{
		BBS,					    //通用实体
        Forum,					    //板块实体
        Thread,
        Post,
        Sql,
        Unkown
	}

    public class EntityTypeUtils
	{
        public static string GetValue(EntityType type)
		{
            if (type == EntityType.BBS)
			{
                return "BBS";
			}
            else if (type == EntityType.Forum)
			{
                return "Forum";
			}
            else if (type == EntityType.Thread)
            {
                return "Thread";
            }
            else if (type == EntityType.Post)
            {
                return "Post";
            }
            else if (type == EntityType.Sql)
            {
                return "Sql";
            }
			else
			{
                return "Unkown";
			}
		}

        public static bool Equals(EntityType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static EntityType GetEntityType(string entity)
        {
            EntityType type = EntityType.Unkown;
            if (!string.IsNullOrEmpty(entity))
            {
                if (entity.ToLower().StartsWith("{bbs.forum."))
                {
                    return EntityType.Forum;
                }
                else if (entity.ToLower().StartsWith("{bbs.thread."))
                {
                    return EntityType.Thread;
                }
                else if (entity.ToLower().StartsWith("{bbs.post."))
                {
                    return EntityType.Post;
                }
                else if (entity.ToLower().StartsWith("{bbs.sql."))
                {
                    return EntityType.Sql;
                }
                else if (entity.ToLower().StartsWith("{bbs."))
                {
                    return EntityType.BBS;
                }
            }
            return type;
        }
	}
}
