<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundCloudApp" %>

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
  <bairong:alerts runat="server"></bairong:alerts>
  
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

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>应用节点 </td>
      <td>用户类型 </td>
      <td>服务器 </td>
      <td>用户名 </td>
      <td>密码 </td>
      <td>ISVKey </td>
      <td>接入用户数</td>
      <td>状态 </td>
      <td>开通时间</td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlSN" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlUserType" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlServerSN" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAdministrator" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlPassword" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlISVKey" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal id="ltlUserCount" runat="server" /></td>
          <td class="center"><asp:Literal ID="ltlIsFilled" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStartDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal></td>
          <td class="center"><input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>