using Imprinno.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Imprinno.Models.Entities
{
    [Table("users", Schema = "acc")]
    public partial class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        [Required]
        [StringLength(512)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(512)]
        public string LastName { get; set; }
        public string Address { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        public bool? EmailVerified { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
        public bool? PhoneVerified { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DOB { get; set; }
        [StringLength(20)]
        public string Sex { get; set; }
        public string AboutMe { get; set; }
        [StringLength(255)]
        public string ProfileImageURL { get; set; }
        [StringLength(50)]
        public string RegistrationIP { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsDeleted { get; set; }
        [StringLength(30)]
        public string Language { get; set; }
        public Guid? RoleId { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("Users")]
        public virtual Role Role { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}
