using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinema.Data;
using Cinema.Models;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GenreController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index1()
        {
            IEnumerable<Genre> objList = _dbContext.Genres;
            return View(objList);
        }
        public IActionResult Index()
        {
            List<Genre> gl = new List<Genre>();
            gl = (from g in _dbContext.Genres select g).ToList();
            gl.Insert(0, new Genre { GenreId = 0, GName = "--Select Genre--" });

            ViewBag.message = gl;
            return View();
        }

        //Get Create
        public IActionResult Create()
        {           
            return View();
        }
    }
}

 