using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGrouping.Models
{
    public class ProductGroup
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Product Reference")]
        [Required(ErrorMessage = "Product Reference Required")]
        [MaxLength(100, ErrorMessage = "Product Reference Too Long, max 100 characters")]
        [Remote("CheckProductReferenceExist", "Validation", ErrorMessage = "Product reference already has a group", HttpMethod = "POST")]
        public string ProductReference { get; set; }

        //another table???
        [DisplayName("Product Owner")]
        [Required(ErrorMessage = "Product Owner Required")]
        [MinLength(7, ErrorMessage = "Product Owner Too Short, min 7 characters")]
        [MaxLength(7, ErrorMessage = "Product Owner Too Long, max 7 characters")]
        [RegularExpression("^\\d{7}", ErrorMessage = "Product Owner must a PID eg. 1111111")]
        public string ProductOwner { get; set; }

        [ForeignKey(nameof(Parent))]
        public Guid? ParentId { get; set; }

        [JsonIgnore]
        public virtual ProductGroup Parent { get; set; }

        public virtual ICollection<ProductGroup> Children { get; set; }
    }
}
