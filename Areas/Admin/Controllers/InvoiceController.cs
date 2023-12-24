using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{
    [Authorize(Policy =AuthorizationPolicies.ITStaff)]
    public class InvoiceController : Controller
    {
        [HttpGet]
        public ActionResult Invoice_Details(string Message)
        {
            BL_POrder objPO = new BL_POrder();

            List<Mod_POrder> pc_List = objPO.Get_All_PO_Data();

            return View("~/Areas/Admin/Views/Purchase_Order/PO_Details.cshtml", pc_List);
        }

        public ActionResult Invoice_Create_Item()
        {

            Mod_POrder mod_PO = new Mod_POrder();
            BL_POrder com = new BL_POrder();

            mod_PO.Vendor_List = com.Vendor_List();

            return View("~/Areas/Admin/Views/Purchase_Order/PO_Create.cshtml", mod_PO);

        }


        [HttpPost]
        public ActionResult Invoice_Create_Post(Mod_POrder PO_Data)
        {
            string Message = "";
            try
            {
                PO_Data.Create_usr_id = HttpContext.User.Identity.Name;
                string Vendor_Id = string.Empty;
                if (ModelState.IsValid)
                {

                    if (PO_Data.File_PO.Length > 0)
                    {

                    }

                    if (PO_Data.File_PO.Length > 0)
                    {

                    }


                    BL_POrder save_data = new BL_POrder();
                    int status = save_data.Save_PO_data(PO_Data, "Add_new", "", out string PO_Id, out string PO_File_Name, out string SLA_File_Name);

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data save successfully");



                        if (PO_Data.File_PO.Length > 0)
                        {
                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                            //create folder if not exist
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            //get file extension
                            FileInfo fileInfo = new FileInfo(PO_Data.File_PO.FileName);
                            string fileName = PO_Data.File_PO.FileName + fileInfo.Extension;

                            string fileNameWithPath = Path.Combine(path, fileName);

                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                PO_Data.File_PO.CopyTo(stream);
                            }



                        }

                        if (PO_Data.File_SLA.Length > 0)
                        {

                            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

                            //create folder if not exist
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);

                            //get file extension
                            FileInfo fileInfo = new FileInfo(PO_Data.File_SLA.FileName);
                            string fileName = PO_Data.File_SLA.FileName + fileInfo.Extension;

                            string fileNameWithPath = Path.Combine(path, fileName);

                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                PO_Data.File_SLA.CopyTo(stream);
                            }

                        }
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

            return RedirectToAction("Vendor_Create_Item", "Vendor");
        }



        public ActionResult Edit_Invoice(string id)
        {
            BL_Vendor Md_Com = new BL_Vendor();
            Mod_Vendor data = Md_Com.Get_Data_By_ID(id);

            //data.File_List = GetFiles_By_Id(id);


            return View("~/Areas/Admin/Views/Vendor/Edit_Vendor.cshtml", data);
        }



        [HttpPost]
        public ActionResult Update_Invoice(Mod_Vendor Get_Data)
        {
            int status = 0;
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    BL_Vendor Md_Asset = new BL_Vendor();

                    string out_param = string.Empty;

                    status = Md_Asset.Save_Vendor_data(Get_Data, "Update", Get_Data.Vendor_id, out out_param);

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data have saved successfully");
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

            return RedirectToAction("Vendor_Details", "Vendor");
        }


        public ActionResult Delete_Invoice(Mod_Vendor Get_Data, string id)
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



    }
}