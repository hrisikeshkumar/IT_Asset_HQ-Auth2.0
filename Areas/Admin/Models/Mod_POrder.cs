using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace IT_Hardware.Areas.Admin.Models
{
    public class Mod_POrder
    {
        public string? PO_id { get; set; }
        [Required]
        public string? PO_No { get; set; }
        [Required]
        public int? PO_Value { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? PO_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? PO_ST_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? PO_End_Date { get; set; }


        [Required]
        public string? PO_Subject { get; set; }
        public string? PO_File_Name { get; set; }
        public string? PO_File_Id { get; set; }
        public string? Budget_Head_Id { get; set; }
        public string? Vendor_id { get; set; }
        public List<SelectListItem>? Vendor_List { get; set; }
        public string? Remarks { get; set; }
        public Boolean SLA_Required { get; set; }
        public string? Create_usr_id { get; set; }
        public DateTime? Create_date { get; set; }
        public string? Verfd_status { get; set; }
        public IFormFile? File_PO { get; set; }
    }
}