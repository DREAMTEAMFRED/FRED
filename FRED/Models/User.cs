using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(248)]
        public string Password { get; set; }

        [Required]
        [MaxLength(1)]
        [DefaultValue("A")]
        public string Status { get; set; }

        public ICollection<Log> Logs { get; set; }
    }
}
