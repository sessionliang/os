using Atom.Core;
using BaiRong.Core;
using SiteServer.WeiXin.Core;
using SiteServer.WeiXin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.STL.ImportExport
{
    internal class MessageIE
    {
        private readonly int publishmentSystemID;
        private readonly string directoryPath;

        public MessageIE(int publishmentSystemID, string directoryPath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.directoryPath = directoryPath;
        }

        public void Export()
        {
            List<MessageInfo> messageInfoList = DataProviderWX.MessageDAO.GetMessageInfoList(this.publishmentSystemID);

            string messageDirectoryPath = PathUtils.Combine(this.directoryPath);

            DirectoryUtils.CreateDirectoryIfNotExists(messageDirectoryPath);

            foreach (MessageInfo messageInfo in messageInfoList)
            {
                string filePath = PathUtils.Combine(directoryPath, messageInfo.ID + ".xml");

                AtomFeed feed = ExportMessageInfo(messageInfo);

                string keywordName = DataProviderWX.KeywordDAO.GetKeywordInfo(messageInfo.KeywordID).Keywords;
                AtomUtility.AddDcElement(feed.AdditionalElements, "KeywordName", AtomUtility.Encrypt(keywordName));

                feed.Save(filePath);

                string messageContentDirectoryPath = PathUtils.Combine(messageDirectoryPath, messageInfo.ID.ToString());

                DirectoryUtils.CreateDirectoryIfNotExists(messageContentDirectoryPath);

                List<MessageContentInfo> messageContentInfoList = DataProviderWX.MessageContentDAO.GetMessageContentInfoList(this.publishmentSystemID, messageInfo.ID);

                foreach (MessageContentInfo messageContentInfo in messageContentInfoList)
                {
                    string fileMessgeContentPath = PathUtils.Combine(messageContentDirectoryPath, messageContentInfo.ID + ".xml");

                    AtomFeed feedMessageContent = ExportMessageContentInfoFeed(messageContentInfo);

                    List<MessageContentInfo> messageContentInfoListEntry = DataProviderWX.MessageContentDAO.GetReplyContentInfoList(messageInfo.ID, messageContentInfo.ID);

                    foreach (MessageContentInfo messageContentInfoEntry in messageContentInfoListEntry)
                    {
                        AtomEntry entry = ExportMessageContentInfoEntry(messageContentInfoEntry);
                        feedMessageContent.Entries.Add(entry);
                    }

                    feedMessageContent.Save(fileMessgeContentPath);
                }
            }
        }
        private static AtomFeed ExportMessageInfo(MessageInfo messageInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in MessageAttribute.AllAttributes)
            {
                object valueObj = messageInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomFeed ExportMessageContentInfoFeed(MessageContentInfo messageContentInfo)
        {
            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (string attributeName in MessageContentAttribute.AllAttributes)
            {
                object valueObj = messageContentInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(feed.AdditionalElements, attributeName, value);
            }

            return feed;
        }
        private static AtomEntry ExportMessageContentInfoEntry(MessageContentInfo messageContentInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in MessageContentAttribute.AllAttributes)
            {
                object valueObj = messageContentInfo.GetValue(attributeName);
                string value = string.Empty;
                if (valueObj != null)
                {
                    value = AtomUtility.Encrypt(valueObj.ToString());
                }
                AtomUtility.AddDcElement(entry.AdditionalElements, attributeName, value);
            }

            return entry;
        }
        public void Import()
        {
            if (!DirectoryUtils.IsDirectoryExists(this.directoryPath)) return;

            string[] filePaths = DirectoryUtils.GetFilePaths(this.directoryPath);

            foreach (string filePath in filePaths)
            {

                AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

                MessageInfo messageInfo = new MessageInfo();

                foreach (string attributeNames in MessageAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, attributeNames));
                    messageInfo.SetValue(attributeNames, value);
                }

                messageInfo.PublishmentSystemID = this.publishmentSystemID;

                string keywordName = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feed.AdditionalElements, "KeywordName"));
                messageInfo.KeywordID = DataProviderWX.KeywordDAO.GetKeywordsIDbyName(this.publishmentSystemID,keywordName);

                int messageID = DataProviderWX.MessageDAO.Insert(messageInfo);

                if (!DirectoryUtils.IsDirectoryExists(this.directoryPath + PathUtils.SeparatorChar + messageInfo.ID)) return;

                string dicMessagePath = DirectoryUtils.GetDirectoryPath(this.directoryPath + PathUtils.SeparatorChar + messageInfo.ID);

                string[] fileMessagePaths = DirectoryUtils.GetFilePaths(dicMessagePath);

                foreach (string fileMessagePath in fileMessagePaths)
                {
                    AtomFeed feedMessage = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(fileMessagePath));

                    MessageContentInfo messageContentInfo = new MessageContentInfo();

                    foreach (string attributeNames in MessageContentAttribute.AllAttributes)
                    {
                        string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(feedMessage.AdditionalElements, attributeNames));
                        messageContentInfo.SetValue(attributeNames, value);
                    }
                    messageContentInfo.MessageID = messageID;
                    messageContentInfo.PublishmentSystemID = this.publishmentSystemID;

                    int messageContentID = DataProviderWX.MessageContentDAO.Insert(messageContentInfo);
    
                    foreach (AtomEntry entry in feedMessage.Entries)
                    {
                        MessageContentInfo messageContentInfoEntry = new MessageContentInfo();

                        foreach (string attributeNames in MessageContentAttribute.AllAttributes)
                        {
                            string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeNames));
                            messageContentInfoEntry.SetValue(attributeNames, value);
                        }

                        messageContentInfoEntry.MessageID = messageID;
                        messageContentInfoEntry.ReplyID = messageContentID;
                        messageContentInfoEntry.PublishmentSystemID = this.publishmentSystemID;

                        DataProviderWX.MessageContentDAO.Insert(messageContentInfoEntry);
                    }
                }
            }
        }
    }
}
