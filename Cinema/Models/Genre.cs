using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Display(Name = "Genre")]
        public string GName { get; set; }
        public ICollection<Film> Films { get; set; }

    }
}
