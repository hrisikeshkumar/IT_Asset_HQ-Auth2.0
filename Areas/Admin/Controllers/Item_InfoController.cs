using IT_Hardware.Areas.Admin.Data;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace IT_Hardware.Areas.Admin.Controllers
{
    public class Item_InfoController : Controller
    {
        private IHostingEnvironment Environment;
        public Item_InfoController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        [HttpPost]
        public JsonResult GetDetails(string serialno)
        {
            ItemInfo_BL itemInfo = new ItemInfo_BL();   
            return Json(itemInfo.Get_Item_IssueData(serialno));       
        }


        public ContentResult Download(string SerialNo, string FileType)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path= string.Empty;
            if (FileType == "PO")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/PO/");
            }
            else if(FileType == "Invoice")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/Invoice/");
            }
            else if (FileType == "Approval")
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/PO/");
            }
            

            byte[] bytes = System.IO.File.ReadAllBytes(path + SerialNo);

            //Convert File to Base64 string and send to Client.
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

            return Content(base64);

        }


    }
}
