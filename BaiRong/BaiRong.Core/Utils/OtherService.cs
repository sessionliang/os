using BaiRong.Core;

namespace BaiRong.Core.WebService
{
    public class OtherService
    {
        private OtherService() { }

        public const string ServiceName = "OtherService";

        public static string GetLoadingChannelsServiceUrl()
        {
            return PageUtils.GetAjaxServiceUrlByPage(ServiceName, "GetLoadingChannels");
        }
    }
}
