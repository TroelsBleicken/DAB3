using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class WallController : Controller
    {
        // GET: Wall
        private WallRepository _wallRepository;
        private FeedRepository _feedRepository;
        private UserRepository _userRepository;

        public WallController(WallRepository wallRepository, FeedRepository feedRepository, UserRepository userRepository)
        {
            _wallRepository = wallRepository;
            _feedRepository = feedRepository;
            _userRepository = userRepository;
        }
        public ActionResult Index()
        {
            return View(_wallRepository.GetAll());
        }

        // GET: Wall/Details/5
        public ActionResult Details(string id)
        {
            var wall = _wallRepository.GetWallById(id);
            if (wall != null)
                return View(wall);

            wall = _wallRepository.GetWallByUserId(id);
            if (wall != null)
                return View(wall);

            wall = new Wall {User = id};
            _wallRepository.AddWall(wall);
            return View(wall);
        }

        public ActionResult DetailsFromUser(string viewerId, string userId)
        {
            var user = _userRepository.GetUser(userId);

            if (user.Blocked.Contains(viewerId))
                return View("BlockedPage"); 
            var wall = _wallRepository.GetWallByUserId(userId);

            ViewData["visitor"] = userId;
            return View(wall);
        }

        public ActionResult DetailsFromCircle(string circleId, string userId)
        {
            var user = _userRepository.GetUser(userId);

            var wall = _wallRepository.GetWallByCircleId(circleId);

            ViewData["visitor"] = userId;
            return View(wall);
        }

        // GET: Wall/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Wall/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Wall wall)
        {
            try
            {
                _wallRepository.AddWall(wall);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wall/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Wall/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Wall wall)
        {
            try
            {
                _wallRepository.Update(wall);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Wall/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Wall/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            try
            {
                _wallRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddPost(string postId, string WallId)
        {
            try
            {
                var wall = _wallRepository.GetWallById(WallId);
                if(wall.Posts == null)
                    wall.Posts = new List<string>();
                wall.Posts.Add(postId);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

    }
}