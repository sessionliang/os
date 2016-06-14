using System;
using System.Collections;

namespace BaiRong.Model
{
    public class VoteContentAttribute
	{
        protected VoteContentAttribute()
		{
		}

        //hidden
        public const string IsImageVote = "IsImageVote";
        public const string IsSummary = "IsSummary";
        public const string Participants = "Participants";
        public const string IsClosed = "IsClosed";
        public const string IsTop = "IsTop";

        //system
        public const string Title = "Title";
        public const string SubTitle = "SubTitle";
        public const string MaxSelectNum = "MaxSelectNum";
        public const string ImageUrl = "ImageUrl";
        public const string Content = "Content";
        public const string Summary = "Summary";
        public const string AddDate = "AddDate";
        public const string EndDate = "EndDate";
        public const string IsVotedView = "IsVotedView";
        public const string HiddenContent = "HiddenContent";

        public const string CheckTaskDate = "CheckTaskDate";                    //审核时间
        public const string UnCheckTaskDate = "UnCheckTaskDate";                //下架时间

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList();
                arraylist.AddRange(HiddenAttributes);
                arraylist.AddRange(SystemAttributes);
                return arraylist;
            }
        }

        private static ArrayList hiddenAttributes;
        public static ArrayList HiddenAttributes
        {
            get
            {
                if (hiddenAttributes == null)
                {
                    hiddenAttributes = new ArrayList(ContentAttribute.HiddenAttributes);
                    hiddenAttributes.Add(IsImageVote.ToLower());
                    hiddenAttributes.Add(IsSummary.ToLower());
                    hiddenAttributes.Add(Participants.ToLower());
                    hiddenAttributes.Add(IsClosed.ToString());
                    hiddenAttributes.Add(IsTop.ToLower());
                }

                return hiddenAttributes;
            }
        }

        private static ArrayList systemAttributes;
        public static ArrayList SystemAttributes
        {
            get
            {
                if (systemAttributes == null)
                {
                    systemAttributes = new ArrayList();
                    systemAttributes.Add(Title.ToLower());
                    systemAttributes.Add(SubTitle.ToLower());
                    systemAttributes.Add(MaxSelectNum.ToLower());
                    systemAttributes.Add(ImageUrl.ToLower());
                    systemAttributes.Add(Content.ToLower());
                    systemAttributes.Add(Summary.ToLower());
                    systemAttributes.Add(AddDate.ToLower());
                    systemAttributes.Add(EndDate.ToLower());
                    systemAttributes.Add(IsVotedView.ToLower());
                    systemAttributes.Add(HiddenContent.ToLower());
                }

                return systemAttributes;
            }
        }

        private static ArrayList excludeAttributes;
        public static ArrayList ExcludeAttributes
        {
            get
            {
                if (excludeAttributes == null)
                {
                    excludeAttributes = new ArrayList(ContentAttribute.ExcludeAttributes);

                    excludeAttributes.Add(SubTitle.ToLower());
                    excludeAttributes.Add(MaxSelectNum.ToLower());
                    excludeAttributes.Add(AddDate.ToLower());
                    excludeAttributes.Add(EndDate.ToLower());
                    excludeAttributes.Add(IsVotedView.ToLower());
                    excludeAttributes.Add(HiddenContent.ToLower());
                }

                return excludeAttributes;
            }
        }
	}
}
