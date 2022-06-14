using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apisanpham.Models
{
    public class loaisp
    {
        public int pro_type_id { get; set; }
        public string pro_type_name { get; set; }
        public loaisp(int pro_type_id,string pro_type_name)
        {
            this.pro_type_id = pro_type_id;
            this.pro_type_name = pro_type_name;
        }
    }
}
