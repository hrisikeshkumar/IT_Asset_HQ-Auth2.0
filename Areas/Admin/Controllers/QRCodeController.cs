using Humanizer;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
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



            List<CodeInfo> qrCodeInputs = new List<CodeInfo>();

            foreach (string str in SelectAsset)
            {
                CodeInfo data = new CodeInfo();
                data.URL = URL + str;
                data.Serial_No = new QRCode_BL().Get_SerialNo(str);
                qrCodeInputs.Add(data);
            }


            foreach (var input in qrCodeInputs)
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(input.URL, QRCodeGenerator.ECCLevel.Q);
                var pngQrCode = new PngByteQRCode(qrCodeData);
                byte[] qrCodeBytes = pngQrCode.GetGraphic(20);
                var base64 = Convert.ToBase64String(qrCodeBytes);
                qrCodes.Add(input.Serial_No, $"data:image/png;base64,{base64}");
            }

            return View(qrCodes);

        }

        public IActionResult Asset_Info_Histroy(string id)
        {

            QRCode_BL DLayer = new QRCode_BL();
            AssetInfo_Model model = DLayer.Asset_Detail_Info(id);

            return View(model);
        }

        public IActionResult RaiseIssue(string assetid, string type)
        {
            QRCode_BL DLayer = new QRCode_BL();
            RaiseIssue_Mod mod ;
            if (type == "New")
            {
                ViewBag.ActionName = "RaiseIssue_Insert";
                mod = DLayer.Get_Asset_Service_Info(assetid, type);
            }         
            else
            {
                ViewBag.ActionName = "RaiseIssue_Update";
                mod = DLayer.Get_Asset_Service_Info(assetid, type);
            }

            return View(mod);
        }

        [HttpPost]
        public IActionResult RaiseIssue_Insert(RaiseIssue_Mod data)
        {
            QRCode_BL DLayer = new QRCode_BL();
            data.UserId = HttpContext.User.Identity.Name;
            int status = DLayer.InsUpd_AssetService(data, "Insert_AssetService");

            return RedirectToAction("Asset_Info_Histroy", new { id = data.AssetId });
        }

        [HttpPost]
        public IActionResult RaiseIssue_Update(RaiseIssue_Mod data)
        {
            QRCode_BL DLayer = new QRCode_BL();
            data.UserId = HttpContext.User.Identity.Name;
            int status= DLayer.InsUpd_AssetService(data, "Update_AssetService");

            return RedirectToAction("Asset_Info_Histroy", new { id = data.AssetId });
        }

        [HttpGet]
        public IActionResult Asset_Issue_Histroy(String AssetId)
        {
            QRCode_BL DLayer = new QRCode_BL();
            RaiseIssue_Mod mod = DLayer.Get_Asset_Service_Info(AssetId, "Get_Asset_ServiceIssue");

            return View(mod);
        }

        [HttpGet]
        public IActionResult All_Asset_Issue_Histroy(String Type)
        {
            QRCode_BL DLayer = new QRCode_BL();
            //RaiseIssue_Mod mod = DLayer.Get_Asset_Service_Info(AssetId);

            return View();
        }
    }
}
