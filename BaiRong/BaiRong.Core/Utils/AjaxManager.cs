using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace BaiRong.Core
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AjaxMethodAttribute : Attribute
    {
        public AjaxMethodAttribute()
        {
            _includeControlValuesWithCallBack = true;
        }

        private string _clientSideName;

        public string ClientSideName
        {
            get { return _clientSideName; }
            set { _clientSideName = value; }
        }

        private bool _includeControlValuesWithCallBack;

        public bool IncludeControlValuesWithCallBack
        {
            get { return _includeControlValuesWithCallBack; }
            set { _includeControlValuesWithCallBack = value; }
        }
    }

    [Flags]
    public enum AjaxDebug
    {
        None = 0,
        RequestText = 1,
        ResponseText = 2,
        Errors = 4,
        All = 7
    }

    public class AjaxManager
    {
        public static void RegisterLoginPage(Page page)
        {
            page.PreRender += new EventHandler(OnLoginPagePreRender);
        }

        private static void OnLoginPagePreRender(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest req = context.Request;
            string returnURL = req.QueryString["ReturnURL"];
            if (returnURL != null && returnURL.Length > 0)
            {
                returnURL = context.Server.UrlDecode(returnURL);
                if (returnURL.EndsWith("?Ajax_CallBack=true") ||
                    returnURL.EndsWith("&Ajax_CallBack=true"))
                {
                    HttpResponse resp = context.Response;
                    WriteResult(resp, null, "LOGIN");
                    resp.End();
                }
            }
        }

        public static void Register(Page page)
        {
            Register(page, page.GetType().FullName, false, AjaxDebug.None);
        }

        public static void Register(Page page, string prefix)
        {
            Register(page, prefix, false, AjaxDebug.None);
        }

        public static void Register(Page page, AjaxDebug debug)
        {
            Register(page, page.GetType().FullName, false, debug);
        }

        public static void Register(Page page, string prefix, AjaxDebug debug)
        {
            Register(page, prefix, false, debug);
        }

        public static void Register(Control control)
        {
            Register(control, control.GetType().FullName, true, AjaxDebug.None);
        }

        public static void Register(Control control, string prefix)
        {
            Register(control, prefix, true, AjaxDebug.None);
        }

        public static void Register(Control control, AjaxDebug debug)
        {
            Register(control, control.GetType().FullName, true, debug);
        }

        public static void Register(Control control, string prefix, bool requireID, AjaxDebug debug)
        {
            //			RegisterCount += 1;

            HttpContext context = HttpContext.Current;


            string url = context.Request.RawUrl;
            //			string currentExecutionFilePath = context.Request.CurrentExecutionFilePath;
            //			string filePath = context.Request.FilePath;
            //			if (object.ReferenceEquals(currentExecutionFilePath, filePath))
            //			{
            //				url = filePath;
            //				int lastSlash = url.LastIndexOf('/');
            //				if (lastSlash != -1)
            //				{
            //					url = url.Substring(lastSlash + 1);
            //				}
            //			}
            //			else
            //			{
            //				Uri from = new Uri("file://foo" + filePath, true);
            //				Uri to = new Uri("file://foo" + currentExecutionFilePath, true);
            //				url = from.MakeRelative(to);
            //			}
            //			string queryString = context.Request.Url.Query;
            if (url.IndexOf("?") > -1)
            //if (queryString != null && queryString.Length != 0)
            {
                //url = url + queryString + "&Ajax_CallBack=true";
                url = url + "&Ajax_CallBack=true";
            }
            else
            {
                url += "?Ajax_CallBack=true";
            }


            Type type = control.GetType();
            StringBuilder controlScript = new StringBuilder();
            controlScript.Append("\n<script language=\"javascript\" type=\"text/javascript\">\n");
            string[] prefixParts = prefix.Split('.', '+');
            controlScript.AppendFormat("var {0} = {{\n", prefixParts[0]);
            for (int i = 1; i < prefixParts.Length; ++i)
            {
                controlScript.AppendFormat("\"{0}\": {{\n", prefixParts[i]);
            }
            int methodCount = 0;
            foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                object[] attributes = methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    AjaxMethodAttribute methodAttribute = attributes[0] as AjaxMethodAttribute;
                    string clientSideName = methodAttribute.ClientSideName != null ? methodAttribute.ClientSideName : methodInfo.Name;
                    ++methodCount;
                    controlScript.AppendFormat("\n\"{0}\": function(", clientSideName);
                    if (requireID)
                    {
                        controlScript.AppendFormat("id, ");
                    }
                    foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
                    {
                        controlScript.Append(paramInfo.Name + ", ");
                    }
                    controlScript.AppendFormat(
                        "clientCallBack) {{\n\treturn Ajax_CallBack('{0}', {1}, '{2}', [",
                        type.FullName,
                        requireID ? "id" : "null",
                        methodInfo.Name);
                    int paramCount = 0;
                    foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
                    {
                        ++paramCount;
                        controlScript.Append(paramInfo.Name);
                        controlScript.Append(",");
                    }
                    // If parameters were written, remove the
                    // trailing comma.
                    if (paramCount > 0)
                    {
                        --controlScript.Length;
                    }
                    controlScript.AppendFormat("], clientCallBack, {0}, {1}, {2}, {3},'{4}');\n}}",
                        (debug & AjaxDebug.RequestText) == AjaxDebug.RequestText ? "true" : "false",
                        (debug & AjaxDebug.ResponseText) == AjaxDebug.ResponseText ? "true" : "false",
                        (debug & AjaxDebug.Errors) == AjaxDebug.Errors ? "true" : "false",
                        (methodAttribute.IncludeControlValuesWithCallBack ? "true" : "false"), url);

                    controlScript.Append(",\n");
                }
            }
            // If no methods were found, you probably forget to add
            // [Ajax.Method] attributes to your methods or they weren't public.
            if (methodCount == 0)
            {
                throw new ApplicationException(string.Format("{0} does not contain any public methods with the Ajax.Method attribute.", type.FullName));
            }
            // Remove the trailing comma and newline.
            controlScript.Length -= 2;
            controlScript.Append("\n\n");
            for (int i = 0; i < prefixParts.Length; ++i)
            {
                controlScript.Append("}");
            }
            controlScript.Append(";\n</script>");
            control.Page.RegisterClientScriptBlock("AjaxManager:" + type.FullName, controlScript.ToString());
            control.PreRender += new EventHandler(OnPreRender);
        }

        //		public static int RegisterCount
        //		{
        //			get
        //			{
        //				return HttpContext.Current.Items.Contains("Ajax_RegisterCount")
        //					? (int)HttpContext.Current.Items["Ajax_RegisterCount"]
        //					: 0;
        //			}
        //
        //			set
        //			{
        //				HttpContext.Current.Items["Ajax_RegisterCount"] = value;
        //			}
        //		}

        //		public static int CallBackAttemptCount
        //		{
        //			get
        //			{
        //				return HttpContext.Current.Items.Contains("Ajax_CallBackAttemptCount")
        //					? (int)HttpContext.Current.Items["Ajax_CallBackAttemptCount"]
        //					: 0;
        //			}
        //
        //			set
        //			{
        //				HttpContext.Current.Items["Ajax_CallBackAttemptCount"] = value;
        //			}
        //		}

        public static string CallBackType
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackType"];
            }
        }

        public static string CallBackID
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackID"];
            }
        }

        public static string CallBackMethod
        {
            get
            {
                return HttpContext.Current.Request.Form["Ajax_CallBackMethod"];
            }
        }

        public static bool IsCallBack
        {
            get
            {
                return CallBackType != null;
            }
        }

        static void OnPreRender(object s, EventArgs e)
        {
            if (IsCallBack)
            {
                // CallBackAttemptCount += 1;
                Control control = s as Control;

                MethodInfo methodInfo = null;
                if (control != null)
                {
                    if (control.GetType().FullName == CallBackType || control is Page)
                    {
                        if (control is Page || control.ClientID == CallBackID)
                        {
                            methodInfo = FindTargetMethod(control);
                        }
                    }
                }
                object val = null;
                string error = null;
                HttpResponse resp = HttpContext.Current.Response;
                if (methodInfo != null)
                {
                    try
                    {
                        object[] parameters = ConvertParameters(methodInfo, HttpContext.Current.Request);
                        val = InvokeMethod(control, methodInfo, parameters);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                    WriteResult(resp, val, error);
                    resp.End();
                }
                /* else if (CallBackAttemptCount == RegisterCount)
                {
                    // If you're getting this error, it might be because you're calling
                    // back with the wrong control ID or you forget to register your page
                    // by calling Ajax.Manager.Register. You need to register your page
                    // during both GET and POST requests so don't hide your calls to
                    // Ajax.Manager.Register inside a "if (!IsPostBack)" block.
                    error = "Call back method not found.";
                    WriteResult(resp, val, error);
                    resp.End();
                } */
            }
        }

        static MethodInfo FindTargetMethod(Control control)
        {
            Type type = control.GetType();
            string methodName = CallBackMethod;
            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo != null)
            {
                object[] methodAttributes = methodInfo.GetCustomAttributes(typeof(AjaxMethodAttribute), true);
                if (methodAttributes.Length > 0)
                {
                    return methodInfo;
                }
            }
            return null;
        }

        static object[] ConvertParameters(
            MethodInfo methodInfo,
            HttpRequest req)
        {
            object[] parameters = new object[methodInfo.GetParameters().Length];
            int i = 0;
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                object param = null;
                string paramValue = req.Form["Ajax_CallBackArgument" + i];
                if (paramValue != null)
                {
                    if (paramInfo.ParameterType.IsArray)
                    {
                        Type type = paramInfo.ParameterType.GetElementType();
                        string[] splits = paramValue.Split(',');
                        Array array = Array.CreateInstance(type, splits.Length);
                        for (int index = 0; index < splits.Length; index++)
                        {
                            array.SetValue(Convert.ChangeType(splits[index], type), index);
                        }
                        param = array;
                    }
                    else
                    {
                        param = Convert.ChangeType(paramValue, paramInfo.ParameterType);
                    }
                }
                parameters[i] = param;
                ++i;
            }
            return parameters;
        }

        static object InvokeMethod(
            Control control,
            MethodInfo methodInfo,
            object[] parameters)
        {
            object val = null;
            try
            {
                val = methodInfo.Invoke(control, parameters);
            }
            catch (TargetInvocationException ex)
            {
                // TargetInvocationExceptions should have the actual
                // exception the method threw in its InnerException
                // property.
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
            return val;
        }

        public static void WriteResult(
            HttpResponse resp,
            object val,
            string error)
        {
            resp.ContentType = "application/x-javascript";
            resp.Cache.SetCacheability(HttpCacheability.NoCache);
            StringBuilder sb = new StringBuilder();
            try
            {
                WriteValueAndError(sb, val, error);
            }
            catch (Exception ex)
            {
                // If an exception was thrown while formatting the
                // result value, we need to discard whatever was
                // written and start over with nothing but the error
                // message.
                sb.Length = 0;
                WriteValueAndError(sb, null, ex.Message);
            }
            resp.Write(sb.ToString());
        }

        static void WriteValueAndError(StringBuilder sb, object val, string error)
        {
            sb.Append("{\"value\":");
            WriteValue(sb, val);
            sb.Append(",\"error\":");
            WriteValue(sb, error);
            sb.Append("}");
        }

        static void WriteValue(StringBuilder sb, object val)
        {
            if (val == null || val == System.DBNull.Value)
            {
                sb.Append("null");
            }
            else if (val is string || val is Guid)
            {
                WriteString(sb, val.ToString());
            }
            else if (val is bool)
            {
                sb.Append(val.ToString().ToLower());
            }
            else if (val is double ||
                val is float ||
                val is long ||
                val is int ||
                val is short ||
                val is byte ||
                val is decimal)
            {
                sb.Append(val);
            }
            else if (val is DateTime)
            {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (val is DataSet)
            {
                WriteDataSet(sb, val as DataSet);
            }
            else if (val is DataTable)
            {
                WriteDataTable(sb, val as DataTable);
            }
            else if (val is DataRow)
            {
                WriteDataRow(sb, val as DataRow);
            }
            else if (val is IEnumerable)
            {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else
            {
                WriteSerializable(sb, val);
            }
        }

        static void WriteString(StringBuilder sb, string s)
        {
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }

        static void WriteDataSet(StringBuilder sb, DataSet ds)
        {
            sb.Append("{\"Tables\":{");
            foreach (DataTable table in ds.Tables)
            {
                sb.AppendFormat("\"{0}\":", table.TableName);
                WriteDataTable(sb, table);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (ds.Tables.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}}");
        }

        static void WriteDataTable(StringBuilder sb, DataTable table)
        {
            sb.Append("{\"Rows\":[");
            foreach (DataRow row in table.Rows)
            {
                WriteDataRow(sb, row);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (table.Rows.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("]}");
        }

        static void WriteDataRow(StringBuilder sb, DataRow row)
        {
            sb.Append("{");
            foreach (DataColumn column in row.Table.Columns)
            {
                sb.AppendFormat("\"{0}\":", column.ColumnName);
                WriteValue(sb, row[column]);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (row.Table.Columns.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        static void WriteEnumerable(StringBuilder sb, IEnumerable e)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in e)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("]");
        }

        static void WriteSerializable(StringBuilder sb, object o)
        {
            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            sb.Append("{");
            bool hasMembers = false;
            foreach (MemberInfo member in members)
            {
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(o);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(o, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append("\"");
                    sb.Append(member.Name);
                    sb.Append("\":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers)
            {
                --sb.Length;
            }
            sb.Append("}");
        }
    }
}
