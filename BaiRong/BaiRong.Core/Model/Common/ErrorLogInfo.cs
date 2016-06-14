using System;
using BaiRong.Model;

namespace BaiRong.Model
{
	public class ErrorLogInfo
	{
		private int id;
        private DateTime addDate;
        private string message;
        private string stacktrace;
        private string summary;

		public ErrorLogInfo()
		{
            this.id = 0;
            this.addDate = DateTime.Now;
            this.message = string.Empty;
            this.stacktrace = string.Empty;
            this.summary = string.Empty;
		}

        public ErrorLogInfo(int id, DateTime addDate, string message, string stacktrace, string summary) 
		{
            this.id = id;
            this.addDate = addDate;
            this.message = message;
            this.stacktrace = stacktrace;
            this.summary = summary;
		}

        public int ID
		{
            get { return id; }
            set { id = value; }
		}

        public DateTime AddDate
        {
            get { return addDate; }
            set { addDate = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string Stacktrace
        {
            get { return stacktrace; }
            set { stacktrace = value; }
        }

        public string Summary
		{
            get { return summary; }
            set { summary = value; }
		}
	}
}
