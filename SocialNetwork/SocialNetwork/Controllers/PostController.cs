using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class PostController : Controller
    {
        PostRepository _postRepository;
        private WallRepository _wallRepository;
        public PostController(PostRepository postRepository, WallRepository wallRepository)
        {
            _postRepository = postRepository;
            _wallRepository = wallRepository;
        }
        // GET: Post
        public ActionResult Index()
        {
            return View(_postRepository.GetPosts());
        }

        // GET: Post/Details/5
        public ActionResult Details(string id)
        {
            return View(_postRepository.GetPost(id));
        }

        // GET: Post/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]Post post, string id)
        {
            try
            {
                if(string.IsNullOrEmpty(post.OwnerId))
                    post.OwnerId = id;

                _postRepository.CreatePost(post);
                var wall = _wallRepository.GetWallByUserId(post.OwnerId);

                wall.Posts.Add(post.PostId);

                _wallRepository.Update(wall);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_postRepository.GetPost(id));
        }

        // POST: Post/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [FromForm]Post post)
        {
            try
            {
                _postRepository.UpdatePost(id, post);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Post/Delete/5
        public ActionResult Delete(string id)
        {
            var post = _postRepository.GetPost(id);
            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Post post)
        {
            try
            {
                _postRepository.RemovePost(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(string postId,string userId, string comment)
        {
            try
            {
                _postRepository.AddComment(postId,userId,comment);

                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
        
    }
}