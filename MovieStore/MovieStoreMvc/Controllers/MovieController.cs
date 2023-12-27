using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieStoreMvc.Models.Domain;
using MovieStoreMvc.Repositories.Abstract;


namespace MovieStoreMvc.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileService _fileService;
        private readonly IGenreService _genService;
        private readonly DatabaseContext _context;
        public MovieController(DatabaseContext context, IGenreService genService,IMovieService MovieService, IFileService fileService)
        {
            _movieService = MovieService;
            _fileService = fileService;
            _genService = genService;
           
            _context = context;
        }
        public IActionResult Add()
        {
            var model = new Movie();
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Movie model)
        {
            model.GenreList = _genService.List().Select(a => new SelectListItem { Text = a.GenreName, Value = a.Id.ToString() });
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Add(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(Add));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult Edit(int id)
        {
            var model = _movieService.GetById(id);
            var selectedGenres = _movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Movie model)
        {
            var selectedGenres = _movieService.GetGenreByMovieId(model.Id);
            MultiSelectList multiGenreList = new MultiSelectList(_genService.List(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            if (!ModelState.IsValid)
                return View(model);
            if (model.ImageFile != null)
            {
                var fileReult = this._fileService.SaveImage(model.ImageFile);
                if (fileReult.Item1 == 0)
                {
                    TempData["msg"] = "File could not saved";
                    return View(model);
                }
                var imageName = fileReult.Item2;
                model.MovieImage = imageName;
            }
            var result = _movieService.Update(model);
            if (result)
            {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction(nameof(MovieList));
            }
            else
            {
                TempData["msg"] = "Error on server side";
                return View(model);
            }
        }

        public IActionResult MovieList()
        {
            var data = this._movieService.List();
            return View(data);
        }

        public IActionResult Delete(int id) 
        {
            var result = _movieService.Delete(id);
            return RedirectToAction(nameof(MovieList));
        }

        public IActionResult Favorites()
        {
            // Lấy danh sách các bộ phim yêu thích từ service hoặc database
            var favoriteMovies = _movieService.GetFavoriteMovies();
            return View(favoriteMovies);
        }


        [HttpPost]
        public IActionResult AddToFavorites(int id)
        {
            var result = _movieService.AddToFavorites(id);
            return RedirectToAction("Favorites");
        }

        [HttpPost]
        // public IActionResult RemoveFromFavorites(int id)
        // {
        //     var result = _movieService.RemoveFromFavorites(id);
        //     return RedirectToAction("Favorites");
        // }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromFavorites(int id)
        {
            var result = _movieService.RemoveFromFavorites(id);
            if (result)
            {
                TempData["msg"] = "Removed from Favorites Successfully";
            }
            else
            {
                TempData["msg"] = "Error removing from Favorites";
            }
            return RedirectToAction("Favorites");
        }



    }
}
