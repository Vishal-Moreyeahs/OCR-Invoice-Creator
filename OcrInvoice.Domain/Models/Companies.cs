
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OcrInvoice.Domain.Models
{

    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [EmailAddress]
        public string OfficialMailId { get; set; }

        [Required]
        [EmailAddress]
        public string PrimaryMailId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PrimaryContact { get; set; }

        public string AlternateContact { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Pincode { get; set; }
    }

}
