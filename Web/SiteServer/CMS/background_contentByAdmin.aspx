<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundContentByAdmin" %>

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
  <bairong:alerts runat="server" /> 
  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          栏目：
          <asp:DropDownList ID="NodeIDDropDownList" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          内容状态：
          <asp:DropDownList ID="State" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" class="input-small" runat="server"></asp:DropDownList>
          <asp:CheckBox ID="IsDuplicate" class="checkbox inline" Text="包含重复标题" runat="server"></asp:CheckBox>
        </td>
      </tr>
      <tr>
        <td>
          时间：从
          <bairong:DateTimeTextBox id="DateFrom" class="input-small" Columns="12" runat="server" />
          &nbsp;到&nbsp;
          <bairong:DateTimeTextBox id="DateTo" class="input-small" Columns="12" runat="server" />
          目标：
          <asp:DropDownList ID="SearchType" class="input-small" runat="server"> </asp:DropDownList>
          关键字：
          <asp:TextBox id="Keyword"
            MaxLength="500"
            Size="37"
            runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" id="Search" text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>内容标题(点击查看) </td>
      <td>栏目</td>
      <asp:Literal ID="ltlColumnHeadRows" runat="server"></asp:Literal>
      <td width="50"> 状态 </td>
      <td width="50"> 链接 </td>
      <td width="50"> 点击量 </td> 
      <asp:Literal ID="ltlCommandHeadRows" runat="server"></asp:Literal> 
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td>
            <asp:Literal ID="ltlItemTitle" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlChannel" runat="server"></asp:Literal>
          </td>
          <asp:Literal ID="ltlColumnItemRows" runat="server"></asp:Literal>
          <td class="center" nowrap>
            <asp:Literal ID="ltlItemStatus" runat="server"></asp:Literal>
          </td>
          <td class="center" nowrap>
            <asp:Literal ID="ltlLink" runat="server"></asp:Literal>
          </td>
          <td class="center" nowrap>
            <asp:Literal ID="ltlHits" runat="server"></asp:Literal>
          </td>   
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />
     
  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn" id="btnRetun" OnClick="btnRetun_OnClick" Text="返 回" runat="server" /> 
  </ul>
</form>
</body>
</html>
<!-- check for 3.6 html permissions -->