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

        public List<string> GetFeedPostsByFeedID(string id)
        {
            //List of posts 
            List<Post> FeedPosts = new List<Post>();

            //Feed, the user, and the wall are found 
            var feed = _feedRepository.GetFeedById(id);
            var user = _userRepository.GetUser(feed.User);
            var usersWall = _wallRepository.GetWallByUserId(user.UserId);

            //All posts of the users own wall are read
            if (usersWall.Posts != null)
            {
                foreach (string postID in usersWall.Posts)
                {
                    FeedPosts.Add(_postRepository.GetPost(postID));
                }
            }

            //gennemløber alle brugere som en user følger
            if(user.UserId != null) { 
                foreach (string FriendUserID in user.Following)
                {
                    if (FriendUserID != null)
                    {
                        //Tjekker om en bruger som followes har blokeret brugeren
                        var friendUser = _userRepository.GetUser(FriendUserID);
                        if (!(friendUser.Blocked.Contains(user.UserId)))
                        {
                            //indlæser den followede brugers væg, og tilføjer alle posts fra denne til den samlede liste over posts
                            var SpecificWall = _wallRepository.GetWallByUserId(FriendUserID);
                            List<string> SpecificPosts = SpecificWall.Posts;
                            foreach (string PostID in SpecificPosts)
                            {
                                FeedPosts.Add(_postRepository.GetPost(PostID));
                            }
                        }
                    }
                }
            }

            //Gå igennem alle en users circles
            if(user.Circles != null) { 
                foreach (string CircleID in user.Circles)
                {
                    //opretter liste med alle post i en circle
                    List<Post> PostsInCircle = _postRepository.GetPostsByUserCircle(CircleID);

                    if (PostsInCircle != null)
                    {
                        //gennemløber alle posts i en circle, og tjekker post owner har blokeret brugeren. 
                        foreach (Post circlePost in PostsInCircle)
                        {
                            User postOwner = _userRepository.GetUser(circlePost.OwnerId);
                            if (!(postOwner.Blocked.Contains(user.UserId)))
                            {
                                //tilføjer post til samlet oversigt over posts
                                FeedPosts.Add(circlePost);
                            }
                        }
                    }
                    
                }
            }

            //Sorterer den samlede liste over posts.
            FeedPosts.Sort(delegate (Post t1, Post t2)
                { return (t1.CreationTime.CompareTo(t2.CreationTime)); }
            );
            
            //Konverterer post listen til string liste 

            feed.Posts.Clear();

            if(FeedPosts != null) { 
                foreach (Post FeedPost in FeedPosts)
                {
                    feed.Posts.Add(FeedPost.PostId);
                }
            }

            //Returnerer liste over alle posts som en bruger har adgang til
            return feed.Posts;
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
                feed.Posts = GetFeedPostsByFeedID(id);
                return View(feed);
            }

            feed = _feedRepository.GetFeedByUserId(id);
            if (feed != null)
            {
                feed.Posts = GetFeedPostsByFeedID(feed.FeedId);
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