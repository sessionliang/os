using System;

namespace BaiRong.Model
{
    public class UserSecurityQuestionInfo
    {
        private int id;
        private string question;
        private bool isEnable;


        public UserSecurityQuestionInfo()
        {
            this.id = 0;
            this.question = string.Empty;
            this.isEnable = true;

        }

        public UserSecurityQuestionInfo(int id, string question, bool isEnable)
        {
            this.id = id;
            this.question = question;
            this.isEnable = isEnable;

        }


        public int ID
        {
            get { return id; }
            set { id = value; }
        }


        public string Question
        {
            get { return question; }
            set { question = value; }
        }


        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }
    }
}
