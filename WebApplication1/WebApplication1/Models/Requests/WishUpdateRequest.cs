﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Requests
{
    public class WishUpdateCreate
    {
        [Required]
        public int Id { get; set; }
    }
}