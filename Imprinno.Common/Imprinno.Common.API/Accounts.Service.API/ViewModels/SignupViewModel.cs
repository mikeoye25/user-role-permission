using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Service.API.ViewModels
{
    public class SignupViewModel
    {

        [Required]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }
        [Required]
        [StringLength(512)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(512)]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
