using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace EC.Models
{
  
    public class CartItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ItemID { get; set; }

        [Display(Name = "Product Id")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        [Required]
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]               
        public double Total { get; set; }

        [Display(Name = "Shopping Cart")]
        public int CartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

        [Range(minimum: 0, maximum: Double.MaxValue, ErrorMessage = "Can not be a negative value.")]
        [Required]
        public int Quantity { get; set; }

        public double Sum()
        {
            return Product.Price * Quantity;
        }
    }

    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    
        public int CartId { get; set; }

        [Display(Name = "Items")]
        public List<CartItem> CartItems { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public double Total { get; set;}
        public DateTime Date { get; set;}
    }

    public class OrderDetails
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber {get ;set; }

        
        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Country { get; set; }

        [Display(Name = "Street Address 1")]
        [Required]
        public string Address { get; set; }  
        
        [Display(Name = "Street Address 2")]
        [Required]
        public string Address2 { get; set; }
        
        [Display(Name = "City")]
        [Required]
        public string City { get; set; }

        public int CartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
    