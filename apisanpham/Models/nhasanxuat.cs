using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apisanpham.Models
{
    public class nhasanxuat
    {
        public int nsx_id { get; set; }
        public string nsx_name { get; set; }
        public nhasanxuat(int nsx_id,string nsx_name)
        {
            this.nsx_id = nsx_id;
            this.nsx_name = nsx_name;
        }
    }
}
