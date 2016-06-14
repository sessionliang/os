<%@ Page Language="C#"  Inherits="SiteServer.CMS.BackgroundPages.BackgroundShareStatistics" %>

<%@ Register TagPrefix="bairong" Namespace="BaiRong.Controls" Assembly="BaiRong.Controls" %>

<%@ Register TagPrefix="site" Namespace="SiteServer.CMS.Controls" Assembly="SiteServer.CMS" %>
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

  <div class="well well-small">
    <table class="table table-noborder">
      <tr>
        <td>
          时间：从
          <bairong:DateTimeTextBox id="DateFrom" Columns="12" runat="server" />
          &nbsp;到&nbsp;
          <bairong:DateTimeTextBox id="DateTo" Columns="12" runat="server" />
          <span>输入指定页面的地址:<asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></span>
          <asp:Button ID="btnCheck" class="btn" runat="server" Text="查询" OnClick="btnCheck_Click" />
        </td>
      </tr>
    </table>
  </div>

  <table width="98%" border="0" class="center" cellpadding="0" cellspacing="0" id="head1" style="margin-top:20px">
    <tr>
      <td colspan="2"><table border="0" cellpadding="0" cellspacing="0" style="height:28px;">
          <tr>
            <td width="84" class="center" style="background:url(../Pic/itemnote2.gif)">&nbsp;平台统计&nbsp;</td>
            <td width="84" class="center" style="background:url(../Pic/itemnote1.gif)"><a href="#" onclick="ShowItem2()"><u>分享统计</u></a></td>
          </tr>
        </table></td>
    </tr>
  </table>
  <table width="98%" border="0" class="center" cellpadding="0" cellspacing="0" id="head2" style="display:none;margin-top:20px;line-height:28px;" >
    <tr>
      <td colspan="2"><table style="height:28" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="84" class="center" style="background:url(../Pic/itemnote1.gif)"><a href="#" onclick="ShowItem1()"><u>平台统计</u></a>&nbsp;</td>
            <td width="84" class="center"  style="background:url(../Pic/itemnote2.gif)">分享统计</td>
          </tr>
        </table></td>
     
    </tr>
  </table>
  <table width="98%"  border="0" class="center" cellpadding="2" cellspacing="2" id="needset" style="border:1px solid #cfcfcf;background:#ffffff;margin-top:-5px;">
    <tr>
      <td width="400%" height="24" class="bline"><table width="498" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="152">&nbsp;bURL社交影响力分析：</td>
            <td width='346'>&nbsp;</td>
          </tr>
          <tr>
            <td colspan="2"> 说明：数据格式“平台.(访问占用百分比)-访问次数”。 <br />
              <asp:Literal ID="ltlPlatform" runat="server"></asp:Literal></td>
          </tr>
        </table></td>
    </tr>
  </table>
  <table width="98%"  border="0" class="center" cellpadding="2" cellspacing="2" id="adset" style="border:1px solid #cfcfcf;background:#ffffff; margin-top:-5px;display:none">
    <tr>
      <td height="24" class="bline" ><table width="498" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td height="22"><strong>分享统计信息：</strong></td>
          </tr>
          <tr>
            <td height="22"><p>分享量统计表:</p>
              <p><asp:Literal ID="ltlShareCounts" runat="server"></asp:Literal></p>
              <p>点击统计表</p>
              <p><asp:Literal ID="ltlClickCoutns" runat="server"></asp:Literal></p></td>
          </tr>
        </table></td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->