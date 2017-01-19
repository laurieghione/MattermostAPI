using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattermostAPI
{
    public class Team
    {
        public string id { get; set; }
        public string display_name { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string email { get; set; }
        public string create_at { get; set; }
        public string update_at { get; set; }
    }
}
