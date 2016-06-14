
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
    public interface IUserNewGroupDAO
    { 
         
        ArrayList GetInfoList(string whrerStr);
         
        /// <summary>
        /// �޸ķ�����������
        /// </summary>
        /// <param name="publishmentSystemID"></param>
        /// <param name="itemID"></param>
        /// <param name="contentNum"></param>
        void UpdateContentNum(int itemID, int contentNum);


        UserNewGroupInfo GetInfo(int groupID);

    }
}
