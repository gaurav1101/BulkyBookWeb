using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class Product
    {
        public int id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,10000)]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 1-50")]
        public double Price { get; set; }
      
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 50-100")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 10000)]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }
        [ValidateNever]
        public string imgUrl { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }

        [Required]
        [DisplayName("Cover Type")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
