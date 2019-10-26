using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }


        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Description")]
        [Required]
        [DataType(DataType.Text)]
        public string Description { get; set; }


        [Display(Name = "Quantity")]
        [Range(minimum: 0, maximum: Double.MaxValue, ErrorMessage = "Can not have a negative quantity")]
        [Required]
        public int Quantity { get; set; } 

        [Display(Name = "Date Added")]
        public DateTime Date { get; set; }


        [Display(Name = "Product Category")]
        public List<Category> Categories { get; set; }

        [Display(Name = "Cart Items")]
        public List<CartItem> CartItems { get; set; }

        [NotMapped]
        public List<int> category { get; set; }


        [Display(Name = "Price")]
        [Required]
        [DataType(DataType.Currency)]
        [Range(minimum: 0, maximum: Double.MaxValue, ErrorMessage = "Can not have a negative price")]
        public double Price { get; set; }

        
        [Display(Name = "Product Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageFile { get; set; }
    }


    


}