using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    /// <summary>
    /// Base class for all products
    /// </summary>
    public class Item : IValidatableObject
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public string Category { get; set; }
        [Required]
        public string SubCategory { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Unit { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Range(1, 10)]
        [DisplayName("Rate the Product")]
        public int Rating { get; set; }
        
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var field = new[] { "Description" };

            if (Description.ToLower().Contains(" bad words") || Description.ToLower().Contains(" really abd words "))
                yield return new ValidationResult("Obscene words are not allows in description");

        }
    }
}
