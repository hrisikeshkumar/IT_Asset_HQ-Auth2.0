using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace IT_Hardware.Areas.Admin.Models
{
    public class Mod_Admin_dashB
    {
        public Proposal_details Prop_detail { get; set; }
        public DateTime IT_Initiate_Date { get; set; }
        public List<mod_Admin_Propsal_List> List_Proposal { get; set; }
        public List<mod_Admin_Bill_Process_List> List_Bill_Process { get; set; }
        public List<mod_SLA_Expiary_List> List_SLA_Expiary { get; set; }
    }
     

    public class Proposal_details
    {
        public string? Proposal_Id { get; set; }
        public string? Budget_Name { get; set; }
        public string? Budget_Year { get; set; }
        public string? Utilization_Details { get; set; }      
        public string? Dte_IT_Copy { get; set; }
        public string? Dte_IT_Remarks { get; set; }
        public string? FA_Remarks { get; set; }
        public string? Secretary_Approval_Required { get; set; }
        public string? Sec_Office_Remarks { get; set; }
        public string? Purchase_Remarks { get; set; }
        public string? Other_Dept_Remarks { get; set; }
        public string? PO_File { get; set; }
        public string? Proposal_Type { get; set; }
        public string? Budget_Head_Type { get; set; }
        public string? PO_Info { get; set; }
        public string? Invoice_Info { get; set; }
        public string? Assets_Info { get; set; }
        public string? Completed_Status { get; set; }
        public bool Status { get; set; }

        public string? Update_UserId { get; set; }

        public List<File_List>? Prop_Files { get; set; }

    }

    public class File_List
    {
        public string File_Id { get; set; }
        public string File_Name { get; set; }
    }

    public class mod_Admin_Propsal_List
    {
       public string Proposal_Id { get; set; }
        public string Utilization_Details { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime IT_Initiate_Date { get; set; }
        public string Status { get; set; }
              
    }

    public class mod_Admin_Bill_Process_List
    {
        public string Proposal_Id { get; set; }
        public string Utilization_Details { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime IT_Initiate_Date { get; set; }
        public string Status { get; set; }

    }

    public class mod_SLA_Expiary_List
    {
        public string SLA_Id { get; set; }
        public string Vendor_Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Expiary_Date { get; set; }
        public string Status { get; set; }

    }

    public class Grid_Class
    {
        public string Particular { get; set; }
        public string Proposal_Id { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
    }

}