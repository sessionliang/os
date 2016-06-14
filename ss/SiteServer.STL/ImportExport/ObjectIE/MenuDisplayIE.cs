using System;
using System.Collections;
using Atom.Core;
using BaiRong.Core.IO;
using BaiRong.Model;
using SiteServer.CMS.Model;
using BaiRong.Core;
using SiteServer.CMS.Core;

namespace SiteServer.STL.ImportExport
{
	internal class MenuDisplayIE
	{
		private readonly int publishmentSystemID;
		private readonly string filePath;

		public MenuDisplayIE(int publishmentSystemID, string filePath)
		{
			this.publishmentSystemID = publishmentSystemID;
			this.filePath = filePath;
		}


		public void ExportMenuDisplay()
		{
			AtomFeed feed = AtomUtility.GetEmptyFeed();

			ArrayList menuDisplayArrayList = DataProvider.MenuDisplayDAO.GetMenuDisplayInfoArrayList(this.publishmentSystemID);

			foreach (MenuDisplayInfo menuDisplayInfo in menuDisplayArrayList)
			{
				AtomEntry entry = ExportMenuDisplayInfo(menuDisplayInfo);
				feed.Entries.Add(entry);
			}

			feed.Save(filePath);
		}

		private static AtomEntry ExportMenuDisplayInfo(MenuDisplayInfo menuDisplayInfo)
		{
			AtomEntry entry = AtomUtility.GetEmptyEntry();

			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuDisplayID", menuDisplayInfo.MenuDisplayID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "PublishmentSystemID", menuDisplayInfo.PublishmentSystemID.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuDisplayName", menuDisplayInfo.MenuDisplayName);
			AtomUtility.AddDcElement(entry.AdditionalElements, "Vertical", menuDisplayInfo.Vertical);
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontFamily", menuDisplayInfo.FontFamily);
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontSize", menuDisplayInfo.FontSize.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontWeight", menuDisplayInfo.FontWeight);
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontStyle", menuDisplayInfo.FontStyle);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemHAlign", menuDisplayInfo.MenuItemHAlign);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemVAlign", menuDisplayInfo.MenuItemVAlign);
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontColor", menuDisplayInfo.FontColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemBgColor", menuDisplayInfo.MenuItemBgColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "FontColorHilite", menuDisplayInfo.FontColorHilite);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuHiliteBgColor", menuDisplayInfo.MenuHiliteBgColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "XPosition", menuDisplayInfo.XPosition);
			AtomUtility.AddDcElement(entry.AdditionalElements, "YPosition", menuDisplayInfo.YPosition);
			AtomUtility.AddDcElement(entry.AdditionalElements, "HideOnMouseOut", menuDisplayInfo.HideOnMouseOut);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuWidth", menuDisplayInfo.MenuWidth.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemHeight", menuDisplayInfo.MenuItemHeight.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemPadding", menuDisplayInfo.MenuItemPadding.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemSpacing", menuDisplayInfo.MenuItemSpacing.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuItemIndent", menuDisplayInfo.MenuItemIndent.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "HideTimeout", menuDisplayInfo.HideTimeout.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuBgOpaque", menuDisplayInfo.MenuBgOpaque);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuBorder", menuDisplayInfo.MenuBorder.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "BGColor", menuDisplayInfo.BGColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuBorderBgColor", menuDisplayInfo.MenuBorderBgColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "MenuLiteBgColor", menuDisplayInfo.MenuLiteBgColor);
			AtomUtility.AddDcElement(entry.AdditionalElements, "ChildMenuIcon", menuDisplayInfo.ChildMenuIcon);
			AtomUtility.AddDcElement(entry.AdditionalElements, "AddDate", menuDisplayInfo.AddDate.ToLongDateString());
            AtomUtility.AddDcElement(entry.AdditionalElements, "IsDefault", menuDisplayInfo.IsDefault.ToString());
			AtomUtility.AddDcElement(entry.AdditionalElements, "Description", menuDisplayInfo.Description);

			return entry;
		}


		public void ImportMenuDisplay(bool overwrite)
		{
			if (!FileUtils.IsFileExists(this.filePath)) return;
            AtomFeed feed = AtomFeed.Load(FileUtils.GetFileStreamReadOnly(filePath));

			foreach (AtomEntry entry in feed.Entries)
			{
				string MenuDisplayName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuDisplayName");

				if (!string.IsNullOrEmpty(MenuDisplayName))
				{
					MenuDisplayInfo menuDisplayInfo = new MenuDisplayInfo();
					menuDisplayInfo.PublishmentSystemID = this.publishmentSystemID;
					menuDisplayInfo.MenuDisplayName = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuDisplayName");
					menuDisplayInfo.Vertical = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Vertical");
					menuDisplayInfo.FontFamily = AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontFamily");
					menuDisplayInfo.FontSize = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontSize") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontSize")) : 0;
					menuDisplayInfo.FontWeight = AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontWeight");
					menuDisplayInfo.FontStyle = AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontStyle");
					menuDisplayInfo.MenuItemHAlign = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemHAlign");
					menuDisplayInfo.MenuItemVAlign = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemVAlign");
					menuDisplayInfo.FontColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontColor");
					menuDisplayInfo.MenuItemBgColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemBgColor");
					menuDisplayInfo.FontColorHilite = AtomUtility.GetDcElementContent(entry.AdditionalElements, "FontColorHilite");
					menuDisplayInfo.MenuHiliteBgColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuHiliteBgColor");
					menuDisplayInfo.XPosition = AtomUtility.GetDcElementContent(entry.AdditionalElements, "XPosition");
					menuDisplayInfo.YPosition = AtomUtility.GetDcElementContent(entry.AdditionalElements, "YPosition");
					menuDisplayInfo.HideOnMouseOut = AtomUtility.GetDcElementContent(entry.AdditionalElements, "HideOnMouseOut");
					menuDisplayInfo.MenuWidth = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuWidth") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuWidth")) : 0;
					menuDisplayInfo.MenuItemHeight = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemHeight") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemHeight")) : 0;
					menuDisplayInfo.MenuItemPadding = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemPadding") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemPadding")) : 0;
					menuDisplayInfo.MenuItemSpacing = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemSpacing") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemSpacing")) : 0;
					menuDisplayInfo.MenuItemIndent = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemIndent") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuItemIndent")) : 0;
					menuDisplayInfo.HideTimeout = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "HideTimeout") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "HideTimeout")) : 0;
					menuDisplayInfo.MenuBgOpaque = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuBgOpaque");
					menuDisplayInfo.MenuBorder = (AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuBorder") != string.Empty) ? int.Parse(AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuBorder")) : 0;
					menuDisplayInfo.BGColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "BGColor");
					menuDisplayInfo.MenuBorderBgColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuBorderBgColor");
					menuDisplayInfo.MenuLiteBgColor = AtomUtility.GetDcElementContent(entry.AdditionalElements, "MenuLiteBgColor");
					menuDisplayInfo.ChildMenuIcon = AtomUtility.GetDcElementContent(entry.AdditionalElements, "ChildMenuIcon");
					menuDisplayInfo.Description = AtomUtility.GetDcElementContent(entry.AdditionalElements, "Description");
					menuDisplayInfo.AddDate = DateTime.Now;
                    MenuDisplayInfo srcMenuDisplayInfo = DataProvider.MenuDisplayDAO.GetMenuDisplayInfoByMenuDisplayName(this.publishmentSystemID, menuDisplayInfo.MenuDisplayName);
					if (srcMenuDisplayInfo != null)
					{
						if (overwrite)
						{
							menuDisplayInfo.IsDefault = srcMenuDisplayInfo.IsDefault;
							menuDisplayInfo.MenuDisplayID = srcMenuDisplayInfo.MenuDisplayID;
                            DataProvider.MenuDisplayDAO.Update(menuDisplayInfo);
						}
						else
						{
                            menuDisplayInfo.MenuDisplayName = DataProvider.MenuDisplayDAO.GetImportMenuDisplayName(this.publishmentSystemID, menuDisplayInfo.MenuDisplayName);
							menuDisplayInfo.IsDefault = false;
                            DataProvider.MenuDisplayDAO.Insert(menuDisplayInfo);
						}
					}
					else
					{
						menuDisplayInfo.IsDefault = false;
                        DataProvider.MenuDisplayDAO.Insert(menuDisplayInfo);
					}
				}
			}
		}

	}
}
