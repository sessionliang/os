<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Digg" %>

<div>
	<asp:PlaceHolder ID="phGood" runat="server">
    <div class='diggArea'>
        <div class='diggNum'><asp:Literal ID="ltlGoodNum" runat="server"></asp:Literal></div>
        <div class="diggLink"><a href="javascript:;" onclick="<%=ClickStringOfGood%>"><asp:Literal ID="ltlGoodText" runat="server"></asp:Literal></a></div>
    </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phBad" runat="server">
    <div class='diggArea'>
        <div class='diggNum'><asp:Literal ID="ltlBadNum" runat="server"></asp:Literal></div>
        <div class="diggLink"><a href="javascript:;" onclick="<%=ClickStringOfBad%>"><asp:Literal ID="ltlBadText" runat="server"></asp:Literal></a></div>
    </div>
    </asp:PlaceHolder>
</div>