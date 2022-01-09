using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class FilmViewModel
    {
        [Key]
        public int FilmId { get; set; }
        public string Title { get; set; }
        public int RatingId { get; set; }
        public string Rating { get; set; }
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        public string GName { get; set; }
        public int Year { get; set; }

        [Display(Name = "Runtime(mins)")]
        public int Runtime { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }    


 


    }
}
