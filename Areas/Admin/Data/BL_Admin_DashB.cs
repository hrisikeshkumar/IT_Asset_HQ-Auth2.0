using Microsoft.AspNetCore.Mvc.Rendering;
using IT_Hardware.Areas.Admin.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace IT_Hardware.Areas.Admin.Data
{
    public class BL_Admin_DashB
    {
        public List<mod_Admin_Propsal_List> Get_List_Proposal()
        {

            mod_Admin_Propsal_List BL_data;
            List<mod_Admin_Propsal_List> current_data = new List<mod_Admin_Propsal_List>();

            try
            {
                DataTable dt_Comuter;
                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_IT_Proposal"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Proposal_List");
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
                    BL_data = new mod_Admin_Propsal_List();

                    BL_data.Proposal_Id = Convert.ToString(dr["Proposal_Id"]);

                    BL_data.Utilization_Details = Convert.ToString(dr["Utilization_Details"]);

                    BL_data.IT_Initiate_Date = Convert.ToDateTime(dr["IT_Initiate_Date"]);

                    BL_data.Status = Convert.ToString(dr["Completed_Status"]);

                    current_data.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return current_data;
        }


        public List<mod_Admin_Bill_Process_List> Get_List_Bills()
        {

            mod_Admin_Bill_Process_List BL_data;
            List<mod_Admin_Bill_Process_List> current_data = new List<mod_Admin_Bill_Process_List>();

            try
            {
                DataTable dt_Comuter;
                
                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_IT_Proposal"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Bill_List");
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
                    BL_data = new mod_Admin_Bill_Process_List();

                    BL_data.Proposal_Id = Convert.ToString(dr["Proposal_Id"]);

                    BL_data.Utilization_Details = Convert.ToString(dr["Utilization_Details"]);

                    BL_data.IT_Initiate_Date = Convert.ToDateTime(dr["IT_Initiate_Date"]);

                    BL_data.Status = Convert.ToString(dr["Completed_Status"]);

                    current_data.Add(BL_data);
                }

            }
            catch (Exception ex) { }

            return current_data;
        }


        public void Get_Proposal_By_Id(Mod_Admin_dashB BL_data, string Proposal_Id)
        {

            try
            {
                DataTable dt_Comuter;
                
                SqlConnection con = new DBConnection().con;


                using (SqlCommand cmd = new SqlCommand("sp_IT_Proposal"))
                {
                    SqlParameter sqlP_type = new SqlParameter("@Type", "Get_Proposal_by_Id");
                    cmd.Parameters.Add(sqlP_type);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;


                    SqlParameter sql_Proposal_Id = new SqlParameter("@Proposal_Id", Proposal_Id);
                    cmd.Parameters.Add(sql_Proposal_Id);


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

                    BL_data.Prop_detail.Proposal_Id = Convert.ToString(dt_Comuter.Rows[0]["Proposal_Id"]);

                    BL_data.Prop_detail.Utilization_Details = Convert.ToString(dt_Comuter.Rows[0]["Utilization_Details"]);

                    BL_data.Prop_detail.Dte_IT_Remarks = Convert.ToString(dt_Comuter.Rows[0]["Dte_IT_Copy"]);

                    BL_data.Prop_detail.FA_Remarks = Convert.ToString(dt_Comuter.Rows[0]["FA_Remarks"]);

                    BL_data.Prop_detail.Sec_Office_Remarks = Convert.ToString(dt_Comuter.Rows[0]["Sec_Office_Remarks"]);

                    BL_data.Prop_detail.Purchase_Remarks = Convert.ToString(dt_Comuter.Rows[0]["Purchase_Remarks"]);

                    BL_data.Prop_detail.Other_Dept_Remarks = Convert.ToString(dt_Comuter.Rows[0]["Other_Dept_Remarks"]);

                    BL_data.Prop_detail.Completed_Status = Convert.ToString(dt_Comuter.Rows[0]["Completed_Status"]);


                }
            }
            catch (Exception ex) { }


        }

        public int Update_proposal(Proposal_details Proposal)
        {
            int status = 0;
            
            SqlConnection con = new DBConnection().con;
            try
            {

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "sp_IT_Proposal";

                cmd.Connection = con;

                SqlParameter sqlP_type = new SqlParameter("@Type", "Update_Status");
                cmd.Parameters.Add(sqlP_type);
 
                SqlParameter Proposal_Id = new SqlParameter("@Proposal_Id", Proposal.Proposal_Id);
                cmd.Parameters.Add(Proposal_Id);

                SqlParameter Dte_IT_Remarks = new SqlParameter("@Dte_IT_Remarks", Proposal.Dte_IT_Remarks);
                cmd.Parameters.Add(Dte_IT_Remarks);

                SqlParameter FA_Remarks = new SqlParameter("@FA_Remarks", Proposal.FA_Remarks);
                cmd.Parameters.Add(FA_Remarks);

                SqlParameter Sec_Office_Remarks = new SqlParameter("@Sec_Office_Remarks", Proposal.Sec_Office_Remarks);
                cmd.Parameters.Add(Sec_Office_Remarks);

                SqlParameter Purchase_Remarks = new SqlParameter("@Purchase_Remarks", Proposal.Purchase_Remarks);
                cmd.Parameters.Add(Purchase_Remarks);

                SqlParameter Other_Dept_Remarks = new SqlParameter("@Other_Dept_Remarks", Proposal.Other_Dept_Remarks);
                cmd.Parameters.Add(Other_Dept_Remarks);

                SqlParameter Status = new SqlParameter("@Status",   Proposal.Status ==true? 1:0 );
                cmd.Parameters.Add(Status);

                SqlParameter UserId = new SqlParameter("@UserId", Proposal.Update_UserId);
                cmd.Parameters.Add(UserId);


                con.Open();

                status = cmd.ExecuteNonQuery();


            }
            catch (Exception ex) { status = -1; }
            finally { con.Close(); }

            return status;
        }
    }
}