using System;
using System.ComponentModel.DataAnnotations;
namespace MyFirstAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int restaurant_id { get; set; }
        public string name { get; set; }

        public Restaurant()
        {
            name = "";
        }
    }
}