using System;
using System.ComponentModel.DataAnnotations;
namespace MyFirstAPI.Models
{
    public class Review
    {
        [Key]
        public int reviewer_id { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }

        public virtual Restaurant? Restaurant { get; set; }


        public Review()
        {
            comment = "";
        }
    }
}