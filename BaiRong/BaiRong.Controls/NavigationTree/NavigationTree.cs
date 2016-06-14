using System.Web.UI.WebControls;
using BaiRong.Controls;
using BaiRong.Core;
using System.Web;
using BaiRong.Model;
using System.Web.UI;
using System.Text;

namespace BaiRong.Controls
{
    public class NavigationTree : TabDrivenTemplatedWebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            BuildNavigationTree(builder, base.GetTabs(), 0, true);
            writer.Write(builder);
        }

        private string productID;
        public string ProductID { set { this.productID = value; } }

        private int b2cPublishmentSystemCount;
        public int B2CPublishmentSystemCount { set { this.b2cPublishmentSystemCount = value; } }

        /// <summary>
        /// Creates the markup for the current TabCollection
        /// </summary>
        /// <returns></returns>
        protected void BuildNavigationTree(StringBuilder builder, TabCollection tc, int parentsCount, bool isDisplay)
        {
            if (tc != null && tc.Tabs != null)
            {
                foreach (Tab parent in tc.Tabs)
                {
                    if (ProductManager.IsRemoveTabs)
                    {
                        if (parent.HasHref && ProductManager.RemovedTabs.Contains(parent.Href))
                        {
                            continue;
                        }
                    }

                    //只有安装了b2c产品，才能显示该菜单
                    if (b2cPublishmentSystemCount == 0 && StringUtils.EqualsIgnoreCase("b2c/background_orderListAll.aspx", parent.Href))
                        continue;

                    if (TabManager.IsValid(parent, this.PermissionArrayList))
                    {
                        string linkUrl = base.FormatLink(parent);
                        if (!string.IsNullOrEmpty(linkUrl) && !StringUtils.EqualsIgnoreCase(linkUrl, PageUtils.UNCLICKED_URL))
                        {
                            linkUrl = PageUtils.GetLoadingUrl(linkUrl);
                        }
                        bool hasChildren = (parent.Children != null && parent.Children.Length > 0) ? true : false;
                        bool openWindow = hasChildren ? false : StringUtils.EndsWithIgnoreCase(parent.Href, "Main.aspx");

                        NavigationTreeItem item = NavigationTreeItem.CreateNavigationBarItem(isDisplay, parent.Selected, parentsCount, hasChildren, openWindow, parent.Text, linkUrl, parent.Target, parent.Enabled, parent.IconUrl);

                        builder.Append(item.GetTrHtml());
                        if (parent.Children != null && parent.Children.Length > 0)
                        {
                            TabCollection tc2 = new TabCollection(parent.Children);
                            BuildNavigationTree(builder, tc2, parentsCount + 1, parent.Selected);
                        }
                    }
                }
            }
        }

        private string _selected = null;
        public override string Selected
        {
            get
            {
                if (_selected == null)
                    _selected = Context.Items["ControlPanelSelectedNavItem"] as string;

                return _selected;
            }
            set
            {
                _selected = value;
            }
        }
    }
}
