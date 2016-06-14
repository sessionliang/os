<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundConfigurationCredit2" %>

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

  <div class="popover popover-static">
  <h3 class="popover-title">积分奖赏设置</h3>
  <div class="popover-content">
    
    <asp:dataGrid id="MyDataGrid" runat="server" showHeader="true"
        ShowFooter="false"
        AutoGenerateColumns="false"
        HeaderStyle-CssClass="info thead"
        CssClass="table table-bordered table-hover"
        gridlines="none">
      <Columns>
        <asp:TemplateColumn HeaderText="动作">
          <ItemTemplate>
            <asp:Literal ID="ltlRuleName" runat="server"></asp:Literal>
          </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="周期">
          <ItemTemplate>
            <asp:Literal ID="ltlPeriodType" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="100" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="最高次数">
          <ItemTemplate>
            <asp:Literal ID="ltlMaxNum" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="100" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="威望">
          <ItemTemplate>
            <input id="<%# Container.DataItem%>_Prestige" name="<%# Container.DataItem%>_Prestige" type="text" value="<%# GetCreditValue("Prestige", (string)Container.DataItem)%>"  style="width:60px;" />
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="贡献">
          <ItemTemplate>
            <input id="<%# Container.DataItem%>_Contribution" name="<%# Container.DataItem%>_Contribution" type="text" value="<%# GetCreditValue("Contribution", (string)Container.DataItem)%>"  style="width:60px;" />
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="金钱">
          <ItemTemplate>
            <input id="<%# Container.DataItem%>_Currency" name="<%# Container.DataItem%>_Currency" type="text" value="<%# GetCreditValue("Currency", (string)Container.DataItem)%>"  style="width:60px;" />
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn>
          <ItemTemplate>
            <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
      </Columns>
    </asp:dataGrid>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button ID="SetButton" class="btn btn-primary" OnClick="SetButton_Click" Text="设 置" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->