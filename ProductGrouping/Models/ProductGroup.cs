using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProductGrouping.Models
{
    public class ProductGroup
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Product Reference")]
        [Required(ErrorMessage = "Product Reference Required")]
        [MaxLength(10, ErrorMessage = "Product Reference Too Long, max 10 characters")]
        [Remote("CheckProductReferenceExist", "Validation", ErrorMessage = "Product reference already has a group", HttpMethod = "POST")]
        public string ProductReference { get; set; }

        [DisplayName("Product Owner")]
        [Required(ErrorMessage = "Product Owner Required")]
        [MinLength(7, ErrorMessage = "Product Owner Too Short, min 7 characters")]
        [MaxLength(7, ErrorMessage = "Product Owner Too Long, max 7 characters")]
        [RegularExpression("^\\d{7}", ErrorMessage = "Product Owner must a PID eg. 1111111")]
        public string ProductOwner { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group Required")]
        [MaxLength(100, ErrorMessage = "Group Too Long, max 100 characters")]
        public string Group { get; set; }

        [DisplayName("Site")]
        [Required(ErrorMessage = "Site Required")]
        [MaxLength(10, ErrorMessage = "Site Too Long, max 10 characters")]
        public string Site { get; set; }
    }
}
