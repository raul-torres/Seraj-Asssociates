using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerajAssociates.Models
{
    public class Admin
    {
        [Key]
        public int AdminId{get;set;}
        

        [Required(ErrorMessage = "You need to have a username")]
        public string LoginName{get;set;}


        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage ="Password must be atleast 8 characters long")]
        [Required(ErrorMessage = "Password is required")]
        [RegularExpression("^.*(?=.{6,18})(?=.*)(?=.*[A-Za-z])(?=.*[@%&#%^&*!]{1,}).*$", ErrorMessage = "Password must contain atleast 1 letter, 1 number and 1 special character")]
        public string Password{get;set;}
    /* -------------------------------------------------------------------------------- */
    // DATETIMEs
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    /* -------------------------------------------------------------------------------- */
    // PASSWORD COMPARING
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmedPassword {get;set;}
    }
}