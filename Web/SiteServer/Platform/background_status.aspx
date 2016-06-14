<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundStatus" %>

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

  <style type="text/css">
  .normal {
    background: url('../pic/icon/normal.png') no-repeat 0 0;margin-right: 8px;display: inline-block;width: 18px;height: 18px;vertical-align: middle;
  }
  .issue{
    background: url('../pic/icon/issue.png') no-repeat 0 0;margin-right: 8px;display: inline-block;width: 18px;height: 18px;vertical-align: middle;
  }
  .text{
    vertical-align: middle;margin-right: 0px;width: 180px;height: 20px;color:#333333 !important;
  }
  </style>

  <div class="well-small">
    <h4 style="float:left"><asp:Literal id="ltlDateTime" runat="server" /></h4>
    <table style="float:right; margin-top:5px;">
      <tr>
      <td width="60"><span class="normal"></span><span class="text">正常</span></td>
      <td width="60"><span class="issue"></span><span class="text">问题</span></td>
      </tr>
    </table>
    <div style="clear:both"></div>
  </div>

  <div class="popover popover-static">
  <h3 class="popover-title">SiteServer Service 服务组件</h3>
  <div class="popover-content">
    
    <table class="table noborder">
      <tr>
        <td>
          <table class="table">
            <tr>
              <asp:Literal id="ltlService" runat="server" />
            </tr>
          </table>
        </td>
      </tr>
    </table>
  
    </div>
  </div>

  <div class="popover popover-static">
  <h3 class="popover-title">定时任务状态</h3>
  <div class="popover-content">
    
    <table class="table noborder">
      <tr>
        <td>
          <table class="table">
            <tr>
              <asp:Literal id="ltlTaskCreate" runat="server" />
              <asp:Literal id="ltlTaskPublish" runat="server" />
              <asp:Literal id="ltlTaskGather" runat="server" />
              <asp:Literal id="ltlTaskBackup" runat="server" />
            </tr>
          </table>
        </td>
      </tr>
    </table>
  
    </div>
  </div>

  <asp:PlaceHolder id="phStorage" runat="server">
    <div class="popover popover-static">
    <h3 class="popover-title">存储空间状态</h3>
    <div class="popover-content">
      
      <table class="table noborder">
        <tr>
          <td>
            <table class="table">
              <asp:Literal id="ltlStorage" runat="server" />
            </table>
          </td>
        </tr>
      </table>
    
      </div>
    </div>
  </asp:PlaceHolder>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->