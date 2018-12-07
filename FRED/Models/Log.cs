using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FRED.Models
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        [DefaultValue(0)]
        public int LoginCount { get; set; }

        [Required]
        public string LastLogin { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

    }
}
