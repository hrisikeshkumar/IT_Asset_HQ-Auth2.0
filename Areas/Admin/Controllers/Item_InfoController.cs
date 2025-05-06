using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace IT_Hardware.Areas.Admin.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.ITStaffs)]
    [Area("Admin")]
    public class Item_InfoController : Controller
    {
        private IHostingEnvironment Environment;
        public Item_InfoController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

       
        public JsonResult GetDetails(string serialno)
        {
            ItemInfo_BL itemInfo = new ItemInfo_BL();
            ItemInfo_Mod mod = new ItemInfo_Mod();
            mod.Serial_No = serialno;
            return Json(itemInfo.Get_Item_IssueData(mod));       
        }


        public ContentResult Download(string SerialNo, string FileType)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path= string.Empty;
            string FileName = string.Empty;

            ItemInfo_BL itemInfo = new ItemInfo_BL();
            ItemdetailInfo_Mod data = new ItemdetailInfo_Mod();
            data.Serial_No = SerialNo;
            itemInfo.Get_Item_IssueData(data);


            if (FileType == "PO")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/PO/");
                FileName= data.PO_Info_FileId;
            }
            else if(FileType == "Invoice")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/Invoice/");
                FileName = data.Invoice_FileId;
            }
            else if (FileType == "Approval")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/FileMovement/");
                FileName = data.Approval_Info_FileId;
            }
            

            byte[] bytes = System.IO.File.ReadAllBytes(path + data.Invoice_FileId);

            //Convert File to Base64 string and send to Client.
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

            return Content(base64);

        }


    }
}
