using Imprinno.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Imprinno.Models.Entities
{
    [Table("roles", Schema = "acc")]
    public partial class Role
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public Role()
        {
            Users = new HashSet<User>();
        }
        [Required]
        [StringLength(30)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Permissions { get; set; } 
        [InverseProperty("Role")]
        public virtual ICollection<User> Users { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        [DataType(DataType.Date)]
        public DateTime UpdatedOn { get; set; }
    }
}
