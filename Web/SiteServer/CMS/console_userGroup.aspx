<%@ Page Language="C#" Inherits="SiteServer.CMS.BackgroundPages.ConsoleUserGroup" %>

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
  <asp:Literal id="ltlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <div class="popover popover-static">
    <h3 class="popover-title">积分用户组</h3>
    <div class="popover-content">
    
      <div class="alert alert-info">
        积分用户组以积分确定组别和权限，系统将根据用户积分判断所在组别并赋予相应的阅读及操作权限。
      </div>

      <asp:dataGrid id="MyDataGrid1" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
        <Columns>
          <asp:TemplateColumn HeaderText="会员组名称">
            <ItemTemplate>
              <asp:Literal ID="ltlGroupName" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle cssClass="center" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="最低积分">
            <ItemTemplate>
              <asp:Literal ID="ltlCreditsFrom" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="最高积分">
            <ItemTemplate>
              <asp:Literal ID="ltlCreditsTo" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="星星数">
            <ItemTemplate>
              <asp:Literal ID="ltlStars" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="权限级别">
            <ItemTemplate>
              <asp:Literal ID="ltlRank" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:Literal ID="ltlAddUrl" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" cssClass="center" />
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" cssClass="center" />
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" cssClass="center" />
          </asp:TemplateColumn>
        </Columns>
      </asp:dataGrid>

      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button ID="AddButton1" class="btn btn-success" Text="添加积分用户组" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

  <div class="popover popover-static">
    <h3 class="popover-title">特殊用户组</h3>
    <div class="popover-content">
    
      <div class="alert alert-info">
        特殊用户组是人为设定，不需要指定积分，特殊组的用户需要在编辑会员时将其加入。
      </div>

      <asp:dataGrid id="MyDataGrid2" showHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="info thead" CssClass="table table-bordered table-hover" gridlines="none" runat="server">
        <Columns>
          <asp:TemplateColumn HeaderText="会员组名称">
            <ItemTemplate>
              <asp:Literal ID="ltlGroupName" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle cssClass="center" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="星星数">
            <ItemTemplate>
              <asp:Literal ID="ltlStars" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn HeaderText="权限级别">
            <ItemTemplate>
              <asp:Literal ID="ltlRank" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" HorizontalAlign="left" />
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:Literal ID="ltlEditUrl" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" cssClass="center" />
          </asp:TemplateColumn>
          <asp:TemplateColumn>
            <ItemTemplate>
              <asp:Literal ID="ltlDeleteUrl" runat="server"></asp:Literal>
            </ItemTemplate>
            <ItemStyle Width="70" cssClass="center" />
          </asp:TemplateColumn>
        </Columns>
      </asp:dataGrid>
  
      <hr />
      <table class="table noborder">
        <tr>
          <td class="center">
            <asp:Button ID="AddButton2" class="btn btn-success" Text="添加特殊用户组" runat="server" />
          </td>
        </tr>
      </table>
  
    </div>
  </div>

</form>
</body>
</html>
<!-- check for 3.6 html permissions -->