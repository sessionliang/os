<%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.FrameworkUserLanguage" %>

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
    <li><a href="framework_userTheme.aspx"><lan>主题设置</lan></a></li>
    <li class="active"><a href="framework_userLanguage.aspx"><lan>语言设置</lan></a></li>
    <li><a href="framework_userProfile.aspx"><lan>修改资料</lan></a></li>
    <li><a href="framework_userPassword.aspx"><lan>更改密码</lan></a></li>
  </ul>

  <div class="popover popover-static">
  <h3 class="popover-title">语音设置</h3>
  <div class="popover-content">
    
    <input type="hidden" id="CurrentLanguage" value="<%=GetCurrentLanguage()%>" />
    <asp:dataGrid id="MyDataGrid" runat="server" showHeader="true"
        ShowFooter="false"
        AutoGenerateColumns="false"
        HeaderStyle-CssClass="info thead"
        CssClass="table table-bordered table-hover"
        gridlines="none">
      <Columns>
        <asp:TemplateColumn HeaderText="<lan>选择</lan>">
          <ItemTemplate>
            <input type="radio" name="choose" id="choose" onClick="document.getElementById('CurrentLanguage').value=this.value;" value='<%# Container.DataItem%>' <%# GetChecked((string)Container.DataItem)%> />
          </ItemTemplate>
          <ItemStyle Width="100" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="<lan>语言</lan>">
          <ItemTemplate> <%#GetLanguageName((string)Container.DataItem) %> </ItemTemplate>
          <ItemStyle cssClass="center"/>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
          <ItemTemplate> <%#Container.DataItem %> </ItemTemplate>
          <ItemStyle Width="100" cssClass="center"/>
        </asp:TemplateColumn>
      </Columns>
    </asp:dataGrid>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <input type="button" class="btn btn-primary" value="<lan>修 改</lan>" onClick="javascript:location.href='framework_userLanguage.aspx?module=<%=Module%>&SetLanguage=True&Language=' + document.getElementById('CurrentLanguage').value;" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</HTML>
