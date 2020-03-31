using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SerajAssociates.Models
{
    public class Image
    {
        [Key]
        public int ImageId {get;set;}

        public string Path {get;set;}

        public bool Cover{get;set;} = false;

    /* -------------------------------------------------------------------------------- */
    // DATETIMEs
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    /* -------------------------------------------------------------------------------- */
    // RELATIONS
        public int ProjectId {get;set;}
        public Project ThisProject{get;set;}
    }
    
}