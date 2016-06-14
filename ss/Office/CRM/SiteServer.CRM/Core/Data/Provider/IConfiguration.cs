using SiteServer.CRM.Model;
namespace SiteServer.CRM.Core
{
    public interface IConfigurationDAO
    {
        void Update(ConfigurationInfo info);

        ConfigurationInfo GetConfigurationInfo();
    }
}
