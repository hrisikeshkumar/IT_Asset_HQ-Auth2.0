using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

namespace IT_Hardware.Areas.Admin.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult AssetsInfo()
        {
            QRCode_BL DLayer = new QRCode_BL();

            List<QRCode_Model> model = DLayer.AssetsList();

            return View(model);
        }

        public IActionResult CodeGeneration()
        {
            return View();
        }

        public IActionResult Asset_Detail_Information()
        {
            return View();
        }

        public IActionResult RaiseIssue()
        {
            return View();
        }

    }
}
