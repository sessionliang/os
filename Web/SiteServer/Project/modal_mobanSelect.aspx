<%@ Page Language="C#" Inherits="SiteServer.Project.BackgroundPages.Modal.MobanSelect" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
  
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
      <td>阿里云</td>
      <td>表单</td>
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
          
          <td class="center"><asp:Literal ID="ltlIsAliyun" runat="server"></asp:Literal></td>
          <td class="center"><asp:Literal ID="ltlIsInitializationForm" runat="server"></asp:Literal></td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="spContents" runat="server" class="table table-pager" />

</form>
</body>
</html>