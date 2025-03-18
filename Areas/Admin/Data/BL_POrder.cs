using Microsoft.AspNetCore.Mvc.Rendering;
using IT_Hardware.Areas.Admin.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace IT_Hardware.Areas.Admin.Data
{
    public class BL_Porder
    {
        public List<Mod_POrder> Get_All_PO_Data()
        {

            Mod_POrder BL_data;
            List<Mod_POrder> current_data = new List<Mod_POrder>();

            try
            {
                DataTable dt_Comuter;
                
                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_POrder"))
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
                    BL_data = new Mod_POrder();

                    BL_data.PO_id = Convert.ToString(dr["PO_id"]);
                    BL_data.PO_No = Convert.ToString(dr["PO_No"]);
                    BL_data.PO_Subject = Convert.ToString(dr["PO_Sub"]);
                    BL_data.PO_End_Date =  Convert.ToDateTime( dr["PO_End_Date"]) ;
                    BL_data.PO_Value = Convert.ToInt32(dr["PO_Value"]);

                    current_data.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return current_data;
        }

        public int Save_PO_data(Mod_POrder Data, string type, string PO_ID, out string PO_ID_Update, out string PO_File_Name)
        {

            int status = -1;
            PO_ID_Update = string.Empty;        
            PO_File_Name = string.Empty;
            
            SqlConnection con = new DBConnection().con;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_POrder";

                cmd.Connection = con;

                if (type == "Update" || type == "Delete")
                {
                    SqlParameter PO_Id = new SqlParameter("@PO_id", Data.PO_id);
                    cmd.Parameters.Add(PO_Id);
                }

                SqlParameter PO_No = new SqlParameter("@PO_No", Data.PO_No);   
                cmd.Parameters.Add(PO_No);

                SqlParameter PO_Date = new SqlParameter("@PO_Date", Data.PO_Date);
                cmd.Parameters.Add(PO_Date);

                SqlParameter PO_Value = new SqlParameter("@PO_Value", Data.PO_Value);
                cmd.Parameters.Add(PO_Value);

                SqlParameter Vendor_id = new SqlParameter("@Vendor_id", Data.Vendor_id);
                cmd.Parameters.Add(Vendor_id);

                SqlParameter PO_ST_Date = new SqlParameter("@PO_ST_Date", Data.PO_ST_Date);
                cmd.Parameters.Add(PO_ST_Date);

                SqlParameter PO_End_Date = new SqlParameter("@PO_End_Date", Data.PO_End_Date);
                cmd.Parameters.Add(PO_End_Date);

                SqlParameter PO_File_Present = new SqlParameter("@PO_File_Present", Data.PO_File_Name);
                cmd.Parameters.Add(PO_File_Present);

                SqlParameter PO_Subject = new SqlParameter("@PO_Subject", Data.PO_Subject);
                cmd.Parameters.Add(PO_Subject);

                SqlParameter Remarks = new SqlParameter("@Remarks", Data.Remarks);
                cmd.Parameters.Add(Remarks);

                SqlParameter User_Id = new SqlParameter("@Create_Usr_Id", Data.Create_usr_id);
                cmd.Parameters.Add(User_Id);

                SqlParameter sqlP_type = new SqlParameter("@Type", type);
                cmd.Parameters.Add(sqlP_type);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {

                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            PO_ID_Update = Convert.ToString(dt.Rows[0]["PO_id"]);
                            status = Convert.ToInt32(dt.Rows[0]["Row_Effect"]);
                            PO_File_Name = Convert.ToString(dt.Rows[0]["PO_File_Name"]);
                        }

                    }
                }

            }
            catch (Exception ex) { status = -1; }
            finally { con.Close(); }

            return status;
        }

        public Mod_POrder Get_Data_By_ID(string PO_Id)
        {
            Mod_POrder Data = new Mod_POrder();

            try
            {
                DataTable dt_PObyID;
                SqlConnection con = new DBConnection().con;

                using (SqlCommand cmd = new SqlCommand("sp_POrder"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Data_By_ID");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.Add(sqlP_type);

                    SqlParameter VendorId = new SqlParameter("@PO_id", PO_Id);
                    cmd.Parameters.Add(VendorId);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            dt_PObyID = dt;
                        }
                    }
                }

                if (dt_PObyID.Rows.Count > 0)
                {
                    Data.PO_id = Convert.ToString(dt_PObyID.Rows[0]["PO_id"]);
                    Data.PO_No = Convert.ToString(dt_PObyID.Rows[0]["PO_No"]);
                    Data.PO_Subject = Convert.ToString(dt_PObyID.Rows[0]["PO_Sub"]);
                    Data.PO_Value = Convert.ToInt32(dt_PObyID.Rows[0]["PO_Value"]);

                    if (Convert.ToString( dt_PObyID.Rows[0]["PO_Date"]) != string.Empty)
                        Data.PO_Date = Convert.ToDateTime(dt_PObyID.Rows[0]["PO_Date"]);
                    if (Convert.ToString(dt_PObyID.Rows[0]["PO_ST_Date"]) != string.Empty)
                        Data.PO_ST_Date = Convert.ToDateTime(dt_PObyID.Rows[0]["PO_ST_Date"]);
                    if (Convert.ToString(dt_PObyID.Rows[0]["PO_End_Date"]) != string.Empty)
                        Data.PO_End_Date = Convert.ToDateTime(dt_PObyID.Rows[0]["PO_End_Date"]);

                    Data.PO_File_Name = Convert.ToString(dt_PObyID.Rows[0]["PO_File"]) ;

                    if (Data.PO_File_Name != string.Empty)
                        Data.PO_File_Name = Data.PO_File_Name + ".pdf";

                    Data.Vendor_id = Convert.ToString(dt_PObyID.Rows[0]["Vendor_Id"]);
                    Data.Remarks = Convert.ToString(dt_PObyID.Rows[0]["Remarks"]); 

                }

            }
            catch (Exception ex) { }

            return Data;
        }

        public List<SelectListItem> Vendor_List()
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