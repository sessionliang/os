<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.BackgroundCommentContent" %>
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

  <script type="text/javascript" >
  $(document).ready(function(){
   loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
  });
  </script>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          栏目：
          <asp:DropDownList ID="NodeIDDropDownList" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          类型：
          <asp:DropDownList ID="TypeDropDownList" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          每页显示条数：
          <asp:DropDownList ID="PageNum" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
            <asp:ListItem Text="默认" Value="0" Selected="true"></asp:ListItem>
            <asp:ListItem Text="10" Value="10"></asp:ListItem>
            <asp:ListItem Text="16" Value="16"></asp:ListItem>
            <asp:ListItem Text="20" Value="20"></asp:ListItem>
            <asp:ListItem Text="30" Value="30"></asp:ListItem>
            <asp:ListItem Text="50" Value="50"></asp:ListItem>
            <asp:ListItem Text="100" Value="100"></asp:ListItem>
            <asp:ListItem Text="200" Value="200"></asp:ListItem>
            <asp:ListItem Text="300" Value="300"></asp:ListItem>
          </asp:DropDownList>
          <asp:CheckBox ID="IsSearchChildren" Text="含子栏目" Checked="true" AutoPostBack="true" OnCheckedChanged="Search_OnClick" runat="server"></asp:CheckBox>
        </td>
      </tr>
      <tr>
        <td>
          时间：
          <asp:DropDownList ID="SearchDate" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
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
          审核状态：
          <asp:DropDownList ID="IsChecked" class="input-medium" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server">
            <asp:ListItem Text="显示全部" Value="" Selected="true"></asp:ListItem>
            <asp:ListItem Text="已审核的评论" Value="True"></asp:ListItem>
            <asp:ListItem Text="未审核的评论" Value="False"></asp:ListItem>
          </asp:DropDownList>
          关键字：
          <asp:TextBox id="Keyword" MaxLength="500" Size="30" runat="server"/>
          <asp:Button class="btn" OnClick="Search_OnClick" id="Search" text="搜 索"  runat="server"/>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="80">评论人</td>
      <td>评论</td>
      <td>评论页面</td>
      <td width="110">添加日期</td>
      <td width="30">&nbsp;</td>
      <td width="20" class="center">
        <input type="checkbox" onClick="selectRows(document.getElementById('contents'),this.checked);">
      </td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td>
            <asp:Literal ID="ltlUserName" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlContent" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="ltlPageUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlAddDate" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <input type="checkbox" name="ContentIDCollection" value='<%#DataBinder.Eval(Container.DataItem, "CommentID")%>' />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:PlaceHolder ID="phCheck" runat="server" Visible="false">
      <asp:Button class="btn btn-success" id="btnCheck" Text="审核通过" runat="server" />
    </asp:PlaceHolder>
    <asp:Button class="btn btn-success" id="btnRecommend" Text="设置为精彩评论" runat="server" />
    <asp:Button class="btn" id="btnRecommendFalse" Text="设置为非精彩评论" runat="server" />
    <asp:PlaceHolder ID="phDelete" runat="server" Visible="false">
      <asp:Button class="btn" id="btnDelete" Text="删 除" runat="server" />
    </asp:PlaceHolder>
  </ul>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->