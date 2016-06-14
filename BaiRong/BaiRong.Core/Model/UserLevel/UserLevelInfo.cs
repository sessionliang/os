using System;
using BaiRong.Model;

namespace BaiRong.Model
{
    public class UserLevelInfo
    {
        private int id;
        private string levelSN;
        private string levelName;
        private EUserLevelType levelType;
        private int minNum;
        private int maxNum;
        private int stars;
        private string color;
        private string extendValues;

        public UserLevelInfo(int id, string levelSN, string levelName, EUserLevelType levelType, int minNum, int maxNum, int stars, string color, string extendValues)
        {
            this.id = id;
            this.levelSN = levelSN;
            this.levelName = levelName;
            this.levelType = levelType;
            this.minNum = minNum;
            this.maxNum = maxNum;
            this.stars = stars;
            this.color = color;
            this.extendValues = extendValues;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string LevelSN
        {
            get { return levelSN; }
            set { levelSN = value; }
        }

        public string LevelName
        {
            get { return levelName; }
            set { levelName = value; }
        }

        public EUserLevelType LevelType
        {
            get { return levelType; }
            set { levelType = value; }
        }

        public int MinNum
        {
            get { return minNum; }
            set { minNum = value; }
        }

        public int MaxNum
        {
            get { return maxNum; }
            set { maxNum = value; }
        }

        public int Stars
        {
            get { return stars; }
            set { stars = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string ExtendValues
        {
            get
            {
                return extendValues;
            }
            set
            {
                extendValues = value;
            }
        }

        UserLevelInfoExtend additional;
        public UserLevelInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new UserLevelInfoExtend(this.extendValues);
                }
                return this.additional;
            }
            set
            {
                this.additional = value;
            }
        }
    }
}
