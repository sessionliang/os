using SiteServer.B2C.Model;

namespace SiteServer.B2C.Core
{
	public interface IB2CConfigurationDAO
	{
		void Update(B2CConfigurationInfo info);

        B2CConfigurationInfo GetConfigurationInfo(int nodeID);
	}
}
