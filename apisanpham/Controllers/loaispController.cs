using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apisanpham.services;
namespace apisanpham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class loaispController : Controller
    {
        service sv = new service();
        [HttpGet]
        public JsonResult getall()
        {
            return new JsonResult(sv.listloaisp());
        }
        [HttpGet("type/{name}")]
        public JsonResult getbyname(string name)
        {
            var result = from sanpham in sv.listsanpham()
                         join loaisp in sv.listloaisp() on sanpham.pro_type equals loaisp.pro_type_id
                         where loaisp.pro_type_name == name
                         select new {
                             id = sanpham.id,
                             pro_name = sanpham.pro_name,
                             pro_type = sanpham.pro_type,
                             img = sanpham.img,
                             nsx_id = sanpham.nsx_id,
                             price = sanpham.price,
                             amount = 1
                         };
            return new JsonResult(result);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
