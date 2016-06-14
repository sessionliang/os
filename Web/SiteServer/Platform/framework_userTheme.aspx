<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkUserTheme" %>

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
  <bairong:alerts runat="server" />

  <script language="javascript" type="text/javascript">
  function gotoMain()
  {
    window.top.location.href = "../main.aspx?module=<%=Module%>";
  }
  </script>

  <ul class="nav nav-pills">
    <li class="active"><a href="framework_userTheme.aspx"><lan>主题设置</lan></a></li>
    <li><a href="framework_userLanguage.aspx"><lan>语言设置</lan></a></li>
    <li><a href="framework_userProfile.aspx"><lan>修改资料</lan></a></li>
    <li><a href="framework_userPassword.aspx"><lan>更改密码</lan></a></li>
  </ul>
  
  <div class="popover popover-static">
  <h3 class="popover-title"><lan>主题设置</lan></h3>
  <div class="popover-content">

    <input type="hidden" id="CurrentTheme" value="<%=GetCurrentTheme()%>" />

    <asp:dataGrid id="dgContents" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" runat="server">
      <Columns>
        <asp:TemplateColumn HeaderText="选择">
          <ItemTemplate>
            <asp:Literal id="ltlSelect" runat="server" />
          </ItemTemplate>
          <ItemStyle Width="50" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="主题名称">
          <ItemTemplate>
            <asp:Literal id="ltlName" runat="server" />
          </ItemTemplate>
          <ItemStyle Width="120" cssClass="center"/>
        </asp:TemplateColumn>
      </Columns>
    </asp:dataGrid>
    
    <table class="table noborder table-hover">
      <tr>
        <td width="120"></td>
        <td>
          
        </td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <input type="button" class="btn btn-primary" value="修 改" onClick="javascript:location.href='framework_userTheme.aspx?module=<%=Module%>&SetTheme=True&ThemeName=' + document.getElementById('CurrentTheme').value;" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->