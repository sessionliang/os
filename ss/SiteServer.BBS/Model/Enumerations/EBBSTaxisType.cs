using System;
using System.Text;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.BBS.Core.TemplateParser.Model;
using System.Collections;
using BaiRong.Core;
using System.Web.UI.WebControls;

namespace SiteServer.BBS.Model
{
    public enum EBBSTaxisType
    {
        OrderByID,
        OrderByIDDesc,
        OrderByAddDate,
        OrderByAddDateDesc,
        OrderByTaxis,
        OrderByTaxisDesc,
        OrderByHits,
        OrderByLastDate,
        OrderByRandom
    }

    public class EBBSTaxisTypeUtils
    {
        public static string GetValue(EBBSTaxisType type)
        {
            if (type == EBBSTaxisType.OrderByID)
            {
                return "OrderByID";
            }
            else if (type == EBBSTaxisType.OrderByIDDesc)
            {
                return "OrderByIDDesc";
            }
            else if (type == EBBSTaxisType.OrderByAddDate)
            {
                return "OrderByAddDate";
            }
            else if (type == EBBSTaxisType.OrderByAddDateDesc)
            {
                return "OrderByAddDateDesc";
            }
            else if (type == EBBSTaxisType.OrderByTaxis)
            {
                return "OrderByTaxis";
            }
            else if (type == EBBSTaxisType.OrderByTaxisDesc)
            {
                return "OrderByTaxisDesc";
            }
            else if (type == EBBSTaxisType.OrderByHits)
            {
                return "OrderByHits";
            }
            else if (type == EBBSTaxisType.OrderByLastDate)
            {
                return "OrderByLastDate";
            }
            else if (type == EBBSTaxisType.OrderByRandom)
            {
                return "OrderByRandom";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetOrderByString(EContextType contextType, EBBSTaxisType taxisType)
        {
            return GetOrderByString(contextType, taxisType, string.Empty, null);
        }

        public static string GetOrderByString(EContextType contextType, EBBSTaxisType taxisType, string orderByString, ArrayList orderedContentIDArrayList)
        {
            if (!string.IsNullOrEmpty(orderByString))
            {
                if (orderByString.Trim().ToUpper().StartsWith("ORDER BY "))
                {
                    return orderByString;
                }
                else
                {
                    return "ORDER BY " + orderByString;
                }
            }

            string retval = string.Empty;
            if (contextType == EContextType.Forum)
            {
                if (taxisType == EBBSTaxisType.OrderByID)
                {
                    retval = "ORDER BY ID ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByIDDesc)
                {
                    retval = "ORDER BY ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDate)
                {
                    retval = "ORDER BY AddDate ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDateDesc)
                {
                    retval = "ORDER BY AddDate DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxis)
                {
                    retval = "ORDER BY Taxis ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxisDesc)
                {
                    retval = "ORDER BY Taxis DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByHits)
                {
                    if (orderedContentIDArrayList != null && orderedContentIDArrayList.Count > 0)
                    {
                        orderedContentIDArrayList.Reverse();
                        retval = string.Format("ORDER BY CHARINDEX(CONVERT(VARCHAR,NodeID), '{0}') DESC, Taxis ASC", TranslateUtils.ObjectCollectionToString(orderedContentIDArrayList));
                    }
                    else
                    {
                        retval = "ORDER BY Taxis ASC";
                    }
                }
                else if (taxisType == EBBSTaxisType.OrderByRandom)
                {
                    retval = "ORDER BY NEWID() DESC";
                }
            }
            else if (contextType == EContextType.Thread)
            {
                if (taxisType == EBBSTaxisType.OrderByID)
                {
                    retval = "ORDER BY ID ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByIDDesc)
                {
                    retval = "ORDER BY ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDate)
                {
                    retval = "ORDER BY AddDate ASC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDateDesc)
                {
                    retval = "ORDER BY AddDate DESC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxis)
                {
                    retval = "ORDER BY Taxis ASC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxisDesc)
                {
                    retval = "ORDER BY Taxis DESC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByHits)
                {
                    retval = "ORDER BY Hits DESC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByLastDate)
                {
                    retval = "ORDER BY LastDate DESC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByRandom)
                {
                    retval = "ORDER BY NEWID() DESC";
                }
            }
            else if (contextType == EContextType.Post)
            {
                if (taxisType == EBBSTaxisType.OrderByID)
                {
                    retval = "ORDER BY ID ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByIDDesc)
                {
                    retval = "ORDER BY ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxis)
                {
                    retval = "ORDER BY Taxis ASC";
                }
                else if (taxisType == EBBSTaxisType.OrderByTaxisDesc)
                {
                    retval = "ORDER BY Taxis DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDate)
                {
                    retval = "ORDER BY AddDate ASC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByAddDateDesc)
                {
                    retval = "ORDER BY AddDate DESC, ID DESC";
                }
                else if (taxisType == EBBSTaxisType.OrderByRandom)
                {
                    retval = "ORDER BY NEWID() DESC";
                }
            }
            return retval;
        }

        public static string GetText(EBBSTaxisType type)
        {
            if (type == EBBSTaxisType.OrderByID)
            {
                return "内容ID（升序）";
            }
            else if (type == EBBSTaxisType.OrderByIDDesc)
            {
                return "内容ID（降序）";
            }
            else if (type == EBBSTaxisType.OrderByAddDate)
            {
                return "添加时间（升序）";
            }
            else if (type == EBBSTaxisType.OrderByAddDateDesc)
            {
                return "添加时间（降序）";
            }
            else if (type == EBBSTaxisType.OrderByTaxis)
            {
                return "自定义排序（反方向）";
            }
            else if (type == EBBSTaxisType.OrderByTaxisDesc)
            {
                return "自定义排序";
            }
            else if (type == EBBSTaxisType.OrderByHits)
            {
                return "点击量排序";
            }
            else if (type == EBBSTaxisType.OrderByLastDate)
            {
                return "最后回复排序";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EBBSTaxisType GetEnumType(string typeStr)
        {
            EBBSTaxisType retval = EBBSTaxisType.OrderByTaxisDesc;

            if (Equals(EBBSTaxisType.OrderByID, typeStr))
            {
                retval = EBBSTaxisType.OrderByID;
            }
            else if (Equals(EBBSTaxisType.OrderByIDDesc, typeStr))
            {
                retval = EBBSTaxisType.OrderByIDDesc;
            }
            else if (Equals(EBBSTaxisType.OrderByAddDate, typeStr))
            {
                retval = EBBSTaxisType.OrderByAddDate;
            }
            else if (Equals(EBBSTaxisType.OrderByAddDateDesc, typeStr))
            {
                retval = EBBSTaxisType.OrderByAddDateDesc;
            }
            else if (Equals(EBBSTaxisType.OrderByTaxis, typeStr))
            {
                retval = EBBSTaxisType.OrderByTaxis;
            }
            else if (Equals(EBBSTaxisType.OrderByTaxisDesc, typeStr))
            {
                retval = EBBSTaxisType.OrderByTaxisDesc;
            }
            else if (Equals(EBBSTaxisType.OrderByHits, typeStr))
            {
                retval = EBBSTaxisType.OrderByHits;
            }
            else if (Equals(EBBSTaxisType.OrderByLastDate, typeStr))
            {
                retval = EBBSTaxisType.OrderByLastDate;
            }

            return retval;
        }

        public static bool Equals(EBBSTaxisType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EBBSTaxisType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EBBSTaxisType type, bool selected)
        {
            ListItem item = new ListItem(GetText(type), GetValue(type));
            if (selected)
            {
                item.Selected = true;
            }
            return item;
        }

        public static void AddListItems(ListControl listControl)
        {
            if (listControl != null)
            {
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByID, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByIDDesc, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByAddDate, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByAddDateDesc, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByTaxis, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByTaxisDesc, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByHits, false));
                listControl.Items.Add(GetListItem(EBBSTaxisType.OrderByLastDate, false));
            }
        }

    }
}
