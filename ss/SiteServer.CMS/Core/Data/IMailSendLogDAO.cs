using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
	public interface IMailSendLogDAO
	{
        void Insert(MailSendLogInfo log);

        void Delete(ArrayList idArrayList);

        void DeleteAll();

        string GetSelectCommend();

        string GetSelectCommend(int publishmentSystemID, string keyword, string dateFrom, string dateTo);
	}
}
