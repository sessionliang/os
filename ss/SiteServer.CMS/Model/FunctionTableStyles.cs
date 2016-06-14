using System;
using BaiRong.Model;
using System.Collections.Specialized;
using BaiRong.Core;
using System.Collections;

namespace SiteServer.CMS.Model
{

    public class FunctionTableStylesAttributes
    {

        protected FunctionTableStylesAttributes()
        {
        }

        public const string FTID = "FTID";
        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string NodeID = "NodeID";
        public const string ContentID = "ContentID";
        public const string TableStyleID = "TableStyleID";
        public const string TableStyle = "TableStyle";
        public const string Enabled = "Enabled";
        public const string AddDate = "AddDate";
        public const string UserName = "UserName";

        private static ArrayList allAttributes;
        public static ArrayList AllAttributes
        {
            get
            {
                if (allAttributes == null)
                {
                    allAttributes = new ArrayList(otherAttributes);
                    allAttributes.Add(FTID.ToLower());
                }
                return allAttributes;
            }
        }


        private static ArrayList otherAttributes;
        public static ArrayList OtherAttributes
        {
            get
            {
                if (otherAttributes == null)
                {
                    otherAttributes = new ArrayList();
                    otherAttributes.Add(PublishmentSystemID.ToLower());
                    otherAttributes.Add(NodeID.ToLower());
                    otherAttributes.Add(ContentID.ToLower());
                    otherAttributes.Add(TableStyleID.ToLower());
                    otherAttributes.Add(TableStyle.ToLower());
                    otherAttributes.Add(AddDate.ToLower());
                    otherAttributes.Add(UserName.ToLower());
                    otherAttributes.Add(Enabled.ToLower());
                }
                return otherAttributes;
            }
        }

    }
    public class FunctionTableStyles : ExtendedAttributes
    {
        public const string TableName = "siteserver_FunctionTableStyles";

        public FunctionTableStyles()
        {
            this.FTID = 0;
            this.PublishmentSystemID = 0;
            this.NodeID = 0;
            this.ContentID = 0; 
            this.UserName = string.Empty;
            this.AddDate = DateTime.Now;
            this.TableStyleID = 0;
            this.TableStyle = string.Empty;
            this.Enabled = EBoolean.True.ToString(); 
        }

        public FunctionTableStyles(int id, int publishmentSystemID, int nodeID, int contentID, int tableStyleID, string tableStyle, string enabled, DateTime addDate, string userName)
        {
            this.FTID = id;
            this.PublishmentSystemID = publishmentSystemID;
            this.NodeID = nodeID;
            this.ContentID = contentID;
            this.TableStyleID = tableStyleID;
            this.TableStyle = tableStyle;
            this.UserName = userName;
            this.Enabled = enabled; 
            this.AddDate = addDate;
        }

        public int FTID
        {
            get { return base.GetInt(FunctionTableStylesAttributes.FTID, 0); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.FTID, value.ToString()); }
        }

        public int PublishmentSystemID
        {
            get { return base.GetInt(FunctionTableStylesAttributes.PublishmentSystemID, 0); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.PublishmentSystemID, value.ToString()); }
        }
        public int NodeID
        {
            get { return base.GetInt(FunctionTableStylesAttributes.NodeID, 0); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.NodeID, value.ToString()); }
        }
        public int ContentID
        {
            get { return base.GetInt(FunctionTableStylesAttributes.ContentID, 0); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.ContentID, value.ToString()); }
        }


        public string UserName
        {
            get { return base.GetExtendedAttribute(FunctionTableStylesAttributes.UserName); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.UserName, value); }
        }


        public DateTime AddDate
        {
            get { return base.GetDateTime(FunctionTableStylesAttributes.AddDate, DateTime.Now); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.AddDate, value.ToString()); }
        }

        public int TableStyleID
        {
            get { return base.GetInt(FunctionTableStylesAttributes.TableStyleID, 0); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.TableStyleID, value.ToString()); }
        }

        public string TableStyle
        {
            get { return base.GetString(FunctionTableStylesAttributes.TableStyle, string.Empty); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.TableStyle, value); }
        }
        public string Enabled
        {
            get { return base.GetString(FunctionTableStylesAttributes.Enabled, string.Empty); }
            set { base.SetExtendedAttribute(FunctionTableStylesAttributes.Enabled, value); }
        }

        protected override ArrayList GetDefaultAttributesNames()
        {
            return FunctionTableStylesAttributes.AllAttributes;
        }
    }
}
