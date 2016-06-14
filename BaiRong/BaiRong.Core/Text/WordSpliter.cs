using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BaiRong.Core.Data.Provider;

namespace BaiRong.Core
{
    /// <summary>
    /// 分词类
    /// </summary>
    public static class WordSpliter
    {
        #region 属性
        private static string SplitChar = " ";//分隔符
        #endregion
        //
        #region 数据缓存函数
        /// <summary>
        /// 数据缓存函数
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="val">缓存的数据</param>
        private static void SetCache(string key, object val)
        {
            if (val == null)
                val = " ";
            System.Web.HttpContext.Current.Application.Lock();
            System.Web.HttpContext.Current.Application.Set(key, val);
            System.Web.HttpContext.Current.Application.UnLock();
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="mykey"></param>
        /// <returns></returns>
        private static object GetCache(string key)
        {
            return System.Web.HttpContext.Current.Application.Get(key);
        }
        #endregion
        //
        #region 读取文本

        private static SortedList ReadContent(string productID, int publishmentSystemID)
        {
            string cacheKey = "BaiRong.Core.WordSpliter" + productID + publishmentSystemID;
            if (GetCache(cacheKey) == null)
            {
                SortedList arrText = new SortedList();

                ArrayList tagArrayList = BaiRongDataProvider.TagDAO.GetTagArrayList(productID, publishmentSystemID);
                if (tagArrayList.Count > 0)
                {
                    foreach (string line in tagArrayList)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            arrText.Add(line.Trim(), line.Trim());
                        }
                    }
                }
                SetCache(cacheKey, arrText);
            }
            return (SortedList)GetCache(cacheKey);
        }
        #endregion
        //
        #region 判断某字符串是否在指定字符数组中
        private static bool StrIsInArray(string[] StrArray, string val)
        {
            for (int i = 0; i < StrArray.Length; i++)
                if (StrArray[i] == val) return true;
            return false;
        }
        #endregion
        //
        #region 正则检测
        private static bool IsMatch(string str, string reg)
        {
            return new Regex(reg).IsMatch(str);
        }
        #endregion
        //
        #region 首先格式化字符串(粗分)
        private static string FormatStr(string val)
        {
            string result = "";
            if (val == null || val == "")
                return "";
            //
            char[] CharList = val.ToCharArray();
            //
            string Spc = SplitChar;//分隔符
            int StrLen = CharList.Length;
            int CharType = 0; //0-空白 1-英文 2-中文 3-符号
            //
            for (int i = 0; i < StrLen; i++)
            {
                string StrList = CharList[i].ToString();
                if (StrList == null || StrList == "")
                    continue;
                //
                if (CharList[i] < 0x81)
                {
                    #region
                    if (CharList[i] < 33)
                    {
                        if (CharType != 0 && StrList != "\n" && StrList != "\r")
                        {
                            result += " ";
                            CharType = 0;
                        }
                        continue;
                    }
                    else if (IsMatch(StrList, "[^0-9a-zA-Z@\\.%#:\\/&_-]"))//排除这些字符
                    {
                        if (CharType == 0)
                            result += StrList;
                        else
                            result += Spc + StrList;
                        CharType = 3;
                    }
                    else
                    {
                        if (CharType == 2 || CharType == 3)
                        {
                            result += Spc + StrList;
                            CharType = 1;
                        }
                        else
                        {
                            if (IsMatch(StrList, "[@%#:]"))
                            {
                                result += StrList;
                                CharType = 3;
                            }
                            else
                            {
                                result += StrList;
                                CharType = 1;
                            }//end if No.4
                        }//end if No.3
                    }//end if No.2
                    #endregion
                }//if No.1
                else
                {
                    //如果上一个字符为非中文和非空格，则加一个空格
                    if (CharType != 0 && CharType != 2)
                        result += Spc;
                    //如果是中文标点符号
                    if (!IsMatch(StrList, "^[\u4e00-\u9fa5]+$"))
                    {
                        if (CharType != 0)
                            result += Spc + StrList;
                        else
                            result += StrList;
                        CharType = 3;
                    }
                    else //中文
                    {
                        result += StrList;
                        CharType = 2;
                    }
                }
                //end if No.1

            }//exit for
            //
            return result;
        }
        #endregion
        //
        #region 分词
        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="key">关键词</param>
        /// <returns></returns>
        private static ArrayList StringSpliter(string[] key, string productID, int publishmentSystemID)
        {
            ArrayList List = new ArrayList();
            SortedList dict = ReadContent(productID, publishmentSystemID);//载入词典
            //
            for (int i = 0; i < key.Length; i++)
            {
                if (IsMatch(key[i], @"^(?!^\.$)([a-zA-Z0-9\.\u4e00-\u9fa5]+)$")) //中文、英文、数字
                {
                    if (IsMatch(key[i], "^[\u4e00-\u9fa5]+$"))//如果是纯中文
                    {
                        int keyLen = key[i].Length;
                        if (keyLen < 2)
                            continue;
                        else if (keyLen <= 7)
                            List.Add(key[i]);
                        //开始分词
                        for (int x = 0; x < keyLen; x++)
                        {
                            //x：起始位置//y：结束位置
                            for (int y = x; y < keyLen; y++)
                            {
                                string val = key[i].Substring(x, keyLen - y);
                                if (val == null || val.Length < 2)
                                    break;
                                else if (val.Length > 10)
                                    continue;
                                if (dict.Contains(val))
                                    List.Add(val);
                            }
                        }
                    }
                    else if (!IsMatch(key[i], @"^(\.*)$"))//不全是小数点
                    {
                        List.Add(key[i]);
                    }
                }
            }
            return List;
        }
        #endregion
        //
        #region 得到分词结果
        /// <summary>
        /// 得到分词结果
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string[] DoSplit(string content, string productID, int publishmentSystemID)
        {
            ArrayList KeyList = StringSpliter(FormatStr(content).Split(SplitChar.ToCharArray()), productID, publishmentSystemID);
            KeyList.Insert(0, content);
            //去掉重复的关键词
            for (int i = 0; i < KeyList.Count; i++)
            {
                for (int j = 0; j < KeyList.Count; j++)
                {
                    if (KeyList[i].ToString() == KeyList[j].ToString())
                    {
                        if (i != j)
                        {
                            KeyList.RemoveAt(j); j--;
                        }
                    }
                }
            }
            return (string[])KeyList.ToArray(typeof(string));
        }
        /// <summary>
        /// 得到分词关键字，以逗号隔开
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetKeywords(string content, string productID, int publishmentSystemID, int totalNum)
        {
            string _value = "";
            string[] _key = DoSplit(content, productID, publishmentSystemID);
            int currentNum = 1;
            for (int i = 1; i < _key.Length; i++)
            {
                if (totalNum > 0 && currentNum > totalNum) break;
                string key = _key[i].Trim();
                if (key.Length == 1) continue;
                if (i == 1)
                    _value = key;
                else
                    _value += " " + key;
                currentNum++;
            }
            return _value;
        }
        #endregion
    }
}
