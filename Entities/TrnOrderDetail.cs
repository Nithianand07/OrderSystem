using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Enterprises.Entities
{
    public class TrnOrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailNo { get; set; }

        public int OrderNo { get; set; }

        [ForeignKey(nameof(OrderNo))]
        [JsonIgnore]
        public virtual TrnOrderHead? OrderHead { get; set; }

        public int ItemID { get; set; }

        public string? Reference { get; set; }

        public string? Narration { get; set; }

        public int Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal ItemAmount { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
