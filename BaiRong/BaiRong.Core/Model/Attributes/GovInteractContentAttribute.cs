using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace BaiRong.Model
{
    public class GovInteractContentAttribute
    {
        protected GovInteractContentAttribute()
        {
        }

        //hidden
        public const string DepartmentName = "DepartmentName";
        public const string QueryCode = "QueryCode";
        public const string State = "State";
        public const string IPAddress = "IPAddress";
        public const string Location = "Location";
        public const string IsRecommend = "IsRecommend";        //是否推荐

        //basic
        public const string RealName = "RealName";
        public const string Organization = "Organization";
        public const string CardType = "CardType";
        public const string CardNo = "CardNo";
        public const string Phone = "Phone";
        public const string PostCode = "PostCode";
        public const string Address = "Address";
        public const string Email = "Email";
        public const string Fax = "Fax";

        public const string TypeID = "TypeID";
        public const string IsPublic = "IsPublic";
        public const string Title = "Title";
        public const string Content = "Content";
        public const string FileUrl = "FileUrl";
        public const string DepartmentID = "DepartmentID";

        //extend
        public const string TranslateFromNodeID = "TranslateFromNodeID";

        //不存在
        public const string Reply = "Reply";
        public const string ReplyDepartment = "ReplyDepartment";
        public const string ReplyUserName = "ReplyUserName";
        public const string ReplyDate = "ReplyDate";
        public const string ReplyFileUrl = "ReplyFileUrl";
        public const string NavigationUrl = "NavigationUrl";
        public const string CountOfPhotos = "CountOfPhotos";			//商品图片数

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
                    hiddenAttributes.Add(DepartmentName.ToLower());
                    hiddenAttributes.Add(QueryCode.ToString());
                    hiddenAttributes.Add(State.ToLower());
                    hiddenAttributes.Add(IPAddress.ToLower());
                    hiddenAttributes.Add(Location.ToLower());
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
                    systemAttributes = new ArrayList(ContentAttribute.SystemAttributes);

                    systemAttributes.Add(RealName.ToLower());
                    systemAttributes.Add(Organization.ToLower());
                    systemAttributes.Add(CardType.ToLower());
                    systemAttributes.Add(CardNo.ToLower());
                    systemAttributes.Add(Phone.ToLower());
                    systemAttributes.Add(PostCode.ToLower());
                    systemAttributes.Add(Address.ToLower());
                    systemAttributes.Add(Email.ToLower());
                    systemAttributes.Add(Fax.ToLower());

                    systemAttributes.Add(TypeID.ToLower());
                    systemAttributes.Add(IsPublic.ToLower());
                    systemAttributes.Add(Title.ToLower());
                    systemAttributes.Add(Content.ToLower());
                    systemAttributes.Add(FileUrl.ToLower());
                    systemAttributes.Add(DepartmentID.ToLower());
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
                    excludeAttributes = new ArrayList();
                }

                return excludeAttributes;
            }
        }
    }
}
