using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class CartTemplateInfo : CaseExtendedAttributes
    {
        //是否存在新增商品
        public bool isNew
        {
            get { return base.GetBool("isNew", false); }
            set { base.SetExtendedAttribute("isNew", value); }
        }

        //新增商品名称
        public string newTitle
        {
            get { return base.GetString("newTitle", string.Empty); }
            set { base.SetExtendedAttribute("newTitle", value); }
        }

        //购物列表
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
