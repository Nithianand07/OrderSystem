using System.ComponentModel.DataAnnotations;

namespace Enterprises.Dtos
{
    public class OrderDetailDto
    {
        public int OrderDetailNo { get; set; }

        [Required]
        public int ItemID { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Rate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Discount { get; set; }
    }

    public class OrderCreateDto
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public string OrderType { get; set; }
            = string.Empty;

        [Required]
        public int AccountID { get; set; }

        public string? Reference { get; set; }

        public string? Narration { get; set; }

        [Required]
        public List<OrderDetailDto> Details
        { get; set; } = new();
    }
    public class PurchaseSalesReportDto
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string OrderType { get; set; } = string.Empty;
    }
    public class PurchaseSalesListDto
    {
        public int OrderNo { get; set; }

        public string OrderCode { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public string OrderType { get; set; } = string.Empty;

        public string AccountName { get; set; } = string.Empty;

        public string? Narration { get; set; }

        public string? Reference { get; set; }

        public decimal Amount { get; set; }
    }
}