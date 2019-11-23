﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameSphere.Models;

namespace GameSphere.Controllers
{
    public class HomeController : Controller
    {   //TODO allow users to click on following and followers to see who is following them
        //TODO allow users to post to the wall and reply to posts
        //TODO make home profile page with quiz answers showing
        //testingdata
        #region

        public HomeController()
        {
            if(Repository.Users.Count == 0)
            {
                User test1 = new User()
                {
                    UserName = "test",
                    Game = "Call of Duty",
                    Console = "Xbox",
                    Genre = "FPS",
                    Platform = "Twitch",
                    Privacy = true
                };               
                User test2 = new User()
                {
                    UserName = "test2",
                    Game = "Halo 4",
                    Console = "PC",
                    Genre = "Horror",
                    Platform = "YoutubeGaming",
                    Privacy = false                  
                };               
                Post p = new Post()
                {
                    User = test2,
                    Message = "This new site is amazing"
                };
                User test3 = new User()
                {
                    UserName = "test3",
                    Game = "Halo 5",
                    Console = "PC",
                    Genre = "Horror",
                    Platform = "YoutubeGaming",
                    Privacy = false
                };
                Post p2 = new Post()
                {
                    User = test3,
                    Message = "This is WAY COOLER THAN FACEBOOK"
                };
                test2.AddPost(p);
                test3.AddPost(p2);
                test1.AddFollowing(test2);
                test1.AddFollowing(test3);
                test1.AddFollower(test2);
                test2.AddFollower(test1);
                test2.AddFollowing(test1);
                test3.AddFollower(test1);
                Repository.Users.Add(test3);
                Repository.Users.Add(test2);
                Repository.Users.Add(test1);
            }
        }
        #endregion

        [HttpGet]
        public IActionResult Index()
        {         
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Index(string u)
        {         
            User user = Repository.GetUserByUserName(u);          
            if (user == null)
                return RedirectToAction("Index");
            else
                return RedirectToAction("HomePage", user);
        }

        public ActionResult HomePage(User user)
        {           
            User u = Repository.GetUserByUserName(user.UserName);
            ViewBag.postCount = u.Posts.Count;
            ViewBag.followingCount = u.Following.Count;
            ViewBag.followerCount = u.Followers.Count;
            return View(u);
        }

        [HttpGet]
        public IActionResult SignUp()
        {          
            return View();
        }

        [HttpPost]
        public RedirectToActionResult SignUp(string username, string game, string console,
                                             string genre, string platform, bool privacy)
        {
            User user = new User();
            user.UserName = username;
            user.Game = game;
            user.Console = console;
            user.Genre = genre;
            user.Platform = platform;
            user.Privacy = privacy;
            Repository.Users.Add(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Privacy(string title)
        {         
            return View("Privacy", title);
        }

        [HttpPost]
        public RedirectToActionResult Privacy(string title, bool privacy)
        {
            User user = Repository.GetUserByUserName(title);
            user.changeUserPrivacy(privacy);
            return RedirectToAction("Homepage", user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
