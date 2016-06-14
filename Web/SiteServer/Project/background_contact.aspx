<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundContact" %>

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
  
  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <div class="well well-small">
    <asp:HyperLink ID="hlAdd" NavigateUrl="javascript:;" runat="server" Text="新 增"></asp:HyperLink>
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
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td style="width:30px;">编号</td>
      <td>联系人 </td>
      <td>职位 </td>
      <td>角色 </td>
      <td>手机 </td>
      <td>邮箱 </td>
      <td>QQ </td>
      <td style="width:110px;">创建时间</td>
      <td style="width:40px;"></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlID" runat="server"></asp:Literal></td>
          <td>&nbsp;<asp:Literal ID="ltlContactName" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlJobTitle" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlAccountRole" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlMobile" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlEmail" runat="server"></asp:Literal></td>
          <td><asp:Literal ID="ltlQQ" runat="server"></asp:Literal></td>
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