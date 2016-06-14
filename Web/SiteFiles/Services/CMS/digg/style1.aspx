<%@ Page language="c#" trace="false" enableViewState="false" Inherits="SiteServer.CMS.Services.Digg" %>

<div>
<div class="newdigg" id="newdigg">
	<asp:PlaceHolder ID="phGood" runat="server">
	<div class="diggbox digg_good" onmousemove="this.style.backgroundPosition='left bottom';" onmouseout="this.style.backgroundPosition='left top';" onclick="<%=ClickStringOfGood%>">
		<div class="digg_act"><asp:Literal ID="ltlGoodText" runat="server"></asp:Literal></div>
		<div class="digg_num">(<asp:Literal ID="ltlGoodNum" runat="server"></asp:Literal>)</div>
		<div class="digg_percent">
			<div class="digg_percent_bar"><span style="width:<%=GoodPercentage%>%"></span></div>
			<div class="digg_percent_num"><%=GoodPercentage%>%</div>
		</div>
	</div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phBad" runat="server">
	<div class="diggbox digg_bad" onmousemove="this.style.backgroundPosition='right bottom';" onmouseout="this.style.backgroundPosition='right top';" onclick="<%=ClickStringOfBad%>">
		<div class="digg_act"><asp:Literal ID="ltlBadText" runat="server"></asp:Literal></div>
		<div class="digg_num">(<asp:Literal ID="ltlBadNum" runat="server"></asp:Literal>)</div>
		<div class="digg_percent">
			<div class="digg_percent_bar"><span style="width:<%=BadPercentage%>%"></span></div>
			<div class="digg_percent_num"><%=BadPercentage%>%</div>
		</div>
	</div>
    </asp:PlaceHolder>
</div>
</div>