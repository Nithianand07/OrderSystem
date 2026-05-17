using System.ComponentModel.DataAnnotations;

namespace Enterprises.Dtos
{
    public class AccountDto
    {
        public int AccountID { get; set; }

        [Required]
        [MaxLength(8)]
        public string AccountType { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string AccountName { get; set; } = string.Empty;

        [MaxLength(12)]
        public string? ContactNumber { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        public bool ActiveFlg { get; set; }
    }
}
