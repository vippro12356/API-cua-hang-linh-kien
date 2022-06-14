using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apisanpham.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using apisanpham.services;
namespace apisanpham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class sanphamController : Controller
    {
        service sv = new service();
        public sanphamController()
        {
            
        }
        public List<sanpham> listsanpham()
        {
            
            return sv.listsanpham();
        }
        [HttpGet]
        public JsonResult getall()
        {
            var result = from sanpham in listsanpham()
                         select new 
                         {
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
        
        [HttpGet("search/{name}")]
        public JsonResult findproduct(string name)
        {
            var result = listsanpham().Where(t => t.pro_name.Contains(name,StringComparison.OrdinalIgnoreCase)).ToList();
            var list = from x in result
                       select new
                       {
                           id = x.id,
                           pro_name = x.pro_name,
                           pro_type = x.pro_type,
                           img = x.img,
                           nsx_id = x.nsx_id,
                           price = x.price,
                           amount = 1
                       };
            return new JsonResult(list);
        }
        [HttpGet("{name}")]
        public JsonResult getbyname(string name)
        {
            var sp = from x in listsanpham()
                     where x.pro_name == name
                     select new
                     {
                         id = x.id,
                         pro_name = x.pro_name,
                         pro_type = x.pro_type,
                         img = x.img,
                         nsx_id = x.nsx_id,
                         price = x.price,
                         amount = 1
                     };
            return new JsonResult(sp);
        }
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}
