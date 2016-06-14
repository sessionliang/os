using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BaiRong.Core.JWT
{
    public class JWTAdministrator
    {
        public string userName { get; set; }
        public string displayName { get; set; }

        public Dictionary<string, string> config { get; set; }

        public JWTAdministrator() { }

        public JWTAdministrator(Model.AdministratorInfo administratorInfo)
        {
            this.userName = administratorInfo.UserName;
            this.displayName = administratorInfo.DisplayName;
            if (string.IsNullOrEmpty(this.displayName))
            {
                this.displayName = administratorInfo.Email;
            }
            if (string.IsNullOrEmpty(this.displayName))
            {
                this.displayName = administratorInfo.UserName;
            }
        }
    }
}
