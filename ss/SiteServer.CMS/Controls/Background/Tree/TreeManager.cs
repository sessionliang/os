using BaiRong.Core;
using BaiRong.Model;
using SiteServer.CMS.Core;
using SiteServer.CMS.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SiteServer.CMS.Controls
{
    public class TreeManager
    {
        private static string assemblyString;
        private static string namespaceString;

        static TreeManager()
        {
            assemblyString = "SiteServer.CMS";
            if (BaiRongDataProvider.DatabaseType == EDatabaseType.SqlServer)
            {
                namespaceString = "SiteServer.CMS.Provider.Data.SqlServer";
            }
            else if (BaiRongDataProvider.DatabaseType == EDatabaseType.Oracle)
            {
                namespaceString = "SiteServer.CMS.Provider.Data.Oracle";
            }
        }

        public static List<TreeBaseItem> GetItemInfoArrayListByParentID(int publishmentSystemID, int parentID, string classifyType)
        {
            if (classifyType == "InputClassify")
            {
                #region by 20151206 sofuny 默认创建一个全部分类和默认分类，表单分类
                DataProvider.InputClassifyDAO.SetDefaultInfo(publishmentSystemID);
                #endregion
            }
            string className = namespaceString + "." + classifyType + "DAO";
            ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);
            return treeDAO.GetItemInfoArrayListByParentID(publishmentSystemID, parentID);

        }


        public static ArrayList GetItemIDArrayListByParentID(int publishmentSystemID, int parentID, string classifyType)
        {
            #region by 20151206 sofuny 表单分类增加了权限，当前用户为管理员
            if (classifyType == "InputClassify")
            { 
                string className = namespaceString + "." + classifyType + "DAO";
                ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);

                if (!PermissionsManager.Current.IsConsoleAdministrator && !PermissionsManager.Current.IsSystemAdministrator)
                {
                    ArrayList itemList = new ArrayList();
                    InputClissifyInfo pinfo = DataProvider.InputClassifyDAO.GetDefaultInfo(publishmentSystemID);
                    itemList.Add(pinfo.ItemID);
                    //获取当前登录用户所有的表单分类权限及截取表单分类ID
                    ArrayList websitePermissionArrayList = SiteServer.CMS.Core.Security.ProductPermissionsManager.Current.WebsitePermissionSortedList[publishmentSystemID] as ArrayList;
                    if (websitePermissionArrayList != null && websitePermissionArrayList.Count > 0)
                    {
                        foreach (string permission in websitePermissionArrayList)
                        {
                            if (permission.StartsWith(AppManager.CMS.Permission.WebSite.InputClassifyView + "_"))
                            {
                                itemList.Add(permission.TrimStart((AppManager.CMS.Permission.WebSite.InputClassifyView + "_").ToCharArray()));
                            }
                        }
                    }
                    return treeDAO.GetItemIDArrayListByParentID(publishmentSystemID, parentID, itemList);
                }
                return treeDAO.GetItemIDArrayListByParentID(publishmentSystemID, parentID);
            }
            #endregion
            else
            {
                string className = namespaceString + "." + classifyType + "DAO";
                ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);
                return treeDAO.GetItemIDArrayListByParentID(publishmentSystemID, parentID);
            }
        }

        public static TreeBaseItem GetItemInfo(int publishmentSystemID, int itemID, string classifyType)
        {
            string className = namespaceString + "." + classifyType + "DAO";
            ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);
            return treeDAO.GetItemInfo(publishmentSystemID, itemID);
        }

        public static void AddListItems(ListItemCollection listItemCollection, int publishmentSystemID, int parentID, bool isSeeOwning, bool isShowContentNum, string classifyType)
        {

            string className = namespaceString + "." + classifyType + "DAO";
            ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);

            ArrayList arraylist = treeDAO.GetItemIDArrayListByItemID(publishmentSystemID);
            int itemCount = arraylist.Count;
            bool[] isLastItemArray = new bool[itemCount];
            foreach (int itemID in arraylist)
            {
                TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);

                ListItem listitem = new ListItem(TreeManager.GetSelectText(publishmentSystemID, itemInfo, isLastItemArray, isShowContentNum), itemInfo.ItemID.ToString());
                if (!itemInfo.Enabled)
                {
                    listitem.Attributes.Add("style", "color:gray;");
                }

                listItemCollection.Add(listitem);
            }
        }

        /// <summary>
        /// by 20151112 sofuny
        /// 
        /// 加载有分类的树
        /// </summary>
        /// <param name="listItemCollection"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <param name="isSeeOwning"></param>
        /// <param name="isShowContentNum"></param>
        /// <param name="classifyType"></param>
        /// <param name="classifyID"></param>
        /// <param name="showLayer">显示几层 1，2，3...
        /// 如果没有默认的顶层则从1开始
        /// 如果有默认的顶层则从2开始
        /// 如果传递为0则不做限制
        /// </param>
        public static void AddListItemsByClassify(ListItemCollection listItemCollection, int publishmentSystemID, int parentID, bool isSeeOwning, bool isShowContentNum, string classifyType, int classifyID, int showLayer)
        {

            string className = namespaceString + "." + classifyType + "DAO";
            ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);

            ArrayList arraylist = treeDAO.GetItemInfoByClassifyID(publishmentSystemID, classifyID);
            int itemCount = arraylist.Count;
            bool[] isLastItemArray = new bool[itemCount];

            if (showLayer != 0)
                foreach (int itemID in arraylist)
                {
                    TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);
                    if (itemInfo.ParentsCount < showLayer)
                    {
                        ListItem listitem = new ListItem(TreeManager.GetSelectText(publishmentSystemID, itemInfo, isLastItemArray, isShowContentNum), itemInfo.ItemID.ToString());
                        if (!itemInfo.Enabled)
                        {
                            listitem.Attributes.Add("style", "color:gray;");
                        }

                        listItemCollection.Add(listitem);
                    }
                }
            else
                foreach (int itemID in arraylist)
                {
                    TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);

                    ListItem listitem = new ListItem(TreeManager.GetSelectText(publishmentSystemID, itemInfo, isLastItemArray, isShowContentNum), itemInfo.ItemID.ToString());
                    if (!itemInfo.Enabled)
                    {
                        listitem.Attributes.Add("style", "color:gray;");
                    }

                    listItemCollection.Add(listitem);
                }
        }



        /// <summary>
        /// by 20151111 sofuny 
        /// 
        /// 传递可以显示多少层
        /// </summary>
        /// <param name="listItemCollection"></param>
        /// <param name="publishmentSystemID"></param>
        /// <param name="parentID"></param>
        /// <param name="isSeeOwning"></param>
        /// <param name="isShowContentNum"></param>
        /// <param name="classifyType"></param>
        /// <param name="showLayer">显示几层  1，2，3...
        /// 如果没有默认的顶层则从1开始
        /// 如果有默认的顶层则从2开始
        /// 如果传递为0则不做限制
        /// </param>
        public static void AddListItems(ListItemCollection listItemCollection, int publishmentSystemID, int parentID, bool isSeeOwning, bool isShowContentNum, string classifyType, int showLayer)
        {
            string className = namespaceString + "." + classifyType + "DAO";
            ITreeDAO treeDAO = (ITreeDAO)Assembly.Load(assemblyString).CreateInstance(className);

            ArrayList arraylist = treeDAO.GetItemIDArrayListByItemID(publishmentSystemID);
            int itemCount = arraylist.Count;
            bool[] isLastItemArray = new bool[itemCount];

            if (showLayer != 0)
                foreach (int itemID in arraylist)
                {
                    TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);
                    if (itemInfo.ParentsCount < showLayer)
                    {
                        ListItem listitem = new ListItem(TreeManager.GetSelectText(publishmentSystemID, itemInfo, isLastItemArray, isShowContentNum), itemInfo.ItemID.ToString());
                        if (!itemInfo.Enabled)
                        {
                            listitem.Attributes.Add("style", "color:gray;");
                        }

                        listItemCollection.Add(listitem);
                    }
                }
            else
                foreach (int itemID in arraylist)
                {
                    TreeBaseItem itemInfo = TreeManager.GetItemInfo(publishmentSystemID, itemID, classifyType);
                    ListItem listitem = new ListItem(TreeManager.GetSelectText(publishmentSystemID, itemInfo, isLastItemArray, isShowContentNum), itemInfo.ItemID.ToString());
                    if (!itemInfo.Enabled)
                    {
                        listitem.Attributes.Add("style", "color:gray;");
                    }

                    listItemCollection.Add(listitem);
                }
        }

        public static string GetSelectText(int publishmentSystemID, TreeBaseItem itemInfo, bool[] isLastItemArray, bool isShowContentNum)
        {
            string retval = string.Empty;
            //if (itemInfo.ParentID == itemInfo.PublishmentSystemID)
            //{
            //    itemInfo.IsLastItem = true;
            //}
            if (itemInfo.IsLastItem == false)
            {
                isLastItemArray[itemInfo.ParentsCount] = false;
            }
            else
            {
                isLastItemArray[itemInfo.ParentsCount] = true;
            }
            for (int i = 0; i < itemInfo.ParentsCount; i++)
            {
                if (isLastItemArray[i])
                {
                    retval = String.Concat(retval, "　");
                }
                else
                {
                    retval = String.Concat(retval, "│");
                }
            }
            if (itemInfo.IsLastItem)
            {
                retval = String.Concat(retval, "└");
            }
            else
            {
                retval = String.Concat(retval, "├");
            }
            retval = String.Concat(retval, itemInfo.ItemName);

            if (isShowContentNum)
            {
                retval = String.Concat(retval, " (", itemInfo.ContentNum, ")");
            }
            return retval;
        }
    }
}
