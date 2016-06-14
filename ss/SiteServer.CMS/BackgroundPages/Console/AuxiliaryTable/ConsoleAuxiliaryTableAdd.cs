using System;
using System.Collections;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;



namespace SiteServer.CMS.BackgroundPages
{
    public class ConsoleAuxiliaryTableAdd : BackgroundBasePage
    {
        public Literal ltlPageTitle;

        public TextBox TableENName;
        public TextBox TableCNName;
        public TextBox Description;
        public RadioButtonList AuxiliaryTableType;

        protected override bool IsSaasForbidden { get { return true; } }

        public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            string enName = base.GetQueryString("ENName");

            if (!IsPostBack)
            {
                string pageTitle = (!string.IsNullOrEmpty(enName)) ? "编辑辅助表" : "添加辅助表";
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Auxiliary, pageTitle, AppManager.Platform.Permission.Platform_Auxiliary);

                this.ltlPageTitle.Text = pageTitle;

                //cms
                AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.BackgroundContent, true));

                if (ProductManager.IsProductExists(AppManager.B2C.AppID))
                {
                    //b2c
                    AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.GoodsContent, false));
                    AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.BrandContent, false));
                }

                if (ProductManager.IsProductExists(AppManager.WCM.AppID))
                {
                    //wcm
                    AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.GovPublicContent, false));
                    AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.GovInteractContent, false));
                }
                //others
                AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.VoteContent, false));
                AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.JobContent, false));
                AuxiliaryTableType.Items.Add(EAuxiliaryTableTypeUtils.GetListItem(EAuxiliaryTableType.UserDefined, false));

                if (!string.IsNullOrEmpty(enName))
                {
                    AuxiliaryTableInfo info = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(enName);
                    if (info != null)
                    {
                        TableENName.Text = info.TableENName;
                        TableENName.Enabled = false;
                        TableCNName.Text = info.TableCNName;
                        Description.Text = info.Description;

                        ControlUtils.SelectListItems(AuxiliaryTableType, EAuxiliaryTableTypeUtils.GetValue(info.AuxiliaryTableType));

                        AuxiliaryTableType.Enabled = false;
                    }
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs E)
        {
            if (base.Page.IsPostBack && base.Page.IsValid)
            {
                if (base.GetQueryString("ENName") != null)
                {
                    string tableENName = base.GetQueryString("ENName");
                    AuxiliaryTableInfo info = BaiRongDataProvider.TableCollectionDAO.GetAuxiliaryTableInfo(tableENName);
                    info.TableCNName = TableCNName.Text;
                    info.Description = Description.Text;
                    try
                    {
                        BaiRongDataProvider.TableCollectionDAO.Update(info);

                        LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "修改辅助表", string.Format("辅助表:{0}", tableENName));

                        base.SuccessMessage("辅助表修改成功！");
                    }
                    catch (Exception ex)
                    {
                        base.FailMessage(ex, "辅助表修改失败！");
                    }
                }
                else
                {
                    ArrayList TableENNameList = BaiRongDataProvider.TableCollectionDAO.GetTableENNameCollection();
                    if (TableENNameList.IndexOf(TableENName.Text) != -1)
                    {
                        base.FailMessage("辅助表添加失败，辅助表标识已存在！");
                    }
                    else if (BaiRongDataProvider.TableStructureDAO.IsTableExists(TableENName.Text))
                    {
                        base.FailMessage("辅助表添加失败，数据库中已存在此表！");
                    }
                    else
                    {
                        AuxiliaryTableInfo info = new AuxiliaryTableInfo();
                        info.TableENName = TableENName.Text;
                        info.TableCNName = TableCNName.Text;
                        info.Description = Description.Text;
                        info.AuxiliaryTableType = EAuxiliaryTableTypeUtils.GetEnumType(AuxiliaryTableType.SelectedValue);
                        try
                        {
                            BaiRongDataProvider.TableCollectionDAO.Insert(info);

                            LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "添加辅助表", string.Format("辅助表:{0}", TableENName.Text));

                            base.SuccessMessage("辅助表添加成功！");
                        }
                        catch (Exception ex)
                        {
                            base.FailMessage(ex, "辅助表添加失败！");
                        }
                    }
                }

            }
        }

    }
}
