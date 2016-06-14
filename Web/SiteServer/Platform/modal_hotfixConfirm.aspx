<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.Modal.HotfixConfirm" Trace="false"%>

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
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table noborder">
    <tr>
      <td>
        <div class="alert alert-info">
          升级操作根据网速的不同，可能将持续几分钟，在进行产品升级之前，<font style="color:red">建议手动备份您的数据库和程序文件</font>，确定开始升级吗？
        </div>
        <ul class="breadcrumb breadcrumb-button center">
          <asp:Button class="btn btn-success" OnClick="btnHotfix_OnClick" Text="开始升级" runat="server" />
        </ul>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->