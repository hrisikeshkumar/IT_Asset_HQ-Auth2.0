using IT_Hardware.Areas.Admin.Models;
using System.Data;
using System.Data.SqlClient;

namespace IT_Hardware.Areas.Admin.Data
{
    public class ItemInfo_BL
    {

        public ItemInfo_Mod Get_Item_IssueData(string serialNO)
        {
            ItemInfo_Mod itemInfo = new ItemInfo_Mod();
            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_ItemInfo"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@SerialNo", serialNO);
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

                itemInfo.Serial_No = serialNO;
                if (dt_Comuter.Rows.Count > 0)
                {
                    itemInfo.Invoice_Info = Convert.ToString(dt_Comuter.Rows[0]["Invoice_File"]);
                    itemInfo.PO_Info = Convert.ToString(dt_Comuter.Rows[0]["PO_File"]);
                    itemInfo.Approval_Info = Convert.ToString(dt_Comuter.Rows[0]["Proposal_File"]);
                }

            }
            catch (Exception ex) { }

            return itemInfo;
        }
    }
}
