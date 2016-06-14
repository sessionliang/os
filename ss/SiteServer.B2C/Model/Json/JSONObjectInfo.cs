using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model.Json
{
    public class JSONObjectInfo : Dictionary<string, object>
    {
        public JSONObjectInfo(CaseExtendedAttributes attributes, List<JSONObjectInfo> list)
        {
            if (attributes != null && attributes.Attributes.Count > 0)
            {
                foreach (string key in attributes.Attributes.Keys)
                {
                    base.Add(key, attributes.GetExtendedAttribute(key));
                }
            }
            if (list != null && list.Count > 0)
            {
                base.Add("List", list);
            }
        }

        public JSONObjectInfo(CaseExtendedAttributes attributes, string list1Name, List<JSONObjectInfo> list1, string list2Name, List<JSONObjectInfo> list2)
        {
            if (attributes != null && attributes.Attributes.Count > 0)
            {
                foreach (string key in attributes.Attributes.Keys)
                {
                    base.Add(key, attributes.GetExtendedAttribute(key));
                }
            }
            if (!string.IsNullOrEmpty(list1Name) && list1 != null && list1.Count > 0)
            {
                base.Add(list1Name, list1);
            }
            if (!string.IsNullOrEmpty(list2Name) && list2 != null && list2.Count > 0)
            {
                base.Add(list2Name, list2);
            }
        }
    }
}
