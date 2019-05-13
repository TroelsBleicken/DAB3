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
        public CircleController(CircleRepository circleRepository, UserRepository userRepository) 
        {
            _circleRepository = circleRepository;
            _userRepository = userRepository;
        }
        public ActionResult Index()
        {
            return View(_circleRepository.GetCircles());
        }

        // GET: Circle/Details/5
        public ActionResult Details(string id)
        {

            var circle = _circleRepository.GetCircleById(id);
            var list = new List<string>();
            foreach (var userId in circle.Users)
            {
                list.Add(_userRepository.GetUser(userId).Name);
            }

            ViewData["Message"] = list;
            
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

        public ActionResult AddUser(string UserId, string CircleName)
        {
            try
            {
                var circle = _circleRepository.GetCircleByName(CircleName);
                if(circle.Users == null)
                    circle.Users = new List<string>();

                circle.Users.Add(UserId);
                _circleRepository.UpdateCircle(circle);
                var user = _userRepository.GetUser(UserId);
                user.Circles.Add(CircleName);

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