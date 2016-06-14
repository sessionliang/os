using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model
{
    public class KeywordInfo
    {
        private int keywordID;
        private string keyword;
        private string alternative;
        private EKeywordGrade grade;
        private int classifyID;

        public KeywordInfo()
        {
            this.keywordID = 0;
            this.keyword = string.Empty;
            this.alternative = string.Empty;
            this.grade = EKeywordGrade.Normal;
            this.classifyID = 0;
        }

        public KeywordInfo(int keywordID, string keyword, string alternative, EKeywordGrade grade, int classifyID)
        {
            this.keywordID = keywordID;
            this.keyword = keyword;
            this.alternative = alternative;
            this.grade = grade;
            this.classifyID = classifyID;
        }

        public int KeywordID
        {
            get { return keywordID; }
            set { keywordID = value; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public string Alternative
        {
            get { return alternative; }
            set { alternative = value; }
        }

        public EKeywordGrade Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public int ClassifyID
        {
            get { return classifyID; }
            set { classifyID = value; }
        }
    }
}
