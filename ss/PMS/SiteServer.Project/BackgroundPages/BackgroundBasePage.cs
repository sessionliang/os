using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using SiteServer.Project.Model;
using SiteServer.Project.Core;
using BaiRong.Model;
using System.Collections.Specialized;

namespace SiteServer.Project.BackgroundPages
{
    public class BackgroundBasePage : BaiRong.BackgroundPages.BackgroundBasePage
    {
        //显示标题
        public Literal literalTitle;
        //显示操作消息面板
        //public Label myLabel;
        ////显示非操作信息面板
        public Label MessageLabel;
        public Control MessageLabelRow;

        //表单提交按钮
        public Button Submit;
        //显示"修改"或"添加"

        public Message messageCtrl;

        private bool messageImmidiatary = true;
        private bool messageRight = true;
        private string message = string.Empty;
        private string scripts = string.Empty;

        private void setMessage(bool isRight, string theMessage)
        {
            this.messageRight = isRight;
            this.message = theMessage;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (!StringUtils.IsNullorEmpty(this.message))
            {
                if (this.messageImmidiatary && this.messageCtrl != null)
                {
                    if (this.messageRight)
                    {
                        this.messageCtrl.IsShowImmidiatary = true;
                        this.messageCtrl.MessageType = MessageUtils.Message.EMessageType.Success;
                        this.messageCtrl.Content = this.message;
                    }
                    else
                    {
                        this.messageCtrl.IsShowImmidiatary = true;
                        this.messageCtrl.MessageType = MessageUtils.Message.EMessageType.Error;
                        this.messageCtrl.Content = this.message;
                    }
                }
                else
                {
                    if (this.messageRight)
                    {
                        MessageUtils.SaveMessage(MessageUtils.Message.EMessageType.Success, this.message);
                    }
                    else
                    {
                        MessageUtils.SaveMessage(MessageUtils.Message.EMessageType.Error, this.message);
                    }
                }
                //writer.Write(@"<script type=""text/javascript"">displayMessage({0}, '{1}');</script>", this.messageRight.ToString().ToLower(), this.message.Replace("'", string.Empty).Replace("\r\n", string.Empty));
            }
            base.Render(writer);
            if (!StringUtils.IsNullorEmpty(this.scripts))
            {
                writer.Write(@"<script type=""text/javascript"">{0}</script>", this.scripts);
            }
        }

        public void AddWaitAndRedirectScript(string redirectUrl)
        {
            this.scripts += string.Format(@"
setTimeout(""_goto('{0}')"", 800);
", redirectUrl);
        }

        public void FailMessage(string theMessage)
        {
            this.setMessage(false, theMessage);
        }

        public void SuccessMessage(string theMessage)
        {
            this.setMessage(true, theMessage);
        }

        public void ChangeLiteralToEdit()
        {
            if (literalTitle != null) literalTitle.Text = "修改";
            if (Submit != null) Submit.Text = "修 改";
        }

        public void ChangeLiteralToAdd()
        {
            if (literalTitle != null) literalTitle.Text = "添加";
            if (Submit != null) Submit.Text = "添 加";
        }

        public void MyDataGrid_ItemBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes["onmouseover"] = "this.className=\'tdbg-dark\';";
                e.Item.Attributes["onmouseout"] = "this.className=\'tdbg\';";
            }
        }

        private int projectID = -1;
        public int ProjectID
        {
            get
            {
                if (projectID == -1)
                {
                    projectID = TranslateUtils.ToInt(Request.QueryString["ProjectID"]);
                }
                return projectID;
            }
            set
            {
                projectID = value;
                projectInfo = null;
            }
        }

        private ProjectInfo projectInfo;
        public ProjectInfo ProjectInfo
        {
            get
            {
                if (projectInfo == null)
                {
                    projectInfo = DataProvider.ProjectDAO.GetProjectInfo(this.ProjectID);
                }
                return projectInfo;
            }
        }

        #region Input Basic

        private ExtendedAttributes attributes;
        public ExtendedAttributes Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new ExtendedAttributes();
                }
                return this.attributes;
            }
        }

        public void AddAttributes(ExtendedAttributes extendedAttributes)
        {
            if (extendedAttributes != null)
            {
                this.Attributes.SetExtendedAttribute(extendedAttributes.Attributes);
            }
        }

        public void AddAttributes(NameValueCollection attributes)
        {
            if (attributes != null)
            {
                this.Attributes.SetExtendedAttribute(attributes);
            }
        }

        public NameValueCollection GetAttributes()
        {
            return this.Attributes.Attributes;
        }

        public virtual string GetValue(string attributeName)
        {
            if (this.attributes != null)
            {
                return this.attributes.GetExtendedAttribute(attributeName);
            }
            return string.Empty;
        }

        public void SetValue(string name, string value)
        {
            this.Attributes.SetExtendedAttribute(name, value);
        }

        public void RemoveValue(string name)
        {
            this.Attributes.Attributes.Remove(name.ToLower());
        }

        public string GetSelected(string attributeName, string value)
        {
            if (this.attributes != null)
            {
                if (this.attributes.GetExtendedAttribute(attributeName) == value)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public string GetSelected(string attributeName, string value, bool isDefault)
        {
            if (this.attributes != null)
            {
                if (this.attributes.GetExtendedAttribute(attributeName) == value)
                {
                    return @"selected=""selected""";
                }
            }
            else
            {
                if (isDefault)
                {
                    return @"selected=""selected""";
                }
            }
            return string.Empty;
        }

        public string GetChecked(string attributeName, string value)
        {
            if (this.attributes != null)
            {
                if (this.attributes.GetExtendedAttribute(attributeName) == value)
                {
                    return @"checked=""checked""";
                }
            }
            return string.Empty;
        }

        public string GetChecked(string attributeName, string value, bool isDefault)
        {
            if (this.attributes != null)
            {
                if (this.attributes.GetExtendedAttribute(attributeName) == value)
                {
                    return @"checked=""checked""";
                }
            }
            else
            {
                if (isDefault)
                {
                    return @"checked=""checked""";
                }
            }
            return string.Empty;
        }

        #endregion
    }
}
