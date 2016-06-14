using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System;
using BaiRong.Model;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    public sealed class UserMessageManager
    {
        private UserMessageManager()
        {

        }

        //--------------消息数缓存----------------
        private const string cacheKey = "BaiRong.Core.MessageCounts";

        private static Hashtable GetActiveHashtable()
        {
            Hashtable ht = CacheUtils.Get(cacheKey) as Hashtable;
            if (ht == null)
            {
                ht = new Hashtable();
                CacheUtils.Insert(cacheKey, ht, null, CacheUtils.DayFactor);
            }
            return ht;
        }

        public static void RemoveCache(string userName)
        {
            Hashtable ht = GetActiveHashtable();

            lock (ht.SyncRoot)
            {
                ht.Remove(userName);
            }
        }

        private static void UpdateCache(IDictionary ht, string userName, string counts)
        {
            lock (ht.SyncRoot)
            {
                ht[userName] = counts;
            }
        }

        public static string[] GetMessageCounts(string userName)
        {
            string[] retval = new string[] { "0", "0" };

            Hashtable ht = GetActiveHashtable();

            string counts = ht[userName] as string;

            if (counts == null)
            {
                int unRead = BaiRongDataProvider.UserMessageDAO.GetUnReadedMessageCount(userName);
                int total = BaiRongDataProvider.UserMessageDAO.GetMessageCount(userName);

                counts = string.Format("{0},{1}", unRead, total);

                UpdateCache(ht, userName, counts);
            }

            if (!string.IsNullOrEmpty(counts))
            {
                retval = counts.Split(',');
            }

            return retval;
        }

        //--------------消息数缓存----------------

        public static void SendPrivateMessage(string messageFrom, string messageTo, string content)
        {
            DateTime addDate = DateTime.Now;
            UserMessageInfo messageInfo = new UserMessageInfo(0, messageFrom, messageTo, EUserMessageType.Private, 0, false, addDate, content, addDate, content);

            BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
        }

        public static void SendSystemMessage(string messageTo, string content)
        {
            DateTime addDate = DateTime.Now;
            UserMessageInfo messageInfo = new UserMessageInfo(0, string.Empty, messageTo, EUserMessageType.System, 0, false, addDate, content, addDate, content);

            BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
        }

        public static void SendSystemMessage(ArrayList messageToArrar, string title, string content)
        {
            foreach (string messageTo in messageToArrar)
            {
                SendSystemMessage(messageTo, title, content);
            }
        }


        public static void SendSystemMessage(string messageTo, string title, string content)
        {
            DateTime addDate = DateTime.Now;
            UserMessageInfo messageInfo = new UserMessageInfo(0, string.Empty, messageTo, EUserMessageType.System, 0, false, addDate, content, addDate, content, title);

            BaiRongDataProvider.UserMessageDAO.Insert(messageInfo);
        }
    }
}
