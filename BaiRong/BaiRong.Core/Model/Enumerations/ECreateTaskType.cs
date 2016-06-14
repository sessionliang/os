using System;
using BaiRong.Core;

namespace BaiRong.Model
{
    public enum ECreateTaskType
    {
        Queuing,      //正在排队
        Processing,   //处理中
        Completed,   //已完成
        Cancel          //已取消
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
                return "正在排队";
            }
            else if (type == ECreateTaskType.Processing)
            {
                return "处理中";
            }
            else if (type == ECreateTaskType.Completed)
            {
                return "已完成";
            }
            else if (type == ECreateTaskType.Cancel)
            {
                return "已取消";
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
