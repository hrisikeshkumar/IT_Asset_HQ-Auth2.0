using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace IT_Hardware.Areas.Admin.Models
{
    public class Mod_POrder
    {
        public string PO_id { get; set; }
        [Required]
        public string PO_No { get; set; }
        [Required]
        public int PO_Value { get; set; }

        [Required]
        public DateOnly PO_ST_Date { get; set; }
        [Required]
        public DateOnly PO_End_Date { get; set; }
        [Required]
        public string PO_Subject { get; set; }
       // [Required]
        public string Vendor_id { get; set; }
        public List<SelectListItem> Vendor_List { get; set; }
        public string Remarks { get; set; }
        public string Create_usr_id { get; set; }
        public DateTime? Create_date { get; set; }
        public string Verfd_status { get; set; }
        public string Verfd_usr_id { get; set; }
        public DateTime? Verfd_date { get; set; }
        public IFormFile File_PO { get; set; }
        public IFormFile File_SLA { get; set; }

    }
}