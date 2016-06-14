<%@ Page Language="C#" Inherits="SiteServer.BBS.BackgroundPages.BackgroundConfigurationCredit1" %>

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
  <bairong:alerts text="总积分是衡量用户级别的唯一标准，你可以在此设定用户的总积分计算公式。" runat="server"></bairong:alerts>

  <div class="popover popover-static">
  <h3 class="popover-title">总积分计算公式</h3>
  <div class="popover-content">
    
    <table class="table noborder table-hover">
      <tr>
        <td><strong>总积分计算公式:</strong>&nbsp;&nbsp;
        <asp:Literal ID="ltlCreditCalculate" runat="server"></asp:Literal></td>
      </tr>
    </table>
  
    <hr />
    <table class="table noborder">
      <tr>
        <td class="center">
          <asp:Button ID="SetButton" class="btn btn-success" Text="设置总积分计算公式" runat="server" />
        </td>
      </tr>
    </table>
  
    </div>
  </div>

  <div class="popover popover-static">
  <h3 class="popover-title">系统积分类型</h3>
  <div class="popover-content">
    
    <asp:dataGrid id="MyDataGrid1" runat="server" showHeader="true"
        ShowFooter="false"
        AutoGenerateColumns="false"
        HeaderStyle-CssClass="info thead"
        CssClass="table table-bordered table-hover"
        gridlines="none">
      <Columns>
        <asp:TemplateColumn HeaderText="积分代号">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditID" runat="server"></asp:Literal>
          </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="积分名称">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditName" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="积分单位">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditUnit" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="初始值">
          <ItemTemplate>
            <asp:Literal ID="ltlInitial" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="是否启用">
          <ItemTemplate>
            <asp:Literal ID="ltlUsing" runat="server"></asp:Literal>
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
  
    </div>
  </div>

  <div class="popover popover-static">
  <h3 class="popover-title"><asp:Literal id="ltlPageTitle" runat="server" /></h3>
  <div class="popover-content">
    
    <asp:dataGrid id="MyDataGrid2" runat="server" showHeader="true"
        ShowFooter="false"
        AutoGenerateColumns="false"
        HeaderStyle-CssClass="info thead"
        CssClass="table table-bordered table-hover"
        gridlines="none">
      <Columns>
        <asp:TemplateColumn HeaderText="积分代号">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditID" runat="server"></asp:Literal>
          </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="积分名称">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditName" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="积分单位">
          <ItemTemplate>
            <asp:Literal ID="ltlCreditUnit" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="初始值">
          <ItemTemplate>
            <asp:Literal ID="ltlInitial" runat="server"></asp:Literal>
          </ItemTemplate>
          <ItemStyle Width="70" cssClass="center" />
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="是否启用">
          <ItemTemplate>
            <asp:Literal ID="ltlUsing" runat="server"></asp:Literal>
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
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->