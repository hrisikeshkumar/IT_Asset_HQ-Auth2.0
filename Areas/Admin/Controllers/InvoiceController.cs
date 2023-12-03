using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;

namespace IT_Hardware.Areas.Admin.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Admin/Invoice
        public ActionResult Index()
        {
            return View();
        }
    }
}