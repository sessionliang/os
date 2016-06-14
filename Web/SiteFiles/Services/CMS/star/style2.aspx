<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Star" %>
<span class="stlStar">
	<span class="stars">
    	<asp:Repeater ID="rpStars" runat="server">
        	<itemtemplate><asp:Literal id="ltlStar" runat="server" /></itemtemplate>
        </asp:Repeater>
    </span>
</span>