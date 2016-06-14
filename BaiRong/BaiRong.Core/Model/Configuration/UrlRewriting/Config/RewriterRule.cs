using System;

namespace BaiRong.Core.UrlRewriting
{
	[Serializable()]
	public class RewriterRule
	{
        private string lookFor, sendTo;
		private bool isRewriting = true;

        public string LookFor
        {
            get
            {
                return lookFor;
            }
            set
            {
                lookFor = value;
            }
        }

        public string SendTo
        {
            get
            {
                return sendTo;
            }
            set
            {
                sendTo = value;
            }
        }

        public bool IsRewriting
        {
            get
            {
                return isRewriting;
            }
            set
            {
                isRewriting = value;
            }
        }
	}
}
