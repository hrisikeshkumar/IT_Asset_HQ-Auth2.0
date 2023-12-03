using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace IT_Hardware.Areas.Admin.Models
{
    public class Mod_SLA
    {

        public string SLA_Id { get; set; }
        [Required]
        public string Vendor_id { get; set; }

        public string Vendor_Name { get; set; }
        public List<SelectListItem> Vendor_List { get; set; }
        [Required]
        public string Service_Type_Short { get; set; }
        public string Service_Type_Details { get; set; }
        public string PO_Copy { get; set; }
        public string SLA_Copy { get; set; }
        public string Approval_Copy { get; set; }
        public string Remarks { get; set; }
        [Required]
        public DateTime? Service_ST_DT { get; set; }
        [Required]
        public DateTime? Expiry_DT { get; set; }
        public string Create_usr_id { get; set; }
        public DateTime? Create_date { get; set; }
        public string Verfd_status { get; set; }
        public string Verfd_usr_id { get; set; }
        public DateTime? Verfd_date { get; set; }

        public List<IFormFile> All_Fies { get; set; }
        public List< IFormFile> File_List { get; set; }

    }
}