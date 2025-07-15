using System.ComponentModel.DataAnnotations;

namespace IT_Hardware.Areas.Admin.Models
{
    public class QRCode_Model
    {
        public string? Item_Issue_Id { get; set; }
        public int? SelectAsset { get; set; }
        public string? Asset_Id { get; set; }
        public string? Asset_Type { get; set; }
        public string? Asset_SerialNo { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? EmployeeName { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Issued_date { get; set; }
        public string? UserId { get; set; }
    }

    public class AssetInfo_Model : QRCode_Model
    {
        public string? PO_No { get; set; }
        public DateTime? PO_Date { get; set; }
        public int? PO_Value { get; set; }
        public int? Asset_Value { get; set; }
        public DateTime? Procuremt_Date { get; set; }
        public DateTime? Warranty_End_Date { get; set; }
        public string? VenderName { get; set; }

        public List<ShiftAsset>? ShiftingHistory { get; set; }
        public List<AssetService>? ServiceHistory { get; set; }
    }

    public class ShiftAsset
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Issued_date { get; set; }

        public string? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? Designation { get; set; }

        public string? Department { get; set; }
    }

    public class AssetService
    {
        public string? VenderId { get; set; }
        public string? VenderName { get; set; }

        public string? EmployeeName { get; set; }
        public string? Designation { get; set; }

        public string? Department { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Issue_Create_Date { get; set; }
        public string? IssueInfo { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime? Issue_Resolve_Date { get; set; }
        public string? Resolution_Detail { get; set; }
    }

}
