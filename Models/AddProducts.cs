using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace EC.Models
{
    public class AddProducts
    {
        [Key]
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name ="Categories")]
        public List<int> categories { get; set; }
    }

    
}