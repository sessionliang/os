<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundContract" %>

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
      <td>合同号 </td>
      <td>合同类型 </td>
      <td>负责人 </td>
      <td>合同金额 </td>
      <td>合同名称 </td>
      <td>添加日期</td>
      <td>寄出</td>
      <td>寄出日期</td>
      <td>收到</td>
      <td>收到日期</td>
      <td></td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
          <td class="center"><asp:Literal ID="ltlSN" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlContractType" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlChargeUserName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAmount" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlContractTitle" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          
          <td class="center"><asp:Literal ID="ltlIsContract" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlContractDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsConfirm" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlConfirmDate" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlDocumentUrl" runat="server"></asp:Literal></td>
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