using DAL;
using EFProject.Models;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFProject.Controllers
{
    public class TweetController : Controller
    {
        private PersonService person;

        public TweetController()
        {
            person = new PersonService();
        }
        // GET: Tweet
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ListTweets()
        {
            var tweetList = person.GetTweets(User.Identity.Name);
            List<TweetModel> tweetOut = new List<TweetModel>();
            foreach (var tweet in tweetList)
            {
                tweetOut.Add(new TweetModel { Message = tweet.message, UserName = tweet.Person.fullName, Created = tweet.created,Tweet_id=tweet.tweet_id, CanEdit = (tweet.user_id == User.Identity.Name ? true : false) });
            }
            return PartialView(tweetOut);
        }

        [HttpPost]
        public JsonResult AddTweet(TweetModel tweet)
        {
            Model.Tweet newTweet = new Model.Tweet();
            newTweet.message = tweet.Message;
            newTweet.tweet_id = tweet.Tweet_id;
            newTweet.user_id = User.Identity.Name;
            person.AddOrUpdateTweet(newTweet);
            return Json("Ok");
        }
        [HttpPost]
        public JsonResult DeleteTweet(TweetModel tweet)
        {
            person.DeleteTweet(tweet.Tweet_id);
            return Json("Ok");
        }
       
        public ActionResult RegisterAccount(PersonModel personmodel)
        {
            if (!string.IsNullOrWhiteSpace(personmodel.Email))
            {
                var personDet = person.GetPersonDetails(personmodel.Email);
                return View(personDet);
            }
            return View();
        }
        [HttpPost]
        public ActionResult RegisterandUpdateAccount(PersonModel personmodel)
        {
            if (ModelState.IsValid)
            {
                Person personEntity = new Person();
                personEntity.active = true;
                personEntity.email = personmodel.Email;
                personEntity.user_id = personmodel.User_id;
                personEntity.fullName = personmodel.FullName;
                personEntity.password = personmodel.Password;
                person.RegisterOrUpdateAccount(personEntity);
                return RedirectToAction("Index", "Tweet");
            }
            return View(person);
        }
        [HttpPost]
        public JsonResult DeleteAccount(PersonModel personmodel)
        {
            person.DeleteAccount(personmodel.User_id);
            return Json("Ok");
        }
        public PartialViewResult AddTweet()
        {
           return PartialView();
        }
        public ViewResult Following()
        {
                List<PersonModel> PersonModelList = new List<PersonModel>();
            var personAll =  person.GetAllPersonDetails();
                var personFollowers = person.GetAllFollowingDetailsUser(User.Identity.Name);
                foreach (var per in personAll)
                {
                    if (personFollowers.Any(x => x.fullName == per.fullName && User.Identity.Name != per.user_id))
                    {
                        PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = true });
                    }
                    else if(User.Identity.Name != per.user_id)
                    {
                        PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = false });
                    }
                }

            return View("FindPeople", PersonModelList);
        }
        public ViewResult Followings(string searchName)
        {
            List<PersonModel> PersonModelList = new List<PersonModel>();

            var personAll = string.IsNullOrWhiteSpace(searchName) ? person.GetAllPersonDetails() : person.GetAllPersonDetails(searchName);
            var personFollowers = person.GetAllFollowingDetailsUser(User.Identity.Name);
            foreach (var per in personAll)
            {
                if (personFollowers.Any(x => x.fullName == per.fullName && User.Identity.Name != per.user_id))
                {
                    PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = true });
                }
                else if ( User.Identity.Name != per.user_id)
                {
                    PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = false });
                }
            }

            return View("FindPeople", PersonModelList);
        }
        public ViewResult GetFollowing()
        {
            List<PersonModel> PersonModelList = new List<PersonModel>();
                var personFollowers = person.GetAllFollowingDetailsUser(User.Identity.Name);
                foreach (var per in personFollowers)
                {
                    if (User.Identity.Name != per.user_id)
                    {
                        PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = true });
                    }
                }
            return View("Following", PersonModelList);
        }
        public ViewResult GetFollowers()
        {
            List<PersonModel> PersonModelList = new List<PersonModel>();
            var personFollowers = person.GetAllFollowerDetailsUser(User.Identity.Name);
            foreach (var per in personFollowers)
            {
                if (User.Identity.Name != per.user_id)
                {
                    PersonModelList.Add(new PersonModel { FullName = per.fullName, User_id = per.user_id, IsFollowing = true });
                }
            }
            return View("Followers", PersonModelList);
        }

        [HttpPost]
        public JsonResult AddFollowing(PersonModel personmodel)
        {
            person.AddFollowing(personmodel.User_id, User.Identity.Name);
            return Json("Ok");
        }

        [HttpPost]
        public JsonResult GetTweetandFollowing()
        {
            var tweetList = person.GetTweets(User.Identity.Name);
            var personFollowing = person.GetAllFollowingDetailsUser(User.Identity.Name);
            var personFollowers = person.GetAllFollowerDetailsUser(User.Identity.Name);
            return Json(new { totalTweet = tweetList.Count, following = personFollowing.Count , followers = personFollowers.Count });
        }

    }
}