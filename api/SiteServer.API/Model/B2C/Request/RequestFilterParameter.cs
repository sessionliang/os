using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteServer.API.Model.B2C
{
    public class RequestFilterParameter
    {
        public string Keywords { get; set; }
        public string Filters { get; set; }
        public string Order { get; set; }
        public string Page { get; set; }
        public string StlPageContents { get; set; }

        /// <summary>
        /// 是否有货
        /// </summary>
        public string HasGoods { get; set; }
        /// <summary>
        /// 支持货到付款
        /// </summary>
        public string IsCOD { get; set; }
        /// <summary>
        /// 是否跳转
        /// </summary>
        public string IsRedirect { get; set; }
        /// <summary>
        /// 全局搜索关键字
        /// </summary>
        public string FilterSearchWords { get; set; }
    }
}