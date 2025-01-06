using IT_Hardware.Areas.Admin.Models;
using System.Data.SqlClient;
using System.Data;

namespace IT_Hardware.Areas.Admin.Data
{
    public class RoChapterInfo_BL
    {
        public List<RoChapterInfo> Get_ChaptersData(string chapterName)
        {

            RoChapterInfo BL_data;
            List<RoChapterInfo> Listdata = new List<RoChapterInfo>();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().conChapter;

                using (SqlCommand cmd = new SqlCommand("sp_ChapterInfo"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_List");
                    SqlParameter sqlP_chapterName = new SqlParameter("@chapterName", chapterName);
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
                    BL_data = new RoChapterInfo();
                    BL_data.ChapterName = Convert.ToString(dr["Unique_Id"]);
                    BL_data.ItemName = Convert.ToString(dr["Emp_Code"]);
                    BL_data.SerialNo = Convert.ToString(dr["Emp_Name"]);
                    BL_data.ProcDate = Convert.ToDateTime(dr["Emp_Designation"]);                   
                    BL_data.Price = Convert.ToInt32(dr["Emp_Dept"]);
                    BL_data.FundSource = Convert.ToString(dr["Emp_Type"]);
                    BL_data.SOFile = Convert.ToString(dr["Emp_Location"]);
                    BL_data.InvFile = Convert.ToString(dr["Dept_name"]);

                    if(Convert.ToString(dr["Designation_name"]) != string.Empty)
                        foreach (string file in Convert.ToString(dr["Designation_name"]).Split(','))
                        {
                            BL_data.QuoteFiles.Add(file);
                        }

                    Listdata.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return Listdata;
        }

    }
}
