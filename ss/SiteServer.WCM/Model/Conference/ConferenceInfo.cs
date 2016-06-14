using BaiRong.Core;
using BaiRong.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;

namespace SiteServer.WeiXin.Model
{
    public class ConferenceAttribute
    {
        protected ConferenceAttribute()
        {
        }

        public const string ID = "ID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string KeywordID = "KeywordID";
        public const string UserCount = "UserCount";
        public const string PVCount = "PVCount";
        public const string Template = "Template";
        public const string StartDate = "StartDate";
        public const string EndDate = "EndDate";
        public const string Title = "Title";
        public const string ImageUrl = "ImageUrl";
        public const string Summary = "Summary";
        public const string BackgroundImageUrl = "BackgroundImageUrl";
        public const string ConferenceName = "ConferenceName";
        public const string Address = "Address";
        public const string Duration = "Duration";
        public const string Introduction = "Introduction";
        public const string AgendaTitle1 = "AgendaTitle1";
        public const string AgendaList1 = "AgendaList1";
        public const string AgendaTitle2 = "AgendaTitle2";
        public const string AgendaList2 = "AgendaList2";
        public const string GuestTitle1 = "GuestTitle1";
        public const string GuestList1 = "GuestList1";
        public const string GuestTitle2 = "GuestTitle2";
        public const string GuestList2 = "GuestList2";
        public const string MapTitle = "MapTitle";
        public const string MapPostion = "MapPostion";
        public const string EndTitle = "EndTitle";
        public const string EndImageUrl = "EndImageUrl";
        public const string EndSummary = "EndSummary";

        private static List<string> allAttributes;
        public static List<string> AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new List<string>();
                    allAttributes.Add(ID);
                    allAttributes.Add(PublishmentSystemID);
                    allAttributes.Add(KeywordID);
                    allAttributes.Add(UserCount);
                    allAttributes.Add(PVCount);
                    allAttributes.Add(Template);
                    allAttributes.Add(StartDate);
                    allAttributes.Add(EndDate);
                    allAttributes.Add(Title);
                    allAttributes.Add(ImageUrl);
                    allAttributes.Add(Summary);
                    allAttributes.Add(BackgroundImageUrl);
                    allAttributes.Add(ConferenceName);
                    allAttributes.Add(Address);
                    allAttributes.Add(Duration);
                    allAttributes.Add(Introduction);
                    allAttributes.Add(AgendaTitle1);
                    allAttributes.Add(AgendaList1);
                    allAttributes.Add(AgendaTitle2);
                    allAttributes.Add(AgendaList2);
                    allAttributes.Add(GuestTitle1);
                    allAttributes.Add(GuestList1);
                    allAttributes.Add(GuestTitle2);
                    allAttributes.Add(GuestList2);
                    allAttributes.Add(MapTitle);
                    allAttributes.Add(MapPostion);
                    allAttributes.Add(EndTitle);
                    allAttributes.Add(EndImageUrl);
                    allAttributes.Add(EndSummary);
                }

                return allAttributes;
            }
        }
    }
    public class ConferenceInfo : BaseInfo
    {
        public ConferenceInfo() { }
        public ConferenceInfo(object dataItem) : base(dataItem) { }
        public ConferenceInfo(NameValueCollection form) : base(form) { }
        public ConferenceInfo(IDataReader rdr) : base(rdr) { }
        public int PublishmentSystemID { get; set; }
        public int KeywordID { get; set; }
        public int UserCount { get; set; }
        public int PVCount { get; set; }
        public string Template { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Summary { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string ConferenceName { get; set; }
        public string Address { get; set; }
        public string Duration { get; set; }
        public string Introduction { get; set; }
        public string AgendaTitle1 { get; set; }
        public string AgendaList1 { get; set; }
        public string AgendaTitle2 { get; set; }
        public string AgendaList2 { get; set; }
        public string GuestTitle1 { get; set; }
        public string GuestList1 { get; set; }
        public string GuestTitle2 { get; set; }
        public string GuestList2 { get; set; }
        public string MapTitle { get; set; }
        public string MapPostion { get; set; }
        public string EndTitle { get; set; }
        public string EndImageUrl { get; set; }
        public string EndSummary { get; set; }

        protected override List<string> AllAttributes
        {
            get
            {
                return ConferenceAttribute.AllAttributes;
            }
        }
    }
}
