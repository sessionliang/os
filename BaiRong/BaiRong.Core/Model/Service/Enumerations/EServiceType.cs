using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace BaiRong.Model.Service
{
    public enum EServiceType
	{
		Create,	        //生成
        Publish,	    //发布
        Gather,			//采集
        Backup,			//备份
        Check,          //审核
        UnCheck,         //下架
        Subscribe         //订阅  by 20151106 sofuny
	}

    public class EServiceTypeUtils
	{
		public static string GetValue(EServiceType type)
		{
            if (type == EServiceType.Create)
			{
                return "Create";
			}
            else if (type == EServiceType.Publish)
			{
                return "Publish";
			}
            else if (type == EServiceType.Gather)
			{
                return "Gather";
			}
            else if (type == EServiceType.Backup)
			{
                return "Backup";
			}
            else if (type == EServiceType.Check)
            {
                return "Check";
            }
            else if (type == EServiceType.UnCheck)
            {
                return "UnCheck";
            }
            else if (type == EServiceType.Subscribe)
            {
                return "Subscribe";
            }
			else
			{
				throw new Exception();
			}
		}

        public static string GetClassName(EServiceType type)
        {
            return GetValue(type) + "Execution";
        }

		public static string GetText(EServiceType type)
		{
            if (type == EServiceType.Create)
			{
                return "定时生成";
			}
            else if (type == EServiceType.Publish)
            {
                return "定时发布";
            }
            else if (type == EServiceType.Gather)
			{
                return "定时采集";
			}
            else if (type == EServiceType.Backup)
			{
                return "定时备份";
			}
            else if (type == EServiceType.Check)
            {
                return "定时审核";
            }
            else if (type == EServiceType.UnCheck)
            {
                return "定时下架";
            }
            else if (type == EServiceType.Subscribe)
            {
                return "定时推送订阅";
            }
			else
			{
				throw new Exception();
			}
		}

		public static EServiceType GetEnumType(string typeStr)
		{
			EServiceType retval = EServiceType.Create;

            if (Equals(EServiceType.Create, typeStr))
			{
                retval = EServiceType.Create;
			}
            else if (Equals(EServiceType.Publish, typeStr))
			{
                retval = EServiceType.Publish;
			}
            else if (Equals(EServiceType.Gather, typeStr))
			{
                retval = EServiceType.Gather;
			}
            else if (Equals(EServiceType.Backup, typeStr))
			{
                retval = EServiceType.Backup;
			}
            else if (Equals(EServiceType.Check, typeStr))
            {
                retval = EServiceType.Check;
            }
            else if (Equals(EServiceType.UnCheck, typeStr))
            {
                retval = EServiceType.UnCheck;
            }
            else if (Equals(EServiceType.Subscribe, typeStr))
            {
                retval = EServiceType.Subscribe;
            }

			return retval;
		}

		public static bool Equals(EServiceType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static bool Equals(string typeStr, EServiceType type)
		{
			return Equals(type, typeStr);
		}

        public static ListItem GetListItem(EServiceType type, bool selected)
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
                listControl.Items.Add(GetListItem(EServiceType.Create, false));
                listControl.Items.Add(GetListItem(EServiceType.Publish, false));
                listControl.Items.Add(GetListItem(EServiceType.Gather, false));
                listControl.Items.Add(GetListItem(EServiceType.Backup, false));
                //listControl.Items.Add(GetListItem(EServiceType.Check, false));
                //listControl.Items.Add(GetListItem(EServiceType.TurnOff, false));
            }
        }
	}
}
