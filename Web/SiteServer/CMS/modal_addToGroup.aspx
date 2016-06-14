<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.Modal.AddToGroup" Trace="false"%>

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
<asp:Button id="btnSubmit" useSubmitBehavior="false" OnClick="Submit_OnClick" runat="server" style="display:none" />
<bairong:alerts runat="server"></bairong:alerts>

  <table class="table table-noborder table-hover">
    <tr>
      <td>
        <asp:CheckBoxList ID="cblGroupNameCollection" RepeatColumns="2" RepeatDirection="Horizontal" class="noborder" Width="100%" runat="server"/>
      </td>
    </tr>
    <tr>
      <td>
        <hr />
      </td>
    </tr>
    <tr>
      <td>
        <ul class="breadcrumb">
        <table width="100%">
          <tr>
          <td>
            <label class="checkbox inline"><input type="checkbox" onClick="_checkFormAll(this.checked);"> 全选</label>
          </td>
          <td class="align-right">
              <asp:Button ID="btnAddGroup" class="btn" runat="server"></asp:Button>
          </td>
        </tr>
        </table>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->