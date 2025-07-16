using Humanizer;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        public IActionResult CodeGeneration(List<string> SelectAsset)
        {

            var qrCodes = new Dictionary<string, string>();
            var qrGenerator = new QRCodeGenerator();

            var configuation = new ConfigurationDoc().GetConfiguration();
            string URL= configuation.GetSection("QRCodeURL").Value;



            List<string> qrCodeInputs = new List<string>();

            foreach (string str in SelectAsset)
            {
                qrCodeInputs.Add(URL + str);
            }


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

        public IActionResult Asset_Info_Histroy(string id)
        {

            QRCode_BL DLayer = new QRCode_BL();
            AssetInfo_Model model = DLayer.Asset_Detail_Info(id);

            return View(model);
        }

        public IActionResult RaiseIssue()
        {
            return View();
        }

    }
}
