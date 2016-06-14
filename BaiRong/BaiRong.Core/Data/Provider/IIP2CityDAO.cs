using System;
using System.Data;
using System.Collections;

using BaiRong.Model;

namespace BaiRong.Core.Data.Provider
{
	public interface IIP2CityDAO
	{
        SortedList GetCityWithNumSortedList(ArrayList ipAddressArrayList);

        string GetCity(string ipAddress);

        void TranslateIP2City();
	}
}
