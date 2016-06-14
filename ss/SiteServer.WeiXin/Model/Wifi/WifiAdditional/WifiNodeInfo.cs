using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{
    public class List
    {
        public string id { get; set; }
        public string name { get; set; }
        public string gw_ip { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string wx_bind { get; set; }
        public string wechat_id { get; set; }
        public string wechat_img { get; set; }
        public string online { get; set; }
        public string today_newfans { get; set; }
        public string today_fans { get; set; }
        public string today_visiter { get; set; }
    }

    public class NodeConf
    {
        public string total { get; set; }
        public int page { get; set; }
        public int perpage { get; set; }
        public int totalpage { get; set; }
    }

    public class NodeData
    {
        public List<List> list { get; set; }
        public NodeConf conf { get; set; }
    }

    public class WifiNodeListInfo
    {
        public int ret { get; set; }
        public NodeData data { get; set; }
        public string msg { get; set; }
    }

    public class NodeResultData
    {
        public int id { get; set; }
    }

    public class WifiNodeResult
    {
        public int ret { get; set; }
        public NodeResultData data { get; set; }
        public string msg { get; set; }
    }

    public class DetailNodeData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string gw_ip { get; set; }
        public string create_time { get; set; }
        public string update_time { get; set; }
        public string wx_bind { get; set; }
        public string wechat_id { get; set; }
        public string wechat_img { get; set; }
        public string online { get; set; }
        public string today_newfans { get; set; }
        public string today_fans { get; set; }
        public string today_visiter { get; set; }
        public string sday_visiter { get; set; }
        public string sday_fans { get; set; }
        public string sday_newfans { get; set; }
        public string ssday_visiter { get; set; }
        public string ssday_fans { get; set; }
        public string ssday_newfans { get; set; }

    }

    public class WifiNodeDetailInfo
    {
        public int ret { get; set; }
        public DetailNodeData data { get; set; }
        public string msg { get; set; }
    }
}
