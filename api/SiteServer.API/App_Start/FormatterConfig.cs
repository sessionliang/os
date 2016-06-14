using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using WebApiContrib.Formatting.Jsonp;

//https://github.com/WebApiContrib/WebApiContrib.Formatting.Jsonp
namespace SiteServer.API
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(MediaTypeFormatterCollection formatters)
        {
            var jsonFormatter = formatters.JsonFormatter;
            jsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            // Insert the JSONP formatter in front of the standard JSON formatter.
            var jsonpFormatter = new JsonpMediaTypeFormatter(formatters.JsonFormatter);
            formatters.Insert(0, jsonpFormatter);
            // Add the text/html formatter
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}