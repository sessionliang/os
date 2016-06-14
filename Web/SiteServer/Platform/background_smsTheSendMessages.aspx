<%@ Page Language="C#" AutoEventWireup="true" Inherits="BaiRong.BackgroundPages.BackgroundSMSTheSendMessages" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <table class="table table-bordered table-hover">
    <tr class="info thead">
        <td width="50">序号</td>
        <td width="250">手机号</td>
        <td>内容</td>
        <td width="130">发送时间</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
                <td>
                    <%#Container.ItemIndex+1 %>
                </td>
                <td>
                    <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
                </td>
                <td>
                    <asp:Literal ID="ltlSendtime" runat="server"></asp:Literal>
                </td>
          </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" ID="Delete" Text="清除记录" OnClick="Delete_OnClick" runat="server" />
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->