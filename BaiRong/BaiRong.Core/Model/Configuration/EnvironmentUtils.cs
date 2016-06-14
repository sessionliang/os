using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Globalization;
using System.IO;

using BaiRong.Core;
using BaiRong.Core.IO;

namespace BaiRong.Core
{
    public class EnvironmentUtils
    {
        private EnvironmentUtils()
        {
        }

        public static bool IsWebContext
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
