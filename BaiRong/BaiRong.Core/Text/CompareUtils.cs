using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Drawing;
using BaiRong.Core;

namespace BaiRong.Core
{
	public class CompareUtils
	{
        public static bool Contains(string strCollection, string item)
        {
            bool contains = false;
            if (strCollection != null && strCollection.Length != 0 && item != null && item.Length != 0)
            {
                ArrayList arraylist = TranslateUtils.StringCollectionToArrayList(strCollection);
                contains = arraylist.Contains(item.Trim());
            }
            return contains;
        }

        public static bool ContainsInt(ICollection collection, int item)
        {
            bool contains = false;
            if (collection != null && collection.Count != 0)
            {
                foreach (int i in collection)
                {
                    if (i == item)
                    {
                        contains = true;
                        break;
                    }
                }
            }
            return contains;
        }
	}
}
