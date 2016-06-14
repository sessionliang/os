<%@ Page Language="C#" Inherits="SiteServer.B2C.BackgroundPages.Modal.GoodsPhotoSelect" Trace="false" %>

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

  <script type="text/javascript" language="javascript">
  <asp:Literal id="ltlScript" runat="server"></asp:Literal>
  </script>

  <table class="table table-noborder table-hover">
    <tr>
      <td>
          <asp:Repeater ID="rptPhotos" runat="server">
          <itemtemplate>
          <div style="margin: 2px 10px; float: left">
            <input name="PhotoIDCollection" type="checkbox" value="<%#DataBinder.Eval(Container.DataItem,"ID")%>">
            <input id="Url_<%#DataBinder.Eval(Container.DataItem,"ID")%>" name="Url_<%#DataBinder.Eval(Container.DataItem,"ID")%>" type="hidden" value="<%#ParseIconUrl((string)DataBinder.Eval(Container.DataItem,"SmallUrl"))%>">
            <img src="<%#ParseIconUrl((string)DataBinder.Eval(Container.DataItem,"SmallUrl"))%>" />
          </div>
          </itemtemplate>
          </asp:Repeater>
      </td>
    </tr>
  </table>

</form>
</body>
</html>
<!-- check for 3.6.4 html permissions -->