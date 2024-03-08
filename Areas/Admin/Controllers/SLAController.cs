using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using System.Data.SqlClient;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{
    [Authorize(Policy = AuthorizationPolicies.ITStaff)]
    public class SLAController : Controller
    {

        //private IHostingEnvironment Environment;

        //public SLAController(IHostingEnvironment _environment)
        //{
        //    Environment = _environment;
        //}



        public ActionResult SLA_Details()
        {
            BL_SLA com = new BL_SLA();

            List<Mod_SLA> SLA_List = com.Get_SLAData();

            return View("~/Areas/Admin/Views/SLA/SLA_Details.cshtml", SLA_List);
        }



        [HttpGet]
        public ActionResult SLA_Create_Item(string Message)
        {
            ViewBag.Message = Message;

            BL_SLA BL_data = new BL_SLA();

            Mod_SLA Mod_data = new Mod_SLA();
            
            Mod_data.Vendor_List = BL_data.Vendor_List();


            return View("~/Areas/Admin/Views/SLA/SLA_Create_Item.cshtml", Mod_data);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SLA_CreateItem_Post(Mod_SLA Get_Data)
        {

            string Message = "";
            try
            {
                
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;      

                string SLA_Id = string.Empty;
                string SLA_FileName = string.Empty;

                if (ModelState.IsValid)
                {
                    BL_SLA save_data = new BL_SLA();
                    int status=0;

                    if (Get_Data.All_Files.Length > 0)
                    {
                        FileInfo fileInfo = new FileInfo(Get_Data.All_Files.FileName);
                        Get_Data.SLA_File_Name= fileInfo.Extension;
                        status = save_data.Save_SLA_data(Get_Data, "Add_new", "", out SLA_Id, out SLA_FileName);

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/SLA");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        //get file extension
                        
                        string fileName = SLA_FileName + fileInfo.Extension;

                        string fileNameWithPath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            Get_Data.All_Files.CopyTo(stream);
                        }
                    }
                    else
                    {
                        status = save_data.Save_SLA_data(Get_Data, "Add_new", "", out SLA_Id, out SLA_FileName);
                    }

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data saved successfully");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }

                }
                else
                {
                    TempData["Message"] = String.Format("Required data are not provided");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("SLA_Create_Item", "SLA");
        }


        public ActionResult Edit_SLA(string id)
        {
            BL_SLA BL_data = new BL_SLA();

            Mod_SLA Mod_data = new Mod_SLA();
            BL_data.Get_Data_By_ID(Mod_data, id);

            Mod_data.Vendor_List = BL_data.Vendor_List();

            return View("~/Areas/Admin/Views/SLA/Edit_SLA.cshtml", Mod_data);
        }


        public ActionResult Update_SLA(Mod_SLA Get_Data, string Item_id)
        {
            int status = 0;
            string SLA_Id_I = string.Empty;
            string SLA_FileName = string.Empty;
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    BL_SLA Md_Asset = new BL_SLA();

                    status = Md_Asset.Save_SLA_data(Get_Data, "Update", Item_id,  out SLA_Id_I, out SLA_FileName );

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data saved successfully");
                    }
                    else
                    {
                        TempData["Message"] = String.Format("Data is not saved");
                    }
                }
                else
                {
                    TempData["Message"] = String.Format("Required data are not provided");
                }
            }
            catch (Exception ex)
            {

                TempData["Message"] = string.Format("ShowFailure();");

            }

            return RedirectToAction("SLA_Details", "SLA");
        }


        public ActionResult Delete_SLA(Mod_SLA Get_Data, string id)
        {
           
            try
            {
                Get_Data.Create_usr_id = HttpContext.User.Identity.Name;
                if (ModelState.IsValid)
                {
                    string SLA_Id = string.Empty;
                    string SLA_FileName = string.Empty;

                    BL_SLA Md_Asset = new BL_SLA();

                    int status = Md_Asset.Save_SLA_data(Get_Data, "Delete", id, out SLA_Id, out SLA_FileName);

                    if (status>0)
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

            return RedirectToAction("SLA_Details", "SLA");
        }



        //protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        //{
        //    if (filterContext.HttpContext.Request.IsAuthenticated)
        //    {

        //        if (filterContext.Result is HttpUnauthorizedResult)
        //        {
        //            filterContext.Result = new RedirectResult("~/Authorization/AccessDedied");
        //        }
        //    }
        //    else
        //    {
        //        filterContext.Result = new RedirectResult("~/Log_In/Log_In");
        //    }
        //}




        //-----------------------------------  File Function ---------------------------------------------



        [HttpPost]
        public JsonResult FiliUpload(string SLA_Id)
        {

          
            string SLA_Id1 = SLA_Id.ToString();
            IFormFile postedFile = Request.Form.Files[0];
            if (postedFile.Length > 0)
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files/SLA");

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

            return Json(GetFiles_By_Id(""));

        }



        //private static List<FileModel> GetFiles()
        //{
        //    List<FileModel> files = new List<FileModel>();
        //    
        //    using (SqlConnection con = new DBConnection().con)
        //    {
        //        using (SqlCommand cmd = new SqlCommand("SELECT File_Id, File_Name FROM File_table"))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            using (SqlDataReader sdr = cmd.ExecuteReader())
        //            {
        //                while (sdr.Read())
        //                {
        //                    files.Add(new FileModel
        //                    {
        //                        File_Id = Convert.ToString(sdr["File_Id"]),
        //                        File_Name = Convert.ToString(sdr["File_Name"])
        //                    }) ;
        //                }
        //            }
        //            con.Close();
        //        }
        //    }
        //    return files;
        //}



        private static List<FileModel> GetFiles_By_Id(string SLA_Id)
        {
            List<FileModel> files = new List<FileModel>();
            
            using (SqlConnection con = new DBConnection().con)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT File_Id, File_Name FROM File_table where  LTRIM(RTRIM(File_table))= 'SLA' and  LTRIM(RTRIM(File_Ref_Id))=LTRIM(RTRIM('" + SLA_Id + "')) "))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new FileModel
                            {
                                File_Id = Convert.ToString(sdr["File_Id"]),
                                File_Name = Convert.ToString(sdr["File_Name"])
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }



        public JsonResult DeleteFile(string FileId, string RefId)
        {

            
            using (SqlConnection con = new DBConnection().con)
            {
                string query = "delete from File_table where LTRIM(RTRIM(File_Id)) =LTRIM(RTRIM(@File_Id))";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@File_Id", FileId);               
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }


            return Json(GetFiles_By_Id(RefId));

        }



        public FileResult Download(string fileId)
        {

            //Build the File Path.
            string path = Path.Combine("wwwroot/Files/SLA/") + fileId;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileId);

            /*  Download files which was save on the database
            byte[] bytes;
            string fileName, contentType;
            
            using (SqlConnection con = new DBConnection().con)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select File_Id, File_table, File_Name, ContentType, File_Data from File_table WHERE LTRIM(RTRIM(File_Id))= LTRIM(RTRIM(@Id))";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["File_Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["File_Name"].ToString();
                    }
                    con.Close();
                }
            }
            return File(bytes, contentType, fileName);
            */

        }

    }
}