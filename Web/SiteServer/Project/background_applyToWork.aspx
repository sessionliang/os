<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundApplyToWork" %>

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
  <bairong:alerts text="以下操作按钮仅当办件信息为特定状态时才起作用" runat="server" />

  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <div class="well well-small">
    <asp:HyperLink ID="hlAccept" NavigateUrl="javascript:;" runat="server" Text="受 理"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlDeny" NavigateUrl="javascript:;" runat="server" Text="拒 绝"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlCheck" NavigateUrl="javascript:;" runat="server" Text="审 核"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlRedo" NavigateUrl="javascript:;" runat="server" Text="要求返工"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlSetting" NavigateUrl="javascript:;" runat="server" Text="设置属性"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlSwitchTo" NavigateUrl="javascript:;" runat="server" Text="转 办"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlTranslate" NavigateUrl="javascript:;" runat="server" Text="转 移"></asp:HyperLink>
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlComment" NavigateUrl="javascript:;" runat="server" Text="批 注"></asp:HyperLink>
    <asp:PlaceHolder ID="phDelete" runat="server">
    &nbsp;|&nbsp;
    <asp:HyperLink ID="hlDelete" NavigateUrl="javascript:;" runat="server" Text="删 除"></asp:HyperLink>
    </asp:PlaceHolder>
  </div>

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          排序：
          <asp:DropDownList ID="ddlTaxis" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          办理状态：
          <asp:DropDownList ID="ddlState" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          类型：
          <asp:DropDownList ID="ddlTypeID" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          发起人：
          <asp:DropDownList ID="ddlAddUserName" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          负责人：
          <asp:DropDownList ID="ddlUserName" class="input-small" AutoPostBack="true" OnSelectedIndexChanged="Search_OnClick" runat="server"></asp:DropDownList>
          关键字：
          <asp:TextBox ID="tbKeyword" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
          <asp:Button OnClick="Search_OnClick" Text="搜 索" class="btn" style="margin-bottom: 0px" runat="server"></asp:Button>
        </td>
      </tr>
    </table>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td class="center" style="width:60px;">办件编号</td>
      <td>办件标题(点击进入操作界面) </td>
      <td class="center" style="width:60px;">类型</td>
      <td class="center" style="width:100px;">提交日期</td>
      <td class="center" style="width:100px;">发起人</td>
      <td class="center" style="width:100px;">负责人</td>
      <td class="center" style="width:80px;">期限</td>
      <td class="center" style="width:60px;">状态</td>
      <td class="center" style="width:60px;">流动轨迹</td>
      <td class="center" style="width:60px;">快速查看</td>
      <td class="center" style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <asp:Literal ID="ltlTr" runat="server"></asp:Literal>
          <td class="center" style="width:50px;"><asp:Literal ID="ltlApplyID" runat="server"></asp:Literal></td>
          <td>&nbsp;<asp:Literal ID="ltlTitle" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlType" runat="server"></asp:Literal></td>
          <td class="center" style="width:100px;"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          <td class="center" style="width:100px;"><asp:Literal ID="ltlAddUserName" runat="server"></asp:Literal></td>
          <td class="center" style="width:100px;"><asp:Literal ID="ltlUserName" runat="server"></asp:Literal></td>
          <td class="center" style="width:80px;"><asp:Literal ID="ltlLimit" runat="server"></asp:Literal></td>
          <td class="center" style="width:60px;"><asp:Literal ID="ltlState" runat="server"></asp:Literal></td>
          <td class="center" style="width:60px;"><asp:Literal ID="ltlFlowUrl" runat="server"></asp:Literal></td>
          <td class="center" style="width:60px;"><asp:Literal ID="ltlViewUrl" runat="server"></asp:Literal></td>
          <td class="center" style="width:20px;"><input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' /></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>