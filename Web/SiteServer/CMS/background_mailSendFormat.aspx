<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundMailSendFormat" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts text="推荐好友功能在模板中使用&amp;lt;stl:action type=&quot;SendMail&quot;&gt;&amp;lt;/stl:action&gt;实现，邮件格式在此设置。" runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">邮件格式设置</h3>
    <div class="popover-content">
    
      <table class="table noborder table-hover">
        <tr>
          <td width="100"><bairong:help HelpText="设置邮件标题" Text="邮件标题：" runat="server" ></bairong:help></td>
          <td width="190"><asp:TextBox style="width:400px" id="MailSendTitle" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="MailSendTitle"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/></td>
          <td>系统发送的邮件标题，不支持 HTML。</td>
        </tr>
        <tr>
          <td><bairong:help HelpText="设置邮件正文" Text="邮件正文：" runat="server" ></bairong:help></td>
          <td><asp:TextBox TextMode="MultiLine" style="width:400px" Rows="8" id="MailSendContent" runat="server"/>
            <asp:RequiredFieldValidator
                      ControlToValidate="MailSendContent"
                      errorMessage=" *" foreColor="red" 
                      Display="Dynamic"
                      runat="server"/></td>
          <td>系统发送的邮件正文。标题内容均支持变量替换，可以使用如下变量:<BR>
            {receiver} : 邮件接收者<br>
            {sender} : 推荐人<BR>
            {time} :   发送时间<BR>
            {sitename} : 站点名称<br>
            {siteurl}
            : 站点地址<BR>
            {title} : 内容标题<br>
            {content} : 内容正文<br>
            {pageUrl} : 推荐页面地址</td>
        </tr>
      </table>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button class="btn btn-primary" id="Submit" text="确 定" onclick="Submit_OnClick" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->