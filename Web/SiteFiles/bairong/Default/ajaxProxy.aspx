<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace=System.Net %>
<%@ Import Namespace=System.IO %>
<%@ Import Namespace=System.Text %>
<%@ Import Namespace=System.Collections %>
<%@ Import Namespace=System.Collections.Specialized %>

<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

		string charset = Page.Request.QueryString["charset"];
		string url = Page.Request.QueryString["url"];
		string isXmlContent = Page.Request.QueryString["isXmlContent"];
		Encoding encoding = Encoding.GetEncoding(charset);
		
		if (url.IndexOf("?") == -1)
		{
			url += "?isCrossSite=True";
		}
		else
		{
			url += "&isCrossSite=True";
		}
		
		foreach (string key in Page.Request.QueryString.Keys){
			if (key != "charset" && key != "url" && key != "isXmlContent"){
				string value = Page.Request.QueryString[key];
				url += string.Format("&{0}={1}", key, value);
			}
		}		
		
		this.Page.Response.Clear();
		if (isXmlContent == "true")
		{
			this.Page.Response.ContentType = "Text/XML";
		}
		this.Page.Response.Expires = 0;
		this.Page.Response.Cache.SetNoStore();
		this.Page.Response.Write(TransferHtmlPage(encoding, url));
		this.Page.Response.End();
    }
    
    public string TransferHtmlPage(Encoding encoding, string url)
    {
        string result = string.Empty;
		try
		{
			WebClient webClient = new WebClient();
			webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			
			NameValueCollection attributes = new NameValueCollection();
			foreach (string key in base.Request.Form.Keys)
			{
				attributes.Add(key, HttpUtility.UrlEncode(base.Request.Form[key], encoding));
			}

			byte[] responseData = webClient.UploadValues(url, "POST", attributes);
			result = encoding.GetString(responseData);
		}
		catch (Exception ex)
		{
			result = string.Format(@"<p style='color:red;text-align:center;'>服务器获取文件内容出错：{0}<br />网址：{1}<br />编码：{2}</p>", ex.Message, url, encoding.EncodingName);
		}

		return result;
    }

</script>
