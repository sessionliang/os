using System.Collections;
using System.Collections.Generic;
using SiteServer.B2C.Model;

namespace SiteServer.B2C.Core
{
	public interface IShipmentDAO
	{
        int Insert(ShipmentInfo shipmentInfo);

        void Update(ShipmentInfo shipmentInfo);

        void Delete(int shipmentID);

        ShipmentInfo GetShipmentInfo(int shipmentID);

        bool IsExists(int publishmentSystemID, string shipmentName);

        IEnumerable GetDataSource(int publishmentSystemID);

        List<ShipmentInfo> GetShipmentInfoList(int publishmentSystemID);

        bool UpdateTaxisToUp(int publishmentSystemID, int shipmentID);

        bool UpdateTaxisToDown(int publishmentSystemID, int shipmentID);
	}
}
