<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace=System.Text %>
<%@ Import Namespace=System.Collections %>

<script runat="server">
protected override void OnLoad(EventArgs e)
{
	base.OnLoad(e);

	this.Page.Response.Clear();
	this.Page.Response.Expires = 0;
	this.Page.Response.Cache.SetNoStore();
	
	string callback = Decode(Page.Request.QueryString["callback"]);
	this.Page.Response.Write(callback);
	this.Page.Response.End();
}

public string Decode(string data)
{
	data = data.Replace("0add0", "+").Replace("0equals0", "=").Replace("0and0", "&").Replace("0question0", "?").Replace("0quote0", "'").Replace("0slash0", "/");

	byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes("PlMqAzuE");
	byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes("PlMqAzuE");

	byte[] byEnc;
	try
	{
		byEnc = Convert.FromBase64String(data);
	}
	catch
	{
		return null;
	}

	System.Security.Cryptography.DESCryptoServiceProvider cryptoProvider = new System.Security.Cryptography.DESCryptoServiceProvider();
	System.IO.MemoryStream ms = new System.IO.MemoryStream(byEnc);
	System.Security.Cryptography.CryptoStream cst = new System.Security.Cryptography.CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), System.Security.Cryptography.CryptoStreamMode.Read);
	System.IO.StreamReader sr = new System.IO.StreamReader(cst);
	return sr.ReadToEnd();
}
</script>
