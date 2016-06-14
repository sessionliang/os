using System;
using System.Web;
using System.Web.UI;

using BaiRong.Core;

namespace BaiRong.Controls
{
	public class Message : Control
	{
        private bool isShowImmidiatary = false;
        public bool IsShowImmidiatary
        {
            get { return isShowImmidiatary; }
            set { isShowImmidiatary = value; }
        }

        private MessageUtils.Message.EMessageType messageType = MessageUtils.Message.EMessageType.None;
        public MessageUtils.Message.EMessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }

        private string content = string.Empty;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

		protected override void Render(HtmlTextWriter writer)
		{
            if (isShowImmidiatary)
            {
                writer.Write(MessageUtils.GetMessageHtml(this.messageType, this.content, this));
            }
            else
            {
                writer.Write(MessageUtils.GetMessageHtml(this));
            }
		}
	}
}
