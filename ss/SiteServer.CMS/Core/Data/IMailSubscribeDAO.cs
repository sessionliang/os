using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IMailSubscribeDAO
	{
        void Insert(MailSubscribeInfo msInfo);

        void Delete(ArrayList idArrayList);

        bool IsExists(int publishmentSystemID, string mail);

        string GetSelectCommend();

        string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo);

        ArrayList GetMailSubscribeInfoArrayList(int publishmentSystemID);
	}
}
