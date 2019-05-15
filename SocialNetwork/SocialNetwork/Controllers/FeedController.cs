using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork.Controllers
{
    public class FeedController : Controller
    {
        private FeedRepository _feedRepository;
        private UserRepository _userRepository;
    private PostRepository _postRepository;
        private WallRepository _wallRepository;
        private CircleRepository _circleRepository;

        public FeedController(FeedRepository f, UserRepository u, PostRepository p, WallRepository w, CircleRepository c)
        {
            _feedRepository = f;
            _userRepository = u;
            _postRepository = p;
            _wallRepository = w;
            _circleRepository = c;

    }

        public List<string> GetFeedPostsByUserID(string id)
        {
            List<Post> FeedPosts = new List<Post>();
            var feed = _feedRepository.GetFeedById(id);
            var user = _userRepository.GetUser(feed.User);

            if(user != null) { 
                foreach (string UserID in user.Following)
                {
                    var SpecificWall = _wallRepository.GetWallByUserId(UserID);
                    List<string> SpecificPosts = SpecificWall.Posts;
                    foreach (string PostID in SpecificPosts)
                    {
                        FeedPosts.Add(_postRepository.GetPost(PostID));
                    }
                }
            }

            if(user.Circles != null) { 
                foreach (string CircleID in user.Circles)
                {
                    FeedPosts.Concat(_postRepository.GetPostsByUserCircle(CircleID));

                }
            }

            List<Post> SortedFeedPosts = FeedPosts.OrderBy(o => o.CreationTime).ToList();
            List<string> SortedFeedPostsIDs = new List<string>();

            if(SortedFeedPostsIDs != null) { 
                foreach (Post SortedFeedPost in SortedFeedPosts)
                {
                    SortedFeedPostsIDs.Add(SortedFeedPost.PostId);
                }
            }

            return SortedFeedPostsIDs;
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
            {
                feed.Posts = GetFeedPostsByUserID(feed.User);
                return View(feed);
            }

            feed = _feedRepository.GetFeedByUserId(id);
            if (feed != null)
            {
                feed.Posts = GetFeedPostsByUserID(feed.User);
                return View(feed);
            }

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