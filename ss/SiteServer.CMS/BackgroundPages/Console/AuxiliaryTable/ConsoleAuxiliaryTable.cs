using System;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Data.Provider;
using BaiRong.Model;
using SiteServer.CMS.Core;



namespace SiteServer.CMS.BackgroundPages
{
	public class ConsoleAuxiliaryTable : BackgroundBasePage
	{
		public DataGrid dgContents;

        protected override bool IsSaasForbidden { get { return true; } }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

			if (base.GetQueryString("Delete") != null)
			{
                string ENName = base.GetQueryString("ENName");//辅助表
                string ENName_Archive = ENName + "_Archive";//辅助表归档
			
				try
				{
                    BaiRongDataProvider.TableCollectionDAO.Delete(ENName);//删除辅助表
                    BaiRongDataProvider.TableCollectionDAO.Delete(ENName_Archive);//删除辅助表归档

                    LogUtils.AddLog(BaiRongDataProvider.AdministratorDAO.UserName, "删除辅助表", string.Format("辅助表:{0}", ENName));

					base.SuccessDeleteMessage();
				}
				catch(Exception ex)
				{
                    base.FailDeleteMessage(ex);
				}
			}
			if (!IsPostBack)
            {
                base.BreadCrumbConsole(AppManager.CMS.LeftMenu.ID_Auxiliary, "辅助表管理", AppManager.Platform.Permission.Platform_Auxiliary);

                try
                {
                    this.dgContents.DataSource = BaiRongDataProvider.TableCollectionDAO.GetDataSourceByAuxiliaryTableType();
                    this.dgContents.DataBind();
                }
                catch (Exception ex)
                {
                    PageUtils.RedirectToErrorPage(ex.Message);
                }
			}
		}

        public string GetYesOrNo(string isDefaultStr)
        {
            return StringUtils.GetBoolText(TranslateUtils.ToBool(isDefaultStr));
        }

        public int GetTableUsedNum(string tableENName, string auxiliaryTableType)
        {
            EAuxiliaryTableType tableType = EAuxiliaryTableTypeUtils.GetEnumType(auxiliaryTableType);
            int usedNum = BaiRongDataProvider.TableCollectionDAO.GetTableUsedNum(tableENName, tableType);
            return usedNum;
        }

        public string GetAuxiliatyTableType(string auxiliaryTableType)
        {
            return EAuxiliaryTableTypeUtils.GetText(EAuxiliaryTableTypeUtils.GetEnumType(auxiliaryTableType));
        }

        public string GetIsChangedAfterCreatedInDB(string isCreatedInDB, string isChangedAfterCreatedInDB)
        {
            if (TranslateUtils.ToBool(isCreatedInDB) == false)
            {
                return "----";
            }
            else
            {
                return StringUtils.GetBoolText(TranslateUtils.ToBool(isChangedAfterCreatedInDB));
            }
        }

        public string GetFontColor(string isCreatedInDB, string isChangedAfterCreatedInDB)
        {
            if (EBooleanUtils.Equals(EBoolean.False, isCreatedInDB))
            {
                return string.Empty;
            }
            else
            {
                if (EBooleanUtils.Equals(EBoolean.False, isChangedAfterCreatedInDB))
                {
                    return string.Empty;
                }
                else
                {
                    return "red";
                }
            }
        }
	}
}
