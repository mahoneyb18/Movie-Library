using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Rating
    {   
        [Key]
        public int RatingId { get; set; }

        [Display(Name = "Rating")]
        public string Ratings { get; set; }
        public ICollection<Film> Films { get; set; }

    }
}

