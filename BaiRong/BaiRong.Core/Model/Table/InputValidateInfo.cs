using System;

namespace BaiRong.Model
{
	public class InputValidateInfo
    {
        private bool isRequire;
        private int minNum; 
        private int maxNum;
        private string regExp;
        private string errorMessage;

		public InputValidateInfo()
		{
            this.isRequire = false;
            this.minNum = 0;
            this.maxNum = 0;
            this.regExp = string.Empty;
            this.errorMessage = string.Empty;
		}

        public InputValidateInfo(bool isRequire, int minNum, int maxNum, string regExp, string errorMessage) 
		{
            this.isRequire = isRequire;
            this.minNum = minNum;
            this.maxNum = maxNum;
            this.regExp = regExp;
            this.errorMessage = errorMessage;
		}

        public bool IsRequire
        {
            get { return isRequire; }
            set { isRequire = value; }
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

        public string RegExp
        {
            get { return regExp; }
            set { regExp = value; }
        }

        public string ErrorMessage
		{
            get { return errorMessage; }
            set { errorMessage = value; }
		}
	}
}
