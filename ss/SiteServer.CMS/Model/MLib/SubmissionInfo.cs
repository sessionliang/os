using System;
using System.Collections.Generic;
using System.Text;

namespace SiteServer.CMS.Model.MLib
{
    public class SubmissionInfo
    {
        public int SubmissionID { get; set; }
        public string AddUserName { get; set; }
        public string Title { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsChecked { get; set; }
        public int CheckedLevel { get; set; }
        public DateTime? PassDate { get; set; }
        public int ReferenceTimes { get; set; }
    }
}
