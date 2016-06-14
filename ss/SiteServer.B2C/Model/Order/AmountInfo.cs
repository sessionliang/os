using System;
using System.Collections.Generic;
using System.Web;

namespace SiteServer.B2C.Model
{
    public class AmountInfo
    {
        //原始金额
        public decimal PriceTotal { get; set; }
        //返现
        public decimal PriceReturn { get; set; }
        //运费
        public decimal PriceShipment { get; set; }
        //实际金额
        public decimal PriceActual { get; set; }
    }
}