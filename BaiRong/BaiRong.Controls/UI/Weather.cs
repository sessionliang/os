using System;
using System.Text;
using System.Collections;
using System.Web;
using System.Web.UI;

using BaiRong.Core;
using BaiRong.Model;
using BaiRong.Core.Net;

namespace BaiRong.Controls
{
	public class Weather : LiteralControl 
	{
		public virtual string City
		{
			get 
			{
				Object state = ViewState["City"];
				if ( state != null ) 
				{
					return (string)state;
				}
				return "北京";
			}
			set 
			{
				ViewState["City"] = value;
			}
		}

		public virtual string Provider
		{
			get 
			{
				string provider = Weather.yahoo;
				Object state = ViewState["Provider"];
				if ( state != null ) 
				{
					provider = ((string)state).Trim().ToLower();
					if (provider != Weather.yahoo && provider != Weather.sina)
					{
						provider = Weather.yahoo;
					}
				}
				return provider;
			}
			set 
			{
				ViewState["Provider"] = value;
			}
		}

		private const string sina = "sina";
		private const string yahoo = "yahoo";

		protected override void Render(HtmlTextWriter writer)
		{
			string weatherHtml = this.GetWeatherHtml(this.Provider);
			if (string.IsNullOrEmpty(weatherHtml))
			{
				string otherProvider = (this.Provider == Weather.yahoo) ? Weather.sina : Weather.yahoo;
				weatherHtml = this.GetWeatherHtml(otherProvider);
			}
			writer.Write(weatherHtml);
		}

		protected string GetWeatherHtml(string provider)
		{
			string result = string.Empty;

			try
			{
				string group = "title";
				if (provider == Weather.yahoo)
				{
					string pinyinCity = TranslateUtils.ToPinYin(this.City);
					string weatherUrl = string.Format("http://weather.cn.yahoo.com/weather.html?city={0}&s=1", pinyinCity);
					string regex = string.Format(@"<\!--today\s+-->\s+<div\s+class=""dt_c"">\s+<div\s+class=""tn"">{0}-{1}-{2}</div>\s*(?<title>[\s\S]+?)\s*</div>\s+<\!--//today\s+-->", DateTime.Now.Year, TranslateUtils.ToTwoCharString(DateTime.Now.Month), TranslateUtils.ToTwoCharString(DateTime.Now.Day));

                    string content = WebClientUtils.GetRemoteFileSource(weatherUrl, ECharset.utf_8);

					if (!string.IsNullOrEmpty(content))
					{
						content = RegexUtils.GetContent(group, regex, content);

						string flashRegex = @"<p>\s*(?<title>[\s\S]+?)\s*</p>";
						string weatherStringRegex = @"<span\s+class=""ft1"">\s*(?<title>[\s\S]+?)\s*</span>";
						string highDegreeRegex = @"<span\s+class=""hitp"">\s*(?<title>[\s\S]+?)\s*</span>";
						string lowDegreeRegex = @"<span\s+class=""lotp"">\s*(?<title>[\s\S]+?)\s*</span>";

						string flashHtml = RegexUtils.GetContent(group, flashRegex, content);
						flashHtml = flashHtml.Replace("width=\"64\" height=\"64\"", "width=\"20\" height=\"20\"");
						string weatherString = RegexUtils.GetContent(group, weatherStringRegex, content);
						string highDegree = RegexUtils.GetContent(group, highDegreeRegex, content);
						string lowDegree = RegexUtils.GetContent(group, lowDegreeRegex, content);

						if (!string.IsNullOrEmpty(highDegree))
						{
							result = flashHtml + weatherString + string.Format("&nbsp;{0} ~ {1}", highDegree, lowDegree);
						}
					}
				}
				else if (provider == Weather.sina)
				{
					string weatherUrl = string.Format("http://php.weather.sina.com.cn/search.php?city={0}", PageUtils.UrlEncode(this.City));
					string regex = "<td\\s+bgcolor=\\#FEFEFF\\s+height=25\\s+style=\"padding-left:20px;\"\\s+align=center>24小时</td>\\s*(?<title>[\\s\\S]+?)\\s*</tr>";

                    string content = WebClientUtils.GetRemoteFileSource(weatherUrl, ECharset.gb2312);

					if (!string.IsNullOrEmpty(content))
					{
						content = RegexUtils.GetContent(group, regex, content);

						ArrayList contentArrayList = RegexUtils.GetTagInnerContents("td", content);

						string weatherString = (string)contentArrayList[0];
						string highDegree = (string)contentArrayList[1];
						string lowDegree = (string)contentArrayList[2];

						if (!string.IsNullOrEmpty(highDegree))
						{
							result = weatherString + string.Format("&nbsp;{0} ~ {1}", highDegree, lowDegree);
						}
					}
				}
			}
			catch{}
			
			return result;
		}
	}
}
