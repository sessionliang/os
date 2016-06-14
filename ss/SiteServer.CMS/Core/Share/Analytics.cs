using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Core
{
   public class Analytics
    {
        private int _count;
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        private string _metric;
        public string Metric
        {
            get
            {
                return _metric;
            }
            set
            {
                _metric = value;
            }
        }
    }
}
