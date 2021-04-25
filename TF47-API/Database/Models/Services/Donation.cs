using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF47_API.Database.Models.Services
{
    [Table("ServiceDonations")]
    public class Donation
    {
        [Key]
        public long DonationId { get; set; }
        public DateTime TimeOfDonation { get; set; }
        public User User { get; set; }
        public Guid? UserId { get; set; } = null;
        
        public decimal Amount { get; set; }
        public string Note { get; set; }
    }
}