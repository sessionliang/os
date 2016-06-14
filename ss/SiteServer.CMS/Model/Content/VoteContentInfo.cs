using System;
using BaiRong.Model;
using System.Collections;
using BaiRong.Core;

namespace SiteServer.CMS.Model
{
	public class VoteContentInfo : ContentInfo
	{
		public VoteContentInfo() : base()
		{
            this.IsImageVote = false;
            this.IsSummary = false;
            this.Participants = 0;
            this.IsClosed = false;
            this.SubTitle = string.Empty;
            this.MaxSelectNum = 0;
            this.ImageUrl = string.Empty;
            this.Content = string.Empty;
            this.Summary = string.Empty;
            this.EndDate = DateTime.Now;
            this.IsVotedView = false;
            this.HiddenContent = string.Empty;
		}

        public VoteContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        public bool IsImageVote
        {
            get { return base.GetBool(VoteContentAttribute.IsImageVote, false); }
            set { base.SetExtendedAttribute(VoteContentAttribute.IsImageVote, value.ToString()); }
        }

        public bool IsSummary
        {
            get { return base.GetBool(VoteContentAttribute.IsSummary, false); }
            set { base.SetExtendedAttribute(VoteContentAttribute.IsSummary, value.ToString()); }
        }

        public int Participants
		{
            get { return this.GetInt(VoteContentAttribute.Participants, 0); }
            set { base.SetExtendedAttribute(VoteContentAttribute.Participants, value.ToString()); }
		}

        public bool IsClosed
        {
            get { return base.GetBool(VoteContentAttribute.IsClosed, false); }
            set { base.SetExtendedAttribute(VoteContentAttribute.IsClosed, value.ToString()); }
        }

        public string SubTitle
		{
            get { return this.GetExtendedAttribute(VoteContentAttribute.SubTitle); }
            set { base.SetExtendedAttribute(VoteContentAttribute.SubTitle, value); }
		}

        public int MaxSelectNum
        {
            get { return this.GetInt(VoteContentAttribute.MaxSelectNum, 0); }
            set { base.SetExtendedAttribute(VoteContentAttribute.MaxSelectNum, value.ToString()); }
        }

        public string ImageUrl
        {
            get { return this.GetExtendedAttribute(VoteContentAttribute.ImageUrl); }
            set { base.SetExtendedAttribute(VoteContentAttribute.ImageUrl, value); }
        }

        public string Content
        {
            get { return this.GetExtendedAttribute(VoteContentAttribute.Content); }
            set { base.SetExtendedAttribute(VoteContentAttribute.Content, value); }
        }

        public string Summary
        {
            get { return this.GetExtendedAttribute(VoteContentAttribute.Summary); }
            set { base.SetExtendedAttribute(VoteContentAttribute.Summary, value); }
        }

        public DateTime EndDate
        {
            get { return this.GetDateTime(VoteContentAttribute.EndDate, DateTime.Now); }
            set { base.SetExtendedAttribute(VoteContentAttribute.EndDate, value.ToString()); }
        }

        public bool IsVotedView
        {
            get { return base.GetBool(VoteContentAttribute.IsVotedView, false); }
            set { base.SetExtendedAttribute(VoteContentAttribute.IsVotedView, value.ToString()); }
        }

        public string HiddenContent
        {
            get { return this.GetExtendedAttribute(VoteContentAttribute.HiddenContent); }
            set { base.SetExtendedAttribute(VoteContentAttribute.HiddenContent, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return VoteContentAttribute.AllAttributes;
        }
	}
}
