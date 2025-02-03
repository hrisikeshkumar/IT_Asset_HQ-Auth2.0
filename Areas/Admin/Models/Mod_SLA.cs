using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace IT_Hardware.Areas.Admin.Models
{
    public class Mod_SLA
    {   
        public string? SLA_Id { get; set; }   /*pp*/
        public string Vendor_id { get; set; }
        public string? Vendor_Name { get; set; }   /*pp*/
        public List<SelectListItem>? Vendor_List { get; set; }   /*pp*/
        public string? Service_Type_Short { get; set; }
        public string? Service_Type_Details { get; set; }
        public string? SLA_File_Name { get; set; }
        public string? Remarks { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Service_ST_DT { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Expiry_DT { get; set; }
        public string? Create_usr_id { get; set; }    /*pp*/
        public DateTime? Create_date { get; set; }    /*pp*/
        public string? Verfd_status { get; set; }    /*pp*/
        public IFormFile? All_Files { get; set; }
    }
}