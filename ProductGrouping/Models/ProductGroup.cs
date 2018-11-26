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
        [MaxLength(10, ErrorMessage = "Product Reference Too Long")]
        public string ProductReference { get; set; }

        [DisplayName("Product Owner")]
        [Required(ErrorMessage = "Product Owner Required")]
        [MaxLength(7, ErrorMessage = "Product Owner Too Long")]
        public string ProductOwner { get; set; }

        [DisplayName("Group")]
        [Required(ErrorMessage = "Group Required")]
        [MaxLength(100, ErrorMessage = "Product Owner Too Long")]
        public string Group { get; set; }

        [DisplayName("Site")]
        [Required(ErrorMessage = "Site Required")]
        [MaxLength(10, ErrorMessage = "Site Too Long")]
        public string Site { get; set; }
    }
}
