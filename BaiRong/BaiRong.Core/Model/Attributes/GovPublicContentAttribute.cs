using System;
using System.Collections;

namespace BaiRong.Model
{
	public class GovPublicContentAttribute
	{
        protected GovPublicContentAttribute()
		{
		}

        //system
        public const string ImageUrl = "ImageUrl";                //缩略图
        public const string FileUrl = "FileUrl";                //附件
        public const string Content = "Content";                //内容

        //hidden
        public const string Identifier = "Identifier";              //索引号
        public const string Description = "Description";            //内容概述
        public const string PublishDate = "PublishDate";            //发文日期
        public const string EffectDate = "EffectDate";               //生效日期
        public const string IsAbolition = "IsAbolition";            //是否废止
        public const string AbolitionDate = "AbolitionDate";        //废止日期
        public const string DocumentNo = "DocumentNo";                  //文号
        public const string Publisher = "Publisher";          //发布机构
        public const string Keywords = "Keywords";              //主题词
        public const string DepartmentID = "DepartmentID";      //部门ID
        public const string Category1ID = "Category1ID";
        public const string Category2ID = "Category2ID";
        public const string Category3ID = "Category3ID";
        public const string Category4ID = "Category4ID";
        public const string Category5ID = "Category5ID";
        public const string Category6ID = "Category6ID";

        public const string IsRecommend = "IsRecommend";
        public const string IsHot = "IsHot";
        public const string IsColor = "IsColor";

        public static ArrayList AllAttributes
        {
            get
            {
                ArrayList arraylist = new ArrayList(ContentAttribute.AllAttributes);
                arraylist.AddRange(SystemAttributes);
                return arraylist;
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

                    systemAttributes.Add(ImageUrl.ToLower());
                    systemAttributes.Add(FileUrl.ToLower());
                    systemAttributes.Add(Content.ToLower());

                    systemAttributes.Add(Identifier.ToLower());
                    systemAttributes.Add(Description.ToLower());
                    systemAttributes.Add(PublishDate.ToLower());
                    systemAttributes.Add(EffectDate.ToLower());
                    systemAttributes.Add(IsAbolition.ToLower());
                    systemAttributes.Add(AbolitionDate.ToLower());
                    systemAttributes.Add(DocumentNo.ToLower());
                    systemAttributes.Add(Publisher.ToLower());
                    systemAttributes.Add(Keywords.ToLower());
                    systemAttributes.Add(DepartmentID.ToLower());
                    systemAttributes.Add(Category1ID.ToLower());
                    systemAttributes.Add(Category2ID.ToLower());
                    systemAttributes.Add(Category3ID.ToLower());
                    systemAttributes.Add(Category4ID.ToLower());
                    systemAttributes.Add(Category5ID.ToLower());
                    systemAttributes.Add(Category6ID.ToLower());

                    systemAttributes.Add(IsRecommend.ToLower());
                    systemAttributes.Add(IsHot.ToLower());
                    systemAttributes.Add(IsColor.ToLower());
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

                    excludeAttributes.Add(ContentAttribute.Title.ToLower());

                    excludeAttributes.Add(Identifier.ToLower());
                    excludeAttributes.Add(Description.ToLower());
                    excludeAttributes.Add(PublishDate.ToLower());
                    excludeAttributes.Add(EffectDate.ToLower());
                    excludeAttributes.Add(IsAbolition.ToLower());
                    excludeAttributes.Add(AbolitionDate.ToLower());
                    excludeAttributes.Add(DocumentNo.ToLower());
                    excludeAttributes.Add(Publisher.ToLower());
                    excludeAttributes.Add(Keywords.ToLower());
                    excludeAttributes.Add(DepartmentID.ToLower());
                    excludeAttributes.Add(Category1ID.ToLower());
                    excludeAttributes.Add(Category2ID.ToLower());
                    excludeAttributes.Add(Category3ID.ToLower());
                    excludeAttributes.Add(Category4ID.ToLower());
                    excludeAttributes.Add(Category5ID.ToLower());
                    excludeAttributes.Add(Category6ID.ToLower());

                    excludeAttributes.Add(IsRecommend.ToLower());
                    excludeAttributes.Add(IsHot.ToLower());
                    excludeAttributes.Add(IsColor.ToLower());
                }

                return excludeAttributes;
            }
        }

        private static ArrayList checkBoxAttributes;
        public static ArrayList CheckBoxAttributes
        {
            get
            {
                if (checkBoxAttributes == null)
                {
                    checkBoxAttributes = new ArrayList();
                    checkBoxAttributes.Add(ContentAttribute.IsTop.ToLower());

                    checkBoxAttributes.Add(IsRecommend.ToLower());
                    checkBoxAttributes.Add(IsHot.ToLower());
                    checkBoxAttributes.Add(IsColor.ToLower());
                }

                return checkBoxAttributes;
            }
        }
	}
}
