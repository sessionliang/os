using System;
using System.Web.UI.WebControls;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	
	public enum ELinkType
	{
		LinkNoRelatedToChannelAndContent,	//Ĭ��
		NoLinkIfContentNotExists,			//������ʱ��������
		LinkToOnlyOneContent,				//��һ������ʱ���ӵ�������
		NoLinkIfContentNotExistsAndLinkToOnlyOneContent,						//������ʱ�������ӣ���һ������ʱ���ӵ�������
		LinkToFirstContent,				//���ӵ���һ������
		NoLinkIfContentNotExistsAndLinkToFirstContent,						//������ʱ�������ӣ�������ʱ���ӵ���һ������
		NoLinkIfChannelNotExists,			//����Ŀʱ��������
		LinkToLastAddChannel,				//���ӵ�������ӵ�����Ŀ
		LinkToFirstChannel,					//���ӵ���һ������Ŀ
		NoLinkIfChannelNotExistsAndLinkToLastAddChannel,							//����Ŀʱ�������ӣ�����Ŀʱ���ӵ�������ӵ�����Ŀ
		NoLinkIfChannelNotExistsAndLinkToFirstChannel,						//����Ŀʱ�������ӣ�����Ŀʱ���ӵ���һ������Ŀ
		NoLink								//��������
	}


	public class ELinkTypeUtils
	{
		public static string GetValue(ELinkType type)
		{
			if (type == ELinkType.LinkNoRelatedToChannelAndContent)
			{
				return "LinkNoRelatedToChannelAndContent";
			}
			else if (type == ELinkType.NoLinkIfContentNotExists)
			{
				return "NoLinkIfContentNotExists";
			}
			else if (type == ELinkType.LinkToOnlyOneContent)
			{
				return "LinkToOnlyOneContent";
			}
			else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent)
			{
				return "NoLinkIfContentNotExistsAndLinkToOnlyOneContent";
			}
			else if (type == ELinkType.LinkToFirstContent)
			{
				return "LinkToFirstContent";
			}
			else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent)
			{
				return "NoLinkIfContentNotExistsAndLinkToFirstContent";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExists)
			{
				return "NoLinkIfChannelNotExists";
			}
			else if (type == ELinkType.LinkToLastAddChannel)
			{
				return "LinkToLastAddChannel";
			}
			else if (type == ELinkType.LinkToFirstChannel)
			{
				return "LinkToFirstChannel";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel)
			{
				return "NoLinkIfChannelNotExistsAndLinkToLastAddChannel";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel)
			{
				return "NoLinkIfChannelNotExistsAndLinkToFirstChannel";
			}
			else if (type == ELinkType.NoLink)
			{
				return "NoLink";
			}
			else
			{
				throw new Exception();
			}
		}

		public static string GetText(ELinkType type)
		{
			if (type == ELinkType.NoLinkIfContentNotExists)
			{
				return "������ʱ��������";
			}
			else if (type == ELinkType.LinkToOnlyOneContent)
			{
				return "��һ������ʱ���ӵ�������";
			}
			else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent)
			{
				return "������ʱ�������ӣ���һ������ʱ���ӵ�������";
			}
			else if (type == ELinkType.LinkToFirstContent)
			{
				return "���ӵ���һ������";
			}
			else if (type == ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent)
			{
				return "������ʱ�������ӣ�������ʱ���ӵ���һ������";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExists)
			{
				return "����Ŀʱ��������";
			}
			else if (type == ELinkType.LinkToLastAddChannel)
			{
				return "���ӵ�������ӵ�����Ŀ";
			}
			else if (type == ELinkType.LinkToFirstChannel)
			{
				return "���ӵ���һ������Ŀ";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel)
			{
				return "����Ŀʱ�������ӣ�����Ŀʱ���ӵ�������ӵ�����Ŀ";
			}
			else if (type == ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel)
			{
				return "����Ŀʱ�������ӣ�����Ŀʱ���ӵ���һ������Ŀ";
			}
			else if (type == ELinkType.NoLink)
			{
				return "��������";
			}
			else if (type == ELinkType.LinkNoRelatedToChannelAndContent)
			{
				return "Ĭ��";
			}
			else
			{
				throw new Exception();
			}
		}

		public static ELinkType GetEnumType(string typeStr)
		{
			ELinkType retval = ELinkType.LinkNoRelatedToChannelAndContent;

			if (Equals(ELinkType.NoLinkIfContentNotExists, typeStr))
			{
				retval = ELinkType.NoLinkIfContentNotExists;
			}
			else if (Equals(ELinkType.LinkToOnlyOneContent, typeStr))
			{
				retval = ELinkType.LinkToOnlyOneContent;
			}
			else if (Equals(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent, typeStr))
			{
				retval = ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent;
			}
			else if (Equals(ELinkType.LinkToFirstContent, typeStr))
			{
				retval = ELinkType.LinkToFirstContent;
			}
			else if (Equals(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent, typeStr))
			{
				retval = ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent;
			}
			else if (Equals(ELinkType.NoLinkIfChannelNotExists, typeStr))
			{
				retval = ELinkType.NoLinkIfChannelNotExists;
			}
			else if (Equals(ELinkType.LinkToLastAddChannel, typeStr))
			{
				retval = ELinkType.LinkToLastAddChannel;
			}
			else if (Equals(ELinkType.LinkToFirstChannel, typeStr))
			{
				retval = ELinkType.LinkToFirstChannel;
			}
			else if (Equals(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel, typeStr))
			{
				retval = ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel;
			}
			else if (Equals(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel, typeStr))
			{
				retval = ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel;
			}
			else if (Equals(ELinkType.NoLink, typeStr))
			{
				retval = ELinkType.NoLink;
			}
			else if (Equals(ELinkType.LinkNoRelatedToChannelAndContent, typeStr))
			{
				retval = ELinkType.LinkNoRelatedToChannelAndContent;
			}

			return retval;
		}

		public static bool Equals(ELinkType type, string typeStr)
		{
			if (string.IsNullOrEmpty(typeStr)) return false;
			if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
			{
				return true;
			}
			return false;
		}

        public static bool Equals(string typeStr, ELinkType type)
        {
            return Equals(type, typeStr);
        }

		public static ListItem GetListItem(ELinkType type, bool selected)
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
				listControl.Items.Add(GetListItem(ELinkType.LinkNoRelatedToChannelAndContent, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfContentNotExists, false));
				listControl.Items.Add(GetListItem(ELinkType.LinkToOnlyOneContent, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent, false));
				listControl.Items.Add(GetListItem(ELinkType.LinkToFirstContent, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfChannelNotExists, false));
				listControl.Items.Add(GetListItem(ELinkType.LinkToLastAddChannel, false));
				listControl.Items.Add(GetListItem(ELinkType.LinkToFirstChannel, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel, false));
				listControl.Items.Add(GetListItem(ELinkType.NoLink, false));
			}
		}

        public static bool IsCreatable(NodeInfo nodeInfo)
        {
            bool isCreatable = false;

            if (nodeInfo.LinkType == ELinkType.LinkNoRelatedToChannelAndContent)
            {
                isCreatable = true;
            }
            else if (nodeInfo.LinkType == ELinkType.NoLink)
            {
                isCreatable = false;
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExists)
            {
                if (nodeInfo.ContentNum == 0)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.LinkToOnlyOneContent)
            {
                if (nodeInfo.ContentNum == 1)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExistsAndLinkToOnlyOneContent)
            {
                if (nodeInfo.ContentNum == 0)
                {
                    isCreatable = false;
                }
                else if (nodeInfo.ContentNum == 1)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.LinkToFirstContent)
            {
                if (nodeInfo.ContentNum >= 1)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfContentNotExistsAndLinkToFirstContent)
            {
                isCreatable = false;
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExists)
            {
                if (nodeInfo.ChildrenCount == 0)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.LinkToLastAddChannel)
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.LinkToFirstChannel)
            {
                if (nodeInfo.ChildrenCount > 0)
                {
                    isCreatable = false;
                }
                else
                {
                    isCreatable = true;
                }
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExistsAndLinkToLastAddChannel)
            {
                isCreatable = false;
            }
            else if (nodeInfo.LinkType == ELinkType.NoLinkIfChannelNotExistsAndLinkToFirstChannel)
            {
                isCreatable = false;
            }

            return isCreatable;
        }

	}
}
