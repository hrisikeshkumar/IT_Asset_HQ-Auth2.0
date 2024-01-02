using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using DocumentFormat.OpenXml.EMMA;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy = AuthorizationPolicies.ITStaff)]
    public class Purchase_OrderController : Controller
    {

        
        [HttpGet]
        public ActionResult PO_Details(string Message)
        {
            BL_Porder objPO = new BL_Porder();

            List<Mod_POrder> pc_List = objPO.Get_All_PO_Data();

            return View("~/Areas/Admin/Views/Purchase_Order/PO_Details.cshtml", pc_List);
        }


        
        public ActionResult PO_Create_Item()
        {

            Mod_POrder mod_PO = new Mod_POrder();
            BL_Porder com = new BL_Porder();

            mod_PO.Vendor_List = com.Vendor_List();

            return View("~/Areas/Admin/Views/Purchase_Order/PO_Create.cshtml", mod_PO);

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
                            

                            //for (int i = 0; i < Request.Files.Count; i++)
                            //{
                            //    //HttpPostedFile httpPostedFile = Request.Files;

                            //    var postedFile = Request.Files[i];

                            //    if (postedFile != null)
                            //    {

                            //        byte[] bytes;
                            //        using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                            //        {
                            //            bytes = br.ReadBytes(postedFile.ContentLength);
                            //        }
                            //        
                            //        using (SqlConnection con = new DBConnection().con)
                            //        {
                            //            string query = "INSERT INTO File_table(File_Id,  File_Table, File_Ref_Id, File_Name, ContentType, File_Data) VALUES ( dbo.Get_Unique_File_Id(), 'Vendor', @Ref_Id,   @Name, @ContentType, @Data)";
                            //            using (SqlCommand cmd = new SqlCommand(query))
                            //            {
                            //                cmd.Connection = con;
                            //                cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                            //                cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                            //                cmd.Parameters.AddWithValue("@Data", bytes);
                            //                cmd.Parameters.AddWithValue("@Ref_Id", Vendor_Id);
                            //                con.Open();
                            //                cmd.ExecuteNonQuery();
                            //                con.Close();
                            //            }
                            //        }


                            //    }
                            //}


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


       
        public ActionResult Edit_PO(string id)
        {
            BL_Vendor Md_Com = new BL_Vendor();
            Mod_Vendor data = Md_Com.Get_Data_By_ID(id);

            //data.File_List = GetFiles_By_Id(id);


            return View("~/Areas/Admin/Views/Vendor/Edit_Vendor.cshtml", data);
        }


        
        [HttpPost]
        public ActionResult Update_PO(Mod_Vendor Get_Data)
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




    }
}