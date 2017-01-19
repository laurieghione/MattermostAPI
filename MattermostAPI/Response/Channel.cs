using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattermostAPI
{
    public class Channel
    {
        public string id { get; set; }
        public string team_id { get; set; }
        public string display_name { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string creator_id { get; set; }
        public string create_at { get; set; }
        public string header { get; set; }
        public int total_msg_count { get; set; }
        public string update_at { get; set; }
    }
}
