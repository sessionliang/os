<%@ Page Language="C#" Trace="false" Inherits="SiteServer.CMS.BackgroundPages.Modal.UserSelect" %>

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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          注册时间：
          <asp:DropDownList ID="CreateDate" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
            <asp:ListItem Text="全部时间" Value="0" Selected="true"></asp:ListItem>
            <asp:ListItem Text="1天内" Value="1"></asp:ListItem>
            <asp:ListItem Text="2天内" Value="2"></asp:ListItem>
            <asp:ListItem Text="3天内" Value="3"></asp:ListItem>
            <asp:ListItem Text="1周内" Value="7"></asp:ListItem>
            <asp:ListItem Text="1个月内" Value="30"></asp:ListItem>
            <asp:ListItem Text="3个月内" Value="90"></asp:ListItem>
            <asp:ListItem Text="半年内" Value="180"></asp:ListItem>
            <asp:ListItem Text="1年内" Value="365"></asp:ListItem>
          </asp:DropDownList>
          最后活动时间：
          <asp:DropDownList ID="LastActivityDate" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
            <asp:ListItem Text="全部时间" Value="0" Selected="true"></asp:ListItem>
            <asp:ListItem Text="1天内" Value="1"></asp:ListItem>
            <asp:ListItem Text="2天内" Value="2"></asp:ListItem>
            <asp:ListItem Text="3天内" Value="3"></asp:ListItem>
            <asp:ListItem Text="1周内" Value="7"></asp:ListItem>
            <asp:ListItem Text="1个月内" Value="30"></asp:ListItem>
            <asp:ListItem Text="3个月内" Value="90"></asp:ListItem>
            <asp:ListItem Text="半年内" Value="180"></asp:ListItem>
            <asp:ListItem Text="1年内" Value="365"></asp:ListItem>
          </asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td>
          用户组：
          <asp:DropDownList ID="ddlGroupID" class="input-medium" runat="server" OnSelectedIndexChanged="Search_OnClick" AutoPostBack="true" ></asp:DropDownList>
          关键字：
          <asp:TextBox id="Keyword" MaxLength="500" Size="45" runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" id="Search" text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table class="table table-bordered table-hover">
    <tr class="info thead">
      <td>账号</td>
      <td>姓名</td>
      <td>邮箱</td>
      <td>手机号码</td>
      <td>用户组</td>
      <td>注册时间</td>
      <td>注册 IP</td>
      <td>最后活动时间</td>
      <td width="20">
        <input onclick="_checkFormAll(this.checked)" type="checkbox" />
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <td>
                <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlEmail" runat="server"></asp:Literal>
            </td>
            <td>
                <asp:Literal ID="ltlMobile" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlGroup" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlCreateDate" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlCreateIPAddress" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlLastActivityDate" runat="server"></asp:Literal>
            </td>
            <td class="center">
                <asp:Literal ID="ltlSelect" runat="server"></asp:Literal>
            </td>
          </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->