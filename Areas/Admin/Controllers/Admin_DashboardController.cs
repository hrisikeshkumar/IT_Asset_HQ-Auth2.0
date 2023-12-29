using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Hardware.Areas.Admin.Data;
using IT_Hardware.Areas.Admin.Models;
using System.Data.SqlClient;
using IT_Hardware.Infra;

namespace IT_Hardware.Areas.Admin.Controllers
{

    [Authorize(Policy =   AuthorizationPolicies.ITHardwareManager )]
    public class Admin_DashboardController : Controller
    {

        public ActionResult Admin_Dashboard()
        {
            
            BL_Admin_DashB B_Layer = new BL_Admin_DashB();

            Mod_Admin_dashB mod_Data = new Mod_Admin_dashB();

            mod_Data.List_Proposal = B_Layer.Get_List_Proposal();

            mod_Data.List_Bill_Process = B_Layer.Get_List_Bills();

            return View("~/Areas/Admin/Views/Admin_Dashboard/Admin_Dashboard.cshtml", mod_Data);

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

            return View("~/Areas/Admin/Views/Admin_Dashboard/Edit_Proposal.cshtml", mod_Data.Prop_detail);

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



        private static List<File_List> GetFiles_By_Id(string SLA_Id)
        {
            List<File_List> files = new List<File_List>();
            
            using (SqlConnection con = new DBConnection().con)
            {
                using (SqlCommand cmd = new SqlCommand("SELECT File_Id, File_Name FROM File_table where  LTRIM(RTRIM(File_table))= 'Proposal' and  LTRIM(RTRIM(File_Ref_Id))=LTRIM(RTRIM('" + SLA_Id + "')) "))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new File_List
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



        [HttpPost]
        public JsonResult FiliUpload(IFormFile postedFile)
        {

            string SLA_Id = Request.Form["SLA_Id"].ToString();

            using (MemoryStream ms = new MemoryStream())
            {
                postedFile.CopyTo(ms);

                using (SqlConnection con = new DBConnection().con)
                {
                    string query = "INSERT INTO tblFiles VALUES (@Name, @ContentType, @Data)";
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
            }



            return Json(GetFiles_By_Id(SLA_Id));

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

    }
}