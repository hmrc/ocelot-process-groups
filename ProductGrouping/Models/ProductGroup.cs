using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductGrouping.Models
{
    /// <summary>
    /// Product Group Model
    /// </summary>
    public class ProductGroup
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]        
        public Guid Id { get; set; }

        /// <summary>
        /// Product Reference
        /// </summary>
        [DisplayName("Product Reference")]
        [Required(ErrorMessage = "Product Reference Required")]
        [MaxLength(100, ErrorMessage = "Product Reference Too Long, max 100 characters")]
        [Remote("CheckProductReferenceExist", "Validation", ErrorMessage = "Product reference already has a group", HttpMethod = "POST")]
        public string ProductReference { get; set; }

        /// <summary>
        /// Product owner
        /// </summary>
        [DisplayName("Product Owner")]
        [Required(ErrorMessage = "Product Owner Required")]
        [MinLength(7, ErrorMessage = "Product Owner Too Short, min 7 characters")]
        [MaxLength(7, ErrorMessage = "Product Owner Too Long, max 7 characters")]
        [RegularExpression("^\\d{7}", ErrorMessage = "Product Owner must a PID eg. 1111111")]
        public string ProductOwner { get; set; }

        /// <summary>
        /// Parent id
        /// </summary>
        [ForeignKey(nameof(Parent))]        
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Parent Product group
        /// </summary>
        [JsonIgnore]
        public virtual ProductGroup Parent { get; set; }

        /// <summary>
        /// ICollection of product group childern
        /// </summary>
        public virtual ICollection<ProductGroup> Children { get; set; }
    }
}
