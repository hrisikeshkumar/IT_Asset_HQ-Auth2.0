using Microsoft.AspNetCore.Mvc.Rendering;
using IT_Hardware.Areas.Admin.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IT_Hardware.Areas.Admin.Data
{
    public class BL_Invoice
    {

        public List<Mod_Invoice> Get_All_Invoice()
        {

            Mod_Invoice BL_data;
            List<Mod_Invoice> current_data = new List<Mod_Invoice>();

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
                    BL_data = new Mod_Invoice();

                    BL_data.Invoice_id = Convert.ToString(dr["PO_id"]);
                    BL_data.Vendor_Name = Convert.ToString(dr["PO_No"]);
                    BL_data.Invoice_Subject = Convert.ToString(dr["PO_Sub"]);
                    BL_data.Invoice_No = Convert.ToString(dr["PO_No"]);
                    BL_data.Invoice_Value = Convert.ToInt32(dr["PO_Value"]);

                    current_data.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return current_data;
        }

        public int Save_data(Mod_Invoice Data, string type, string PO_ID, out string PO_ID_Update, out string PO_File_Name, out string SLA_File_Name)
        {

            int status = -1;
            PO_ID_Update = string.Empty;
            PO_File_Name = string.Empty;
            SLA_File_Name = string.Empty;


            SqlConnection con = new DBConnection().con;
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_POrder";

                cmd.Connection = con;

                SqlParameter sqlP_type = new SqlParameter("@Type", type);
                cmd.Parameters.Add(sqlP_type);

                if (type == "Update" || type == "Delete")
                {
                    SqlParameter Invoice_Id = new SqlParameter("@PO_id", Data.Invoice_id);
                    cmd.Parameters.Add(Invoice_Id);
                }

                SqlParameter Invoice_No = new SqlParameter("@PO_No", Data.Invoice_No);
                cmd.Parameters.Add(Invoice_No);

                SqlParameter PO_Id = new SqlParameter("@PO_No", Data.PO_Id);
                cmd.Parameters.Add(PO_Id);

                SqlParameter Penalty_Amount = new SqlParameter("@PO_No", Data.Penalty_Amount);
                cmd.Parameters.Add(Penalty_Amount);

                SqlParameter Invoice_Subject = new SqlParameter("@PO_No", Data.Invoice_Subject);
                cmd.Parameters.Add(Invoice_Subject);

                SqlParameter Penalty_Reason = new SqlParameter("@PO_No", Data.Penalty_Reason);
                cmd.Parameters.Add(Penalty_Reason);

                SqlParameter Invoice_Year_Id = new SqlParameter("@PO_No", Data.Invoice_Year_Id);
                cmd.Parameters.Add(Invoice_Year_Id);

                SqlParameter Invoice_Value = new SqlParameter("@PO_Value", Data.Invoice_Value);
                cmd.Parameters.Add(Invoice_Value);

                SqlParameter Invoice_Date = new SqlParameter("@PO_ST_Date", Data.Invoice_Date);
                cmd.Parameters.Add(Invoice_Date);


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
                            PO_ID_Update = Convert.ToString(dt.Rows[0]["Invoice_id"]);
                            status = Convert.ToInt32(dt.Rows[0]["Row_Effect"]);
                            PO_File_Name = Convert.ToString(dt.Rows[0]["Invoice_File_Name"]);
                        }

                    }
                }



            }
            catch (Exception ex) { status = -1; }
            finally { con.Close(); }

            return status;
        }

        public Mod_Invoice Get_Data_By_ID(string Vendor_Id)
        {
            Mod_Invoice Data = new Mod_Invoice();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;

                using (SqlCommand cmd = new SqlCommand("sp_Vendor"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Data_By_ID");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.Add(sqlP_type);

                    SqlParameter VendorId = new SqlParameter("@Vendor_ID", Vendor_Id);
                    cmd.Parameters.Add(VendorId);

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
                    Data.Invoice_id = Convert.ToString(dt_Comuter.Rows[0]["Vendor_ID"]);
                    Data.Invoice_No = Convert.ToString(dt_Comuter.Rows[0]["Vendor_name"]);
                    Data.Invoice_Value = Convert.ToInt32(dt_Comuter.Rows[0]["Vendor_Address"]);
                    Data.Remarks = Convert.ToString(dt_Comuter.Rows[0]["remarks"]);

                }

            }
            catch (Exception ex) { }

            return Data;
        }

        public List<SelectListItem> PO_List()
        {

            List<SelectListItem> List_Item = new List<SelectListItem>();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("Vendor_List"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;

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
                    SelectListItem Listdata = new SelectListItem();
                    Listdata.Value = Convert.ToString(dr["Vendor_ID"]);
                    Listdata.Text = Convert.ToString(dr["Vendor_name"]);

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
