using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.BackgroundPages.Modal;
using SiteServer.STL.BackgroundPages.Modal.StlTemplate;
using SiteServer.STL.Parser.Model;
using SiteServer.STL.Parser.StlElement;
using SiteServer.CMS.Model;
using SiteServer.CMS.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using SiteServer.CMS.Core;

namespace SiteServer.STL.Parser.TemplateDesign
{
    public class TemplateDesignOperation
    {
        public static NameValueCollection Operate(int publishmentSystemID, int templateID, string includeUrl, string operation, NameValueCollection form)
        {
            NameValueCollection retval = new NameValueCollection();
            if (operation == "Move")
            {
                int stlSequence = TranslateUtils.ToIntWithNagetive(form["stlSequence"]);
                string stlSequenceCollection = form["stlSequenceCollection"];
                int stlElementIndex = TranslateUtils.ToInt(form["stlElementIndex"]);
                int stlDivIndex = TranslateUtils.ToInt(form["stlDivIndex"]);

                retval = StlTemplateMove(publishmentSystemID, templateID, includeUrl, stlSequence, stlSequenceCollection, stlElementIndex, stlDivIndex);
            }
            else if (operation == "Insert")
            {
                string stlSequenceCollection = form["stlSequenceCollection"];
                int stlElementIndex = TranslateUtils.ToInt(form["stlElementIndex"]);
                int stlDivIndex = TranslateUtils.ToInt(form["stlDivIndex"]);
                string stlElementToAdd = form["stlElementToAdd"];
                bool stlIsContainer = TranslateUtils.ToBool(form["stlIsContainer"], true);

                retval = StlTemplateInsert(publishmentSystemID, templateID, includeUrl, stlSequenceCollection, stlElementIndex, stlDivIndex, stlElementToAdd, stlIsContainer);
            }
            else if (operation == "UndoRedo")
            {
                bool isUndo = TranslateUtils.ToBool(form["isUndo"]);
                bool isRedo = TranslateUtils.ToBool(form["isRedo"]);

                retval = StlTemplateUndoRedo(publishmentSystemID, templateID, includeUrl, isUndo, isRedo);
            }
            else if (operation == "Save")
            {
                retval = StlTemplateSave(publishmentSystemID, templateID, includeUrl);
            }
            else if (operation == "GuideLine")
            {
                bool isGuideLine = TranslateUtils.ToBool(form["isGuideLine"]);

                retval = StlTemplateGuideLine(publishmentSystemID, isGuideLine);
            }

            return retval;
        }

        private static NameValueCollection StlTemplateMove(int publishmentSystemID, int templateID, string includeUrl, int stlSequence, string stlSequenceCollection, int stlElementIndex, int stlDivIndex)
        {
            NameValueCollection retval = new NameValueCollection();

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
                TemplateDesignManager.MoveStlElement(publishmentSystemInfo, templateInfo, includeUrl, stlSequence, stlSequenceCollection, stlElementIndex, stlDivIndex);
                retval.Add("success", "true");
            }
            catch (Exception ex)
            {
                retval.Add("success", "false");
                retval.Add("errorMessage", ex.Message);
            }

            return retval;
        }

        private static NameValueCollection StlTemplateInsert(int publishmentSystemID, int templateID, string includeUrl, string stlSequenceCollection, int stlElementIndex, int stlDivIndex, string stlElementToAdd, bool stlIsContainer)
        {
            NameValueCollection retval = new NameValueCollection();

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
                TemplateDesignManager.InsertStlElement(publishmentSystemInfo, templateInfo, includeUrl, stlSequenceCollection, stlElementToAdd, stlDivIndex, stlIsContainer);

                retval.Add("success", "true");
            }
            catch (Exception ex)
            {
                retval.Add("success", "false");
                retval.Add("errorMessage", ex.Message);
            }

            return retval;
        }

        private static NameValueCollection StlTemplateUndoRedo(int publishmentSystemID, int templateID, string includeUrl, bool isUndo, bool isRedo)
        {
            NameValueCollection retval = new NameValueCollection();

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);
                if (isUndo)
                {
                    TemplateDesignUndoRedo undoRedo = TemplateDesignUndoRedo.GetInstance(publishmentSystemInfo, templateInfo, includeUrl);
                    undoRedo.Undo(1);

                    TemplateDesignManager.SetAlertText(publishmentSystemID, templateID, "操作成功，模板改动已撤销。");
                }
                else if (isRedo)
                {
                    TemplateDesignUndoRedo undoRedo = TemplateDesignUndoRedo.GetInstance(publishmentSystemInfo, templateInfo, includeUrl);
                    undoRedo.Redo(1);

                    TemplateDesignManager.SetAlertText(publishmentSystemID, templateID, "操作成功，模板改动已恢复。");
                }
                retval.Add("success", "true");
            }
            catch (Exception ex)
            {
                retval.Add("success", "false");
                retval.Add("errorMessage", ex.Message);
            }

            return retval;
        }

        private static NameValueCollection StlTemplateSave(int publishmentSystemID, int templateID, string includeUrl)
        {
            NameValueCollection retval = new NameValueCollection();

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                TemplateInfo templateInfo = TemplateManager.GetTemplateInfo(publishmentSystemID, templateID);

                TemplateDesignManager.SaveTemplateInfo(publishmentSystemInfo, templateInfo, includeUrl);

                TemplateDesignManager.SetAlertText(publishmentSystemID, templateID, "操作成功，模板改动已保存。");
                retval.Add("success", "true");
            }
            catch (Exception ex)
            {
                retval.Add("success", "false");
                retval.Add("errorMessage", ex.Message);
            }

            return retval;
        }

        private static NameValueCollection StlTemplateGuideLine(int publishmentSystemID, bool isGuideLine)
        {
            NameValueCollection retval = new NameValueCollection();

            try
            {
                PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                publishmentSystemInfo.Additional.IsDesignGuideLine = isGuideLine;
                DataProvider.PublishmentSystemDAO.Update(publishmentSystemInfo);

                retval.Add("success", "true");
            }
            catch (Exception ex)
            {
                retval.Add("success", "false");
                retval.Add("errorMessage", ex.Message);
            }

            return retval;
        }
    }
}
