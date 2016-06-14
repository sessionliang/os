using System;
using System.Collections;
using BaiRong.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using SiteServer.CMS.Core;
using BaiRong.Model.Service;
using BaiRong.Core.Service;
using BaiRong.Core.Data.Provider;
using BaiRong.Service;
using BaiRong.Service.Execution;

namespace SiteServer.STL.Execution
{
    public class ExecutionBase
    {
        public virtual void Init()
        {
            EnvironmentManager.InitializeEnvironment();
        }
    }
}
