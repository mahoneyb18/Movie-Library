using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Film
    {
        [Key]
        public int FilmId { get; set; }     
        
        [Required(ErrorMessage ="Film Title is required.")]
        [Display(Name = "Film Title")]
        public string Title { get; set; }
        
        [Display(Name = "Year Released")]
        public int Year { get; set; }

        [Display(Name = "Runtime")]
        public int Runtime { get; set; }

        [Display(Name = "About")]
        public string Description { get; set; }
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int RatingId { get; set; }
        public Rating Rating { get; set; }
    }
}
//add-migration addFilmToDatabase