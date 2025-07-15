using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Infra;
using Microsoft.AspNetCore.Authorization;
using QRCoder;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace IT_Hardware.Areas.Admin.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.ITSupportEngineers)]
    [Area("Admin")]
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

            var qrCodes = new Dictionary<string, string>();
            var qrGenerator = new QRCodeGenerator();

            List<string> qrCodeInputs = new List<string>
            {
                "https://example.com/asset1",
                "https://example.com/asset2",
                "https://example.com/asset3"
            };

            foreach (var input in qrCodeInputs)
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);
                var pngQrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = pngQrCode.GetGraphic(20);
                var base64 = Convert.ToBase64String(qrCodeBytes);
                qrCodes.Add(input, $"data:image/png;base64,{base64}");
            }

            return View(qrCodes);

        }

        public IActionResult Asset_Info_Histroy(string Id)
        {

            QRCode_BL DLayer = new QRCode_BL();
            AssetInfo_Model model = DLayer.Asset_Detail_Info(Id);

            return View(model);
        }

        public IActionResult RaiseIssue()
        {
            return View();
        }

    }
}
