using SiteServer.B2C.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class FilterParameter
    {
        public IEnumerable<FilterInfo> Filters { get; set; }
        public string Order { get; set; }
        public IEnumerable<Dictionary<string, string>> Contents { get; set; }
        public PageItem PageItem { get; set; }
        public bool HasGoods { get; set; }
        public bool IsCOD { get; set; }
        public string ChannelName { get; set; }
    }
}