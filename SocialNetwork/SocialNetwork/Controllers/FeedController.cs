using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class FeedController : Controller
    {
        private FeedRepository _feedRepository;
        private UserRepository _userRepository;

        public FeedController(FeedRepository f, UserRepository u)
        {
            _feedRepository = f;
            _userRepository = u;
        }
        // GET: Feed
        public ActionResult Index()
        {
            var feeds = _feedRepository.GetFeeds();
            return View(feeds);
        }

        // GET: Feed/Details/5
        public ActionResult Details(string id)
        {
            var feed = _feedRepository.GetFeedById(id);

            if (feed != null)
                return View(feed);
            feed = _feedRepository.GetFeedByUserId(id);
            if (feed != null)
                return View(feed);
            feed = new Feed
            {
                Posts = new List<string>(),
                User = id
            };
            _feedRepository.InsertFeed(feed);
            return View(feed);
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
        public ActionResult Edit(int id, Feed feed)
        {
            try
            {
                _feedRepository.UpdateFeed(feed);

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