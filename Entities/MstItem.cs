using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises.Entities
{
    public class MstItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }
        public string ItemCode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string ItemUOM { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int StockQty { get; set; }
        public bool ActiveFlg { get; set; } = true;
        public bool IsDelete { get; set; } = false;
    }
}
