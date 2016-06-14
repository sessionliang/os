using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class ConsigneeTemplateInfo : CaseExtendedAttributes
    {
        //列表
        private List<JSONObjectInfo> list = new List<JSONObjectInfo>();
        public List<JSONObjectInfo> List
        {
            get { return list; }
        }

        public JSONObjectInfo JSONObject
        {
            get
            {
                return new JSONObjectInfo(this, this.list);
            }
        }
    }
}
