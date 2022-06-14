using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using apisanpham.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net.Mail;
using System.Net;
using System.Data;

namespace apisanpham.services
{
    public class service:connect
    {
        
        public service()
        {
            
        }
        public List<sanpham> listsanpham()
        {
            List<sanpham> listsp = new List<sanpham>();
            string sql = @"select * from sanpham";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int id = int.Parse(read["id"].ToString());
                        string pro_name = read["pro_name"].ToString();
                        int price = int.Parse(read["price"].ToString());
                        int nsx_id = int.Parse(read["nsx_id"].ToString());
                        int pro_type_id = int.Parse(read["pro_type_id"].ToString());
                        string img = read["img"].ToString();
                        sanpham sp = new sanpham(id, pro_name, price, nsx_id, pro_type_id, img);
                        listsp.Add(sp);
                    }
                    read.Close();
                    con.Close();
                }
            }
            return listsp;
        }
        public List<nhasanxuat>listnhasanxuat()
        {
            List<nhasanxuat> listnsx = new List<nhasanxuat>();
            string sql = @"select * from nhasanxuat";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int id = int.Parse(read["nsx_id"].ToString());                       
                        string nsx_name = read["nsx_name"].ToString();
                        nhasanxuat nsx = new nhasanxuat(id, nsx_name);
                        listnsx.Add(nsx);
                    }
                    read.Close();
                    con.Close();
                }
            }
            return listnsx;
        }
        public List<loaisp> listloaisp()
        {
            List<loaisp> listlsp = new List<loaisp>();
            string sql = @"select * from loaisp";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int pro_type_id = int.Parse(read["pro_type_id"].ToString());
                        string pro_type_name = read["pro_type_name"].ToString();
                        loaisp loaisp = new loaisp(pro_type_id, pro_type_name);
                        listlsp.Add(loaisp);
                    }
                    read.Close();
                    con.Close();
                }
            }
            return listlsp;
        }
        public void dathang(Dondathang ddh)
        {
            int iddh = taodondh(ddh);
            themctdh(ddh.Sanpham, iddh);
            sendemail(ddh.Email);
        }
        public int taodondh(Dondathang ddh)
        {
            int id_dh;
            string sql = "insert into dondh output inserted.iddh values(N'" + ddh.Hoten + "',N'"+ddh.Diachi+"','"+ddh.SDT+"','"+ddh.Email+"',"+ddh.Tongtien+")";
            string conn = connectionString;
            using(SqlConnection con=new SqlConnection(conn))
            {
                con.Open();
                using(SqlCommand cmd=new SqlCommand(sql,con))
                {
                    id_dh = (int)cmd.ExecuteScalar();
                }
                con.Close();
            }
            return id_dh;
        }
        public void themctdh(List<sanphamdat> listsp,int id_dh)
        {
            string conn = connectionString;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("iddh", typeof(int)));
            dt.Columns.Add(new DataColumn("id", typeof(int)));
            dt.Columns.Add(new DataColumn("soluong", typeof(int)));
            dt.Columns.Add(new DataColumn("giaban", typeof(int)));
            foreach (sanphamdat sp in listsp)
            {
                DataRow row = dt.NewRow();
                row["iddh"] = id_dh;
                row["id"] = sp.id;
                row["soluong"] = sp.amount;
                row["giaban"] = sp.price;
                dt.Rows.Add(row);
            }
            using(SqlBulkCopy bulk=new SqlBulkCopy(conn))
            {
                bulk.DestinationTableName = "ctddh";
                bulk.WriteToServer(dt);
            }
        }
        public void sendemail(string emailaddress)
        {
            SmtpClient client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = "cowsepmy147@gmail.com",
                    Password= "lfmfzqcuegcgzszl"
                }
            };
            MailAddress from = new MailAddress("cowsepmy147@gmail.com", "Đỗ Ngọc Sơn");
            MailAddress to = new MailAddress(emailaddress, "someone");
            MailMessage mess = new MailMessage()
            {
                From = from,
                Subject = "chúc mừng bạn đặt hàng thành công",
                Body = "đặt thành công r th l",
            };
            mess.To.Add(to);
            client.Send(mess);
        }
    }
}
