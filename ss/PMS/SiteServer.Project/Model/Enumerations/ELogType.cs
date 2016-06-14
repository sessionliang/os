using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.Project.Model
{
    public enum EProjectLogType
	{
        New,                //新办件
        Accept,             //受理
        Deny,               //拒绝
        SwitchTo,           //转办
        Comment,            //批注
        Redo,               //要求返工
        Reply,              //办理
        Check,              //审核
	}

    public class EProjectLogTypeUtils
	{
		public static string GetValue(EProjectLogType type)
		{
            if (type == EProjectLogType.New)
			{
                return "New";
            }
            else if (type == EProjectLogType.Accept)
            {
                return "Accept";
            }
            else if (type == EProjectLogType.Deny)
			{
                return "Deny";
            }
            else if (type == EProjectLogType.SwitchTo)
            {
                return "SwitchTo";
            }
            else if (type == EProjectLogType.Comment)
            {
                return "Comment";
            }
            else if (type == EProjectLogType.Redo)
            {
                return "Redo";
            }
            else if (type == EProjectLogType.Reply)
            {
                return "Reply";
            }
            else if (type == EProjectLogType.Check)
            {
                return "Check";
            }
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(EProjectLogType type)
		{
            if (type == EProjectLogType.New)
            {
                return "新提交办件";
            }
            else if (type == EProjectLogType.Accept)
            {
                return "受理办件";
            }
            else if (type == EProjectLogType.Deny)
            {
                return "拒绝办件";
            }
            else if (type == EProjectLogType.SwitchTo)
            {
                return "转办办件";
            }
            else if (type == EProjectLogType.Comment)
            {
                return "批注办件";
            }
            else if (type == EProjectLogType.Redo)
            {
                return "要求返工";
            }
            else if (type == EProjectLogType.Reply)
            {
                return "回复办件";
            }
            else if (type == EProjectLogType.Check)
            {
                return "审核办件 通过";
            }
            else
            {
                throw new Exception();
            }
		}

		public static EProjectLogType GetEnumType(string typeStr)
		{
            EProjectLogType retval = EProjectLogType.New;

            if (Equals(EProjectLogType.New, typeStr))
			{
                retval = EProjectLogType.New;
            }
            else if (Equals(EProjectLogType.Accept, typeStr))
            {
                retval = EProjectLogType.Accept;
            }
            else if (Equals(EProjectLogType.Deny, typeStr))
			{
                retval = EProjectLogType.Deny;
            }
            else if (Equals(EProjectLogType.SwitchTo, typeStr))
            {
                retval = EProjectLogType.SwitchTo;
            }
            else if (Equals(EProjectLogType.Comment, typeStr))
            {
                retval = EProjectLogType.Comment;
            }
            else if (Equals(EProjectLogType.Redo, typeStr))
            {
                retval = EProjectLogType.Redo;
            }
            else if (Equals(EProjectLogType.Reply, typeStr))
            {
                retval = EProjectLogType.Reply;
            }
            else if (Equals(EProjectLogType.Check, typeStr))
            {
                retval = EProjectLogType.Check;
            }
			return retval;
		}

		public static bool Equals(EProjectLogType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, EProjectLogType type)
        {
            return Equals(type, typeStr);
        }

        public static ListItem GetListItem(EProjectLogType type, bool selected)
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
                listControl.Items.Add(GetListItem(EProjectLogType.New, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Accept, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Deny, false));
                listControl.Items.Add(GetListItem(EProjectLogType.SwitchTo, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Comment, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Redo, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Reply, false));
                listControl.Items.Add(GetListItem(EProjectLogType.Check, false));
            }
        }
	}
}
