using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Services
{
    interface IFilmRepository
    {
       // private readonly ApplicationDbContext _dbContext;
        public List getGenreList()
        {
            List<Genre> gl = new List<Genre>();

            gl = (from g in _dbContext.Genre select g).ToList();
            gl.Insert(0, new Genre { GenreId = 0, Name = "--Select Genre--" });

           // return (ViewBag.message = gl);
            return gl;
        }




    }
}
 