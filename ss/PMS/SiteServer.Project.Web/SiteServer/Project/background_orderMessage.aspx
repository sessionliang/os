<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundOrderMessage" %>

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

    $('img.cover').click(function(){
          if ($(this).attr('isLarge') != 'true'){
            $(this).attr('src', $(this).attr('largeUrl'));
            $(this).parent().css('width', '415px');
            $(this).attr('isLarge', 'true');
          }else{
            $(this).attr('src', $(this).attr('smallUrl'));
            $(this).parent().css('width', '214px');
            $(this).attr('isLarge', 'false');
          }
          return false;
    });

  });
  </script>

  <div class="well well-small">
    <asp:HyperLink ID="hlAdd" NavigateUrl="javascript:;" runat="server" Text="新 增"></asp:HyperLink>
        &nbsp;|&nbsp;
    <asp:HyperLink ID="hlDelete" NavigateUrl="javascript:;" runat="server" Text="删 除"></asp:HyperLink>
  </div>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td>次序 </td>
      <td>消息名称 </td>
      <td>发送短信</td>
      <td>发送邮件</td>
      <td>短信模板</td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlIndex" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlMessageName" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsSMS" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsEmail" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlTemplateSMS" runat="server"></asp:Literal></td>
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