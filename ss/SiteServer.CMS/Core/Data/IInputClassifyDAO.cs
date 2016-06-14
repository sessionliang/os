using System;
using System.Collections.Generic;
using System.Text;
using SiteServer.CMS.Model;
using System.Collections;

namespace SiteServer.CMS.Core
{
    public interface IInputClassifyDAO : ITreeDAO
    {
        int InsertInputClassifyInfo(int publishmentSystemID, int parentID, string itemName, string itemIndexName);

        int InsertInputClassifyInfo(InputClissifyInfo inputClassifyInfo);

        void UpdateInputClassifyInfo(InputClissifyInfo inputClassifyInfo);

        InputClissifyInfo GetInputClassifyInfo(int itemID);

        void Delete(int deleteID);

        void UpdateInputCount(int publishmentSystemID, int classifyID, int type);


        /// <summary>
        /// 设置默认分类
        /// </summary>
        /// <returns></returns>
        int SetDefaultInfo(int publishmentSystemID);

        InputClissifyInfo GetDefaultInfo(int publishmentSystemID);

        /// <summary>
        /// 修改某个分类的内容数量为所有表单的总数
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="id"></param>
        void UpdateCountByAll(int publishmentSystemID, int id);

        /// <summary>
        /// 调整某个分类下表单数量及更新全部分类下表单的数量
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="classifyID"></param> 
        /// <param name="pid">全部分类ID</param> 
        void UpdateInputCountByClassifyID(int publishmentSystemID, int classifyID, int pid);

    }
}
