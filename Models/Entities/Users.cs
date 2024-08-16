using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cater4Us_Backend.Models.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }


        [Required]
        public bool isVerified { get; set; } = false;

        [MaxLength(50)]
        public string VerificationCode { get; set; }

        public int Role { get; set; } = 0;

        public Users()
        {
            VerificationCode = GenerateVerificationCode();
        }


        private string GenerateVerificationCode()
        {
            // Generate a unique verification code, you can adjust the format as needed
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
}
