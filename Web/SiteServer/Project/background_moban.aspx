<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.BackgroundMoban" %>

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
          模板编号：
          <asp:TextBox ID="tbSN" style="height:20px; line-height:20px;" onFocus="this.className='colorfocus';" onBlur="this.className='colorblur';" Size="20" runat="server"></asp:TextBox>
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
      <td>模板编号 </td>
      <td width="214">模板图片 </td>
      <td>组别 </td>
      <td>行业 </td>
      <td>颜色 </td>
      <td>描述 </td>
      <td>上线日期</td>
      <td>下载</td>
      <td>阿里云</td>
      <td>表单</td>
      <td></td>
      <td></td>
      <td style="width:20px;"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="rptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center"><asp:Literal ID="ltlSN" runat="server"></asp:Literal></td>
          <td class="center">
            <asp:Literal ID="ltlCover" runat="server"></asp:Literal>
          </td>
          <td class="center"><asp:Literal ID="ltlCategory" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIndustry" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlColor" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlSummary" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlAddDate" runat="server"></asp:Literal></td>
          
          <td class="center"><asp:Literal ID="ltlDownloadUrl" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsAliyun" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsInitializationForm" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlInitializationFormUrl" runat="server"></asp:Literal></td>
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