using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apisanpham.Models
{
    public class sanpham
    {
        public int id { get; set; }
        public string pro_name { get; set; }
        public int price { get; set; }
        public int nsx_id { get; set; }
        public int pro_type { get; set; }
        public string img { get; set; }
        public sanpham() { }
        public sanpham(int id,string pro_name,int price,int nsx_id,int pro_type,string img)
        {
            this.id = id;
            this.pro_name = pro_name;
            this.price = price;
            this.nsx_id = nsx_id;
            this.pro_type = pro_type;
            this.img = img;
        }
    }
}
