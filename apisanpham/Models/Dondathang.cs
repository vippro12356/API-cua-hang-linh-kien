using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
namespace apisanpham.Models
{
    [BsonIgnoreExtraElements]
    public class Dondathang
    {
        public string Hoten { get; set; }
        public string Diachi { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public List<sanphamdat> Sanpham { get; set; }
        public int Tongtien { get; set; }
        public Dondathang(string Hoten,string Diachi,string SDT,string Email,List<sanphamdat>Sanpham,int Tongtien)
        {
            this.Hoten = Hoten;
            this.Diachi = Diachi;   
            this.SDT = SDT;
            this.Email = Email;
            this.Sanpham = Sanpham;
            this.Tongtien = Tongtien;
        }
    }
}
