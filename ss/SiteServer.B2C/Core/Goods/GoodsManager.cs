using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using System.Collections;

using SiteServer.B2C.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;
using System.Text;
using System;
using SiteServer.CMS.Model;

namespace SiteServer.B2C.Core
{
	public class GoodsManager
	{
        public static string GetGoodsSN()
        {
            return StringUtils.GetShortGUID(true);
        }

        public static bool IsOnSale(GoodsContentInfo contentInfo, GoodsInfo goodsInfo)
        {
            if (contentInfo != null && goodsInfo != null)
            {
                if (contentInfo.IsChecked && goodsInfo.IsOnSale && goodsInfo.Stock != 0) return true;
            }
            return false;
        }
	}
}
