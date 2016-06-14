using SiteServer.Project.Model;
namespace SiteServer.Project.Core
{
    public interface IConfigurationDAO
    {
        void Update(ConfigurationInfo info);

        ConfigurationInfo GetConfigurationInfo();
    }
}
