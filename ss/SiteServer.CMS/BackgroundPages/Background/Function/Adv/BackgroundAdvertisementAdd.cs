using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Controls;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using BaiRong.Controls;




namespace SiteServer.CMS.BackgroundPages
{
	public class BackgroundAdvertisementAdd : BackgroundBasePage
	{
        public Literal ltlPageTitle;
		public PlaceHolder AdvertisementBase;
		public TextBox AdvertisementName;
		public RadioButtonList AdvertisementType;
		public CheckBox IsDateLimited;
		public HtmlTableRow StartDateRow;
		public HtmlTableRow EndDateRow;
        public DateTimeTextBox StartDate;
        public DateTimeTextBox EndDate;
        public ListBox NodeIDCollectionToChannel;
        public ListBox NodeIDCollectionToContent;
        public HtmlTableRow FileTemplateIDCollectionRow;
        public CheckBoxList FileTemplateIDCollection;

        public PlaceHolder AdvertisementFloatImage;
        public TextBox NavigationUrl;
		public TextBox ImageUrl;
		public TextBox Height;
		public TextBox Width;
		public Button SelectImage;
        public Button UploadImage;
		public CheckBox IsCloseable;
		public DropDownList PositionType;
		public TextBox PositionX;
        public TextBox PositionY;
		public RadioButtonList RollingType;

        public PlaceHolder AdvertisementScreenDown;
        public TextBox ScreenDownNavigationUrl;
        public TextBox ScreenDownImageUrl;
        public TextBox ScreenDownDelay;
        public TextBox ScreenDownHeight;
        public TextBox ScreenDownWidth;
        public Button ScreenDownSelectImage;
        public Button ScreenDownUploadImage;

        public PlaceHolder AdvertisementOpenWindow;
        public TextBox OpenWindowFileUrl;
        public TextBox OpenWindowWidth;
        public TextBox OpenWindowHeight;

        public PlaceHolder Done;

        public PlaceHolder OperatingError;
		public Label ErrorLabel;

		public Button Previous;
		public Button Next;

		private bool isEdit = false;
		private string editAdvertisementName = string.Empty;
		private EAdvertisementType editAdvertisementType = EAdvertisementType.FloatImage;
        private bool[] isLastNodeArray;
        private EPositionType ePositionType = EPositionType.LeftTop;

		public string PublishmentSystemUrl
		{
			get
			{
				return base.PublishmentSystemInfo.PublishmentSystemUrl;
			}
		}

		public string RootUrl
		{
			get
			{
                return ConfigUtils.Instance.ApplicationPath;
			}
		}

        public string GetPreviewImageSrc(string adType)
        {
            EAdvertisementType type = EAdvertisementTypeUtils.GetEnumType(adType);
            string imageUrl = this.ImageUrl.Text;
            if (type == EAdvertisementType.ScreenDown)
            {
                imageUrl = this.ScreenDownImageUrl.Text;
            }
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string extension = PathUtils.GetExtension(imageUrl);
                if (EFileSystemTypeUtils.IsImage(extension))
                {
                    return PageUtility.ParseNavigationUrl(base.PublishmentSystemInfo, imageUrl);
                }
                else if (EFileSystemTypeUtils.IsFlash(extension))
                {
                    return PageUtils.GetIconUrl("flash.jpg");
                }
                else if (EFileSystemTypeUtils.IsPlayer(extension))
                {
                    return PageUtils.GetIconUrl("player.gif");
                }
            }
            return PageUtils.GetIconUrl("empty.gif");
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID");
			if (base.GetQueryString("AdvertisementName") != null)
			{
				this.isEdit = true;
				this.editAdvertisementName = base.GetQueryString("AdvertisementName");
                if (DataProvider.AdvertisementDAO.IsExists(this.editAdvertisementName, base.PublishmentSystemID))
				{
                    this.editAdvertisementType = DataProvider.AdvertisementDAO.GetAdvertisementType(this.editAdvertisementName, base.PublishmentSystemID);
				}
				else
				{
					this.ErrorLabel.Text = string.Format("不存在名称为“{0}”的广告！", this.editAdvertisementName);
					SetActivePanel(WizardPanel.OperatingError, OperatingError);
					return;
				}
			}

			if (!Page.IsPostBack)
            {
                string pageTitle = this.isEdit ? "编辑漂浮广告" : "添加漂浮广告";
                base.BreadCrumb(AppManager.CMS.LeftMenu.ID_Function, AppManager.CMS.LeftMenu.Function.ID_Advertisement, pageTitle, AppManager.CMS.Permission.WebSite.Advertisement);

                this.ltlPageTitle.Text = pageTitle;

                this.StartDate.Text = DateUtils.GetDateAndTimeString(DateTime.Now);
                this.EndDate.Text = DateUtils.GetDateAndTimeString(DateTime.Now.AddMonths(1));

				EAdvertisementTypeUtils.AddListItems(AdvertisementType);
				ControlUtils.SelectListItems(AdvertisementType, EAdvertisementTypeUtils.GetValue(EAdvertisementType.FloatImage));

                ArrayList nodeIDArrayList = DataProvider.NodeDAO.GetNodeIDArrayListByPublishmentSystemID(base.PublishmentSystemID);
                int nodeCount = nodeIDArrayList.Count;
                this.isLastNodeArray = new bool[nodeCount];
                foreach (int theNodeID in nodeIDArrayList)
                {
                    NodeInfo nodeInfo = NodeManager.GetNodeInfo(base.PublishmentSystemID, theNodeID);

                    string title = WebUtils.GetChannelListBoxTitle(base.PublishmentSystemID, nodeInfo.NodeID, nodeInfo.NodeName, nodeInfo.NodeType, nodeInfo.ParentsCount, nodeInfo.IsLastNode, this.isLastNodeArray);
                    ListItem listitem = new ListItem(title, nodeInfo.NodeID.ToString());
                    this.NodeIDCollectionToChannel.Items.Add(listitem);
                    title = title + string.Format("({0})", nodeInfo.ContentNum);
                    ListItem listitem2 = new ListItem(title, nodeInfo.NodeID.ToString());
                    this.NodeIDCollectionToContent.Items.Add(listitem2);
                }

                ArrayList fileTemplateInfoArrayList = DataProvider.TemplateDAO.GetTemplateInfoArrayListByType(base.PublishmentSystemID, ETemplateType.FileTemplate);
                if (fileTemplateInfoArrayList.Count > 0)
                {
                    foreach (TemplateInfo fileTemplateInfo in fileTemplateInfoArrayList)
                    {
                        ListItem listitem = new ListItem(fileTemplateInfo.CreatedFileFullName, fileTemplateInfo.TemplateID.ToString());
                        this.FileTemplateIDCollection.Items.Add(listitem);
                    }
                }
                else
                {
                    this.FileTemplateIDCollectionRow.Visible = false;
                }

                EPositionTypeUtils.AddListItems(this.PositionType, ERollingType.Static);

				ERollingTypeUtils.AddListItems(RollingType);
				ControlUtils.SelectListItems(RollingType, ERollingTypeUtils.GetValue(ERollingType.FollowingScreen));

                string showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.ImageUrl.ClientID);
				this.SelectImage.Attributes.Add("onclick", showPopWinString);

                showPopWinString = Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.ImageUrl.ClientID);
                this.UploadImage.Attributes.Add("onclick", showPopWinString);

                showPopWinString = Modal.SelectImage.GetOpenWindowString(base.PublishmentSystemInfo, this.ScreenDownImageUrl.ClientID);
                this.ScreenDownSelectImage.Attributes.Add("onclick", showPopWinString);

                showPopWinString = Modal.UploadImageSingle.GetOpenWindowStringToTextBox(base.PublishmentSystemID, this.ScreenDownImageUrl.ClientID);
                this.ScreenDownUploadImage.Attributes.Add("onclick", showPopWinString);

				SetActivePanel(WizardPanel.AdvertisementBase, AdvertisementBase);

                if (this.isEdit)
                {
                    AdvertisementInfo adInfo = DataProvider.AdvertisementDAO.GetAdvertisementInfo(this.editAdvertisementName, base.PublishmentSystemID);
                    this.AdvertisementName.Text = adInfo.AdvertisementName;
                    this.AdvertisementName.Enabled = false;
                    this.AdvertisementType.SelectedValue = EAdvertisementTypeUtils.GetValue(this.editAdvertisementType);

                    this.IsDateLimited.Checked = adInfo.IsDateLimited;
                    this.StartDate.Text = DateUtils.GetDateAndTimeString(adInfo.StartDate);
                    this.EndDate.Text = DateUtils.GetDateAndTimeString(adInfo.EndDate);
                    ControlUtils.SelectListItems(this.NodeIDCollectionToChannel, TranslateUtils.StringCollectionToArrayList(adInfo.NodeIDCollectionToChannel));
                    ControlUtils.SelectListItems(this.NodeIDCollectionToContent, TranslateUtils.StringCollectionToArrayList(adInfo.NodeIDCollectionToContent));
                    ControlUtils.SelectListItems(this.FileTemplateIDCollection, TranslateUtils.StringCollectionToArrayList(adInfo.FileTemplateIDCollection));

                    if (adInfo.AdvertisementType == EAdvertisementType.FloatImage)
                    {
                        AdvertisementFloatImageInfo adFloatImageInfo = new AdvertisementFloatImageInfo(adInfo.Settings);
                        this.IsCloseable.Checked = adFloatImageInfo.IsCloseable;
                        this.ePositionType = adFloatImageInfo.PositionType;
                        this.PositionX.Text = adFloatImageInfo.PositionX.ToString();
                        this.PositionY.Text = adFloatImageInfo.PositionY.ToString();
                        this.RollingType.SelectedValue = ERollingTypeUtils.GetValue(adFloatImageInfo.RollingType);

                        this.NavigationUrl.Text = adFloatImageInfo.NavigationUrl;
                        this.ImageUrl.Text = adFloatImageInfo.ImageUrl;
                        this.Height.Text = adFloatImageInfo.Height.ToString();
                        this.Width.Text = adFloatImageInfo.Width.ToString();
                    }
                    else if (adInfo.AdvertisementType == EAdvertisementType.ScreenDown)
                    {
                        AdvertisementScreenDownInfo adScreenDownInfo = new AdvertisementScreenDownInfo(adInfo.Settings);
                        this.ScreenDownNavigationUrl.Text = adScreenDownInfo.NavigationUrl;
                        this.ScreenDownImageUrl.Text = adScreenDownInfo.ImageUrl;
                        this.ScreenDownDelay.Text = adScreenDownInfo.Delay.ToString();
                        this.ScreenDownWidth.Text = adScreenDownInfo.Width.ToString();
                        this.ScreenDownHeight.Text = adScreenDownInfo.Height.ToString();
                    }
                    else if (adInfo.AdvertisementType == EAdvertisementType.OpenWindow)
                    {
                        AdvertisementOpenWindowInfo adOpenWindowInfo = new AdvertisementOpenWindowInfo(adInfo.Settings);
                        this.OpenWindowFileUrl.Text = adOpenWindowInfo.FileUrl;
                        this.OpenWindowWidth.Text = adOpenWindowInfo.Width.ToString();
                        this.OpenWindowHeight.Text = adOpenWindowInfo.Height.ToString();
                    }
                }
                this.ReFresh(null, EventArgs.Empty);
			}

			base.SuccessMessage(string.Empty);
		}

		public void ReFresh(object sender, EventArgs E)
		{
			if (this.IsDateLimited.Checked)
			{
				this.StartDateRow.Visible = true;
				this.EndDateRow.Visible = true;
			}
			else
			{
				this.StartDateRow.Visible = false;
				this.EndDateRow.Visible = false;
			}

            this.PositionType.Items.Clear();
            ERollingType rollingType = ERollingTypeUtils.GetEnumType(this.RollingType.SelectedValue);
            EPositionTypeUtils.AddListItems(this.PositionType, rollingType);
            ControlUtils.SelectListItems(this.PositionType, EPositionTypeUtils.GetValue(this.ePositionType));
		}

		public WizardPanel CurrentWizardPanel
		{
			get
			{
				if (ViewState["WizardPanel"] != null)
					return (WizardPanel)ViewState["WizardPanel"];

				return WizardPanel.AdvertisementBase;
			}
			set
			{
				ViewState["WizardPanel"] = value;
			}
		}


		public enum WizardPanel
		{
			AdvertisementBase,
			AdvertisementFloatImage,
            AdvertisementScreenDown,
            AdvertisementOpenWindow,
			OperatingError,
			Done
		}

		void SetActivePanel(WizardPanel panel, Control controlToShow)
		{
            PlaceHolder currentPanel = FindControl(CurrentWizardPanel.ToString()) as PlaceHolder;
			if (currentPanel != null)
				currentPanel.Visible = false;

			switch (panel)
			{
				case WizardPanel.AdvertisementBase:
                    this.Previous.CssClass = "btn disabled";
					break;
				case WizardPanel.Done:
                    base.AddWaitAndRedirectScript(PageUtils.GetCMSUrl("background_advertisement.aspx?PublishmentSystemID=" + base.PublishmentSystemID));
                    this.Next.CssClass = "btn btn-primary disabled";
                    this.Previous.CssClass = "btn disabled";
					break;
				case WizardPanel.OperatingError:
                    this.Next.CssClass = "btn btn-primary disabled";
                    this.Previous.CssClass = "btn disabled";
					break;
				default:
					this.Next.CssClass = "btn btn-primary";
                    this.Previous.CssClass = "btn";
					break;
			}

			controlToShow.Visible = true;
			CurrentWizardPanel = panel;
		}


		public bool Validate_AdvertisementBase(out string errorMessage)
		{
			if (this.isEdit)
			{
				errorMessage = string.Empty;
				return true;
			}
			else
			{
                if (DataProvider.AdvertisementDAO.IsExists(this.AdvertisementName.Text, base.PublishmentSystemID))
				{
					errorMessage = string.Format("名称为“{0}”的广告已存在，请更改广告名称！", this.AdvertisementName.Text);
					return false;
				}
				else
				{
					errorMessage = string.Empty;
					return true;
				}
			}
		}

		public bool Validate_InsertFloatImageAdvertisement(out string errorMessage)
		{
            AdvertisementInfo adInfo = new AdvertisementInfo(this.AdvertisementName.Text, base.PublishmentSystemID, EAdvertisementTypeUtils.GetEnumType(this.AdvertisementType.SelectedValue), this.IsDateLimited.Checked, TranslateUtils.ToDateTime(this.StartDate.Text), TranslateUtils.ToDateTime(this.EndDate.Text), DateTime.Now, ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToChannel), ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToContent), ControlUtils.GetSelectedListControlValueCollection(this.FileTemplateIDCollection), string.Empty);
            AdvertisementFloatImageInfo adFloatImageInfo = new AdvertisementFloatImageInfo(this.IsCloseable.Checked, EPositionTypeUtils.GetEnumType(this.PositionType.SelectedValue), TranslateUtils.ToInt(this.PositionX.Text), TranslateUtils.ToInt(this.PositionY.Text), ERollingTypeUtils.GetEnumType(this.RollingType.SelectedValue), this.NavigationUrl.Text, this.ImageUrl.Text, TranslateUtils.ToInt(this.Height.Text), TranslateUtils.ToInt(this.Width.Text));
            adInfo.Settings = adFloatImageInfo.ToString();
			try
			{
				if (this.isEdit)
				{
                    DataProvider.AdvertisementDAO.Update(adInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "修改漂浮广告", string.Format("广告名称：{0}", adInfo.AdvertisementName));
				}
				else
				{
                    DataProvider.AdvertisementDAO.Insert(adInfo);

                    StringUtility.AddLog(base.PublishmentSystemID, "新增漂浮广告", string.Format("广告名称：{0}", adInfo.AdvertisementName));
				}
				errorMessage = string.Empty;
				return true;
			}
			catch(Exception ex)
			{
				errorMessage = "操作失败！";
				return false;
			}
		}

        public bool Validate_InsertOpenWindowAdvertisement(out string errorMessage)
        {
            AdvertisementInfo adInfo = new AdvertisementInfo(this.AdvertisementName.Text, base.PublishmentSystemID, EAdvertisementTypeUtils.GetEnumType(this.AdvertisementType.SelectedValue), this.IsDateLimited.Checked, TranslateUtils.ToDateTime(this.StartDate.Text), TranslateUtils.ToDateTime(this.EndDate.Text), DateTime.Now, ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToChannel), ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToContent), ControlUtils.GetSelectedListControlValueCollection(this.FileTemplateIDCollection), string.Empty);
            AdvertisementOpenWindowInfo adOpenWindowInfo = new AdvertisementOpenWindowInfo(this.OpenWindowFileUrl.Text, TranslateUtils.ToInt(this.OpenWindowHeight.Text), TranslateUtils.ToInt(this.OpenWindowWidth.Text));
            adInfo.Settings = adOpenWindowInfo.ToString();
            try
            {
                if (this.isEdit)
                {
                    DataProvider.AdvertisementDAO.Update(adInfo);
                }
                else
                {
                    DataProvider.AdvertisementDAO.Insert(adInfo);
                }
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "操作失败！";
                return false;
            }
        }

        public bool Validate_InsertScreenDownAdvertisement(out string errorMessage)
        {
            AdvertisementInfo adInfo = new AdvertisementInfo(this.AdvertisementName.Text, base.PublishmentSystemID, EAdvertisementTypeUtils.GetEnumType(this.AdvertisementType.SelectedValue), this.IsDateLimited.Checked, TranslateUtils.ToDateTime(this.StartDate.Text), TranslateUtils.ToDateTime(this.EndDate.Text), DateTime.Now, ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToChannel), ControlUtils.GetSelectedListControlValueCollection(this.NodeIDCollectionToContent), ControlUtils.GetSelectedListControlValueCollection(this.FileTemplateIDCollection), string.Empty);
            AdvertisementScreenDownInfo adScreenDownInfo = new AdvertisementScreenDownInfo(this.ScreenDownNavigationUrl.Text, this.ScreenDownImageUrl.Text, TranslateUtils.ToInt(this.ScreenDownDelay.Text), TranslateUtils.ToInt(this.ScreenDownHeight.Text), TranslateUtils.ToInt(this.ScreenDownWidth.Text));
            adInfo.Settings = adScreenDownInfo.ToString();
            try
            {
                if (this.isEdit)
                {
                    DataProvider.AdvertisementDAO.Update(adInfo);
                }
                else
                {
                    DataProvider.AdvertisementDAO.Insert(adInfo);
                }
                errorMessage = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "操作失败！";
                return false;
            }
        }

		public void NextPanel(Object sender, EventArgs e)
		{
			string errorMessage = "";
			switch (CurrentWizardPanel)
			{
				case WizardPanel.AdvertisementBase:

					if (Validate_AdvertisementBase(out errorMessage))
					{
                        EAdvertisementType adType = EAdvertisementTypeUtils.GetEnumType(this.AdvertisementType.SelectedValue);
                        if (adType == EAdvertisementType.FloatImage)
                        {
                            SetActivePanel(WizardPanel.AdvertisementFloatImage, AdvertisementFloatImage);
                        }
                        else if (adType == EAdvertisementType.ScreenDown)
                        {
                            SetActivePanel(WizardPanel.AdvertisementScreenDown, AdvertisementScreenDown);
                        }
                        else if (adType == EAdvertisementType.OpenWindow)
                        {
                            SetActivePanel(WizardPanel.AdvertisementOpenWindow, AdvertisementOpenWindow);
                        }						
					}
					else
					{
                        base.FailMessage(errorMessage);
						SetActivePanel(WizardPanel.AdvertisementBase, AdvertisementBase);
					}

					break;

                case WizardPanel.AdvertisementFloatImage:

                    if (Validate_InsertFloatImageAdvertisement(out errorMessage))
					{
						SetActivePanel(WizardPanel.Done, Done);
					}
					else
					{
						ErrorLabel.Text = errorMessage;
						SetActivePanel(WizardPanel.OperatingError, OperatingError);
					}

					break;

                case WizardPanel.AdvertisementScreenDown:

                    if (Validate_InsertScreenDownAdvertisement(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.Done, Done);
                    }
                    else
                    {
                        ErrorLabel.Text = errorMessage;
                        SetActivePanel(WizardPanel.OperatingError, OperatingError);
                    }

                    break;

                case WizardPanel.AdvertisementOpenWindow:

                    if (Validate_InsertOpenWindowAdvertisement(out errorMessage))
                    {
                        SetActivePanel(WizardPanel.Done, Done);
                    }
                    else
                    {
                        ErrorLabel.Text = errorMessage;
                        SetActivePanel(WizardPanel.OperatingError, OperatingError);
                    }

                    break;

				case WizardPanel.Done:
					break;
			}
		}

		public void PreviousPanel(Object sender, EventArgs e)
		{
			switch (CurrentWizardPanel)
			{
				case WizardPanel.AdvertisementBase:
					break;

				case WizardPanel.AdvertisementFloatImage:
                    SetActivePanel(WizardPanel.AdvertisementBase, AdvertisementBase);
					break;

                case WizardPanel.AdvertisementScreenDown:
                    SetActivePanel(WizardPanel.AdvertisementBase, AdvertisementBase);
                    break;

                case WizardPanel.AdvertisementOpenWindow:
                    SetActivePanel(WizardPanel.AdvertisementBase, AdvertisementBase);
                    break;
			}
		}
	}
}
