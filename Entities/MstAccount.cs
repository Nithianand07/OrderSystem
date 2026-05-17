using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enterprises.Entities
{
    public class MstAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }
        public string AccountType { get; set; } = string.Empty; // "Customer" or "Supplier"
        public string AccountName { get; set; } = string.Empty;
        public string? ContactNumber { get; set; }
        public string? EmailAddress { get; set; }
        public bool ActiveFlg { get; set; }
        public bool IsDelete { get; set; } = false;
    }
}
