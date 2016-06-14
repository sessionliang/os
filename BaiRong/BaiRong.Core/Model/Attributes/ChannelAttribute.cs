using System;
using System.Collections;

namespace BaiRong.Model
{
	public class ChannelAttribute
	{
        private ChannelAttribute()
		{
		}
		
        //hidden
		public static string ID = "ID";
        public static string NodeID = "NodeID";
        public static string PublishmentSystemID = "PublishmentSystemID";

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList();
                    hiddenAttributes.Add(ID.ToLower());
                    hiddenAttributes.Add(NodeID.ToLower());
                    hiddenAttributes.Add(PublishmentSystemID.ToLower());
                }

                return hiddenAttributes;
            }
        }
		
	}
}
