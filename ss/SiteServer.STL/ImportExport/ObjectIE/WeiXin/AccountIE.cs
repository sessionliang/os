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
    internal class AccountIE
    {

        private readonly int publishmentSystemID;
        private readonly string filePath;

        public AccountIE(int publishmentSystemID, string filePath)
        {
            this.publishmentSystemID = publishmentSystemID;
            this.filePath = filePath;
        }

        public void Export()
        {
            List<AccountInfo> accountInfoList = DataProviderWX.AccountDAO.GetAccountInfoList(this.publishmentSystemID);

            AtomFeed feed = AtomUtility.GetEmptyFeed();

            foreach (AccountInfo accountInfo in accountInfoList)
            {
                AtomEntry entry = ExportAccountInfo(accountInfo);
                feed.Entries.Add(entry);
            }

            feed.Save(filePath);
        }

        private static AtomEntry ExportAccountInfo(AccountInfo accountInfo)
        {
            AtomEntry entry = AtomUtility.GetEmptyEntry();

            foreach (string attributeName in AccountAttribute.AllAttributes)
            {
                object valueObj = accountInfo.GetValue(attributeName);
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
            if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

            foreach (AtomEntry entry in feed.Entries)
            {
                AccountInfo accountInfo = new AccountInfo();

                foreach (string attributeName in AccountAttribute.AllAttributes)
                {
                    string value = AtomUtility.Decrypt(AtomUtility.GetDcElementContent(entry.AdditionalElements, attributeName));
                    accountInfo.SetValue(attributeName, value);
                }

                accountInfo.PublishmentSystemID = this.publishmentSystemID;
                DataProviderWX.AccountDAO.Insert(accountInfo);
            }
        }

    }
}
