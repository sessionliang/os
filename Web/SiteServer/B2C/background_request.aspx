<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.BackgroundRequest" %>
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
  
  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <div class="well well-small">
    <asp:HyperLink ID="hlAdd" NavigateUrl="javascript:;" runat="server" Text="新 增"></asp:HyperLink>
        &nbsp;|&nbsp;
    <asp:HyperLink ID="hlSetting" NavigateUrl="javascript:;" runat="server" Text="设 置"></asp:HyperLink>
        &nbsp;|&nbsp;
    <asp:HyperLink ID="hlDelete" NavigateUrl="javascript:;" runat="server" Text="删 除"></asp:HyperLink>
  </div>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          排序：
          <asp:DropDownList ID="ddlTaxis" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
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
      <td>网址</td>
      <td>发起人</td>
      <td style="width:110px;">创建时间</td>
      <td>负责人</td>
      <td>评价</td>
      <td style="width:60px;">状态</td>
      <td style="width:40px;"></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlSN" runat="server"></asp:Literal></td>
          <td>&nbsp;<asp:Literal ID="ltlSubject" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlRequestType" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlWebsite" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          
          <td class="center"><asp:Literal ID="ltlAdminUserName" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlEstimateUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
          <td class="center"><input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:HyperLink id="hlConfiguration" class="btn btn-success" runat="server" text="设置问题类型"></asp:HyperLink>
  </ul>

</form>
</body>
</html>