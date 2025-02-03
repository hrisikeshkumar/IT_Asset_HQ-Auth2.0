using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using DocumentFormat.OpenXml.EMMA;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy = AuthorizationPolicies.ITStaffs)]
    [Area("Admin")]
    public class Purchase_OrderController : Controller
    {

        
        [HttpGet]
        public ActionResult PO_Details(string Message)
        {
            BL_Porder objPO = new BL_Porder();
            List<Mod_POrder> pc_List = objPO.Get_All_PO_Data();

            return View( pc_List);
        }

        
        public ActionResult PO_Create_Item()
        {
            Mod_POrder mod_PO = new Mod_POrder();
            BL_Porder com = new BL_Porder();
            mod_PO.Vendor_List = com.Vendor_List();

            return View( mod_PO);
        }


        [HttpPost]
        public ActionResult PO_Create_Post(Mod_POrder PO_Data)
        {
            string Message = "";
            try
            {
                PO_Data.Create_usr_id = HttpContext.User.Identity.Name;
                string Vendor_Id = string.Empty;
                if (ModelState.IsValid)
                {

                    BL_Porder save_data = new BL_Porder();

                    int status = 0;

                    if (PO_Data.File_PO.Length > 0)
                    {
                        PO_Data.PO_File_Name = "Y";
                        status = save_data.Save_PO_data(PO_Data, "Add_new", "", out string PO_Id, out string PO_File_Name);

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/HQ/PO/");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        //get file extension
                        FileInfo fileInfo = new FileInfo(PO_Data.File_PO.FileName);
                        string fileName = PO_File_Name + fileInfo.Extension;

                        string fileNameWithPath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            PO_Data.File_PO.CopyTo(stream);
                        }
                    }
                    else 
                    {
                        PO_Data.PO_File_Name = "N";
                        status = save_data.Save_PO_data(PO_Data, "Add_new", "", out string PO_Id, out string PO_File_Name);
                    }

                    if (status>0)
                    {
                        TempData["Message"] = String.Format("Data save successfully");
                    }
                    else
                    {

                        TempData["Message"] = String.Format("Data is not saved");
                    }
                }
                else
                {
                    TempData["Message"] = String.Format("Required Data are not Provided");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("PO_Create_Item", "Purchase_Order");
        }


        public ActionResult Edit_PO(string id)
        {
            
            BL_Porder data = new BL_Porder();
            Mod_POrder mod_PO = data.Get_Data_By_ID( id);
            mod_PO.Vendor_List = data.Vendor_List();

            return View( mod_PO);
        }


        [HttpPost]
        public JsonResult FiliUpload(string SLA_Id)
        {


            string SLA_Id1 = SLA_Id.ToString();
            IFormFile postedFile = Request.Form.Files[0];
            if (postedFile.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/PO");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(postedFile.FileName);
                string fileName = SLA_Id + fileInfo.Extension;

                string fileNameWithPath = Path.Combine(path, fileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
            }

            return Json("");
        }


        public ActionResult Delete_PO(Mod_Vendor Get_Data, string id)
        {
            int status = 0;
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {


                    BL_Vendor Md_Asset = new BL_Vendor();

                    string out_param = string.Empty;

                    status = Md_Asset.Save_Vendor_data(Get_Data, "Delete", id, out out_param);

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data saved successfully");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("Vendor_Details", "Vendor");
        }


        public FileResult Download(string fileId)
        {

            //Build the File Path.
            string path = Path.Combine("wwwroot/Files/PO/") + fileId;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileId);

        }

    }
}