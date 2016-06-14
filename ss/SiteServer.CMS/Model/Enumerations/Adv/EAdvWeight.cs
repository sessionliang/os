using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
    public enum EAdvWeight 
    {
        Level1,       //1��
        Level2,       //2��
        Level3,       //3��
        Level4,       //4��
        Level5,       //5��
        Level6,       //6��
        Level7,       //7��
        Level8,       //8��
        Level9,       //9��
        Level10       //10��
    }

    public class EAdvWeightUtils
    {
        public static string GetValue(EAdvWeight  type)
        {
            if (type == EAdvWeight .Level1)
            {
                return "1";
            }
            else if (type == EAdvWeight .Level2)
            {
                return "2";
            }
            else if (type == EAdvWeight .Level3)
            {
                return "3";
            }
            else if (type == EAdvWeight .Level4)
            {
                return "4";
            }
            else if (type == EAdvWeight .Level5)
            {
                return "5";
            }
            else if (type == EAdvWeight .Level6)
            {
                return "6";
            }
            else if (type == EAdvWeight .Level7)
            {
                return "7";
            }
            else if (type == EAdvWeight .Level8)
            {
                return "8";
            }
            else if (type == EAdvWeight .Level9)
            {
                return "9";
            }
            else if (type == EAdvWeight .Level10)
            {
                return "10";
            }
           else
            {
                throw new Exception();
            }
        }

        public static string GetText(EAdvWeight  type)
        {
            if (type == EAdvWeight .Level1)
            {
                return "1";
            }
            else if (type == EAdvWeight .Level2)
            {
                return "2";
            }
            else if (type == EAdvWeight .Level3)
            {
                return "3";
            }
            else if (type == EAdvWeight .Level4)
            {
                return "4";
            }
            else if (type == EAdvWeight .Level5)
            {
                return "5";
            }
            else if (type == EAdvWeight .Level6)
            {
                return "6";
            }
            else if (type == EAdvWeight .Level7)
            {
                return "7";
            }
            else if (type == EAdvWeight .Level8)
            {
                return "8";
            }
            else if (type == EAdvWeight .Level9)
            {
                return "9";
            }
            else if (type == EAdvWeight .Level10)
            {
                return "10";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EAdvWeight  GetEnumType(string typeStr)
        {
            EAdvWeight  retval = EAdvWeight .Level1;

            if (Equals(EAdvWeight .Level1, typeStr))
            {
                retval = EAdvWeight .Level1;
            }
            else if (Equals(EAdvWeight .Level2, typeStr))
            {
                retval = EAdvWeight .Level2;
            }
            else if (Equals(EAdvWeight .Level3, typeStr))
            {
                retval = EAdvWeight .Level3;
            }
            else if (Equals(EAdvWeight .Level4, typeStr))
            {
                retval = EAdvWeight .Level4;
            }
            else if (Equals(EAdvWeight .Level5, typeStr))
            {
                retval = EAdvWeight .Level5;
            }
            else if (Equals(EAdvWeight .Level6, typeStr))
            {
                retval = EAdvWeight .Level6;
            }
            else if (Equals(EAdvWeight .Level7, typeStr))
            {
                retval = EAdvWeight .Level7;
            }
            else if (Equals(EAdvWeight .Level8, typeStr))
            {
                retval = EAdvWeight .Level8;
            }
            else if (Equals(EAdvWeight .Level9, typeStr))
            {
                retval = EAdvWeight .Level9;
            }
            else if (Equals(EAdvWeight .Level10, typeStr))
            {
                retval = EAdvWeight .Level10;
            }
            return retval;
        }

        public static bool Equals(EAdvWeight  type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EAdvWeight  type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EAdvWeight  type, bool selected)
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
                listControl.Items.Add(GetListItem(EAdvWeight .Level1, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level2, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level3, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level4, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level5, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level6, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level7, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level8, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level9, false));
                listControl.Items.Add(GetListItem(EAdvWeight .Level10, false));
            }
        }
    }
}
