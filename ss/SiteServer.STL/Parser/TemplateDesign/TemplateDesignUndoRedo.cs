using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;

namespace SiteServer.STL.Parser.TemplateDesign
{
    public class TemplateDesignUndoRedo
    {
        private class TemplateChangeObject
        {
            private string templateContent;

            public TemplateChangeObject(string templateContent)
            {
                this.templateContent = templateContent;
            }

            public string TemplateContent
            {
                get
                {
                    return this.templateContent;
                }
            }
        }

        private Stack<TemplateChangeObject> undoActionsCollection = new Stack<TemplateChangeObject>();
        private Stack<TemplateChangeObject> redoActionsCollection = new Stack<TemplateChangeObject>();

        private PublishmentSystemInfo publishmentSystemInfo;
        private TemplateInfo templateInfo;
        private string includeUrl;
        private string _templateContent;
        private bool isDirty = false;
        private string filePath;
        private CacheDependency cacheDependency = null;

        private TemplateDesignUndoRedo(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl)
        {
            this.publishmentSystemInfo = publishmentSystemInfo;
            this.templateInfo = templateInfo;
            this.includeUrl = RuntimeUtils.DecryptStringByTranslate(includeUrl);

            if (!string.IsNullOrEmpty(includeUrl))
            {
                this.filePath = PathUtility.MapPath(publishmentSystemInfo, PathUtility.AddVirtualToPath(includeUrl));
            }
            else
            {
                this.filePath = TemplateManager.GetTemplateFilePath(publishmentSystemInfo, templateInfo);
            }

            this.cacheDependency = new CacheDependency(this.filePath);

            this._templateContent = this.GetTemplateContentByFilePath();
        }

        private string GetTemplateContentByFilePath()
        {
            try
            {
                if (CacheUtils.Get(this.filePath) == null)
                {
                    string content = FileUtils.ReadText(this.filePath, templateInfo.Charset);
                    CacheUtils.Insert(this.filePath, content, this.cacheDependency, CacheUtils.HourFactor * 2);
                    return content;
                }
                return CacheUtils.Get(this.filePath) as string;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string TemplateContent
        {
            get
            {
                if (this.cacheDependency.HasChanged)
                {
                    this._templateContent = this.GetTemplateContentByFilePath();
                }
                return this._templateContent;
            }
            set
            {
                this._templateContent = value;
            }
        }

        public static string GetTemplateContent(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl)
        {
            return TemplateDesignUndoRedo.GetInstance(publishmentSystemInfo, templateInfo, includeUrl).TemplateContent;
        }

        public static string GetKey(int publishmentSystemID, int templateID, string includeUrl)
        {
            return string.Format("{0}.{1}.{2}", publishmentSystemID, templateID, includeUrl);
        }

        private static Dictionary<string, TemplateDesignUndoRedo> undoRedoDictionary = new Dictionary<string, TemplateDesignUndoRedo>();

        public static TemplateDesignUndoRedo GetInstance(PublishmentSystemInfo publishmentSystemInfo, TemplateInfo templateInfo, string includeUrl)
        {
            string key = TemplateDesignUndoRedo.GetKey(publishmentSystemInfo.PublishmentSystemID, templateInfo.TemplateID, includeUrl);
            if (!TemplateDesignUndoRedo.undoRedoDictionary.ContainsKey(key))
            {
                TemplateDesignUndoRedo.undoRedoDictionary[key] = new TemplateDesignUndoRedo(publishmentSystemInfo, templateInfo, includeUrl);
            }
            return TemplateDesignUndoRedo.undoRedoDictionary[key];
        }

        public bool IsUndo
        {
            get
            {
                return undoActionsCollection.Count > 0;
            }
        }

        public bool IsRedo
        {
            get
            {
                return redoActionsCollection.Count > 0;
            }
        }

        public bool IsDirty
        {
            get
            {
                return this.isDirty;
            }
            set
            {
                this.isDirty = value;
            }
        }

        public void Undo(int level)
        {
            for (int i = 1; i <= level; i++)
            {
                if (undoActionsCollection.Count == 0) return;
                TemplateChangeObject undoChangeObject = undoActionsCollection.Pop();

                TemplateChangeObject changeObject = new TemplateChangeObject(this.TemplateContent);
                this.redoActionsCollection.Push(changeObject);

                this.TemplateContent = undoChangeObject.TemplateContent;
            }

            isDirty = true;
        }

        public void Redo(int level)
        {
            for (int i = 1; i <= level; i++)
            {
                if (redoActionsCollection.Count == 0) return;

                TemplateChangeObject redoChangeObject = redoActionsCollection.Pop();

                TemplateChangeObject changeObject = new TemplateChangeObject(this.TemplateContent);
                undoActionsCollection.Push(changeObject);

                this.TemplateContent = redoChangeObject.TemplateContent;
            }
            isDirty = true;
        }

        public void InsertObjectforUndoRedo(string templateContent)
        {            
            TemplateChangeObject changeObject = new TemplateChangeObject(this.TemplateContent);

            undoActionsCollection.Push(changeObject);
            redoActionsCollection.Clear();

            this.TemplateContent = templateContent;

            isDirty = true;
        }
    }
}
