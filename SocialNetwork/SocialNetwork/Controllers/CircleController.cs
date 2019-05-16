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
    public class CircleController : Controller
    {
        // GET: Circle
        private CircleRepository _circleRepository;
        private UserRepository _userRepository;
        private WallRepository _wallRepository;
        public CircleController(CircleRepository circleRepository, UserRepository userRepository, WallRepository wallRepository) 
        {
            _circleRepository = circleRepository;
            _userRepository = userRepository;
            _wallRepository = wallRepository;
        }
        public ActionResult Index()
        {
            return View(_circleRepository.GetCircles());
        }

        // GET: Circle/Details/5
        public ActionResult Details(string id, string loggedId)
        {

            var circle = _circleRepository.GetCircleById(id);

            
            var user = new List<string>();
            foreach (var userId in circle.Users)
            {
                user.Add(_userRepository.GetUser(userId).Name);
            }

            ViewData["Message"] = user;
            ViewData["userId"] = loggedId;
            return View(circle);
        }

        // GET: Circle/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Circle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Circle circle)
        {
            try
            {
                _circleRepository.InsertCircle(circle);


                var wall = _wallRepository.AddWall(new Wall
                {
                    Circle = circle.CircleId
                });

                circle.WallId = wall.WallId;
                _circleRepository.UpdateCircle(circle);
                return RedirectToAction(nameof(Index));
            } 
            catch
            {
                return View();
            }
        }

        // GET: Circle/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_circleRepository.GetCircleById(id));
        }

        // POST: Circle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, IFormCollection collection)
        {
            try
            {
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Circle/Delete/5
        public ActionResult Delete(string id)
        {
            return View(_circleRepository.GetCircleById(id));
        }

        // POST: Circle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, IFormCollection collection)
        {
            try
            {
                _circleRepository.RemoveCircle(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //Problem at det er circlename og ikke circleID????
        public ActionResult AddUser(string UserId, string CircleId)
        {
            try
            {
                var circle = _circleRepository.GetCircleById(CircleId);
                if(circle.Users == null)
                    circle.Users = new List<string>();

                circle.Users.Add(UserId);
                _circleRepository.UpdateCircle(circle);
                var user = _userRepository.GetUser(UserId);
                if(user.Circles == null)
                    user.Circles = new List<string>();

                user.Circles.Add(CircleId);

                _userRepository.UpdateUser(user.UserId, user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
            
        }

    }
}