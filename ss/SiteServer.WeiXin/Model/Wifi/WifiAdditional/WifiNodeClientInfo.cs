using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteServer.WeiXin.Model
{
    public class ClientList
    {
        public string id { get; set; }
        public string node_id { get; set; }
        public string mac { get; set; }
        public string ip { get; set; }
        public int in_come { get; set; }
        public int out_go { get; set; }
        public int created_at { get; set; }
        public int updated_at { get; set; }
        public string user_agent { get; set; }
        public string user_type { get; set; }
        public int auth_type { get; set; }
    }

    public class ClientConf
    {
        public string total { get; set; }
        public int page { get; set; }
        public int perpage { get; set; }
        public int totalpage { get; set; }
    }

    public class ClientData
    {
        public List<ClientList> list { get; set; }
        public ClientConf conf { get; set; }
    }

    public class WifiNodeClientInfo
    {
        public int ret { get; set; }
        public ClientData data { get; set; }
        public string msg { get; set; }
    }

}
