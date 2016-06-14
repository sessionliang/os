using System.Web.UI;
using BaiRong.Core;
using System.Collections.Specialized;
using System.Collections.Generic;
using System;

namespace SiteServer.CMS.Core
{
    public class JsManager
    {
        public class CMSService
        {
            public const string GatherServiceName = "GatherService";
            public const string OtherServiceName = "OtherService";

            public static string RegisterWaitingTaskScript(string serviceName, string methodName, string parameters)
            {
                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        writeResult(data.resultMessage, data.errorMessage);
        if (data.executeCode)
		{{
			setTimeout(function()
			{{
				eval(data.executeCode);
			}}, 1000);
		}}
    }}, 'json');
}}

$(document).ready(function(){{
    WebServiceUtility_Task();
}});
</script>
", PageUtility.GetCMSServiceUrlByPage(serviceName, methodName), parameters);
            }

            public static string RegisterProgressTaskScript(string serviceName, string methodName, string pars, string userKeyPrefix)
            {
                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        clearInterval(intervalID);
        if (data.isPoweredBy == 'false')
        {{
            $('#poweredby').show();
        }}
        writeResult(data.resultMessage, data.errorMessage);
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            if (data.isPoweredBy == 'false')
            {{
                $('poweredby').show();
            }}
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtility.GetCMSServiceUrlByPage(serviceName, methodName), pars, PageUtility.GetCMSServiceUrlByPage(serviceName, "GetCountArray"), userKeyPrefix);
            }
        }

        public class STLService
        {
            public const string CreateServiceName = "CreateService";
            public const string BackupServiceName = "BackupService";
            public const string PublishServiceName = "PublishService";

            public static string RegisterWaitingTaskScript(string serviceName, string methodName, string parameters)
            {
                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        writeResult(data.resultMessage, data.errorMessage);
        if (data.executeCode)
		{{
			setTimeout(function()
			{{
				eval(data.executeCode);
			}}, 1000);
		}}
    }}, 'json');
}}

$(document).ready(function(){{
    WebServiceUtility_Task();
}});
</script>
", PageUtility.GetSTLServiceUrlByPage(serviceName, methodName), parameters);
            }

            public static string RegisterProgressTaskScript(string serviceName, string methodName, string pars, string userKeyPrefix)
            {
                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        clearInterval(intervalID);
        if (data.isPoweredBy == 'false')
        {{
            $('#poweredby').show();
        }}
        writeResult(data.resultMessage, data.errorMessage);
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            if (data.isPoweredBy == 'false')
            {{
                $('poweredby').show();
            }}
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtility.GetSTLServiceUrlByPage(serviceName, methodName), pars, PageUtility.GetSTLServiceUrlByPage(serviceName, "GetCountArray"), userKeyPrefix);
            }

            public static string RegisterProgressTaskScriptForService(string serviceName, string methodName, string pars, string userKeyPrefix)
            {
                return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        var totalCount = parseInt(data.totalCount);
        var currentCount = parseInt(data.currentCount);
        var queuingCount = parseInt(data.queuingCount);
        var errorCount = parseInt(data.errorCount);
        if (data.isPoweredBy == 'false')
        {{
            $('#poweredby').show();
        }}
        if(data.resultMessage!=''&&data.resultMessage!=undefined)
        {{
              clearInterval(intervalID);
              writeResult(data.resultMessage, data.errorMessage);
        }}
        else{{
              writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if(data.resultMessage!=''&&data.resultMessage!=undefined)
        {{
            clearInterval(intervalID);
            writeResult(data.resultMessage, data.errorMessage);
        }}
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            var queuingCount = parseInt(data.queuingCount);
            var errorCount = parseInt(data.errorCount);
            if (data.isPoweredBy == 'false')
            {{
                $('poweredby').show();
            }}
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtility.GetSTLServiceUrlByPage(serviceName, methodName), pars, PageUtility.GetSTLServiceUrlByPage(serviceName, "GetCountArrayForService"), userKeyPrefix);
            }

            public static string RegisterProgressTaskScript(string serviceName, string methodName, string pars, string userKeyPrefix, bool isRedirect)
            {
                if (!isRedirect)
                {
                    return RegisterProgressTaskScript(serviceName, methodName, pars, userKeyPrefix);
                }
                else
                {
                    return string.Format(@"
<script type=""text/javascript"" language=""javascript"">
var intervalID;
function WebServiceUtility_Task()
{{
	var url = '{0}';
	var pars = '{1}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        clearInterval(intervalID);
        if (data.isPoweredBy == 'false')
        {{
            $('#poweredby').show();
        }}
        writeResult(data.resultMessage, data.errorMessage);
        if (data.executeCode)
		{{
			setTimeout(function()
			{{
				eval(data.executeCode);
			}}, 1000);
		}}
    }}, 'json');
}}

function WebServiceUtility_GetCountArray()
{{
	var url = '{2}';
	var pars = 'userKeyPrefix={3}';

    jQuery.post(url, pars, function(data, textStatus)
    {{
        if (data.totalCount)
        {{
            var totalCount = parseInt(data.totalCount);
            var currentCount = parseInt(data.currentCount);
            if (data.isPoweredBy == 'false')
            {{
                $('poweredby').show();
            }}
            writeProgressBar(totalCount, currentCount, data.message);
        }}
    }}, 'json');
}}

function WebServiceUtility_Initialize()
{{
	WebServiceUtility_Task();
    intervalID = setInterval(WebServiceUtility_GetCountArray, 3000);
}}

$(document).ready(function(){{
    WebServiceUtility_Initialize();
}});
</script>
", PageUtility.GetSTLServiceUrlByPage(serviceName, methodName), pars, PageUtility.GetSTLServiceUrlByPage(serviceName, "GetCountArray"), userKeyPrefix);
                }
            }
        }

        public class AjaxService
        {
            public static NameValueCollection GetWaitingTaskNameValueCollection(string resultMessage, string errorMessage, string executeCode)
            {
                NameValueCollection retval = new NameValueCollection();
                retval.Add("resultMessage", resultMessage);
                retval.Add("errorMessage", errorMessage);
                retval.Add("executeCode", executeCode);
                return retval;
            }

            public static NameValueCollection GetProgressTaskNameValueCollection(string resultMessage, string errorMessage)
            {
                NameValueCollection retval = new NameValueCollection();
                retval.Add("resultMessage", resultMessage);
                retval.Add("errorMessage", errorMessage);
                return retval;
            }

            public static NameValueCollection GetCountArrayNameValueCollection(int totalCount, int currentCount, string message)
            {
                NameValueCollection retval = new NameValueCollection();
                retval.Add("totalCount", totalCount.ToString());
                retval.Add("currentCount", currentCount.ToString());
                retval.Add("message", message);
                return retval;
            }

            public static NameValueCollection GetCountArrayNameValueCollection(int totalCount, int currentCount, int errorCount, int queuingCount, string message)
            {
                NameValueCollection retval = new NameValueCollection();
                retval.Add("totalCount", totalCount.ToString());
                retval.Add("currentCount", currentCount.ToString());
                retval.Add("errorCount", errorCount.ToString());
                retval.Add("queuingCount", queuingCount.ToString());
                retval.Add("message", message);
                return retval;
            }
        }

        public static string GetShowHintScript()
        {
            return GetShowHintScript("操作进行中");
        }

        public static string GetShowHintScript(string message)
        {
            return GetShowHintScript(message, 120);
        }

        public static string GetShowHintScript(string message, int top)
        {
            return string.Format(@"hideBoxAndShowHint(this, '{0}, 请稍候...', {1});", message, top);
        }

        public static void RegisterClientScriptBlock(Page page, string key, string script)
        {
            if (!IsStartupScriptRegistered(page, key))
            {
                page.RegisterClientScriptBlock(key, script);
            }
        }

        public static void RegisterStartupScript(Page page, string key, string script)
        {
            if (!IsStartupScriptRegistered(page, key))
            {
                page.RegisterStartupScript(key, script);
            }
        }

        public static bool IsStartupScriptRegistered(Page page, string key)
        {
            return page.IsStartupScriptRegistered(key);
        }

        public static string GetShowImageScript(string imageClientID, string publishmentSystemUrl)
        {
            return GetShowImageScript("this", imageClientID, publishmentSystemUrl);
        }

        public static string GetShowImageScript(string objString, string imageClientID, string publishmentSystemUrl)
        {
            return string.Format("showImage({0}, '{1}', '{2}', '{3}')", objString, imageClientID, ConfigUtils.Instance.ApplicationPath, publishmentSystemUrl);
        }
        #region json对象解析
        public static List<Analytics> ParseJsonStringToName(string json)
        {
            List<Analytics> analytics = new List<Analytics>();
            if (json.IndexOf("count") != -1)
            {
                json = json.Substring(json.IndexOf("count"));
                json = json.TrimEnd('}');
                json = json.TrimEnd(']');
                json = json.TrimEnd('}');
                json = json.Replace("}", "");
                json = json.Replace("{", "");
                json = json.Replace("\"", "");
                string[] arr = json.Split(',');
                Analytics[] A = new Analytics[arr.Length / 2];
                for (int i = 0; i < arr.Length / 2; i++)
                {
                    A[i] = new Analytics();
                }
                for (int i = 0; i < arr.Length; i++)
                {
                    string[] arr1 = arr[i].Split(':');
                    int j = i / 2;

                    if (arr1[0] == "count")
                    {
                        A[j].Count = Convert.ToInt32((arr1[1]));

                    }
                    if (arr1[0] == "metric")
                    {
                        A[j].Metric = arr1[1];
                    }
                    if (i % 2 == 1)
                    {
                        analytics.Add(A[i / 2]);
                    }
                }
            }
            return analytics;
        }
        #endregion
    }
}
