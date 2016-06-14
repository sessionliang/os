using System;
using System.Collections.Generic;
using System.Text;
using BaiRong.Model;

namespace SiteServer.B2C.Model
{
    public class ShipmentInfo
    {
        private int id;
        private int publishmentSystemID;
        private string shipmentName;
        private EShipmentPeriod shipmentPeriod;
        private bool isEnabled;
        private int taxis;
        private string description;

        public ShipmentInfo()
        {

        }

        public ShipmentInfo(int id, int publishmentSystemID, string shipmentName, EShipmentPeriod shipmentPeriod, bool isEnabled, int taxis, string description)
        {
            this.id = id;
            this.publishmentSystemID = publishmentSystemID;
            this.shipmentName = shipmentName;
            this.shipmentPeriod = shipmentPeriod;
            this.isEnabled = isEnabled;
            this.taxis = taxis;
            this.description = description;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PublishmentSystemID
        {
            get { return publishmentSystemID; }
            set { publishmentSystemID = value; }
        }

        public string ShipmentName
        {
            get { return shipmentName; }
            set { shipmentName = value; }
        }

        public EShipmentPeriod ShipmentPeriod
        {
            get { return shipmentPeriod; }
            set { shipmentPeriod = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public int Taxis
        {
            get { return taxis; }
            set { taxis = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
}