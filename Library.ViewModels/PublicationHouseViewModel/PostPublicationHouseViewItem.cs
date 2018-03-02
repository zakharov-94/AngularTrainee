﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels.PublicationHouseViewModel
{
    public class PostPublicationHouseViewItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
