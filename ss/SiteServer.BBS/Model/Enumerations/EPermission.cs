using System;
using System.Web.UI.WebControls;
using System.Collections;
using BaiRong.Core;
using BaiRong.Model;


namespace SiteServer.BBS.Model
{
    public enum EPermission
    {
        View,               //������
        AddThread,          //���»���
        AddPoll,            //����ͶƱ
        AddPost,            //����ظ�
        DownLoadFile,       //���ظ���
        UploadFile,         //�ϴ�����
        UploadImage,        //�ϴ�ͼƬ
    }

    public class EPermissionUtils
    {
        public static string GetValue(EPermission type)
        {
            if (type == EPermission.View)
            {
                return "View";
            }
            else if (type == EPermission.AddThread)
            {
                return "AddThread";
            }
            else if (type == EPermission.AddPoll)
            {
                return "AddPoll";
            }
            else if (type == EPermission.AddPost)
            {
                return "AddPost";
            }
            else if (type == EPermission.DownLoadFile)
            {
                return "DownLoadFile";
            }
            else if (type == EPermission.UploadFile)
            {
                return "UploadFile";
            }
            else if (type == EPermission.UploadImage)
            {
                return "UploadImage";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(EPermission type)
        {
            if (type == EPermission.View)
            {
                return "������";
            }
            else if (type == EPermission.AddThread)
            {
                return "���»���";
            }
            else if (type == EPermission.AddPoll)
            {
                return "����ͶƱ";
            }
            else if (type == EPermission.AddPost)
            {
                return "����ظ�";
            }
            else if (type == EPermission.DownLoadFile)
            {
                return "���ظ���";
            }
            else if (type == EPermission.UploadFile)
            {
                return "�ϴ�����";
            }
            else if (type == EPermission.UploadImage)
            {
                return "�ϴ�ͼƬ";
            }
            else
            {
                throw new Exception();
            }
        }

        public static EPermission GetEnumType(string typeStr)
        {
            EPermission retval = EPermission.View;

            if (Equals(EPermission.View, typeStr))
            {
                retval = EPermission.View;
            }
            else if (Equals(EPermission.AddThread, typeStr))
            {
                retval = EPermission.AddThread;
            }
            else if (Equals(EPermission.AddPoll, typeStr))
            {
                retval = EPermission.AddPoll;
            }
            else if (Equals(EPermission.AddPost, typeStr))
            {
                retval = EPermission.AddPost;
            }
            else if (Equals(EPermission.DownLoadFile, typeStr))
            {
                retval = EPermission.DownLoadFile;
            }
            else if (Equals(EPermission.UploadFile, typeStr))
            {
                retval = EPermission.UploadFile;
            }
            else if (Equals(EPermission.UploadImage, typeStr))
            {
                retval = EPermission.UploadImage;
            }

            return retval;
        }

        public static bool Equals(EPermission type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, EPermission type)
        {
            return Equals(type, typeStr);
        }

        public static ArrayList GetArrayList()
        {
            ArrayList arraylist = new ArrayList();
            arraylist.Add(EPermission.View);
            arraylist.Add(EPermission.AddThread);
            arraylist.Add(EPermission.AddPoll);
            arraylist.Add(EPermission.AddPost);
            arraylist.Add(EPermission.DownLoadFile);
            arraylist.Add(EPermission.UploadFile);
            arraylist.Add(EPermission.UploadImage);

            return arraylist;
        }

        public static ArrayList GetForbiddenArrayList(EUserGroupType groupType)
        {
            ArrayList arraylist = new ArrayList();

            if (groupType == EUserGroupType.WriteForbidden)
            {
                arraylist.Add(EPermission.AddThread);
                arraylist.Add(EPermission.AddPoll);
                arraylist.Add(EPermission.AddPost);
            }
            else if (groupType == EUserGroupType.ReadForbidden)
            {
                arraylist.Add(EPermission.View);
                arraylist.Add(EPermission.AddThread);
                arraylist.Add(EPermission.AddPoll);
                arraylist.Add(EPermission.AddPost);
                arraylist.Add(EPermission.DownLoadFile);
                arraylist.Add(EPermission.UploadFile);
                arraylist.Add(EPermission.UploadImage);
            }
            else if (groupType == EUserGroupType.Guest)
            {
                arraylist.Add(EPermission.AddThread);
                arraylist.Add(EPermission.AddPoll);
                arraylist.Add(EPermission.AddPost);
                arraylist.Add(EPermission.DownLoadFile);
                arraylist.Add(EPermission.UploadFile);
                arraylist.Add(EPermission.UploadImage);
            }

            return arraylist;
        }
    }
}
