 <%@ Page Language="C#" Inherits="BaiRong.BackgroundPages.BackgroundLevelRule" %>

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
  <h3 class="popover-title">积分（币）规则</h3>
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
        <asp:TemplateColumn HeaderText="积分">
          <ItemTemplate>
            <input id="<%# Container.DataItem%>_Credit" name="<%# Container.DataItem%>_Credit" type="text" value="<%# GetLevelValue("CreditNum", (string)Container.DataItem)%>"  style="width:60px;" />
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="币">
          <ItemTemplate>
            <input id="<%# Container.DataItem%>_Cash" name="<%# Container.DataItem%>_Cash" type="text" value="<%# GetLevelValue("CashNum", (string)Container.DataItem)%>"  style="width:60px;" />
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