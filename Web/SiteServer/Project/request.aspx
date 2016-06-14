<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Request" %>

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

  <ul class="nav nav-pills">
    <li><a href="requestAdd.aspx?domain=<%=Request.QueryString["domain"]%>&licenseID=<%=Request.QueryString["licenseID"]%>">发起工单</a></li>
    <li class="active"><a href="javascript:;">工单列表</a></li>
  </ul>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          工单状态：
          <asp:DropDownList ID="ddlStatus" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          关键字：
          <asp:TextBox ID="tbKeyword" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td style="width:30px;">编号</td>
      <td>主题 </td>
      <td>问题类型</td>
      <td style="width:110px;">提交时间</td>
      <td style="width:90px;">状态</td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlRequestSN" runat="server"></asp:Literal></td>
          <td>&nbsp;<asp:Literal ID="ltlSubject" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlRequestType" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>