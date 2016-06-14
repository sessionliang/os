using System;

namespace BaiRong.Model.Service
{
    public class TaskLogInfo
    {
        private int id;
        private int taskID;
        private bool isSuccess;
        private string errorMessage;
        private string stackTrace;
        private string subtaskErrors;
        private DateTime addDate;

        public TaskLogInfo()
        {
            this.id = 0;
            this.taskID = 0;
            this.isSuccess = false;
            this.errorMessage = string.Empty;
            this.stackTrace = string.Empty;
            this.subtaskErrors = string.Empty;
            this.addDate = DateTime.Now;
        }

        public TaskLogInfo(int id, int taskID, bool isSuccess, string errorMessage, string stackTrace, string subtaskErrors, DateTime addDate)
        {
            this.id = id;
            this.taskID = taskID;
            this.isSuccess = isSuccess;
            this.errorMessage = errorMessage;
            this.stackTrace = stackTrace;
            this.subtaskErrors = subtaskErrors;
            this.addDate = addDate;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int TaskID
        {
            get { return taskID; }
            set { taskID = value; }
        }

        public bool IsSuccess
        {
            get { return isSuccess; }
            set { isSuccess = value; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public string StackTrace
        {
            get { return stackTrace; }
            set { stackTrace = value; }
        }

        public string SubtaskErrors
        {
            get { return subtaskErrors; }
            set { subtaskErrors = value; }
        }

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }
    }
}
