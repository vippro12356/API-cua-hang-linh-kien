using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apisanpham.Models
{
    public class sanphamdat
    {
        public int id { get; set;}
        public int amount { get; set; }
        public int price { get; set; }
        public string pro_name { get; set; }
        public string img { get; set; }      
        public sanphamdat(int id,int amount,int price)
        {
            this.id = id;
            this.amount = amount;
            this.price = price;
            this.pro_name = pro_name;
            this.img = img;
        }
    }
}
