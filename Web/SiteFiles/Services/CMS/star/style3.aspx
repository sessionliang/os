<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Star" %>

<div class="stlStar">
	<div class="stars">
    	<asp:Repeater ID="rpStars" runat="server">
        	<itemtemplate>
                <asp:Literal id="ltlStar" runat="server" />
            </itemtemplate>
        </asp:Repeater>
        <span class="font-num"><asp:Literal ID="ltlNum1" runat="server"></asp:Literal>.<asp:Literal ID="ltlNum2" runat="server"></asp:Literal></span>
    </div>
	
</div>