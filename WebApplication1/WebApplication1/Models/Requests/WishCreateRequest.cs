using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Requests
{
    public class WishCreateRequest
    {
        [Required]
        [MaxLength(200)]
        public string Location { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(300)]
        public string Activity { get; set; }

        [Required]
        [MaxLength(300)]
        public string Image { get; set; }
    }

}