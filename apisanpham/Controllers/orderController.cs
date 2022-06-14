using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apisanpham.services;
using apisanpham.Models;
namespace apisanpham.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class orderController : Controller
    {
        service sv = new service();
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult create(Dondathang ddh)
        {
            sv.dathang(ddh);           
            return new JsonResult("Tạo thành công");
        }
    }
}
