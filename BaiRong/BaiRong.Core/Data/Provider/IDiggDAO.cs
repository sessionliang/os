using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IDiggDAO
	{
		void AddCount(int publishmentSystemID, int relatedIdentity, bool isGood);

        int[] GetCount(int publishmentSystemID, int relatedIdentity);

        void SetCount(int publishmentSystemID, int relatedIdentity, int goodNum, int badNum);

        void Delete(int publishmentSystemID, int relatedIdentity);

        bool IsExists(int publishmentSystemID, int relatedIdentity);

        ArrayList GetRelatedIdentityArrayListByTotal(int publishmentSystemID);
	}
}
