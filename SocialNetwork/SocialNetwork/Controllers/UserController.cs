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

    public class UserController : Controller
    {
        UserRepository _userRepository;
        private FeedRepository _feedRepository;
        public UserController(UserRepository userRepository, FeedRepository feedRepository)
        {
            _userRepository = userRepository;
            _feedRepository = feedRepository;
        }
        // GET: User
        public ActionResult Index()
        {
            return View(_userRepository.GetUsers());
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {

            return View(_userRepository.GetUser(id));
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]User user)
        {
            try
            {
                _userRepository.CreateUser(user);
                _feedRepository.InsertFeedFromUser(user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_userRepository.GetUser(id));
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [FromForm]User user)
        {
            try
            {
                _userRepository.UpdateUser(user.UserId, user);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            var user = _userRepository.GetUser(id);
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, User user)
        {
            try
            {
                _userRepository.RemoveUser(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFollower(string userId, string userToFollowId)
        {
            try
            {
                _userRepository.AddFollower(userId,userToFollowId);
                _userRepository.AddFollowing(userToFollowId,userId);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFollowing(string userId, string userToFollowingId)
        {
            try
            {
                _userRepository.AddFollowing(userId, userToFollowingId);
                _userRepository.AddFollower(userToFollowingId, userId);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBlocking(string userId, string userToBlockId)
        {
            try
            {
                _userRepository.AddBlocked(userId, userToBlockId);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


    }
}