<%@ Page Language="C#" Inherits="SiteServer.GeXia.BackgroundPages.BackgroundCase" %>

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
      <td>公众号类型 </td>
      <td>微信ID </td>
      <td>微信名称 </td>
      <td>LOGO图标 </td>
      <td>缩略图 </td>
      <td>二维码 </td>
      <td>简介</td>
      <td>添加时间</td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlAccountType" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlWeChatID" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlWeChatName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIconUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlThumbUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlQRCodeUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal id="ltlSummary" runat="server" /></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
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