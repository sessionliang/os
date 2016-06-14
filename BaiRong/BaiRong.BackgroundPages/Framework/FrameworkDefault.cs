using BaiRong.Core;


namespace BaiRong.BackgroundPages
{
    public class FrameworkDefault : BackgroundBasePage
	{
        protected override bool IsAccessable
        {
            get { return true; }
        }

        protected string AdminDirectoryName
        {
            get { return FileConfigManager.Instance.AdminDirectoryName; }
        }
	}
}
