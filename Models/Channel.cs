﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Models
{
    [Table("Channel")]
    public class Channel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Betalingstype")]
        public string Name { get; set; }
    }
}

