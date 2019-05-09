using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class FeedController : Controller
    {
        private FeedRepository _feedRepository;

        public FeedController(FeedRepository f)
        {
            _feedRepository = f;
            //var feed = _feedRepository.GetFeedById("5cd408a0594e092e04a3369a");
            //feed.Posts = new List<Post>();
            //feed.Posts.Add(new Post
            //{
            //    Text = "Her en en post"
            //});
            //feed.Posts.Add(new Post
            //{
            //    Text = "Og endnu en"
            //});
            //_feedRepository.UpdateFeed(feed);
        }
        // GET: Feed
        public ActionResult Index()
        {
            return View(_feedRepository.GetFeeds());
        }

        // GET: Feed/Details/5
        public ActionResult Details(string id)
        {
            var feed = _feedRepository.GetFeedById(id);

            if (feed != null)
                return View(feed);
            else
            {
                feed = _feedRepository.GetFeedByUserId(id);
                if (feed != null)
                    return View(feed);
                else
                {
                    feed = new Feed
                    {
                        Posts = new List<string>(),
                        User = id
                    };
                    _feedRepository.InsertFeed(feed);
                    return View(feed);
                }
            }
        }
        

        public ActionResult Create()
        {
            return View();
        }

        // POST: Feed/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] Feed f)
        {
            try
            {
                _feedRepository.InsertFeed(f);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Feed/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Feed/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Feed/Delete/5
        public ActionResult Delete(string id)
        {
            var feed = _feedRepository.GetFeedById(id);
            return View(feed);
        }

        // POST: Feed/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, Feed f)
        {
            try
            {
                _feedRepository.RemoveFeedById(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}