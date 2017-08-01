﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Wish
    {
        public int Id { get; set; }

        public string Location { get; set; }

        public string UserId { get; set; }

        public string Activity { get; set; }

        public string ImageUrl { get; set;}

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
    }
}