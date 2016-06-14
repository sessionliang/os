using System.Web.UI;
using BaiRong.Core;
using System.Web.UI.WebControls;
using BaiRong.Model;
using SiteServer.CMS.Core.Security;
using System.Collections;

using SiteServer.CMS.Model;
using BaiRong.Core.Data.Provider;
using BaiRong.Core.AuxiliaryTable;

namespace SiteServer.CMS.Core
{
	public class LevelManager
	{
        public class LevelInt
        {
            public const int CaoGao = -99;//草稿
            public const int DaiShen = 0;//待审

            public const int Pass1 = 1;//初审通过
            public const int Pass2 = 2;//二审通过
            public const int Pass3 = 3;//三审通过
            public const int Pass4 = 4;//四审通过
            public const int Pass5 = 5;//终审通过

            public const int Fail1 = -1;//初审退稿
            public const int Fail2 = -2;//二审退稿
            public const int Fail3 = -3;//三审退稿
            public const int Fail4 = -4;//四审退稿
            public const int Fail5 = -5;//终审退稿

            public const int NotChange = -100;//保持不变

            public static ArrayList GetFailLevelArrayList()
            {
                ArrayList arraylist = new ArrayList();
                arraylist.Add(Fail1);
                arraylist.Add(Fail2);
                arraylist.Add(Fail3);
                arraylist.Add(Fail4);
                arraylist.Add(Fail5);
                return arraylist;
            }

            public static ArrayList GetUnCheckedLevelArrayList()
            {
                ArrayList arraylist = new ArrayList();
                arraylist.Add(DaiShen);
                arraylist.Add(Pass1);
                arraylist.Add(Pass2);
                arraylist.Add(Pass3);
                arraylist.Add(Pass4);
                return arraylist;
            }

            public static ArrayList GetCheckLevelArrayList(PublishmentSystemInfo publishmentSystemInfo, bool isChecked, int checkedLevel)
            {
                if (isChecked)
                {
                    checkedLevel = 5;
                }

                ArrayList arraylist = new ArrayList();
                arraylist.Add(LevelInt.DaiShen);

                if (checkedLevel >= 1)
                {
                    arraylist.Add(LevelInt.Fail1);
                }
                
                if (checkedLevel >= 2)
                {
                    arraylist.Add(LevelInt.Pass1);
                    arraylist.Add(LevelInt.Fail2);
                }
                
                if (checkedLevel >= 3)
                {
                    arraylist.Add(LevelInt.Pass2);
                    arraylist.Add(LevelInt.Fail3);
                }
                
                if (checkedLevel >= 4)
                {
                    arraylist.Add(LevelInt.Pass3);
                    arraylist.Add(LevelInt.Fail4);
                }
                
                if (checkedLevel >= 5)
                {
                    arraylist.Add(LevelInt.Pass4);
                    arraylist.Add(LevelInt.Fail5);
                }

                return arraylist;
            }

            public static ArrayList GetCheckLevelArrayListOfNeedCheck(PublishmentSystemInfo publishmentSystemInfo, bool isChecked, int checkedLevel)
            {
                if (isChecked)
                {
                    checkedLevel = 5;
                }

                ArrayList arraylist = new ArrayList();
                arraylist.Add(LevelInt.DaiShen);

                if (checkedLevel >= 2)
                {
                    arraylist.Add(LevelInt.Pass1);
                }

                if (checkedLevel >= 3)
                {
                    arraylist.Add(LevelInt.Pass2);
                }

                if (checkedLevel >= 4)
                {
                    arraylist.Add(LevelInt.Pass3);
                }

                if (checkedLevel >= 5)
                {
                    arraylist.Add(LevelInt.Pass4);
                }

                return arraylist;
            }

            public static string GetLevelName(int level, PublishmentSystemInfo publishmentSystemInfo)
            {
                string retval = string.Empty;
                if (level == LevelInt.CaoGao)
                {
                    retval = "草稿";
                }
                else if (level == LevelInt.DaiShen)
                {
                    retval = "待审核";
                }
                else if (level == LevelInt.Pass1)
                {
                    retval = "初审通过";
                }
                else if (level == LevelInt.Pass2)
                {
                    retval = "二审通过";
                }
                else if (level == LevelInt.Pass3)
                {
                    retval = "三审通过";
                }
                else if (level == LevelInt.Pass4)
                {
                    retval = "四审通过";
                }
                else if (level == LevelInt.Pass5)
                {
                    retval = "终审通过";
                }
                else if (level == LevelInt.Fail1)
                {
                    retval = "初审退稿";
                }
                else if (level == LevelInt.Fail2)
                {
                    retval = "二审退稿";
                }
                else if (level == LevelInt.Fail3)
                {
                    retval = "三审退稿";
                }
                else if (level == LevelInt.Fail4)
                {
                    retval = "四审退稿";
                }
                else if (level == LevelInt.Fail5)
                {
                    retval = "终审退稿";
                }
                else if (level == LevelInt.NotChange)
                {
                    retval = "保持不变";
                }

                if (publishmentSystemInfo.IsCheckContentUseLevel)
                {
                    if (publishmentSystemInfo.CheckContentLevel <= level)
                    {
                        retval = "终审通过";
                    }
                }
                else
                {
                    if (level > 1)
                    {
                        retval = "终审通过";
                    }
                }

                return retval;
            }
        }

        public class Level
        {
            public const string CaoGao = "草稿";//草稿
            public const string DaiShen = "待审核";//待审
            public const string YiShenHe = "已审核";//已审核

            public const string NotChange = "保持不变";//保持不变
        }

        public class Level5
        {
            public const string Pass1 = "初审通过，等待二审";
            public const string Pass2 = "二审通过，等待三审";
            public const string Pass3 = "三审通过，等待四审";
            public const string Pass4 = "四审通过，等待终审";
            public const string Pass5 = "终审通过";

            public const string Fail1 = "初审退稿";
            public const string Fail2 = "二审退稿";
            public const string Fail3 = "三审退稿";
            public const string Fail4 = "四审退稿";
            public const string Fail5 = "终审退稿";
        }

        public class Level4
        {
            public const string Pass1 = "初审通过，等待二审";
            public const string Pass2 = "二审通过，等待三审";
            public const string Pass3 = "三审通过，等待终审";
            public const string Pass4 = "终审通过";

            public const string Fail1 = "初审退稿";
            public const string Fail2 = "二审退稿";
            public const string Fail3 = "三审退稿";
            public const string Fail4 = "终审退稿";
        }

        public class Level3
        {
            public const string Pass1 = "初审通过，等待二审";
            public const string Pass2 = "二审通过，等待终审";
            public const string Pass3 = "终审通过";

            public const string Fail1 = "初审退稿";
            public const string Fail2 = "二审退稿";
            public const string Fail3 = "终审退稿";
        }

        public class Level2
        {
            public const string Pass1 = "初审通过，等待终审";
            public const string Pass2 = "终审通过";

            public const string Fail1 = "初审退稿";
            public const string Fail2 = "终审退稿";
        }

        public class Level1
        {
            public const string Pass1 = "终审通过";

            public const string Fail1 = "终审退稿";
        }

        public static void LoadContentLevelToEdit(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, int nodeID, ContentInfo contentInfo, bool isChecked, int checkedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;
            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }

            ListItem listItem = null;

            bool isCheckable = false;
            if (contentInfo != null)
            {
                isCheckable = LevelManager.IsCheckable(publishmentSystemInfo, nodeID, contentInfo.IsChecked, contentInfo.CheckedLevel, isChecked, checkedLevel);
                if (isCheckable)
                {
                    listItem = new ListItem(Level.NotChange, LevelInt.NotChange.ToString());
                    listControl.Items.Add(listItem);
                }
            }

            listItem = new ListItem(Level.CaoGao, LevelInt.CaoGao.ToString());
            listControl.Items.Add(listItem);
            listItem = new ListItem(Level.DaiShen, LevelInt.DaiShen.ToString());
            listControl.Items.Add(listItem);

            if (checkContentLevel == 0 || checkContentLevel == 1)
            {
                listItem = new ListItem(Level1.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 2)
            {
                listItem = new ListItem(Level2.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level2.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 3)
            {
                listItem = new ListItem(Level3.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level3.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level3.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 4)
            {
                listItem = new ListItem(Level4.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass4, LevelInt.Pass4.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 5)
            {
                listItem = new ListItem(Level5.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass4, LevelInt.Pass4.ToString());
                listItem.Enabled = (checkedLevel >= 4);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass5, LevelInt.Pass5.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }

            if (contentInfo == null)
            {
                ControlUtils.SelectListItems(listControl, checkedLevel.ToString());
            }
            else
            {
                if (isCheckable)
                {
                    ControlUtils.SelectListItems(listControl, LevelManager.LevelInt.NotChange.ToString());
                }
                else
                {
                    ControlUtils.SelectListItems(listControl, checkedLevel.ToString());
                }
            }
        }

        public static void LoadContentLevelToList(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, int publishmentSystemID, bool isChecked, int checkedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }

            ListItem listItem = new ListItem("全部", string.Empty);
            listControl.Items.Add(listItem);

            listItem = new ListItem(Level.DaiShen, LevelInt.DaiShen.ToString());
            listControl.Items.Add(listItem);

            if (checkContentLevel == 1)
            {
                if (isChecked)
                {
                    listItem = new ListItem(Level1.Fail1, LevelInt.Fail1.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 2)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level2.Fail1, LevelInt.Fail1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (isChecked)
                {
                    listItem = new ListItem(Level2.Fail2, LevelInt.Fail2.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 3)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level3.Fail1, LevelInt.Fail1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level3.Fail2, LevelInt.Fail2.ToString());
                    listControl.Items.Add(listItem);
                }

                if (isChecked)
                {
                    listItem = new ListItem(Level3.Fail3, LevelInt.Fail3.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 4)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level4.Fail1, LevelInt.Fail1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level4.Fail2, LevelInt.Fail2.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 3)
                {
                    listItem = new ListItem(Level4.Fail3, LevelInt.Fail3.ToString());
                    listControl.Items.Add(listItem);
                }

                if (isChecked)
                {
                    listItem = new ListItem(Level4.Fail4, LevelInt.Fail4.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 5)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level5.Fail1, LevelInt.Fail1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level5.Fail2, LevelInt.Fail2.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 3)
                {
                    listItem = new ListItem(Level5.Fail3, LevelInt.Fail3.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 4)
                {
                    listItem = new ListItem(Level5.Fail4, LevelInt.Fail4.ToString());
                    listControl.Items.Add(listItem);
                }

                if (isChecked)
                {
                    listItem = new ListItem(Level5.Fail5, LevelInt.Fail5.ToString());
                    listControl.Items.Add(listItem);
                }
            }

            if (checkContentLevel == 2)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level2.Pass1, LevelInt.Pass1.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 3)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level3.Pass1, LevelInt.Pass1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level3.Pass2, LevelInt.Pass2.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 4)
            {
                if (checkedLevel >= 1)
                {
                    listItem = new ListItem(Level4.Pass1, LevelInt.Pass1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level4.Pass2, LevelInt.Pass2.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 3)
                {
                    listItem = new ListItem(Level4.Pass3, LevelInt.Pass3.ToString());
                    listControl.Items.Add(listItem);
                }
            }
            else if (checkContentLevel == 5)
            {
                if (checkedLevel >= 2)
                {
                    listItem = new ListItem(Level5.Pass1, LevelInt.Pass1.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 3)
                {
                    listItem = new ListItem(Level5.Pass2, LevelInt.Pass2.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 4)
                {
                    listItem = new ListItem(Level5.Pass3, LevelInt.Pass3.ToString());
                    listControl.Items.Add(listItem);
                }

                if (checkedLevel >= 5)
                {
                    listItem = new ListItem(Level5.Pass4, LevelInt.Pass4.ToString());
                    listControl.Items.Add(listItem);
                }
            }

            ControlUtils.SelectListItems(listControl, string.Empty);
        }

        public static void LoadContentLevelToCheck(ListControl listControl, PublishmentSystemInfo publishmentSystemInfo, bool isChecked, int checkedLevel)
        {
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;
            if (isChecked)
            {
                checkedLevel = checkContentLevel;
            }

            ListItem listItem = new ListItem(Level.CaoGao, LevelInt.CaoGao.ToString());
            listControl.Items.Add(listItem);

            listItem = new ListItem(Level.DaiShen, LevelInt.DaiShen.ToString());
            listControl.Items.Add(listItem);

            if (checkContentLevel == 1)
            {
                listItem = new ListItem(Level1.Fail1, LevelInt.Fail1.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 2)
            {
                listItem = new ListItem(Level2.Fail1, LevelInt.Fail1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level2.Fail2, LevelInt.Fail2.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 3)
            {
                listItem = new ListItem(Level3.Fail1, LevelInt.Fail1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level3.Fail2, LevelInt.Fail2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level3.Fail3, LevelInt.Fail3.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 4)
            {
                listItem = new ListItem(Level4.Fail1, LevelInt.Fail1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level4.Fail2, LevelInt.Fail2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level4.Fail3, LevelInt.Fail3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level4.Fail4, LevelInt.Fail4.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 5)
            {
                listItem = new ListItem(Level5.Fail1, LevelInt.Fail1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level5.Fail2, LevelInt.Fail2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level5.Fail3, LevelInt.Fail3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level5.Fail4, LevelInt.Fail4.ToString());
                listItem.Enabled = (checkedLevel >= 4);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level5.Fail5, LevelInt.Fail5.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }

            if (checkContentLevel == 0 || checkContentLevel == 1)
            {
                listItem = new ListItem(Level1.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 2)
            {
                listItem = new ListItem(Level2.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);

                listItem = new ListItem(Level2.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 3)
            {
                listItem = new ListItem(Level3.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level3.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level3.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 4)
            {
                listItem = new ListItem(Level4.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level4.Pass4, LevelInt.Pass4.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }
            else if (checkContentLevel == 5)
            {
                listItem = new ListItem(Level5.Pass1, LevelInt.Pass1.ToString());
                listItem.Enabled = (checkedLevel >= 1);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass2, LevelInt.Pass2.ToString());
                listItem.Enabled = (checkedLevel >= 2);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass3, LevelInt.Pass3.ToString());
                listItem.Enabled = (checkedLevel >= 3);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass4, LevelInt.Pass4.ToString());
                listItem.Enabled = (checkedLevel >= 4);
                listControl.Items.Add(listItem);
                listItem = new ListItem(Level5.Pass5, LevelInt.Pass5.ToString());
                listItem.Enabled = isChecked;
                listControl.Items.Add(listItem);
            }

            ControlUtils.SelectListItems(listControl, checkedLevel.ToString());
        }

        public static ArrayList GetCheckLevelToPassArrayList(PublishmentSystemInfo publishmentSystemInfo)
        {
            ArrayList arraylist = new ArrayList();
            int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

            arraylist.Add(LevelInt.DaiShen);

            if (checkContentLevel == 0 || checkContentLevel == 1)
            {
                arraylist.Add(LevelInt.Pass1);
            }
            else if (checkContentLevel == 2)
            {
                arraylist.Add(LevelInt.Pass1);
                arraylist.Add(LevelInt.Pass2);
            }
            else if (checkContentLevel == 3)
            {
                arraylist.Add(LevelInt.Pass1);
                arraylist.Add(LevelInt.Pass2);
                arraylist.Add(LevelInt.Pass3);
            }
            else if (checkContentLevel == 4)
            {
                arraylist.Add(LevelInt.Pass1);
                arraylist.Add(LevelInt.Pass2);
                arraylist.Add(LevelInt.Pass3);
                arraylist.Add(LevelInt.Pass4);
            }
            else if (checkContentLevel == 5)
            {
                arraylist.Add(LevelInt.Pass1);
                arraylist.Add(LevelInt.Pass2);
                arraylist.Add(LevelInt.Pass3);
                arraylist.Add(LevelInt.Pass4);
                arraylist.Add(LevelInt.Pass5);
            }

            return arraylist;
        }

        public static string GetCheckState(PublishmentSystemInfo publishmentSystemInfo, bool isChecked, int level)
        {
            if (isChecked)
            {
                return Level.YiShenHe;
            }

            string retval = string.Empty;

            if (level == LevelInt.CaoGao)
            {
                retval = Level.CaoGao;
            }
            else if (level == LevelInt.DaiShen)
            {
                retval = Level.DaiShen;
            }
            else
            {
                int checkContentLevel = publishmentSystemInfo.CheckContentLevel;

                if (checkContentLevel == 1)
                {
                    if (level == LevelInt.Fail1)
                    {
                        retval = Level1.Fail1;
                    }
                }
                else if (checkContentLevel == 2)
                {
                    if (level == LevelInt.Pass1)
                    {
                        retval = Level2.Pass1;
                    }
                    else if (level == LevelInt.Fail1)
                    {
                        retval = Level2.Fail1;
                    }
                    else if (level == LevelInt.Fail2)
                    {
                        retval = Level2.Fail2;
                    }
                }
                else if (checkContentLevel == 3)
                {
                    if (level == LevelInt.Pass1)
                    {
                        retval = Level3.Pass1;
                    }
                    else if (level == LevelInt.Pass2)
                    {
                        retval = Level3.Pass2;
                    }
                    else if (level == LevelInt.Fail1)
                    {
                        retval = Level3.Fail1;
                    }
                    else if (level == LevelInt.Fail2)
                    {
                        retval = Level3.Fail2;
                    }
                    else if (level == LevelInt.Fail3)
                    {
                        retval = Level3.Fail3;
                    }
                }
                else if (checkContentLevel == 4)
                {
                    if (level == LevelInt.Pass1)
                    {
                        retval = Level4.Pass1;
                    }
                    else if (level == LevelInt.Pass2)
                    {
                        retval = Level4.Pass2;
                    }
                    else if (level == LevelInt.Pass3)
                    {
                        retval = Level4.Pass3;
                    }
                    else if (level == LevelInt.Fail1)
                    {
                        retval = Level4.Fail1;
                    }
                    else if (level == LevelInt.Fail2)
                    {
                        retval = Level4.Fail2;
                    }
                    else if (level == LevelInt.Fail3)
                    {
                        retval = Level4.Fail3;
                    }
                    else if (level == LevelInt.Fail4)
                    {
                        retval = Level4.Fail4;
                    }
                }
                else if (checkContentLevel == 5)
                {
                    if (level == LevelInt.Pass1)
                    {
                        retval = Level5.Pass1;
                    }
                    else if (level == LevelInt.Pass2)
                    {
                        retval = Level5.Pass2;
                    }
                    else if (level == LevelInt.Pass3)
                    {
                        retval = Level5.Pass3;
                    }
                    else if (level == LevelInt.Pass4)
                    {
                        retval = Level5.Pass4;
                    }
                    else if (level == LevelInt.Fail1)
                    {
                        retval = Level5.Fail1;
                    }
                    else if (level == LevelInt.Fail2)
                    {
                        retval = Level5.Fail2;
                    }
                    else if (level == LevelInt.Fail3)
                    {
                        retval = Level5.Fail3;
                    }
                    else if (level == LevelInt.Fail4)
                    {
                        retval = Level5.Fail4;
                    }
                    else if (level == LevelInt.Fail5)
                    {
                        retval = Level5.Fail5;
                    }
                }

                if (string.IsNullOrEmpty(retval))
                {
                    if (checkContentLevel == 1)
                    {
                        retval = Level.DaiShen;
                    }
                    else if (checkContentLevel == 2)
                    {
                        retval = Level2.Pass1;
                    }
                    else if (checkContentLevel == 3)
                    {
                        retval = Level3.Pass2;
                    }
                    else if (checkContentLevel == 4)
                    {
                        retval = Level4.Pass3;
                    }
                }
            }

            return string.Format("<span style='color:red'>{0}</span>", retval);
        }

        public static bool IsCheckable(PublishmentSystemInfo publishmentSystemInfo, int nodeID, bool contentIsChecked, int contentCheckLevel, bool isChecked, int checkedLevel)
        {
            if (isChecked || checkedLevel >= 5)
            {
                return true;
            }
            else if (contentIsChecked)
            {
                return false;
            }
            else
            {
                if (checkedLevel == 0)
                {
                    return false;
                }
                else if (checkedLevel == 1)
                {
                    if (contentCheckLevel == LevelManager.LevelInt.CaoGao || contentCheckLevel == LevelManager.LevelInt.DaiShen || contentCheckLevel == LevelManager.LevelInt.Pass1 || contentCheckLevel == LevelManager.LevelInt.Fail1)
                    {
                        return true;
                    }
                    return false;
                }
                else if (checkedLevel == 2)
                {
                    if (contentCheckLevel == LevelManager.LevelInt.CaoGao || contentCheckLevel == LevelManager.LevelInt.DaiShen || contentCheckLevel == LevelManager.LevelInt.Pass1 || contentCheckLevel == LevelManager.LevelInt.Pass2 || contentCheckLevel == LevelManager.LevelInt.Fail1 || contentCheckLevel == LevelManager.LevelInt.Fail2)
                    {
                        return true;
                    }
                    return false;
                }
                else if (checkedLevel == 3)
                {
                    if (contentCheckLevel == LevelManager.LevelInt.CaoGao || contentCheckLevel == LevelManager.LevelInt.DaiShen || contentCheckLevel == LevelManager.LevelInt.Pass1 || contentCheckLevel == LevelManager.LevelInt.Pass2 || contentCheckLevel == LevelManager.LevelInt.Pass3 || contentCheckLevel == LevelManager.LevelInt.Fail1 || contentCheckLevel == LevelManager.LevelInt.Fail2 || contentCheckLevel == LevelManager.LevelInt.Fail3)
                    {
                        return true;
                    }
                    return false;
                }
                else if (checkedLevel == 4)
                {
                    if (contentCheckLevel == LevelManager.LevelInt.CaoGao || contentCheckLevel == LevelManager.LevelInt.DaiShen || contentCheckLevel == LevelManager.LevelInt.Pass1 || contentCheckLevel == LevelManager.LevelInt.Pass2 || contentCheckLevel == LevelManager.LevelInt.Pass3 || contentCheckLevel == LevelManager.LevelInt.Pass4 || contentCheckLevel == LevelManager.LevelInt.Fail1 || contentCheckLevel == LevelManager.LevelInt.Fail2 || contentCheckLevel == LevelManager.LevelInt.Fail3 || contentCheckLevel == LevelManager.LevelInt.Fail4)
                    {
                        return true;
                    }
                    return false;
                }
            }

            return false;
        }
	}
}
