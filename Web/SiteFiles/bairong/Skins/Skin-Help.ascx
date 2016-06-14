<%@ Control %>

<label id="label" runat="server">
	<asp:linkbutton id="cmdHelp" tabIndex="-1" runat="server" CausesValidation="False">
		<asp:image id="imgHelp" tabindex="-1" runat="server" imageurl="~/sitefiles/bairong/icons/help.gif"></asp:image>
	</asp:linkbutton>
	<nobr><asp:label id="lblLabel" runat="server" enableviewstate="False"></asp:label></nobr>
</label><br>
<asp:panel id="pnlHelp" runat="server" cssClass="Help">
	<asp:label id="lblHelp" runat="server" enableviewstate="False"></asp:label>
</asp:panel>