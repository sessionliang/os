using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IConfigDAO
	{
		void Insert(ConfigInfo info);

        void Update(ConfigInfo info);

        bool IsInitialized();

		string GetDatabaseVersion();

        ConfigInfo GetConfigInfo();

        string GetGUID(string key);

        int GetSiteCount();

        //------------------------------

        void Initialize();
	}
}
