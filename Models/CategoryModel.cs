using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EC.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        [DataType(DataType.Text)]
        [Required]
        [MaxLength(200)]
        [Display(Name = "Category")]
        public string category_description { get; set; }
        [Display(Name = "Products")]
        public List<Product> Products { get; set; }
    }
}