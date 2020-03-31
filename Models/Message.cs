using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SerajAssociates.Models
{
    public class Message
    {
        [Key]
        public int MessageId {get; set;}

        [Required(ErrorMessage="Name is required")]
        public string Name {get;set;}

        [Required(ErrorMessage="Email address is required")]
        [EmailAddress]
        public string Email{get;set;}

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Home Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
                
        [Required(ErrorMessage="Message is required")]
        [MinLength(5, ErrorMessage="Message must be at least 5 characters")]
        public string Notes{get;set;}
        
        public bool IsRead{get;set;} = false;

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}