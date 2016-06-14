using System;
using BaiRong.Model;
using BaiRong.Core;

namespace BaiRong.Model
{
    public class AdministratorInfo
    {
        private string userName;
        private string password;
        private EPasswordFormat passwordFormat;
        private string passwordSalt;
        private DateTime creationDate;
        private DateTime lastActivityDate;
        private string lastProductID;
        private int countOfLogin;
        private string creatorUserName;
        private bool isLockedOut;
        private string publishmentSystemIDCollection;
        private int publishmentSystemID;
        private int departmentID;
        private int areaID;
        private string displayName;
        private string question;
        private string answer;
        private string email;
        private string mobile;
        private string theme;
        private string language;

        public AdministratorInfo()
        {
            this.userName = string.Empty;
            this.password = string.Empty;
            this.passwordFormat = EPasswordFormat.Encrypted;
            this.passwordSalt = string.Empty;
            this.creationDate = DateUtils.SqlMinValue;
            this.lastActivityDate = DateUtils.SqlMinValue;
            this.lastProductID = string.Empty;
            this.countOfLogin = 0;
            this.creatorUserName = string.Empty;
            this.isLockedOut = false;
            this.publishmentSystemIDCollection = string.Empty;
            this.publishmentSystemID = 0;
            this.departmentID = 0;
            this.areaID = 0;
            this.displayName = string.Empty;
            this.question = string.Empty;
            this.answer = string.Empty;
            this.email = string.Empty;
            this.mobile = string.Empty;
            this.theme = string.Empty;
            this.language = string.Empty;
        }

        public AdministratorInfo(string userName, string password, EPasswordFormat passwordFormat, string passwordSalt, DateTime creationDate, DateTime lastActivityDate, string lastProductID, int countOfLogin, string creatorUserName, bool isLockedOut, string publishmentSystemIDCollection, int publishmentSystemID, int departmentID, int areaID, string displayName, string question, string answer, string email, string mobile, string theme, string language)
        {
            this.userName = userName;
            this.password = password;
            this.passwordFormat = passwordFormat;
            this.passwordSalt = passwordSalt;
            this.creationDate = creationDate;
            this.lastActivityDate = lastActivityDate;
            this.lastProductID = lastProductID;
            this.countOfLogin = countOfLogin;
            this.creatorUserName = creatorUserName;
            this.isLockedOut = isLockedOut;
            this.publishmentSystemIDCollection = publishmentSystemIDCollection;
            this.publishmentSystemID = publishmentSystemID;
            this.departmentID = departmentID;
            this.areaID = areaID;
            this.displayName = displayName;
            this.question = question;
            this.answer = answer;
            this.email = email;
            this.mobile = mobile;
            this.theme = theme;
            this.language = language;
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public EPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
            set { passwordFormat = value; }
        }

        public string PasswordSalt
        {
            get { return passwordSalt; }
            set { passwordSalt = value; }
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        public string LastProductID
        {
            get { return lastProductID; }
            set { lastProductID = value; }
        }

        public int CountOfLogin
        {
            get { return countOfLogin; }
            set { countOfLogin = value; }
        }

        public string CreatorUserName
        {
            get { return creatorUserName; }
            set { creatorUserName = value; }
        }

        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        public string PublishmentSystemIDCollection
        {
            get { return publishmentSystemIDCollection; }
            set { publishmentSystemIDCollection = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public int DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }

        public int AreaID
        {
            get { return areaID; }
            set { areaID = value; }
        }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = userName;
                }
                return displayName;
            }
            set { displayName = value; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Theme
        {
            get { return theme; }
            set { theme = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }
    }
}
