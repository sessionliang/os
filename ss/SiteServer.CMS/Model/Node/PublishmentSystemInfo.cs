using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.Model
{
    public class PublishmentSystemAttribute
    {
        protected PublishmentSystemAttribute()
        {
        }

        public const string PublishmentSystemID = "PublishmentSystemID";
        public const string PublishmentSystemName = "PublishmentSystemName";
        public const string PublishmentSystemType = "PublishmentSystemType";
        public const string AuxiliaryTableForContent = "AuxiliaryTableForContent";
        public const string AuxiliaryTableForGoods = "AuxiliaryTableForGoods";
        public const string AuxiliaryTableForBrand = "AuxiliaryTableForBrand";
        public const string AuxiliaryTableForGovPublic = "AuxiliaryTableForGovPublic";
        public const string AuxiliaryTableForGovInteract = "AuxiliaryTableForGovInteract";
        public const string AuxiliaryTableForJob = "AuxiliaryTableForJob";
        public const string AuxiliaryTableForVote = "AuxiliaryTableForVote";
        public const string IsCheckContentUseLevel = "IsCheckContentUseLevel";
        public const string CheckContentLevel = "CheckContentLevel";
        public const string PublishmentSystemDir = "PublishmentSystemDir";
        public const string PublishmentSystemUrl = "PublishmentSystemUrl";
        public const string IsHeadquarters = "IsHeadquarters";
        public const string ParentPublishmentSystemID = "ParentPublishmentSystemID";
        public const string GroupSN = "GroupSN";
        public const string Taxis = "Taxis";
        public const string SettingsXML = "SettingsXML";
    }

	[Serializable]
	public class PublishmentSystemInfo
	{
		private int publishmentSystemID = 0;
		private string publishmentSystemName = string.Empty;
        private EPublishmentSystemType publishmentSystemType = EPublishmentSystemType.CMS;
		private string auxiliaryTableForContent = string.Empty;
        private string auxiliaryTableForGoods = string.Empty;
        private string auxiliaryTableForBrand = string.Empty;
        private string auxiliaryTableForGovPublic = string.Empty;
        private string auxiliaryTableForGovInteract = string.Empty;
        private string auxiliaryTableForVote = string.Empty;
        private string auxiliaryTableForJob = string.Empty;
		private bool isCheckContentUseLevel = false;
		private int checkContentLevel = 0;
		private string publishmentSystemDir = string.Empty;
		private string publishmentSystemUrl = string.Empty;
        private bool isHeadquarters = false;
        private int parentPublishmentSystemID = 0;
        private string groupSN = string.Empty;
        private int taxis = 0;
        private string settingsXML = string.Empty;

		public PublishmentSystemInfo()
		{
		}

        public PublishmentSystemInfo(int publishmentSystemID, string publishmentSystemName, EPublishmentSystemType publishmentSystemType, string auxiliaryTableForContent, string auxiliaryTableForGoods, string auxiliaryTableForBrand, string auxiliaryTableForGovPublic, string auxiliaryTableForGovInteract, string auxiliaryTableForVote, string auxiliaryTableForJob, bool isCheckContentUseLevel, int checkContentLevel, string publishmentSystemDir, string publishmentSystemUrl, bool isHeadquarters, int parentPublishmentSystemID, string groupSN, int taxis, string settingsXML) 
		{
			this.publishmentSystemID = publishmentSystemID;
			this.publishmentSystemName = publishmentSystemName;
            this.publishmentSystemType = publishmentSystemType;
			this.auxiliaryTableForContent = auxiliaryTableForContent;
            this.auxiliaryTableForGoods = auxiliaryTableForGoods;
            this.auxiliaryTableForBrand = auxiliaryTableForBrand;
            this.auxiliaryTableForGovPublic = auxiliaryTableForGovPublic;
            this.auxiliaryTableForGovInteract = auxiliaryTableForGovInteract;
            this.auxiliaryTableForVote = auxiliaryTableForVote;
            this.auxiliaryTableForJob = auxiliaryTableForJob;
			this.isCheckContentUseLevel = isCheckContentUseLevel;
			this.checkContentLevel = checkContentLevel;
			this.publishmentSystemDir = publishmentSystemDir;
			this.publishmentSystemUrl = publishmentSystemUrl;
			this.isHeadquarters = isHeadquarters;
            this.parentPublishmentSystemID = parentPublishmentSystemID;
            this.groupSN = groupSN;
            this.taxis = taxis;
            this.settingsXML = settingsXML;
		}

		[XmlIgnore]
		public int PublishmentSystemID
		{
			get{ return publishmentSystemID; }
			set{ publishmentSystemID = value; }
		}

		[XmlIgnore]
		public string PublishmentSystemName
		{
			get{ return publishmentSystemName; }
			set{ publishmentSystemName = value; }
		}

        [XmlIgnore]
        public EPublishmentSystemType PublishmentSystemType
        {
            get { return publishmentSystemType; }
            set { publishmentSystemType = value; }
        }

		[XmlIgnore]
		public string AuxiliaryTableForContent
		{
			get{ return auxiliaryTableForContent; }
			set{ auxiliaryTableForContent = value; }
		}

        [XmlIgnore]
        public string AuxiliaryTableForGoods
        {
            get { return auxiliaryTableForGoods; }
            set { auxiliaryTableForGoods = value; }
        }

        [XmlIgnore]
        public string AuxiliaryTableForBrand
        {
            get { return auxiliaryTableForBrand; }
            set { auxiliaryTableForBrand = value; }
        }

        [XmlIgnore]
        public string AuxiliaryTableForGovPublic
        {
            get { return auxiliaryTableForGovPublic; }
            set { auxiliaryTableForGovPublic = value; }
        }

        [XmlIgnore]
        public string AuxiliaryTableForGovInteract
        {
            get { return auxiliaryTableForGovInteract; }
            set { auxiliaryTableForGovInteract = value; }
        }

        [XmlIgnore]
        public string AuxiliaryTableForJob
        {
            get { return auxiliaryTableForJob; }
            set { auxiliaryTableForJob = value; }
        }

        [XmlIgnore]
        public string AuxiliaryTableForVote
        {
            get { return auxiliaryTableForVote; }
            set { auxiliaryTableForVote = value; }
        }

		[XmlIgnore]
        public bool IsCheckContentUseLevel
		{
			get{ return isCheckContentUseLevel; }
			set{ isCheckContentUseLevel = value; }
		}

		[XmlIgnore]
		public int CheckContentLevel
		{
            get
            {
                if (this.isCheckContentUseLevel)
                {
                    return checkContentLevel;
                }
                else
                {
                    return 1;
                }
            }
			set{ checkContentLevel = value; }
		}

		[XmlIgnore]
		public string PublishmentSystemDir
		{
            get{ return publishmentSystemDir; }
			set{ publishmentSystemDir = value; }
		}

		[XmlIgnore]
		public string PublishmentSystemUrl
		{
            get { return publishmentSystemUrl; }
			set{ publishmentSystemUrl = value; }
		}

		[XmlIgnore]
        public bool IsHeadquarters
		{
			get{ return isHeadquarters; }
			set{ isHeadquarters = value; }
		}

        [XmlIgnore]
        public int ParentPublishmentSystemID
        {
            get { return parentPublishmentSystemID; }
            set { parentPublishmentSystemID = value; }
        }

        [XmlIgnore]
        public string GroupSN
        {
            get { return groupSN; }
            set { groupSN = value; }
        }

        [XmlIgnore]
        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public string SettingsXML
        {
            get { return settingsXML; }
            set
            {
                this.additional = null;
                settingsXML = value;
            }
        }

        PublishmentSystemInfoExtend additional;
        public PublishmentSystemInfoExtend Additional
        {
            get
            {
                if (this.additional == null)
                {
                    this.additional = new PublishmentSystemInfoExtend(this.settingsXML);
                }
                return this.additional;
            }
        }
	}
}
