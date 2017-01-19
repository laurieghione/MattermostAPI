using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MattermostAPI
{
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nickname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string roles { get; set; }
        public string create_at { get; set; }
        public string update_at { get; set; }
        public string delete_at { get; set; }
        public string last_password_update { get; set; }
        public string last_picture_update { get; set; }
    }
}
