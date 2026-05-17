using System.ComponentModel.DataAnnotations;

namespace Enterprises.Dtos
{
    public class ItemDto
    {
        public int ItemID { get; set; }
        [Required, MaxLength(20)]
        public string ItemCode { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ItemName { get; set; } = string.Empty;
        [MaxLength(10)]
        public string ItemUOM { get; set; } = string.Empty;
        [Range(0, double.MaxValue)]
        public decimal PurchasePrice { get; set; }
        [Range(0, double.MaxValue)]
        public decimal SalePrice { get; set; }
        public int StockQty { get; set; }
        public bool ActiveFlg { get; set; }
    }
}
