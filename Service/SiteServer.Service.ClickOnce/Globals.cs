namespace SiteServer.Service.ClickOnce
{

    //These parameters should be read from some config in real applciation
    //Here they're just hard coded
    public class Globals
    {
        //必须与SiteServer.Service项目发布选项中的值一致
        public static string PublisherName
        {
            get { return "百容千域（SiteServer 系列产品）"; }
        }

        //必须与SiteServer.Service项目发布选项中的值一致
        public static string ProductName
        {
            get { return "SiteServer Service 服务组件"; }
        }

        public static string Host
        {
            get { return "http://localhost/"; }
        }

        public static string HelpLink
        {
            get { return "http://localhost/help"; }
        }

        public static string AboutLink
        {
            get { return "http://localhost/about"; }
        }
    }
}
