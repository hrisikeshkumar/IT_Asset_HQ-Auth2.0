using DocumentFormat.OpenXml.EMMA;
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



    }
}
