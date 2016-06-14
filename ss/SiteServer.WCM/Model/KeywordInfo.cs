using System;

namespace SiteServer.WeiXin.Model
{
	public class KeywordInfo
	{
        private int keywordID;
        private int publishmentSystemID;
        private string keywords;
        private bool isDisabled;
        private EKeywordType keywordType;
        private EMatchType matchType;
        private string reply;
        private DateTime addDate;
        private int taxis;

		public KeywordInfo()
		{
            this.keywordID = 0;
            this.publishmentSystemID = 0;
            this.keywords = string.Empty;
            this.isDisabled = false;
            this.keywordType = EKeywordType.Text;
            this.matchType = EMatchType.Exact;
            this.reply = string.Empty;
            this.addDate = DateTime.Now;
            this.taxis = 0;
		}

        public KeywordInfo(int keywordID, int publishmentSystemID, string keywords, bool isDisabled, EKeywordType keywordType, EMatchType matchType, string reply, DateTime addDate, int taxis)
		{
            this.keywordID = keywordID;
            this.publishmentSystemID = publishmentSystemID;
            this.keywords = keywords;
            this.isDisabled = isDisabled;
            this.keywordType = keywordType;
            this.matchType = matchType;
            this.reply = reply;
            this.addDate = addDate;
            this.taxis = taxis;
		}

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public EKeywordType KeywordType
        {
            get { return keywordType; }
            set { keywordType = value; }
        }

        public EMatchType MatchType
        {
            get { return matchType; }
            set { matchType = value; }
        }

        public string Reply
        {
            get { return reply; }
            set { reply = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }
	}
}
