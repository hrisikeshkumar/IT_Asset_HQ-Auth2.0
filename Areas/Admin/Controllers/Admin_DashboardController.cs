using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using System.Data.SqlClient;
using IT_Hardware.Infra;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy =   AuthorizationPolicies.AllAccess)]
    [Area("Admin")]
    public class Admin_DashboardController : Controller
    {

        private IHostingEnvironment Environment;

        public Admin_DashboardController(IHostingEnvironment _environment)
        {
            Environment = _environment;
        }

        public ActionResult Admin_Dashboard()
        {
            
            BL_Admin_DashB B_Layer = new BL_Admin_DashB();

            Mod_Admin_dashB mod_Data = new Mod_Admin_dashB();

            mod_Data.List_Proposal = B_Layer.Get_List_Proposal();

            mod_Data.List_Bill_Process = B_Layer.Get_List_Bills();

           

            //return View("~/Areas/Admin/Views/Admin_Dashboard/Admin_Dashboard.cshtml", mod_Data);

            return View(mod_Data);
        }


        
        public JsonResult Get_Proposal_Detail_for_Modal( string Proposal_Id)
        {
            BL_Admin_DashB B_Layer = new BL_Admin_DashB();

            Proposal_details detail_Data = new Proposal_details();

            detail_Data.Prop_Files = GetFiles_By_Id(Proposal_Id);

            Mod_Admin_dashB mod_Data = new Mod_Admin_dashB();

            mod_Data.Prop_detail = detail_Data;

            B_Layer.Get_Proposal_By_Id( mod_Data, Proposal_Id);

            return Json(mod_Data);
        }


        
        //-----------------------------------         Proposal Details       --------------------------------------------------

        public ActionResult Edit_proposal(Mod_Admin_dashB Proposal)
        {

            BL_Admin_DashB B_Layer = new BL_Admin_DashB();

            Proposal_details detail_Data = new Proposal_details();

            detail_Data.Prop_Files = GetFiles_By_Id(Proposal.Prop_detail.Proposal_Id);

            Mod_Admin_dashB mod_Data = new Mod_Admin_dashB();
            mod_Data.Prop_detail = detail_Data;
            B_Layer.Get_Proposal_By_Id(mod_Data, Proposal.Prop_detail.Proposal_Id);

            return View(mod_Data.Prop_detail);

        }

        
        public ActionResult Update_proposal(Proposal_details Proposal_data)
        {

            int status = 0;
            try
            {
               
                if (ModelState.IsValid)
                {
                    BL_Admin_DashB B_Layer = new BL_Admin_DashB();


                    Proposal_data.Update_UserId = HttpContext.User.Identity.Name;

                    status = B_Layer.Update_proposal(Proposal_data);

                    if (status > 0)
                    {
                        TempData["Message"] = String.Format("Data have saved successfully");
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

            return RedirectToAction("Admin_Dashboard", "Admin_Dashboard");
        }


        private static List<File_List> GetFiles_By_Id(string Proposal_Id)
        {
            List<File_List> files = new List<File_List>();

            try
            {
                using (SqlConnection con = new DBConnection().con)
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT  Proposal_File_ID, Proposal_Id, FileName from Proposal_Files where  LTRIM(RTRIM(Proposal_Id))=LTRIM(RTRIM('" + Proposal_Id + "')) "))
                    {
                        cmd.Connection = con;
                        con.Open();
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                files.Add(new File_List
                                {
                                    File_Id = Convert.ToString(sdr["Proposal_File_ID"]),
                                    File_Name = Convert.ToString(sdr["FileName"])
                                });
                            }
                        }
                        con.Close();
                    }
                }

            }
            catch (Exception ex) { }

            return files;
        }


        [HttpPost]
        public JsonResult FiliUpload( IFormFile postedFile)
        {
            string Proposal_Id = Request.Form["Proposal_Id"].ToString();
            string ext = System.IO.Path.GetExtension(postedFile.FileName);
            if (ext != ".pdf") 
            {
                return Json(new SelectListItem("Duplicate", "Accept Pdf Files only"));
            }
            
            try
            {              
                if (postedFile != null)
                {
                    int FileId=0;
                   
                    using (SqlConnection con = new DBConnection().con)
                    {
                        string query = " Select Proposal_File_ID, Proposal_Id, FileName from Proposal_Files where LTRIM(RTRIM(FileName))= LTRIM(RTRIM(@FileName)";
                        using (SqlCommand cmd = new SqlCommand(query))
                        {
                            cmd.Connection = con;
                            cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(postedFile.FileName));
                            con.Open();

                            SqlDataReader sdr = cmd.ExecuteReader();

                            //Looping through each record
                            while (sdr.Read())
                            {
                                return Json(new SelectListItem("Duplicate", "Please Rename the File and Upload"));
                            }

                            query = " insert into Proposal_Files (Proposal_Id, FileName, Updated_By , Update_Time) " +
                                          " values (@ProposalID, @FileName, @UserId, @Datetime ) ";

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@ProposalID", Proposal_Id);
                            cmd.Parameters.AddWithValue("@FileName", Path.GetFileName(postedFile.FileName));
                            cmd.Parameters.AddWithValue("@UserId", HttpContext.User.Identity.Name.ToString());
                            cmd.Parameters.AddWithValue("@Datetime", DateTime.Now);

                            FileId = Convert.ToInt32(cmd.ExecuteScalar());

                            con.Close();

                        }
                    }

                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "Files\\FileMovement\\");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    using (FileStream stream = new FileStream(Path.Combine(path,  FileId.ToString()+ ext), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }


                }

            }
            catch (Exception ex) { }

            /* Save File to DateBase 
            using (MemoryStream ms = new MemoryStream())
            {
                postedFile.CopyTo(ms);

                using (SqlConnection con = new DBConnection().con)
                {
                    string query = "INSERT INTO   VALUES (@Name, @ContentType, @Data)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                        cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                        cmd.Parameters.AddWithValue("@Data", ms.ToArray());
                        cmd.Parameters.AddWithValue("@Ref_Id", SLA_Id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }*/

            return Json(GetFiles_By_Id(Proposal_Id));
        }


        public JsonResult DeleteFile(string FileId, string RefId)
        {
            try
            {
                /*   Delete File from Database       */
                using (SqlConnection con = new DBConnection().con)
                {
                    string query = "delete from Proposal_Files where LTRIM(RTRIM(Proposal_File_ID)) =LTRIM(RTRIM(@File_Id))";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@File_Id", FileId);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                string file_name = FileId + ".pdf" ;
                string path = Path.Combine(this.Environment.WebRootPath, "Files\\FileMovement\\", file_name);
                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
              
            }catch (Exception ex) { }   

            return Json(GetFiles_By_Id(RefId));
        }


        public ContentResult Download(string fileId)
        {

            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;

            string path = Path.Combine(this.Environment.WebRootPath, "Files\\ITDeptFile\\");

            //Read the File as Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path + fileId);

            //Convert File to Base64 string and send to Client.
            string base64 = Convert.ToBase64String(bytes, 0, bytes.Length);

            return Content(base64);

            /* Download File from Database
            //byte[] bytes;
            //string fileName, contentType;

            //using (SqlConnection con = new DBConnection().con)
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "select File_Id, File_table, File_Name, ContentType, File_Data from File_table WHERE LTRIM(RTRIM(File_Id))= LTRIM(RTRIM(@Id))";
            //        cmd.Parameters.AddWithValue("@Id", fileId);
            //        cmd.Connection = con;
            //        con.Open();
            //        using (SqlDataReader sdr = cmd.ExecuteReader())
            //        {
            //            sdr.Read();
            //            bytes = (byte[])sdr["File_Data"];
            //            contentType = sdr["ContentType"].ToString();
            //            fileName = sdr["File_Name"].ToString();
            //        }
            //        con.Close();
            //    }
            //}

            return File(bytes, contentType, fileName);*/
        }

        /* Own Authentication
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
        //} */

    }
}