using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BP.WebUI.Areas.Admin.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}