<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Star" %>

<div style="height:50px;">
<div class="stlStar">
	<span class="shi"><asp:Literal ID="ltlNum1" runat="server"></asp:Literal></span><span class="ge">.<asp:Literal ID="ltlNum2" runat="server"></asp:Literal></span>
	<div class="stars">
    	<asp:Repeater ID="rpStars" runat="server">
        	<itemtemplate>
                <asp:Literal id="ltlStar" runat="server" />
            </itemtemplate>
        </asp:Repeater>
    </div>
	<span class="scorer">(<span>已有<asp:Literal ID="ltlTotalCount" runat="server"></asp:Literal>人评分</span>)</span>
</div>
</div>