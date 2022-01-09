using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Cinema.Data;
using Cinema.Models;

namespace Cinema.Controllers
{
    public class FilmController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public FilmController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index(string sortOrder)
        {
            IEnumerable<Film> objList = _dbContext.Films;
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            IEnumerable<Rating> objRatingList = _dbContext.Ratings;

            ViewBag.TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.RatingSort = sortOrder == "Rating" ? "rating_desc" : "Rating";
            ViewBag.YearSort = sortOrder == "Year" ? "year_desc" : "Year";
            ViewBag.GenreSort = sortOrder == "Genre" ? "genre_desc" : "Genre";
            ViewBag.RuntimeSort = sortOrder == "Runtime" ? "runtime_desc" : "Runtime";

            var FilmsListQuery = (from c in objList
                                  join g in objGenreList on c.GenreId equals g.GenreId
                                  from r in objRatingList
                                  where c.RatingId == r.RatingId
                                  select new FilmViewModel
                                  {
                                      Title = c.Title,
                                      FilmId = c.FilmId,
                                      Year = c.Year,
                                      Runtime = c.Runtime,
                                      Rating = r.Ratings,
                                      GName = g.GName,
                                      Description = c.Description
                                  });

            switch (sortOrder)
            {
                case "title_desc": //sort by title descending
                    FilmsListQuery = FilmsListQuery.OrderByDescending(f => f.Title);
                    break;
                case "Rating": //sort by ratings ascending
                    FilmsListQuery = FilmsListQuery.OrderBy(f => f.Rating);
                    break;
                case "rating_desc": //sort by ratings descending
                    FilmsListQuery = FilmsListQuery.OrderByDescending(f => f.Rating);
                    break;
                case "Genre": //sort by genre ascending
                    FilmsListQuery = FilmsListQuery.OrderBy(f => f.GName);
                    break;
                case "genre_desc": //sort by genre descending
                    FilmsListQuery = FilmsListQuery.OrderByDescending(f => f.GName);
                    break;
                case "Year": //sort by year ascending
                    FilmsListQuery = FilmsListQuery.OrderBy(f => f.Year);
                    break;
                case "year_desc": //sort by year descending
                    FilmsListQuery = FilmsListQuery.OrderByDescending(f => f.Year);
                    break;
                case "Runtime": //sort by runtime ascending
                    FilmsListQuery = FilmsListQuery.OrderBy(f => f.Runtime);
                    break;
                case "runtime_desc": //sort by runtime descending
                    FilmsListQuery = FilmsListQuery.OrderByDescending(f => f.Runtime);
                    break;
                default:  //sorted by title in ascending
                    FilmsListQuery = FilmsListQuery.OrderBy(f => f.Title);
                    break;
            }
            return View(FilmsListQuery.ToList());
        }

        //Get Create
        [HttpGet]
        public IActionResult Create()
        {
            // Create a dropdown of Genres
                    
            var _rlist = (from r in _dbContext.Ratings
                          select r.Ratings ).ToList();

            var _gl = (from g in _dbContext.Genres
                  select g.GName).ToList();

           // _gl.Insert(0, (new  {GenreId = 0, GName = "--Select Genre--"}));
            
            ViewBag.RatingList = _rlist;
            ViewBag.GenreList = _gl;
            return View(); 
        }

        [HttpPost]
        public IActionResult Create(FilmViewModel obj)
        {
            if (!ModelState.IsValid)
                return View(obj);

            NewModel(obj, 0);
            return RedirectToAction("Index");  
        }

        public void NewModel(FilmViewModel obj, int action)
        {
            IEnumerable<Film> objFilmList = _dbContext.Films;
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            IEnumerable<Rating> objRatingList = _dbContext.Ratings;

            // retrieve genre id 
            var genreId = (from c in objGenreList
                             where obj.GName == c.GName
                             select c.GenreId).First();

            // retrieve rating
            var ratingId = (from r in objRatingList
                              where obj.Rating == r.Ratings
                              select r.RatingId).First();

            switch (action)
            {
                case 0: //Create
                    {
                        Film film = new Film();
                        film.FilmId = obj.FilmId;
                        film.Title = obj.Title;
                        film.Year = obj.Year;
                        film.Runtime = obj.Runtime;
                        film.Description = obj.Description;
                        film.GenreId = genreId;
                        film.RatingId = ratingId;
                        _dbContext.Films.Add(film);
                        _dbContext.SaveChanges();
                        break;
                    }
                case 1: //Edit                
                    Film movie = _dbContext.Films.Find(obj.FilmId);                
                    movie.Title = obj.Title;
                    movie.Year = obj.Year;
                    movie.Runtime = obj.Runtime;
                    movie.Description = obj.Description;
                    movie.GenreId = genreId;
                    movie.RatingId = ratingId;
                    _dbContext.Films.Update(movie);
                    _dbContext.SaveChanges();
                    break;
                default:
                    break;
            }    
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Film movie = _dbContext.Films.Find(id);
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            IEnumerable<Rating> objRatingList = _dbContext.Ratings;

            // retrieve genre name 
            var genreName = (from c in objGenreList
                             where movie.GenreId == c.GenreId
                             select c.GName).First();

            // retrieve rating
            var ratingName = (from r in objRatingList
                             where movie.RatingId == r.RatingId
                             select r.Ratings).First();

            // load the view model to display in the view
            FilmViewModel view = new FilmViewModel();

            view.FilmId = movie.FilmId;
            view.Title = movie.Title;
            view.Rating = ratingName;
            view.Year = movie.Year;
            view.GName = genreName;
            view.Runtime = movie.Runtime;
            view.Description = movie.Description;

            return View(view);
        }
        
        [HttpGet]
        public IActionResult Edit(int? id)
        {        
           if (id == null || id <= 0)
                return BadRequest();

            Film movie = _dbContext.Films.Find(id);
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            IEnumerable<Rating> objRatingList = _dbContext.Ratings;

            if (movie == null)
                return NotFound();

            // retrieve genre name 
            var genreName = (from c in objGenreList
                             where movie.GenreId == c.GenreId
                             select c.GName).First();

            // retrieve rating
            var ratingName = (from r in objRatingList
                              where movie.RatingId == r.RatingId
                              select r.Ratings).First();

            FilmViewModel view = new FilmViewModel();

            view.FilmId = movie.FilmId;
            view.Title = movie.Title;
            view.Year = movie.Year;
            view.GName = genreName;
            view.Rating = ratingName;
            view.Runtime = movie.Runtime;
            view.RatingId = movie.RatingId;
            view.GenreId = movie.GenreId;
            view.Description = movie.Description;

            var _rlist = (from r in _dbContext.Ratings
                          select r.Ratings).ToList();

            var _gl = (from g in _dbContext.Genres
                       select g.GName).ToList();

            ViewBag.RatingList = _rlist;
            ViewBag.GenreList = _gl;

            return View(view);        
        }

        [HttpPost]
        public IActionResult Edit(FilmViewModel obj)
        {
            if (!ModelState.IsValid)
                return View(obj);


            NewModel(obj, 1);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Film movie = _dbContext.Films.Find(id);
            IEnumerable<Genre> objGenreList = _dbContext.Genres;
            IEnumerable<Rating> objRatingList = _dbContext.Ratings;

            // retrieve genre name 
            var genreName = (from c in objGenreList
                             where movie.GenreId == c.GenreId
                             select c.GName).First();

            // retrieve rating
            var ratingName = (from r in objRatingList
                              where movie.RatingId == r.RatingId
                              select r.Ratings).First();

            // load the view model to display in the view
            FilmViewModel view = new FilmViewModel();

            view.FilmId = movie.FilmId;
            view.Title = movie.Title;
            view.Rating = ratingName;
            view.Year = movie.Year;
            view.GName = genreName;
            view.Runtime = movie.Runtime;
            view.Description = movie.Description;

            return View(view);
        }

        [HttpPost]
        public IActionResult Deleted(int id)
        {
            if (!ModelState.IsValid)
                return View();     

            Film movie = _dbContext.Films.Find(id);
            _dbContext.Films.Remove(movie);      
            _dbContext.SaveChanges();
            return RedirectToAction("Index");           
        }

    }
}

 