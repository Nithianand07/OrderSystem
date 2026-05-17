using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises.Entities
{
    public class TrnOrderHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNo { get; set; }
        public string OrderCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; } = string.Empty; // "Purchase" or "Sale"
        public int AccountID { get; set; }
        public string? Reference { get; set; }
        public string? Narration { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDelete { get; set; } = false;
        public virtual List<TrnOrderDetail>? Details { get; set; }
    }
}
