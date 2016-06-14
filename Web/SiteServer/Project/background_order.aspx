<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrder" %>

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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          订单类型：
          <asp:DropDownList ID="ddlType" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          客户ID：
          <asp:TextBox ID="tbLoginName" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          关键字：
          <asp:TextBox ID="tbKeyword" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
        <td class="pull-right">合计：<code><asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal></code></td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>客户ID </td>
      <td>临时域名 </td>
      <td>模板ID </td>
      <td>后台密码 </td>
      <td>下单日期</td>
      <td>授权</td>
      <td>合同</td>
      <td>发票</td>
      <td>退款</td>
      <td width="40">联系</td>
      <td width="50">短信邮箱</td>
      <td>提交表单</td>
      <td>状态</td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlLoginName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlDomainTemp" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlMobanID" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlBackgroundPassword" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlLicense" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsContract" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsInvoice" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsRefund" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlTouch" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlSendMessage" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlForm" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlStatus" runat="server"></asp:Literal></td>
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