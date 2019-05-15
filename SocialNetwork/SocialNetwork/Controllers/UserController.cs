
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{

    public class UserController : Controller
    {
        UserRepository _userRepository;
        private FeedRepository _feedRepository;
        private WallRepository _wallRepository;
        private PostRepository _postRepository;
        public UserController(
            UserRepository userRepository, 
            FeedRepository feedRepository, 
            WallRepository wallRepository,
            PostRepository postRepository)
        {
            _userRepository = userRepository;
            _feedRepository = feedRepository;
            _wallRepository = wallRepository;
            _postRepository = postRepository;
        }
        // GET: User
        public ActionResult Index()
        {
            IList<User> users = _userRepository.GetUsers();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            ViewData["userlist"] = _userRepository.GetUsers();

            var wall = _wallRepository.GetWallByUserId(id);
            if (wall == null)
            {
                wall = new Wall { User = id };
                _wallRepository.AddWall(wall);
            }


            ViewData["wall"] = wall;
            var posts = new List<Post>();

            if (wall != null)
            {
                foreach (var post in wall.Posts)
                {
                    posts.Add(_postRepository.GetPost(post));
                }
            }

            ViewData["posts"] = posts;

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
                _feedRepository.InsertFeedFromUser(user.UserId);
                _wallRepository.AddWall(new Wall{User = user.UserId});
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