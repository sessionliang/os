using System;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum ECreateTaskType
    {
        Queuing,      //�����Ŷ�
        Processing,   //������
        Completed,   //�����
        Cancel          //��ȡ��
    }

    public class ECreateTaskTypeUtils
    {
        public static string GetValue(ECreateTaskType type)
        {
            if (type == ECreateTaskType.Queuing)
            {
                return "Queuing";
            }
            else if (type == ECreateTaskType.Processing)
            {
                return "Processing";
            }
            else if (type == ECreateTaskType.Completed)
            {
                return "Completed";
            }
            else if (type == ECreateTaskType.Cancel)
            {
                return "Cancel";
            }
            else
            {
                throw new Exception();
            }
        }

        public static string GetText(ECreateTaskType type)
        {
            if (type == ECreateTaskType.Queuing)
            {
                return "�����Ŷ�";
            }
            else if (type == ECreateTaskType.Processing)
            {
                return "������";
            }
            else if (type == ECreateTaskType.Completed)
            {
                return "�����";
            }
            else if (type == ECreateTaskType.Cancel)
            {
                return "��ȡ��";
            }
            else
            {
                throw new Exception();
            }
        }

        public static ECreateTaskType GetEnumType(string typeStr)
        {
            ECreateTaskType retval = ECreateTaskType.Queuing;
            if (Equals(ECreateTaskType.Processing, typeStr))
            {
                retval = ECreateTaskType.Processing;
            }
            else if (Equals(ECreateTaskType.Completed, typeStr))
            {
                retval = ECreateTaskType.Completed;
            }
            else if (Equals(ECreateTaskType.Cancel, typeStr))
            {
                retval = ECreateTaskType.Cancel;
            }
            return retval;
        }

        public static bool Equals(ECreateTaskType type, string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr)) return false;
            if (string.Equals(GetValue(type).ToLower(), typeStr.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static bool Equals(string typeStr, ECreateTaskType type)
        {
            return Equals(type, typeStr);
        }
    }
}
