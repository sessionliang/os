<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.OrderUser" %>

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
<bairong:alerts text="请在此选择用户" runat="server"></bairong:alerts>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          用户类型：
          <asp:DropDownList ID="ddlTypeID" class="input-medium" runat="server" OnSelectedIndexChanged="Search_OnClick" AutoPostBack="true" ></asp:DropDownList>
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
          显示条数：
          <asp:DropDownList ID="PageNum" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
            <asp:ListItem Text="默认" Value="0" Selected="true"></asp:ListItem>
            <asp:ListItem Text="30" Value="30"></asp:ListItem>
            <asp:ListItem Text="50" Value="50"></asp:ListItem>
            <asp:ListItem Text="100" Value="100"></asp:ListItem>
            <asp:ListItem Text="200" Value="200"></asp:ListItem>
            <asp:ListItem Text="300" Value="300"></asp:ListItem>
          </asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td>
          所属部门：
          <asp:DropDownList ID="ddlDepartmentID" class="input-medium" runat="server" OnSelectedIndexChanged="Search_OnClick" AutoPostBack="true" ></asp:DropDownList>
          所在区域：
          <asp:DropDownList ID="ddlAreaID" class="input-medium" runat="server" OnSelectedIndexChanged="Search_OnClick" AutoPostBack="true" ></asp:DropDownList>
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
      <td>用户类型</td>
      <td>所属部门</td>
      <td>所在区域</td>
      <td>注册时间</td>
      <td>注册 IP</td>
      <td>最后活动时间</td>
      <td width="40">&nbsp;</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
          <tr>
            <td><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlDisplayName" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlType" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlDepartmentID" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlAreaID" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlCreateDate" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlCreateIPAddress" runat="server"></asp:Literal></td>
            <td><asp:Literal ID="ltlLastActivityDate" runat="server"></asp:Literal></td>
            <td class="center">
              <asp:HyperLink ID="hlSelect" Text="选择" NavigateUrl="javascript:;" runat="server"></asp:HyperLink>
            </td>
          </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />
  
</form>
</body>
</html>
