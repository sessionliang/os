using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;

namespace SiteServer.CMS.BackgroundPages.Modal
{
	public class GatherSet : BackgroundBasePage
	{		
		public CheckBox GatherUrlIsCollection;
		public Control GatherUrlCollectionRow;
		public TextBox GatherUrlCollection;

		public CheckBox GatherUrlIsSerialize;
		public Control GatherUrlSerializeRow;
		public TextBox GatherUrlSerialize;
		public TextBox SerializeFrom;
		public TextBox SerializeTo;
		public TextBox SerializeInterval;
		public CheckBox SerializeIsOrderByDesc;
		public CheckBox SerializeIsAddZero;
		public TextBox UrlInclude;

		protected DropDownList NodeIDDropDownList;
		protected TextBox GatherNum;

		private string gatherRuleName;

        public static string GetOpenWindowString(int publishmentSystemID, string gatherRuleName)
        {
            NameValueCollection arguments = new NameValueCollection();
            arguments.Add("PublishmentSystemID", publishmentSystemID.ToString());
            arguments.Add("GatherRuleName", gatherRuleName);
            return PageUtility.GetOpenWindowString("信息采集", "modal_gatherSet.aspx", arguments, 550, 550);
        }

		public void Page_Load(object sender, EventArgs E)
        {
            if (base.IsForbidden) return;

            PageUtils.CheckRequestParameter("PublishmentSystemID", "GatherRuleName");

            this.gatherRuleName = base.GetQueryString("GatherRuleName");

			if (!IsPostBack)
			{
                base.InfoMessage("采集名称：" + this.gatherRuleName);

				GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.gatherRuleName, base.PublishmentSystemID);
                GatherUrlIsCollection.Checked = gatherRuleInfo.GatherUrlIsCollection;
				GatherUrlCollection.Text = gatherRuleInfo.GatherUrlCollection;
                GatherUrlIsSerialize.Checked = gatherRuleInfo.GatherUrlIsSerialize;
				GatherUrlSerialize.Text = gatherRuleInfo.GatherUrlSerialize;
				SerializeFrom.Text = gatherRuleInfo.SerializeFrom.ToString();
				SerializeTo.Text = gatherRuleInfo.SerializeTo.ToString();
				SerializeInterval.Text = gatherRuleInfo.SerializeInterval.ToString();
                SerializeIsOrderByDesc.Checked = gatherRuleInfo.SerializeIsOrderByDesc;
                SerializeIsAddZero.Checked = gatherRuleInfo.SerializeIsAddZero;
				UrlInclude.Text = gatherRuleInfo.UrlInclude;

				this.GatherNum.Text = gatherRuleInfo.Additional.GatherNum.ToString();

                NodeManager.AddListItemsForAddContent(this.NodeIDDropDownList.Items, base.PublishmentSystemInfo, true);
                ControlUtils.SelectListItems(this.NodeIDDropDownList, gatherRuleInfo.NodeID.ToString());

				this.GatherUrl_CheckedChanged(null, null);
			}
		}

        public override void Submit_OnClick(object sender, EventArgs e)
        {
			if (GatherUrlIsCollection.Checked)
			{
				if (string.IsNullOrEmpty(this.GatherUrlCollection.Text))
				{
                    base.FailMessage("必须填写起始网页地址！");
					return;
				}
			}
			
			if (GatherUrlIsSerialize.Checked)
			{
				if (string.IsNullOrEmpty(this.GatherUrlSerialize.Text))
				{
					base.FailMessage("必须填写起始网页地址！");
					return;
				}

				if (this.GatherUrlSerialize.Text.IndexOf("*") == -1)
				{
					base.FailMessage("起始网页地址必须带有 * 字符！");
					return;
				}

				if (string.IsNullOrEmpty(this.SerializeFrom.Text) || string.IsNullOrEmpty(this.SerializeTo.Text))
				{
                    base.FailMessage("必须填写变动数字范围！");
					return;
				}
				else
				{
					if (TranslateUtils.ToInt(this.SerializeFrom.Text) < 0 || TranslateUtils.ToInt(this.SerializeTo.Text) < 0)
					{
                        base.FailMessage("变动数字范围必须大于等于0！");
						return;
					}
					if (TranslateUtils.ToInt(this.SerializeTo.Text) <= TranslateUtils.ToInt(this.SerializeFrom.Text))
					{
                        base.FailMessage("变动数字范围结束必须大于开始！");
						return;
					}
				}
				
				if (string.IsNullOrEmpty(this.SerializeInterval.Text))
				{
                    base.FailMessage("必须填写数字变动倍数！");
					return;
				}
				else
				{
					if (TranslateUtils.ToInt(this.SerializeInterval.Text) <= 0)
					{
                        base.FailMessage("数字变动倍数必须大于等于1！");
						return;
					}
				}
			}
			
			if(string.IsNullOrEmpty(this.UrlInclude.Text))
			{
                base.FailMessage("必须填写内容地址包含字符串！");
				return;
			}

            GatherRuleInfo gatherRuleInfo = DataProvider.GatherRuleDAO.GetGatherRuleInfo(this.gatherRuleName, base.PublishmentSystemID);

            gatherRuleInfo.GatherUrlIsCollection = this.GatherUrlIsCollection.Checked;
			gatherRuleInfo.GatherUrlCollection = GatherUrlCollection.Text;
            gatherRuleInfo.GatherUrlIsSerialize = this.GatherUrlIsSerialize.Checked;
			gatherRuleInfo.GatherUrlSerialize = GatherUrlSerialize.Text;
			gatherRuleInfo.SerializeFrom = TranslateUtils.ToInt(SerializeFrom.Text);
			gatherRuleInfo.SerializeTo = TranslateUtils.ToInt(SerializeTo.Text);
			gatherRuleInfo.SerializeInterval = TranslateUtils.ToInt(SerializeInterval.Text);
            gatherRuleInfo.SerializeIsOrderByDesc = this.SerializeIsOrderByDesc.Checked;
            gatherRuleInfo.SerializeIsAddZero = this.SerializeIsAddZero.Checked;
			gatherRuleInfo.UrlInclude = UrlInclude.Text;

			gatherRuleInfo.NodeID = TranslateUtils.ToInt(this.NodeIDDropDownList.SelectedValue);
			gatherRuleInfo.Additional.GatherNum = TranslateUtils.ToInt(this.GatherNum.Text);
			DataProvider.GatherRuleDAO.Update(gatherRuleInfo);

            PageUtils.Redirect(ProgressBar.GetRedirectUrlStringWithGather(base.PublishmentSystemID, this.gatherRuleName));
		}

		public void GatherUrl_CheckedChanged(object sender, EventArgs e)
		{
			this.GatherUrlCollectionRow.Visible = false;
			this.GatherUrlSerializeRow.Visible = false;

			if (!this.GatherUrlIsCollection.Checked && !this.GatherUrlIsSerialize.Checked)
			{
                base.FailMessage("必须填写起始网页地址！");
				this.GatherUrlIsCollection.Checked = true;
			}

			if (this.GatherUrlIsCollection.Checked)
			{
				this.GatherUrlCollectionRow.Visible = true;
			}
			
			if (this.GatherUrlIsSerialize.Checked)
			{
				this.GatherUrlSerializeRow.Visible = true;
			}
		}
	}
}
