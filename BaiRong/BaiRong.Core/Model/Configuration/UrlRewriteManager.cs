using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;

using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.UrlRewriting;
using BaiRong.Model;
using BaiRong.Core.Data.Provider;
using System.Collections.Generic;

namespace BaiRong.Core
{
	public class UrlRewriteManager
	{
        private const string CacheKey = "BaiRong.Core.UrlRewriteManager";

        private RewriterRuleCollection rewriterRules = new RewriterRuleCollection();
        private bool isRewriter = false;

        public bool IsRewriter
        {
            get { return isRewriter; }
        }

		public RewriterRuleCollection RewriterRules
		{
            get { return rewriterRules; }
		}

        public static UrlRewriteManager Instance
        {
            get
            {
                UrlRewriteManager manager = CacheUtils.Get(CacheKey) as UrlRewriteManager;
                if (manager == null)
                {
                    try
                    {
                        manager = new UrlRewriteManager();

                        string path = PathUtils.MapPath("~/SiteFiles/Configuration/UrlRewrite.config");
                        if (FileUtils.IsFileExists(path))
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(path);
                            XmlNode rewriteNode = xmlDoc.SelectSingleNode("RewriterConfig");
                            RewriterConfiguration rewriterConfig = RewriterConfiguration.GetConfig(rewriteNode);

                            foreach (RewriterRule rewriterRule in rewriterConfig.Rules)
                            {
                                if (rewriterRule.SendTo != null)
                                {
                                    rewriterRule.SendTo = rewriterRule.SendTo;
                                }
                                manager.rewriterRules.Add(rewriterRule);
                            }
                            manager.isRewriter = true;
                        }

                        if (manager.isRewriter)
                        {
                            CacheUtils.Max(CacheKey, manager, new CacheDependency(path));
                        }
                        else
                        {
                            CacheUtils.Max(CacheKey, manager);
                        }
                    }
                    catch { }
                }
                return manager;
            }
        }
	}
}
