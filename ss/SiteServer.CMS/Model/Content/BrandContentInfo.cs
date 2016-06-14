using System;
using BaiRong.Model;
using System.Collections;

namespace SiteServer.CMS.Model
{
	public class BrandContentInfo : ContentInfo
	{
		public BrandContentInfo() : base()
		{
            
		}

        public BrandContentInfo(object dataItem)
            : base(dataItem)
		{
		}

        protected override ArrayList GetDefaultAttributesNames()
        {
            return BrandContentAttribute.AllAttributes;
        }
	}
}
