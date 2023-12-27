using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;
namespace MovieStoreMvc.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly DatabaseContext ctx;
        public HomeController(IMovieService movieService, IReviewService reviewService, DatabaseContext dbContext)
        {
            ctx = dbContext;
            _movieService = movieService;
            _reviewService = reviewService;
        }
        public IActionResult Index(string term="", int currentPage = 1)
        {
            var movies = _movieService.List(term,true,currentPage);
            return View(movies);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MovieDetail(int movieId)
        {
            var movie = _movieService.GetById(movieId);
           
            return View(movie);
        }

        public IActionResult ReviewDetail(string term = "", int currentPage = 1)
        {
            var movies = _reviewService.List(term, true, currentPage);
            return View(movies);
        }
        

        [HttpPost]
        public IActionResult AddToFavorites(int id)
        {
            var result = _movieService.AddToFavorites(id);
            if (result)
            {
                TempData["msg"] = "Added to Favorites";
            }
            else
            {
                TempData["msg"] = "Error adding to Favorites";
            }
            return RedirectToAction("MovieDetail", new { movieId = id });
        }

        public IActionResult Favorites()
{
    // Lấy danh sách các bộ phim yêu thích từ service hoặc database
    var favoriteMovies = _movieService.GetFavoriteMovies();
    return View(favoriteMovies);
}

        [HttpPost]
        public IActionResult RemoveFromFavorites(int id)
        {
            var result = _movieService.RemoveFromFavorites(id);
            if (result)
            {
                TempData["msg"] = "Removed from Favorites";
            }
            else
            {
                TempData["msg"] = "Error removing from Favorites";
            }
            return RedirectToAction("MovieDetail", new { movieId = id });
        }

        
        
        }
}
