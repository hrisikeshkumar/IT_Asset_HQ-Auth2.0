using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using IT_Hardware.Areas.Admin.Models;
using System.Data;
using System.Data.SqlClient;

namespace IT_Hardware.Areas.Admin.Data
{
    public class QRCode_BL
    {

        public List<QRCode_Model> AssetsList()
        {

            QRCode_Model data;
            List<QRCode_Model> List_data = new List<QRCode_Model>();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_QRCode_Info"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_All_Assets");
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
                    data = new QRCode_Model();


                    data.Item_Issue_Id = Convert.ToString(dr["Issue_Id"]);

                    data.Asset_SerialNo = Convert.ToString(dr["Item_SlNo"]);

                    data.Asset_Type = Convert.ToString(dr["Asset_Type"]);

                    data.Make = Convert.ToString(dr["Model"]);

                    data.EmployeeName = Convert.ToString(dr["Emp_Name"]);

                    data.Designation = Convert.ToString(dr["Designation"]);

                    data.Department = Convert.ToString(dr["Department"]);


                    List_data.Add(data);
                }

            }
            catch (Exception ex) { }

            return List_data;
        }


        public AssetInfo_Model Asset_Detail_Info(string Id)
        {

            AssetInfo_Model model = new AssetInfo_Model();

            try
            {
                DataSet data;
                SqlConnection con ;

                using (con = new DBConnection().con)
                {
                    SqlCommand cmd = new SqlCommand("sp_QRCode_Info");
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Asset_Detail_Info");
                    SqlParameter sqlP_Id = new SqlParameter("@Item_Issue_Id", Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.Parameters.Add(sqlP_type);

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            data = dt;
                        }
                    } 
                }
   

                model.Item_Issue_Id = Convert.ToString(data.Tables[0].Rows[0]["Issue_Id"]);

                model.Asset_Id = Convert.ToString(data.Tables[0].Rows[0]["Item_Id"]);

                model.Asset_Type = Convert.ToString(data.Tables[0].Rows[0]["Asset_Type"]);

                model.Asset_SerialNo = Convert.ToString(data.Tables[0].Rows[0]["Item_SlNo"]);

                model.Procuremt_Date = Convert.ToDateTime(data.Tables[0].Rows[0]["Proc_Date"]);

                model.Warranty_End_Date = Convert.ToDateTime(data.Tables[0].Rows[0]["Warnt_end_DT"]);

                model.Make = Convert.ToString(data.Tables[0].Rows[0]["Make"]);

                model.Model = Convert.ToString(data.Tables[0].Rows[0]["model"]);

                model.Asset_Value = Convert.ToInt32(data.Tables[0].Rows[0]["Asset_Price"]);

                model.VenderName = Convert.ToString(data.Tables[0].Rows[0]["Vendor_name"]);

                model.PO_No = Convert.ToString(data.Tables[0].Rows[0]["PO_No"]);

                model.PO_Date = Convert.ToDateTime(data.Tables[0].Rows[0]["PO_Date"]);

                model.PO_Value = Convert.ToInt32(data.Tables[0].Rows[0]["PO_Value"]);



                foreach (DataRow dr in data.Tables[1].Rows)
                {
                    ShiftAsset shift = new ShiftAsset();

                    shift.Issued_date = Convert.ToDateTime(dr["Item_Issue_date"]);

                    shift.EmployeeName = Convert.ToString(dr["Emp_Name"]);

                    shift.Designation = Convert.ToString(dr["Designation"]);

                    shift.Department = Convert.ToString(dr["Dept"]);

                    model.ShiftingHistory.Add(shift);

                }


                foreach (DataRow dr in data.Tables[1].Rows)
                {
                    AssetService service = new AssetService();

                    service.EmployeeName = Convert.ToString(dr["Emp_Name"]);

                    service.Designation = Convert.ToString(dr["Designation"]);

                    service.Department = Convert.ToString(dr["Department"]);

                    service.Issue_Create_Date = Convert.ToDateTime(dr["Issue_Create_Date"]);

                    service.IssueInfo = Convert.ToString(dr["IssueInfo"]);

                    service.VenderName = Convert.ToString(dr["Vendor_name"]);

                    service.Issue_Resolve_Date = Convert.ToDateTime(dr["Issue_Resolve_Date"]);

                    service.Resolution_Detail = Convert.ToString(dr["Resolution_Detail"]);

                    model.ServiceHistory.Add(service);
                }


            }
            catch (Exception ex) { }

            return model;
        }

    }
}
