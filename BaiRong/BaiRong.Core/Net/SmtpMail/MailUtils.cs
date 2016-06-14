using System;
using System.Text;
using System.Collections;
using System.Net.Sockets;
//using System.Web.Mail;

using BaiRong.Core;
using System.Net.Mail;

namespace BaiRong.Core.Net
{
	public class MailUtils : ISmtpMail
	{
		private string _subject;
        private string _body;
        private string _mailDomain;
		private int _mailserverport;
        private string _fromName;
		private string _username;
        private string _password;
		private bool _isHtml;
        private bool _enabledSsl;
        private MailPriority _priority = MailPriority.Normal;
		private ArrayList _recipients = new ArrayList();

        private MailUtils() { }

        private static ISmtpMail instance = null;
        public static ISmtpMail GetInstance()
        {
            if (instance == null)
            {
                instance = new MailUtils();
            }
            return instance;
        }

		/// <summary>
		/// �ʼ�����
		/// </summary>
		public string Subject
		{
			get
			{
				return this._subject;
			}
			set
			{
				this._subject = value;
			}
		}

		/// <summary>
		/// �ʼ�����
		/// </summary>
		public string Body
		{
			get
			{
				return this._body;
			}
			set
			{
				this._body = value;
			}
		}

		/// <summary>
		/// ������
		/// </summary>
		public string MailDomain
		{
			get
			{
				return this._mailDomain;
			}
			set
			{
				this._mailDomain = value;
			}
		}

		/// <summary>
		/// �ʼ��������˿ں�
		/// </summary>	
		public int MailDomainPort
		{
			set
			{
				this._mailserverport = value;
			}
			get
			{
				return this._mailserverport;
			}
		}

        public string MailFromName
        {
            set
            {
                if (value.Trim() != string.Empty)
                {
                    this._fromName = value.Trim();
                }
                else
                {
                    this._fromName = string.Empty;
                }
            }
            get
            {
                return _fromName;
            }
        }


		/// <summary>
		/// SMTP��֤ʱʹ�õ��û���
		/// </summary>
		public string MailServerUserName
		{
			set
			{
				if(value.Trim() != string.Empty)
				{
					this._username = value.Trim();
				}
				else
				{
					this._username = string.Empty;
				}
			}
			get
			{
				return _username;
			}
		}

		/// <summary>
		/// SMTP��֤ʱʹ�õ�����
		/// </summary>
        public string MailServerPassword
		{
			set
			{
				this._password = value;
			}
			get
			{
				return _password;
			}
		}	

		/// <summary>
		///  �Ƿ�Html�ʼ�
		/// </summary>
		public bool IsHtml
		{
			get
			{
				return this._isHtml;
			}
			set
			{
                this._isHtml = value;
			}
		}

        /// <summary>
        ///  �Ƿ�SSL
        /// </summary>
        public bool EnabledSsl
        {
            get
            {
                return this._enabledSsl;
            }
            set
            {
                this._enabledSsl = value;
            }
        }

        public MailPriority Priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                this._priority = value;
            }
        }

		//�ռ��˵������ַ
		public bool AddRecipient(params string[] usernames)
		{
            this._recipients.Clear();
            foreach (string username in usernames)
            {
                string theUserName = username.ToLower().Trim();
                if (!this._recipients.Contains(theUserName))
                {
                    this._recipients.Add(theUserName);
                }
            }

			return true;
		}

        #region .net 2.0
        ///// <summary>
        ///// ����
        ///// </summary>
        ///// <returns></returns>
        //public bool Send(out string errorMessage)
        //{
        //          errorMessage = string.Empty;
        //	MailMessage myEmail = new MailMessage();
        //          if (string.IsNullOrEmpty(this.MailFromName))
        //          {
        //              myEmail.From = this.MailServerUserName;
        //          }
        //          else
        //          {
        //              myEmail.From = string.Format(@"""{0}"" <{1}>", this.MailFromName, this.MailServerUserName);
        //          }

        //	myEmail.Subject = this.Subject;
        //	myEmail.Body = this.Body;
        //	myEmail.Priority = this.Priority; 
        //	myEmail.BodyFormat = this.IsHtml?MailFormat.Html:MailFormat.Text; //�ʼ���ʽ��.Text��.Html 

        //	// ͨ��SMTP��������֤
        //	myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
        //	myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", this.MailServerUserName);
        //	myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpaccountname",this.MailServerUserName);
        //	myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", this.MailServerPassword);
        //	myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport",this.MailDomainPort);

        //	System.Web.Mail.SmtpMail.SmtpServer = this.MailDomain;

        //          if (this._recipients == null || this._recipients.Count == 0)
        //          {
        //              errorMessage = "���������ռ�������!";
        //              return false;
        //          }
        //          else if (this._recipients.Count == 1)
        //          {
        //              try
        //              {
        //                  myEmail.To = this._recipients[0] as string;
        //                  System.Web.Mail.SmtpMail.Send(myEmail);
        //                  return true;
        //              }
        //              catch (Exception ex)
        //              {
        //                  //if (ex.InnerException != null)
        //                  //{
        //                  //    errorMessage = ex.InnerException.Message;
        //                  //}
        //                  //else
        //                  //{
        //                      errorMessage = "�ʼ���������ͨ���޷�����";
        //                  //}
        //                  return false;
        //              }
        //          }
        //          else
        //          {
        //              foreach (string userName in this._recipients)
        //              {
        //                  try
        //                  {
        //                      myEmail.To = userName;
        //                      System.Web.Mail.SmtpMail.Send(myEmail);
        //                  }
        //                  catch (Exception ex)
        //                  {
        //                      if (ex.InnerException != null)
        //                      {
        //                          errorMessage += ex.InnerException.Message + "<br>";
        //                      }
        //                      else
        //                      {
        //                          errorMessage += ex.Message + "<br>";
        //                      }
        //                  }
        //              }
        //          }

        //          return true;
        //} 
        #endregion

        /// <summary>
        /// ����
        /// </summary>
        /// <returns></returns>
        public bool Send(out string errorMessage)
        {
            string from = string.Empty;
            string to = string.Empty;
            errorMessage = string.Empty;

            // ͨ��SMTP��������֤
            //myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            //myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", this.MailServerUserName);
            //myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpaccountname", this.MailServerUserName);
            //myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", this.MailServerPassword);
            //myEmail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", this.MailDomainPort);

            //System.Web.Mail.SmtpMail.SmtpServer = this.MailDomain;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(this.MailDomain);
            client.UseDefaultCredentials = true;//�Ƿ������֤
            client.Credentials = new System.Net.NetworkCredential(this.MailServerUserName, this.MailServerPassword);//�����֤�˺�����  ��Ҫ�˺������׺���� 123@qq.com  ֻ����д123 ���ɡ�
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.Port = this.MailDomainPort;

            client.EnableSsl = this.EnabledSsl;

            if (this._recipients == null || this._recipients.Count == 0)
            {
                errorMessage = "���������ռ�������!";
                return false;
            }
            else if (this._recipients.Count == 1)
            {
                try
                {
                    if (string.IsNullOrEmpty(this.MailFromName))
                    {
                        from = this.MailServerUserName;
                    }
                    else
                    {
                        from = string.Format(@"""{0}"" <{1}>", this.MailFromName, this.MailServerUserName);
                    }

                    to = this._recipients[0] as string;

                    MailMessage myEmail = new MailMessage(from, to, this.Subject, this.Body);

                    myEmail.Priority = this.Priority;
                    myEmail.IsBodyHtml = this.IsHtml; //�ʼ���ʽ��.Text��.Html 

                    client.Send(myEmail);
                    return true;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        errorMessage = ex.InnerException.Message;
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    return false;
                }
            }
            else
            {
                foreach (string userName in this._recipients)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(this.MailFromName))
                        {
                            from = this.MailServerUserName;
                        }
                        else
                        {
                            from = string.Format(@"""{0}"" <{1}>", this.MailFromName, this.MailServerUserName);
                        }

                        to = userName;

                        MailMessage myEmail = new MailMessage(from, to, this.Subject, this.Body);

                        myEmail.Priority = this.Priority;
                        myEmail.IsBodyHtml = this.IsHtml; //�ʼ���ʽ��.Text��.Html 

                        client.Send(myEmail);
                        //System.Web.Mail.SmtpMail.Send(myEmail);
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            errorMessage += ex.InnerException.Message + "<br>";
                        }
                        else
                        {
                            errorMessage += ex.Message + "<br>";
                        }
                    }
                }
            }

            return true;
        }
    }
}
