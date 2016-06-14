using System.Collections.Specialized;
using BaiRong.Core;
using BaiRong.Model;

namespace SiteServer.CMS.Model
{
	public class TaskBackupInfo : ExtendedAttributes
	{
        public TaskBackupInfo(string serviceParameters)
        {
            NameValueCollection nameValueCollection = TranslateUtils.ToNameValueCollection(serviceParameters);
            base.SetExtendedAttribute(nameValueCollection);
        }

        public EBackupType BackupType
		{
            get { return EBackupTypeUtils.GetEnumType(this.GetExtendedAttribute("BackupType")); }
            set { base.SetExtendedAttribute("BackupType", EBackupTypeUtils.GetValue(value)); }
		}

        public string PublishmentSystemIDCollection
        {
            get { return this.GetExtendedAttribute("PublishmentSystemIDCollection"); }
            set { base.SetExtendedAttribute("PublishmentSystemIDCollection", value); }
        }

        public bool IsBackupAll
        {
            get { return base.GetBool("IsBackupAll", false); }
            set { base.SetExtendedAttribute("IsBackupAll", value.ToString()); }
        }
	}
}
