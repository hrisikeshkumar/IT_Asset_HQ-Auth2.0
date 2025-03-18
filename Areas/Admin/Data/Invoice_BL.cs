using Microsoft.AspNetCore.Mvc.Rendering;
using IT_Hardware.Areas.Admin.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace IT_Hardware.Areas.Admin.Data
{
    public class Invoice_BL
    {
        public List<Invoice_Mod> Get_All_Invoice(string UserId)
        {

            Invoice_Mod BL_data;
            List<Invoice_Mod> current_data = new List<Invoice_Mod>();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_Invoice"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_List");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.Add(sqlP_type);

                    SqlParameter sqlP_User = new SqlParameter("@Create_Usr_Id", UserId);
                    cmd.Parameters.Add(sqlP_User);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_Comuter = dt;
                        }
                    }
                }

                foreach (DataRow dr in dt_Comuter.Rows)
                {
                    BL_data = new Invoice_Mod();

                    BL_data.Invoice_id = Convert.ToString(dr["ID"]);
                    BL_data.Invoice_No = Convert.ToString(dr["Inv_No"]);
                    BL_data.Invoice_Subject = Convert.ToString(dr["Subject"]);
                    BL_data.Invoice_Date =  Convert.ToDateTime(dr["Inv_date"]);
                    BL_data.Invoice_Value = Convert.ToInt32(dr["Inv_Value"]);
                    BL_data.Penalty_Amount = Convert.ToInt32(dr["Penalty_Amt"]);
                    BL_data.Penalty_Reason = Convert.ToString(dr["Penalty_Reason"]);

                    current_data.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return current_data;
        }

        public int Save_data(Invoice_Mod Data, string type, string AppFile_Extension, string InvFile_Extension, 
                               out string Inv_ID_Update, out string Inv_File_Name , out string Approval_FileName)
        {

            int status = -1;
            Inv_ID_Update = string.Empty;
            Inv_File_Name = string.Empty;
            Approval_FileName = string.Empty;

            SqlConnection con = new DBConnection().con;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_Invoice";

                cmd.Connection = con;

                SqlParameter sqlP_type = new SqlParameter("@Type", type);
                cmd.Parameters.Add(sqlP_type);

                if (type == "Update" || type == "Delete")
                {
                    SqlParameter Invoice_Id = new SqlParameter("@Inv_id", Data.Invoice_id);
                    cmd.Parameters.Add(Invoice_Id);
                }

                SqlParameter Invoice_No = new SqlParameter("@Inv_no", Data.Invoice_No);
                cmd.Parameters.Add(Invoice_No);

                SqlParameter Invoice_Subject = new SqlParameter("@Subject", Data.Invoice_Subject);
                cmd.Parameters.Add(Invoice_Subject);

                SqlParameter PO_Id = new SqlParameter("@SO_Id", Data.SanctionOrder_Id);
                cmd.Parameters.Add(PO_Id);

                SqlParameter Invoice_Value = new SqlParameter("@Inv_Value", Data.Invoice_Value);
                cmd.Parameters.Add(Invoice_Value);

                SqlParameter Penalty_Amount = new SqlParameter("@Penalty_Amt", Data.Penalty_Amount);
                cmd.Parameters.Add(Penalty_Amount);  

                SqlParameter Penalty_Reason = new SqlParameter("@Penalty_Reason", Data.Penalty_Reason);
                cmd.Parameters.Add(Penalty_Reason);

                SqlParameter Invoice_Date = new SqlParameter("@Inv_date", Data.Invoice_Date);
                cmd.Parameters.Add(Invoice_Date);


                if (Data.File_Invoice != null)
                {
                    SqlParameter InvFile_Exit = new SqlParameter("@Inv_FileExist", 1);
                    cmd.Parameters.Add(InvFile_Exit);
                }

                SqlParameter InvFile_Ext = new SqlParameter("@Inv_File_Ext", InvFile_Extension);
                cmd.Parameters.Add(InvFile_Ext);

                if (Data.File_CommitteeApproval != null)
                {
                    SqlParameter ApprovalFile_Exit = new SqlParameter("@Approval_FileExist", 1);
                    cmd.Parameters.Add(ApprovalFile_Exit);
                }

                SqlParameter ApprovalFile_Ext = new SqlParameter("@Approval_File_Ext", AppFile_Extension);
                cmd.Parameters.Add(ApprovalFile_Ext);

                SqlParameter Remarks = new SqlParameter("@Remarks", Data.Remarks);
                cmd.Parameters.Add(Remarks);

                SqlParameter User_Id = new SqlParameter("@Create_Usr_Id", Data.Create_usr_id);
                cmd.Parameters.Add(User_Id);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                { 
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            Inv_ID_Update = Convert.ToString(dt.Rows[0]["Inv_Id"]);
                            status = Convert.ToInt32(dt.Rows[0]["Row_Effect"]);
                            Inv_File_Name = Convert.ToString(dt.Rows[0]["Invoice_File"]);
                            Approval_FileName = Convert.ToString(dt.Rows[0]["Approval_File"]);
                        }
                    }
                }
            }
            catch (Exception ex) { status = -1; }
            finally { con.Close(); }

            return status;
        }

        public Invoice_Mod Get_Data_By_ID(string Invoice_Id, string UserId)
        {
            Invoice_Mod Data = new Invoice_Mod();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;

                using (SqlCommand cmd = new SqlCommand("sp_Invoice"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Data_By_ID");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.Add(sqlP_type);

                    SqlParameter Inv_Id = new SqlParameter("@Inv_id", Invoice_Id);
                    cmd.Parameters.Add(Inv_Id);

                    SqlParameter sqlUserId = new SqlParameter("@Create_Usr_Id", UserId);
                    cmd.Parameters.Add(sqlUserId);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_Comuter = dt;
                        }
                    }
                }

                if (dt_Comuter.Rows.Count > 0)
                {
                        Data = new Invoice_Mod();

                        Data.Invoice_id = Convert.ToString(dt_Comuter.Rows[0]["ID"]);
                        Data.Invoice_No = Convert.ToString(dt_Comuter.Rows[0]["Inv_No"]);
                        Data.Invoice_Subject = Convert.ToString(dt_Comuter.Rows[0]["Subject"]);
                        Data.Invoice_Date = Convert.ToDateTime(dt_Comuter.Rows[0]["Inv_date"]);
                        Data.Invoice_Value = Convert.ToInt32(dt_Comuter.Rows[0]["Inv_Value"]);
                        Data.SanctionOrder_Id = Convert.ToString(dt_Comuter.Rows[0]["SO_Id"]);
                        Data.Penalty_Amount = Convert.ToInt32(dt_Comuter.Rows[0]["Penalty_Amt"]);
                        Data.Penalty_Reason = Convert.ToString(dt_Comuter.Rows[0]["Penalty_Reason"]);
                        Data.FileName_Invoice = Convert.ToString(dt_Comuter.Rows[0]["Inv_File_Name"]);
                        Data.Filename_CommitteeApproval  = Convert.ToString(dt_Comuter.Rows[0]["Approval_File_Name"]);
                }

            }
            catch (Exception ex) { }

            return Data;
        }


        public List<SelectListItem> SanctionOrder_List(string UserId)
        {

            List<SelectListItem> List_Item = new List<SelectListItem>();

            try
            {
                DataTable dt_PO;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = 
                    new SqlCommand("SELECT * from  dbo.Get_All_SO(@UserId)"))
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@UserId", UserId.Trim());

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_PO = dt;
                        }
                    }
                }

                foreach (DataRow dr in dt_PO.Rows)
                {
                    SelectListItem Listdata = new SelectListItem();
                    Listdata.Value = Convert.ToString(dr["ID"]);
                    Listdata.Text = Convert.ToString(dr["SO_No"]);

                    List_Item.Add(Listdata);
                }

            }
            catch (Exception ex) { }

            return List_Item;
        }


        public List<SelectListItem> Fin_Year_List()
        {

            List<SelectListItem> List_Item = new List<SelectListItem>();

            try
            {
                DataTable dt_PO;

                SqlConnection con = new DBConnection().con;


                //using (SqlCommand cmd = new SqlCommand("SELECT * from  dbo.Get_All_Budget_Year()"))

                using (SqlCommand cmd = new SqlCommand("SELECT * from  dbo.Get_All_Fin_Year()"))
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_PO = dt;
                        }
                    }
                }

                foreach (DataRow dr in dt_PO.Rows)
                {
                    SelectListItem Listdata = new SelectListItem();
                    Listdata.Value = Convert.ToString(dr["Bud_Id"]);
                    Listdata.Text = Convert.ToString(dr["Bud_Year"]);

                    List_Item.Add(Listdata);
                }

            }
            catch (Exception ex) { }

            return List_Item;
        }

        public List<SelectListItem> Budget_Head_List(string FinYear)
        {

            List<SelectListItem> List_Item = new List<SelectListItem>();

            try
            {
                DataTable dt_PO;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("SELECT * from  dbo.Get_Budget_ByYear(@FinYear)"))
                {

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    SqlParameter SqlFinYear = new SqlParameter("@FinYear", FinYear);
                    cmd.Parameters.Add(SqlFinYear);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_PO = dt;
                        }
                    }
                }

                foreach (DataRow dr in dt_PO.Rows)
                {
                    SelectListItem Listdata = new SelectListItem();
                    Listdata.Value = Convert.ToString(dr["Budget_Head_Id"]);
                    Listdata.Text = Convert.ToString(dr["Budget_Name"]);

                    List_Item.Add(Listdata);
                }

            }
            catch (Exception ex) { }

            return List_Item;
        }

        public string Get_file_name(string type)
        {

            return "";
        }

    }
}
