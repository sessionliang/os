using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

using SiteServer.CMS.BackgroundPages;

namespace SiteServer.STL.BackgroundPages
{
	public class ConsolePublishmentSystemEdit : BackgroundBasePage
	{
		public TextBox PublishmentSystemName;
        public DropDownList ParentPublishmentSystemID;
		public DropDownList AuxiliaryTableForContent;
        public PlaceHolder phB2CTables;
        public DropDownList AuxiliaryTableForGoods;
        public DropDownList AuxiliaryTableForBrand;
        public PlaceHolder phWCMTables; 
        public DropDownList AuxiliaryTableForGovPublic;
        public DropDownList AuxiliaryTableForGovInteract;
        public DropDownList AuxiliaryTableForVote;
        public DropDownList AuxiliaryTableForJob;
        public TextBox Taxis;
		public RadioButtonList IsCheckContentUseLevel;
		public DropDownList CheckContentLevel;
		public TextBox PublishmentSystemDir;
		public Control PublishmentSystemDirRow;

        public HtmlTableRow ParentPublishmentSystemIDRow;
		public HtmlTableRow CheckContentLevelRow;

        public Button Submit;

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");

			if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Site, "修改应用", AppManager.Platform.Permission.Platform_Site);

                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    this.ParentPublishmentSystemIDRow.Visible = false;
                }
                else
                {
                    this.ParentPublishmentSystemIDRow.Visible = true;

                    this.ParentPublishmentSystemID.Items.Add(new ListItem("<无上级应用>", "0"));
                    ArrayList publishmentSystemIDArrayList = PublishmentSystemManager.GetPublishmentSystemIDWithoutUserCenterArrayList();
                    ArrayList mySystemInfoArrayList = new ArrayList();
                    Hashtable parentWithChildren = new Hashtable();
                    foreach (int publishmentSystemID in publishmentSystemIDArrayList)
                    {
                        if (publishmentSystemID == base.PublishmentSystemID) continue;
                        PublishmentSystemInfo publishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(publishmentSystemID);
                        if (publishmentSystemInfo.IsHeadquarters == false)
                        {
                            if (publishmentSystemInfo.ParentPublishmentSystemID == 0)
                            {
                                mySystemInfoArrayList.Add(publishmentSystemInfo);
                            }
                            else
                            {
                                ArrayList children = new ArrayList();
                                if (parentWithChildren.Contains(publishmentSystemInfo.ParentPublishmentSystemID))
                                {
                                    children = (ArrayList)parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID];
                                }
                                children.Add(publishmentSystemInfo);
                                parentWithChildren[publishmentSystemInfo.ParentPublishmentSystemID] = children;
                            }
                        }
                    }
                    foreach (PublishmentSystemInfo publishmentSystemInfo in mySystemInfoArrayList)
                    {
                        AddSite(this.ParentPublishmentSystemID, publishmentSystemInfo, parentWithChildren, 0);
                    }
                    ControlUtils.SelectListItems(this.ParentPublishmentSystemID, base.PublishmentSystemInfo.ParentPublishmentSystemID.ToString());
                }

                ArrayList tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BackgroundContent);
				foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
				{
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
					AuxiliaryTableForContent.Items.Add(li);
				}

                if (EPublishmentSystemTypeUtils.IsB2C(base.PublishmentSystemInfo.PublishmentSystemType))
                {
                    this.phB2CTables.Visible = true;

                    tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GoodsContent);
                    foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                    {
                        ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                        AuxiliaryTableForGoods.Items.Add(li);
                    }

                    tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.BrandContent);
                    foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                    {
                        ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                        AuxiliaryTableForBrand.Items.Add(li);
                    }
                }
                else if (base.PublishmentSystemInfo.PublishmentSystemType == EPublishmentSystemType.WCM)
                {
                    this.phWCMTables.Visible = true;

                    tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GovPublicContent);
                    foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                    {
                        ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                        AuxiliaryTableForGovPublic.Items.Add(li);
                    }

                    tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.GovInteractContent);
                    foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                    {
                        ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                        AuxiliaryTableForGovInteract.Items.Add(li);
                    }
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.VoteContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    AuxiliaryTableForVote.Items.Add(li);
                }

                tableArrayList = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableArrayListCreatedInDBByAuxiliaryTableType(EAuxiliaryTableType.JobContent);
                foreach (AuxiliaryTableInfo tableInfo in tableArrayList)
                {
                    ListItem li = new ListItem(string.Format("{0}({1})", tableInfo.TableCNName, tableInfo.TableENName), tableInfo.TableENName);
                    AuxiliaryTableForJob.Items.Add(li);
                }

                this.Taxis.Text = base.PublishmentSystemInfo.Taxis.ToString();

				IsCheckContentUseLevel.Items.Add(new ListItem("默认审核机制",false.ToString()));
                IsCheckContentUseLevel.Items.Add(new ListItem("多级审核机制", true.ToString()));

                if (base.PublishmentSystemInfo == null)
                {
                    PageUtils.RedirectToErrorPage("应用不存在，请确认后再试！");
                    return;
                }
                PublishmentSystemName.Text = base.PublishmentSystemInfo.PublishmentSystemName;
                ControlUtils.SelectListItems(this.IsCheckContentUseLevel, base.PublishmentSystemInfo.IsCheckContentUseLevel.ToString());
                if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                {
                    ControlUtils.SelectListItems(this.CheckContentLevel, base.PublishmentSystemInfo.CheckContentLevel.ToString());
                    this.CheckContentLevelRow.Visible = true;
                }
                else
                {
                    this.CheckContentLevelRow.Visible = false;
                }
                if (!string.IsNullOrEmpty(base.PublishmentSystemInfo.PublishmentSystemDir))
                {
                    this.PublishmentSystemDir.Text = PathUtils.GetDirectoryName(base.PublishmentSystemInfo.PublishmentSystemDir);
                }
                if (base.PublishmentSystemInfo.IsHeadquarters)
                {
                    this.PublishmentSystemDirRow.Visible = false;
                }
                foreach (ListItem item in AuxiliaryTableForContent.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForContent))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForGoods.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForGoods))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForBrand.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForBrand))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForGovPublic.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForGovPublic))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForGovInteract.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForGovInteract))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForVote.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForVote))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in AuxiliaryTableForJob.Items)
                {
                    if (item.Value.Equals(base.PublishmentSystemInfo.AuxiliaryTableForJob))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }

                this.Submit.Attributes.Add("onclick", JsManager.GetShowHintScript());
			}
		}

        private static void AddSite(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, Hashtable parentWithChildren, int level)
        {
            string padding = string.Empty;
            for (int i = 0; i < level; i++)
            {
                padding += "　";
            }
            if (level > 0)
            {
                padding += "└ ";
            }

            if (parentWithChildren[publishmentSystemInfo.PublishmentSystemID] != null)
            {
                ArrayList children = (ArrayList)parentWithChildren[publishmentSystemInfo.PublishmentSystemID];
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName + string.Format("({0})", children.Count), publishmentSystemInfo.PublishmentSystemID.ToString()));
                level++;
                foreach (PublishmentSystemInfo subSiteInfo in children)
                {
                    AddSite(listControl, subSiteInfo, parentWithChildren, level);
                }
            }
            else
            {
                listControl.Items.Add(new ListItem(padding + publishmentSystemInfo.PublishmentSystemName, publishmentSystemInfo.PublishmentSystemID.ToString()));
            }
        }

		public void IsCheckContentUseLevel_OnSelectedIndexChanged(object sender, EventArgs E)
		{
			if (EBooleanUtils.Equals(this.IsCheckContentUseLevel.SelectedValue, EBoolean.True))
			{
				this.CheckContentLevelRow.Visible = true;
			}
			else
			{
				this.CheckContentLevelRow.Visible = false;
			}
		}

		public override void Submit_OnClick(object sender, EventArgs E)
		{
			if (base.Page.IsPostBack && base.Page.IsValid)
			{
                base.PublishmentSystemInfo.PublishmentSystemName = this.PublishmentSystemName.Text;
                base.PublishmentSystemInfo.Taxis = TranslateUtils.ToInt(this.Taxis.Text);
                base.PublishmentSystemInfo.IsCheckContentUseLevel = TranslateUtils.ToBool(this.IsCheckContentUseLevel.SelectedValue);
                if (base.PublishmentSystemInfo.IsCheckContentUseLevel)
                {
                    base.PublishmentSystemInfo.CheckContentLevel = TranslateUtils.ToInt(this.CheckContentLevel.SelectedValue);
                }

                bool isTableChanged = false;

                if (base.PublishmentSystemInfo.AuxiliaryTableForContent != this.AuxiliaryTableForContent.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForContent = this.AuxiliaryTableForContent.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForGoods != this.AuxiliaryTableForGoods.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForGoods = this.AuxiliaryTableForGoods.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForBrand != this.AuxiliaryTableForBrand.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForBrand = this.AuxiliaryTableForBrand.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForGovPublic != this.AuxiliaryTableForGovPublic.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForGovPublic = this.AuxiliaryTableForGovPublic.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForGovInteract != this.AuxiliaryTableForGovInteract.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForGovInteract = this.AuxiliaryTableForGovInteract.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForVote != this.AuxiliaryTableForVote.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForVote = this.AuxiliaryTableForVote.SelectedValue;
                }
                if (base.PublishmentSystemInfo.AuxiliaryTableForJob != this.AuxiliaryTableForJob.SelectedValue)
                {
                    isTableChanged = true;
                    base.PublishmentSystemInfo.AuxiliaryTableForJob = this.AuxiliaryTableForJob.SelectedValue;
                }

                if (base.PublishmentSystemInfo.IsHeadquarters == false)
                {
                    if (!StringUtils.EqualsIgnoreCase(PathUtils.GetDirectoryName(base.PublishmentSystemInfo.PublishmentSystemDir), PublishmentSystemDir.Text))
                    {
                        ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(base.PublishmentSystemInfo.ParentPublishmentSystemID);
                        if (arraylist.IndexOf(PublishmentSystemDir.Text.ToLower()) != -1)
                        {
                            base.FailMessage("应用修改失败，已存在相同的发布路径！");
                            return;
                        }

                        try
                        {
                            string parentPSPath = ConfigUtils.Instance.PhysicalApplicationPath;
                            if (base.PublishmentSystemInfo.ParentPublishmentSystemID > 0)
                            {
                                PublishmentSystemInfo parentPublishmentSystemInfo = PublishmentSystemManager.GetPublishmentSystemInfo(base.PublishmentSystemInfo.ParentPublishmentSystemID);
                                parentPSPath = PathUtility.GetPublishmentSystemPath(parentPublishmentSystemInfo);
                            }
                            DirectoryUtility.ChangePublishmentSystemDir(parentPSPath, base.PublishmentSystemInfo.PublishmentSystemDir, PublishmentSystemDir.Text);
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "应用修改失败，发布路径文件夹已存在！");
                            return;
                        }
                    }

                    if (this.ParentPublishmentSystemIDRow.Visible && base.PublishmentSystemInfo.ParentPublishmentSystemID != TranslateUtils.ToInt(this.ParentPublishmentSystemID.SelectedValue))
                    {
                        int newParentPublishmentSystemID = TranslateUtils.ToInt(this.ParentPublishmentSystemID.SelectedValue);
                        ArrayList arraylist = DataProvider.NodeDAO.GetLowerSystemDirArrayList(newParentPublishmentSystemID);
                        if (arraylist.IndexOf(PublishmentSystemDir.Text.ToLower()) != -1)
                        {
                            base.FailMessage("应用修改失败，已存在相同的发布路径！");
                            return;
                        }

                        try
                        {
                            DirectoryUtility.ChangeParentPublishmentSystem(base.PublishmentSystemInfo.ParentPublishmentSystemID, TranslateUtils.ToInt(this.ParentPublishmentSystemID.SelectedValue), base.PublishmentSystemID, this.PublishmentSystemDir.Text);
                            base.PublishmentSystemInfo.ParentPublishmentSystemID = newParentPublishmentSystemID;
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "应用修改失败，发布路径文件夹已存在！");   
                            return;
                        }
                    }

                    base.PublishmentSystemInfo.PublishmentSystemDir = this.PublishmentSystemDir.Text;
                }

                try
                {
                    DataProvider.PublishmentSystemDAO.Update(base.PublishmentSystemInfo);
                    if (isTableChanged)
                    {
                        DataProvider.NodeDAO.UpdateContentNum(base.PublishmentSystemInfo);
                    }

                    PublishmentSystemManager.UpdateUrlRewriteFile();

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改应用属性", string.Format("应用:{0}", base.PublishmentSystemInfo.PublishmentSystemName));

                    base.SuccessMessage("应用修改成功！");
                    base.AddWaitAndRedirectScript(PageUtils.GetSTLUrl("console_publishmentSystem.aspx"));
                }
                catch (Exception ex)
                {
                    base.FailMessage(ex, "应用修改失败！");
                }
			}
		}

	}
}
