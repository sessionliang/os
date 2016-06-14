<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.Modal.SelectImage" Trace="false"%>

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

  <script language="javascript" type="text/javascript">
  function selectImage(textBoxUrl, imageUrl) {

    window.parent.document.getElementById('<%=Request.QueryString["TextBoxClientID"]%>').value = textBoxUrl;
    window.parent.closeWindow();
  }
  </script>

  <table class="table table-noborder">
    <TR>
      <TD width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/back.gif" CommandName="NavigationBar" CommandArgument="Back" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/up.gif" CommandName="NavigationBar" CommandArgument="Up" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="50"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/reload.gif" CommandName="NavigationBar" CommandArgument="Reload" OnCommand="LinkButton_Command"></asp:ImageButton></TD>
      <TD width="5"><asp:ImageButton runat="server" ImageUrl="~/sitefiles/bairong/icons/filesystem/management/seperator.gif"></asp:ImageButton></TD>
      <TD width="80"><nobr>
        <asp:HyperLink ID="UploadLink" runat="server">
          <asp:ImageButton  runat="server" ImageUrl="~/sitefiles/bairong/icons/add.gif" ImageAlign="AbsBottom"></asp:ImageButton>
          上传图片</asp:HyperLink>
        </nobr></TD>
      <TD align="right"><SPAN>当前目录：
        <bairong:NoTagText id="CurrentDirectory" runat="server" />
        &nbsp;</SPAN></TD>
    </TR>
  </table>

  <hr />
  
  <asp:Literal id="ltlFileSystems" runat="server"></asp:Literal>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->