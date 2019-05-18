using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork.DAL;
using SocialNetwork.Models;

namespace SocialNetwork
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            DataDropAndSeed();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<UserRepository>();
            services.AddScoped<CircleRepository>();
            services.AddScoped<FeedRepository>();
            services.AddScoped<PostRepository>();
            services.AddScoped<WallRepository>();
            services.AddScoped<CommentRepository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void DataDropAndSeed()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase("SocialNetworkDb");
            var database = client.GetDatabase("SocialNetworkDb");

            #region Users
            var userCollec = database.GetCollection<User>("Users");
            User seedUser1 = new User
            {
                Age = 35,
                Name = "Karsten",
                Gender = "Mand",
                Location = "Amager", 
            };
            User seedUser2 = new User
            {
                Age = 45,
                Name = "Gert",
                Gender = "Mand",
                Location = "Horsens",
                Following = new List<string> { seedUser1.UserId }
            };
            User seedUser3 = new User
            {
                Age = 52,
                Name = "Per",
                Gender = "Mand",
                Location = "Gellerup",
                Following = new List<string> { seedUser2.UserId }
            };
            User seedUser4 = new User
            {
                Age = 38,
                Name = "Jørgen",
                Gender = "Mand",
                Location = "Randers",
                Following = new List<string> { seedUser1.UserId }
            };
            User seedUser5 = new User
            {
                Age = 42,
                Name = "John",
                Gender = "Mand",
                Location = "Herning",
                Following = new List<string> { seedUser2.UserId }
            };
            
            userCollec.InsertOne(seedUser1);
            userCollec.InsertOne(seedUser2);
            userCollec.InsertOne(seedUser3);
            userCollec.InsertOne(seedUser4);
            userCollec.InsertOne(seedUser5);

            seedUser1.Followers = new List<string> { seedUser2.UserId, seedUser4.UserId };
            seedUser2.Following = new List<string> { seedUser1.UserId};
            seedUser4.Following = new List<string> { seedUser1.UserId};
            
            seedUser2.Followers = new List<string> { seedUser3.UserId, seedUser5.UserId };
            seedUser3.Following = new List<string>{seedUser2.UserId};
            seedUser5.Following = new List<string>{seedUser2.UserId};

            seedUser2.Blocked.Add(seedUser4.UserId);
            seedUser1.Blocked.Add(seedUser5.UserId);

            #endregion

            #region Circles

           
            var circleCollec = database.GetCollection<Circle>("Circles");
            Circle seedCircle1 = new Circle
            {
                Name = "Børges Bajere og Burger bonanza",
                Users = new List<string> { seedUser1.UserId, seedUser2.UserId}
            };
            Circle seedCircle2 = new Circle
            {
                Name = "Frederiks Fede Fredagsbar",
                Users = new List<string> { seedUser5.UserId, seedUser3.UserId }
            };
            circleCollec.InsertOne(seedCircle1);
            circleCollec.InsertOne(seedCircle2);

            seedUser1.Circles = new List<string>{seedCircle1.CircleId};
            seedUser2.Circles = new List<string> { seedCircle1.CircleId };

            seedUser3.Circles = new List<string> { seedCircle2.CircleId };
            seedUser5.Circles = new List<string> { seedCircle2.CircleId };

            #endregion

            #region Walls

            var wallCollec = database.GetCollection<Wall>("Walls");
            Wall wallcircle1 = new Wall
            {
                
                Circle = seedCircle1.CircleId
            };
            Wall wallcircle2 = new Wall
            {
                
                Circle = seedCircle2.CircleId
            };
            Wall walluser1 = new Wall
            {
                User = seedUser1.UserId
            };
            Wall walluser2 = new Wall
            {
                User = seedUser2.UserId
            };
            Wall walluser3 = new Wall
            {
                User = seedUser3.UserId
            };
            Wall walluser4 = new Wall
            {
                User = seedUser4.UserId
            };
            Wall walluser5 = new Wall
            {
                User = seedUser5.UserId
            };
            wallCollec.InsertOne(wallcircle1);
            wallCollec.InsertOne(wallcircle2);
            wallCollec.InsertOne(walluser1);
            wallCollec.InsertOne(walluser2);
            wallCollec.InsertOne(walluser3);
            wallCollec.InsertOne(walluser4);
            wallCollec.InsertOne(walluser5);

            seedCircle1.WallId = wallcircle1.WallId;
            seedCircle2.WallId = wallcircle2.WallId;

            #endregion

            #region Feeds

            var feedCollec = database.GetCollection<Feed>("Feeds");
            Feed userfeed1 = new Feed
            {
                User = seedUser1.UserId
            };
            Feed userfeed2 = new Feed
            {
                User = seedUser2.UserId
            };
            Feed userfeed3 = new Feed
            {
                User = seedUser3.UserId
            };
            Feed userfeed4 = new Feed
            {
                User = seedUser4.UserId
            };
            Feed userfeed5 = new Feed
            {
                User = seedUser5.UserId
            };
            feedCollec.InsertOne(userfeed1);
            feedCollec.InsertOne(userfeed2);
            feedCollec.InsertOne(userfeed3);
            feedCollec.InsertOne(userfeed4);
            feedCollec.InsertOne(userfeed5);

            #endregion


            #region Posts

            var postCollec = database.GetCollection<Post>("Posts");
            Post circlePost1 = new Post
            {
                Type = "Text",
                Text = "Så er det sgu snart ferie gutter! Bajere, Burgere, Brunetter! WOOP WOOP",
                CreationTime = DateTime.Now,
                CircleId = seedCircle1.CircleId,
                OwnerId = seedUser1.UserId,
            };
            postCollec.InsertOne(circlePost1);
            wallcircle1.Posts.Add(circlePost1.PostId);
            userfeed1.Posts.Add(circlePost1.PostId);
            userfeed2.Posts.Add(circlePost1.PostId);

            Post circlePost2 = new Post
            {
                Type = "Picture",
                Text = "Til næste bar er der strip, her er et billede af kæmpe kasser til at varme jer op! ;)",
                CreationTime = DateTime.Now,
                CircleId = seedCircle2.CircleId,
                OwnerId = seedUser5.UserId,
            };
            postCollec.InsertOne(circlePost2);
            wallcircle2.Posts.Add(circlePost2.PostId);
            userfeed3.Posts.Add(circlePost2.PostId);
            userfeed5.Posts.Add(circlePost2.PostId);
            Post userPost1 = new Post
            {
                Type = "Video",
                Text = "Hold da op en kæmpe spasser på den her video! HAHAHAHAHA",
                CreationTime = DateTime.Now,
                OwnerId = seedUser2.UserId,
            };
            postCollec.InsertOne(userPost1);
            userfeed2.Posts.Add(userPost1.PostId);
            walluser2.Posts.Add(userPost1.PostId);
            userfeed3.Posts.Add(userPost1.PostId);
            walluser3.Posts.Add(userPost1.PostId);
            Post userPost2 = new Post
            {
                Type = "Text",
                Text = "At seede data til en database er godt nok noget lort...",
                CreationTime = DateTime.Now,
                OwnerId = seedUser3.UserId,
            };
            postCollec.InsertOne(userPost2);
            walluser3.Posts.Add(userPost2.PostId);
            userfeed3.Posts.Add(userPost2.PostId);
            Post userPost3 = new Post
            {
                Type = "Text",
                Text = "Tillykke med fødselsdagen... Håber din røv bgegynder at klø og at dine arme er for korte! MWUHAHAHAHAHA",
                CreationTime = DateTime.Now,
                OwnerId = seedUser4.UserId,
            };
            postCollec.InsertOne(userPost3);
            walluser5.Posts.Add(userPost3.PostId);
            userfeed5.Posts.Add(userPost3.PostId);
            userfeed4.Posts.Add(userPost3.PostId);

            #endregion


            #region Comments

            var commentCollec = database.GetCollection<Comment>("Comments");
            Comment comment1 = new Comment
            {
                PostId = userPost3.PostId,
                CommenterId = seedUser5.UserId,
                TimeStamp = DateTime.Now,
                Text = "Tak for den mega søde kommentar smukke"
            };
            commentCollec.InsertOne(comment1);
            userPost3.Comments.Add(comment1.CommentId);

            Comment comment2 = new Comment
            {
                PostId = circlePost1.PostId,
                CommenterId = seedUser2.UserId,
                TimeStamp = DateTime.Now,
                Text = " FERIE OG BABSER!"
            };
            commentCollec.InsertOne(comment2);
            circlePost1.Comments.Add(comment2.CommentId);
            Comment comment3 = new Comment
            {
                PostId = circlePost1.PostId,
                CommenterId = seedUser1.UserId,
                TimeStamp = DateTime.Now,
                Text = "BURGERE OG BAJERE!"
            };
            commentCollec.InsertOne(comment3);
            circlePost1.Comments.Add(comment2.CommentId);

            #endregion

            var comfilter = Builders<Comment>.Filter.Eq(c => c.CommentId, comment1.CommentId);
            commentCollec.ReplaceOne(comfilter, comment1, new UpdateOptions { IsUpsert = true });
            comfilter = Builders<Comment>.Filter.Eq(c => c.CommentId, comment2.CommentId);
            commentCollec.ReplaceOne(comfilter, comment2, new UpdateOptions { IsUpsert = true });
            comfilter = Builders<Comment>.Filter.Eq(c => c.CommentId, comment3.CommentId);
            commentCollec.ReplaceOne(comfilter, comment3, new UpdateOptions { IsUpsert = true });

            var pfilter = Builders<Post>.Filter.Eq(p => p.PostId, circlePost1.PostId);
            postCollec.ReplaceOne(pfilter, circlePost1, new UpdateOptions { IsUpsert = true });
            pfilter = Builders<Post>.Filter.Eq(p => p.PostId, circlePost2.PostId);
            postCollec.ReplaceOne(pfilter, circlePost2, new UpdateOptions { IsUpsert = true });
            pfilter = Builders<Post>.Filter.Eq(p => p.PostId, userPost1.PostId);
            postCollec.ReplaceOne(pfilter, userPost1, new UpdateOptions { IsUpsert = true });
            pfilter = Builders<Post>.Filter.Eq(p => p.PostId, userPost2.PostId);
            postCollec.ReplaceOne(pfilter, userPost2, new UpdateOptions { IsUpsert = true });
            pfilter = Builders<Post>.Filter.Eq(p => p.PostId, userPost3.PostId);
            postCollec.ReplaceOne(pfilter, userPost3, new UpdateOptions { IsUpsert = true });

            var ffilter = Builders<Feed>.Filter.Eq(f => f.FeedId, userfeed1.FeedId);
            feedCollec.ReplaceOne(ffilter, userfeed1, new UpdateOptions { IsUpsert = true });
            ffilter = Builders<Feed>.Filter.Eq(f => f.FeedId, userfeed2.FeedId);
            feedCollec.ReplaceOne(ffilter, userfeed2, new UpdateOptions { IsUpsert = true });
            ffilter = Builders<Feed>.Filter.Eq(f => f.FeedId, userfeed3.FeedId);
            feedCollec.ReplaceOne(ffilter, userfeed3, new UpdateOptions { IsUpsert = true });
            ffilter = Builders<Feed>.Filter.Eq(f => f.FeedId, userfeed4.FeedId);
            feedCollec.ReplaceOne(ffilter, userfeed4, new UpdateOptions { IsUpsert = true });
            ffilter = Builders<Feed>.Filter.Eq(f => f.FeedId, userfeed5.FeedId);
            feedCollec.ReplaceOne(ffilter, userfeed5, new UpdateOptions { IsUpsert = true });

            var wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, walluser1.WallId);
            wallCollec.ReplaceOne(wfilter, walluser1, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, walluser2.WallId);
            wallCollec.ReplaceOne(wfilter, walluser2, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, walluser3.WallId);
            wallCollec.ReplaceOne(wfilter, walluser3, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, walluser4.WallId);
            wallCollec.ReplaceOne(wfilter, walluser4, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, walluser5.WallId);
            wallCollec.ReplaceOne(wfilter, walluser5, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, wallcircle1.WallId);
            wallCollec.ReplaceOne(wfilter, wallcircle1, new UpdateOptions { IsUpsert = true });
            wfilter = Builders<Wall>.Filter.Eq(w => w.WallId, wallcircle2.WallId);
            wallCollec.ReplaceOne(wfilter, wallcircle2, new UpdateOptions { IsUpsert = true });

            var cfilter = Builders<Circle>.Filter.Eq(c => c.CircleId, seedCircle1.CircleId);
            circleCollec.ReplaceOne(cfilter, seedCircle1, new UpdateOptions { IsUpsert = true });
            cfilter = Builders<Circle>.Filter.Eq(c => c.CircleId, seedCircle2.CircleId);
            circleCollec.ReplaceOne(cfilter, seedCircle2, new UpdateOptions { IsUpsert = true });

            userCollec.ReplaceOne(Builders<User>.Filter.Eq(u => u.UserId, seedUser1.UserId), seedUser1, new UpdateOptions { IsUpsert = true });
            userCollec.ReplaceOne(Builders<User>.Filter.Eq(u => u.UserId, seedUser2.UserId), seedUser2, new UpdateOptions { IsUpsert = true });
            userCollec.ReplaceOne(Builders<User>.Filter.Eq(u => u.UserId, seedUser3.UserId), seedUser3, new UpdateOptions { IsUpsert = true });
            userCollec.ReplaceOne(Builders<User>.Filter.Eq(u => u.UserId, seedUser4.UserId), seedUser4, new UpdateOptions { IsUpsert = true });
            userCollec.ReplaceOne(Builders<User>.Filter.Eq(u => u.UserId, seedUser5.UserId), seedUser5, new UpdateOptions { IsUpsert = true });
        }
    }
}
