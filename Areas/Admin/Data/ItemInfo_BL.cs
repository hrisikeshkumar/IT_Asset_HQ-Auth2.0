using IT_Hardware.Areas.Admin.Models;
using Microsoft.Graph.Models;
using System.Data;
using System.Data.SqlClient;

namespace IT_Hardware.Areas.Admin.Data
{
    public class ItemInfo_BL
    {

        public ItemInfo_Mod Get_Item_IssueData(ItemInfo_Mod itemInfo)
        {            
            getdetails(itemInfo);

            return itemInfo;
        }

        public ItemdetailInfo_Mod Get_Item_IssueData(ItemdetailInfo_Mod itemInfo)
        {
            getdetails(itemInfo);
            return itemInfo;
        }


        private void getdetails(ItemInfo_Mod item)
        {

            ItemdetailInfo_Mod data = new ItemdetailInfo_Mod();

            try
            {
                DataTable dt_Comuter;

                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_ItemInfo"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@SerialNo", item.Serial_No);
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
                data.Serial_No = item.Serial_No;

                if (dt_Comuter.Rows.Count > 0)
                {
                    data.Invoice_Info = Convert.ToString(dt_Comuter.Rows[0]["Invoice_File"]);
                    data.PO_Info = Convert.ToString(dt_Comuter.Rows[0]["PO_File"]);
                    data.Approval_Info = Convert.ToString(dt_Comuter.Rows[0]["Proposal_File"]);
                    data.Invoice_FileId = Convert.ToString(dt_Comuter.Rows[0]["Invoice_File_Id"]);
                    data.PO_Info_FileId = Convert.ToString(dt_Comuter.Rows[0]["PO_File_Id"]);
                    data.Approval_Info_FileId = Convert.ToString(dt_Comuter.Rows[0]["Proposal_File_Id"]);
                }

                item = data;

            }
            catch (Exception ex) { }
        }

    }
}
