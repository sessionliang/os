using System;

namespace SiteServer.WeiXin.Model
{
	public class KeywordMatchInfo
	{
        private int matchID;
        private int publishmentSystemID;
        private string keyword;
        private int keywordID;
        private bool isDisabled;
        private EKeywordType keywordType;
        private EMatchType matchType;

		public KeywordMatchInfo()
		{
            this.matchID = 0;
            this.publishmentSystemID = 0;
            this.keyword = string.Empty;
            this.keywordID = 0;
            this.isDisabled = false;
            this.keywordType = EKeywordType.Text;
            this.matchType = EMatchType.Exact;
		}

        public KeywordMatchInfo(int matchID, int publishmentSystemID, string keyword, int keywordID, bool isDisabled, EKeywordType keywordType, EMatchType matchType)
		{
            this.matchID = matchID;
            this.publishmentSystemID = publishmentSystemID;
            this.keyword = keyword;
            this.keywordID = keywordID;
            this.isDisabled = isDisabled;
            this.keywordType = keywordType;
            this.matchType = matchType;
		}

        public int MatchID
        {
            get { return matchID; }
            set { matchID = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
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
	}
}
