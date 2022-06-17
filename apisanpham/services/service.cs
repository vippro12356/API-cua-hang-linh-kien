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
using System.Text.RegularExpressions;
namespace apisanpham.services
{
    public class service:connect
    {
        
        public service()
        {
            
        }
        static Regex ValidEmailRegex = CreateValidEmailRegex();
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        internal static bool EmailIsValid(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }
        public List<sanpham> listsanpham()
        {
            List<sanpham> listsp = new List<sanpham>();
            string sql = @"select * from SanPham";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int id = int.Parse(read["MaSanPham"].ToString());
                        string pro_name = read["TenSanPham"].ToString();
                        int price = int.Parse(read["GiaBan"].ToString());
                        int nsx_id = int.Parse(read["HangSanXuat"].ToString());
                        int pro_type_id = int.Parse(read["LoaiSanPham"].ToString());
                        string img = read["Image"].ToString();
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
            string sql = @"select * from HangSanXuat";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int id = int.Parse(read["MaHangSanXuat"].ToString());                       
                        string nsx_name = read["TenHangSanXuat"].ToString();
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
            string sql = @"select * from LoaiSanPham";
            string conn = connectionString;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        int pro_type_id = int.Parse(read["MaLoaiSanPham"].ToString());
                        string pro_type_name = read["TenLoaiSanPham"].ToString();
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
            sendemail(ddh.Email,ddh,iddh);
        }
        public int taodondh(Dondathang ddh)
        {
            int id_dh;
            string sql = "insert into DH output inserted.iddh values(N'" + ddh.Hoten + "',N'"+ddh.Diachi+"','"+ddh.SDT+"','"+ddh.Email+"',"+ddh.Tongtien+")";
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
            dt.Columns.Add(new DataColumn("MaSanPham", typeof(int)));
            dt.Columns.Add(new DataColumn("SoLuong", typeof(int)));
            dt.Columns.Add(new DataColumn("DonGia", typeof(int)));
            foreach (sanphamdat sp in listsp)
            {
                DataRow row = dt.NewRow();
                row["iddh"] = id_dh;
                row["MaSanPham"] = sp.id;
                row["SoLuong"] = sp.amount;
                row["DonGia"] = sp.price;
                dt.Rows.Add(row);
            }
            using(SqlBulkCopy bulk=new SqlBulkCopy(conn))
            {
                bulk.DestinationTableName = "CTDDH";
                bulk.WriteToServer(dt);
            }
        }
        public void sendemail(string emailaddress,Dondathang ddh,int id_dh)
        {
            if(emailaddress.Length==0)
            {

            }
            else
            {
                if(EmailIsValid(emailaddress))
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
                            UserName = "ngocsonmmt789@gmail.com",
                            Password = "nkstprnqpmijnccs"
                        }
                    };
                    MailAddress fromemail = new MailAddress("ngocsonmmt789@gmail.com", "Nhóm 4 Quản Lý Cửa Hàng Linh Kiện Máy Tính");
                    MailAddress toemail = new MailAddress(emailaddress, "someone");
                    MailMessage mess = new MailMessage()
                    {
                        From = fromemail,
                        Subject = "chúc mừng bạn đặt hàng thành công",
                        IsBodyHtml = true,                                   
                    };
                    mess.Body += "<h1>Xin chào:" + ddh.Hoten + "</h1>";
                    mess.Body += "<h3>Đơn hàng của bạn đã đặt thành công<h3>";
                    mess.Body += "<h3>Mã đơn hàng của bạn:" + id_dh.ToString() + "</h3>";
                    mess.Body += "<h3>Danh sách các mặt hàng đã đặt</h3>";
                    mess.Body += "<table><thead>";
                    mess.Body += "<tr><th>Mã sản phẩm</th><th>Tên sản phẩm</th><th>Số lượng</th><th>Đơn giá</th></thead>";
                    mess.Body += "<tbody>";
                    foreach (var item in ddh.Sanpham)
                    {
                        mess.Body += "<tr><td>" + item.id.ToString() + "</td>" + "<td>" + item.pro_name + "</td>" + "<td>" + item.amount.ToString() + "</td>" + "<td>" + item.price.ToString() + "Đ</td></tr>";
                    }
                    mess.Body += "</tbody></table>";
                    mess.Body += "<h4>Tổng tiền:" + ddh.Tongtien.ToString() + "Đ</h4>";
                    mess.To.Add(toemail);
                    client.Send(mess);
                }
                else
                {

                }
            }
        }
    }
}
