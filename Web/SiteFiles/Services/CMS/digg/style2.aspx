<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Digg" %>

<table border="0" cellpadding="0" cellspacing="8" class="newdigg">
  <tr>
    <asp:PlaceHolder ID="phGood" runat="server">
    <td>
      <table border="0" align="center" cellpadding="0" cellspacing="0" class="digg">
        <tr>
          <td class="diggnum" id="diggnum">
            <strong><asp:Literal ID="ltlGoodNum" runat="server"></asp:Literal></strong>
          </td>
        </tr>
        <tr>
          <td class="diggit">
            <a href="javascript:;" onclick="<%=ClickStringOfGood%>"><asp:Literal ID="ltlGoodText" runat="server"></asp:Literal></a>
          </td>
        </tr>
      </table>
    </td>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phBad" runat="server">
    <td>
      <table border="0" align="center" cellpadding="0" cellspacing="0" class="digg">
        <tr>
          <td class="diggnum" id="diggnum">
            <strong><asp:Literal ID="ltlBadNum" runat="server"></asp:Literal></strong>
          </td>
        </tr>
        <tr>
          <td class="diggit">
            <a href="javascript:;" onclick="<%=ClickStringOfBad%>"><asp:Literal ID="ltlBadText" runat="server"></asp:Literal></a>
          </td>
        </tr>
      </table>
    </td>
    </asp:PlaceHolder>
  </tr>
</table>
