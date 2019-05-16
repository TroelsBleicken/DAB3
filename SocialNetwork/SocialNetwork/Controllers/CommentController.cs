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
    public class CommentController : Controller
    {
        private CommentRepository _commentRepository;
        private PostRepository _postRepository;

        public CommentController(CommentRepository commentRepository, PostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            
        }

        // GET: Comment
        public ActionResult Index()
        {
            return View(_commentRepository.GetComments());
        }

        // GET: Comment/Details/5
        public ActionResult Details(string id)
        {
            return View(_commentRepository.GetComment(id));
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm]Comment comment)
        {
            try
            {
                comment.TimeStamp = DateTime.Now;
                _commentRepository.CreateComment(comment);
                var post = _postRepository.GetPost(comment.PostId);

                post.Comments.Add(comment.CommentId);

                _postRepository.UpdatePost(post.PostId, post);
                
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(string id)
        {
            return View(_commentRepository.GetComment(id));
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [FromForm]Comment comment)
        {
            try
            {
                _commentRepository.UpdateComment(id, comment);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(string id)
        {
            var comment = _commentRepository.GetComment(id);
            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Comment comment)
        {
            try
            {
                _commentRepository.RemoveComment(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}