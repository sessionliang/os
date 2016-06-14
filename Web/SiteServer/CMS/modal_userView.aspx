<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.UserView" Trace="false"%>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-striped">
    <tr>
      <td width="118" height="25">账号：</td>
      <td><asp:Literal id="ltlUserName" runat="server" /></td>
      <td>姓名：</td>
      <td width="118"><asp:Literal id="ltlDisplayName" runat="server" /></td>
    </tr>
    <tr>
      <td height="25">注册时间：</td>
      <td><asp:Literal id="ltlCreateDate" runat="server" /></td>
      <td>最后活动时间：</td>
      <td><asp:Literal ID="ltlLastActivityDate" runat="server" /></td>
    </tr>
    <tr>
      <td width="118" height="25">用户组：</td>
      <td><asp:Literal id="ltlGroup" runat="server" /></td>
      <td>注册 IP：</td>
      <td><asp:Literal id="ltlCreateIPAddress" runat="server" /></td>
    </tr>
    <tr>
      <td width="118" height="25">电子邮箱：</td>
      <td><asp:Literal id="ltlEmail" runat="server" /></td>
      <td>手机号码：</td>
      <td><asp:Literal ID="ltlMobile" runat="server" /></td>
    </tr>
  </table>
  <asp:DataList ID="MyDataList" RepeatColumns="2" cssClass="table table-striped" runat="server">
    <itemtemplate>
      <table width="100%" class="noborder">
        <tr>
          <td width="118">
            <asp:Literal id="ltlDataKey" runat="server" />
            ：</td>
          <td><asp:Literal id="ltlDataValue" runat="server" /></td>
        </tr>
      </table>
    </itemtemplate>
  </asp:DataList>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->