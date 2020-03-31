using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SerajAssociates.Models
{
    public class Project
    {
        [Key]
        public int ProjectId {get;set;}


        [Required(ErrorMessage = "Project name is required.")]
        [MinLength(3, ErrorMessage ="Project must be atleast 3 characters long.")]
        public string Name {get;set;}
        public string Description {get;set;}

/* -------------------------------------------------------------------------------- */
// DATETIMEs
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

/* -------------------------------------------------------------------------------- */
// RELATIONS
        public List<Image> Album {get;set;}
    }
}