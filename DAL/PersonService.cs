using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;

namespace DAL
{
    public class PersonService
    {
        FSDEntities contextEntities;
        public PersonService()
        {
            contextEntities = new FSDEntities();
        }
        public void SavePerson(Person person)
        {
            person.active = true;
            person.user_id = person.email;
            person.joined = DateTime.Now;
            contextEntities.People.Add(person);
            contextEntities.SaveChanges();
        }
        public bool ValidateUser(string userName, string password)
        {
            var person = contextEntities.People.FirstOrDefault(x => x.email == userName && x.password == password && x.active == true);
            if (person != null)
            {
                return true;
            }
            return false;
        }
        public bool ValidateUserName(string userName)
        {
            var person = contextEntities.People.FirstOrDefault(x => x.email == userName);
            if (person != null)
            {
                return false;
            }
            return true;
        }
        public List<Tweet> GetTweets(string user_id)
        {
            var tweetEntity = contextEntities.Tweets.Where(x=>x.user_id == user_id).ToList();
            var currectPerson = contextEntities.People.FirstOrDefault(x => x.user_id == user_id);
            var addTeet = currectPerson.People.SelectMany(yy => yy.Tweets).ToList();
            tweetEntity.AddRange(addTeet);
            return tweetEntity.OrderByDescending(x=>x.created).ToList();
        }
        public void AddOrUpdateTweet(Tweet tweet)
        {
            Tweet tweetEntity = contextEntities.Tweets.FirstOrDefault(x => x.tweet_id == tweet.tweet_id);
            if (tweetEntity != null)
            {
                tweetEntity.message = tweet.message;
                contextEntities.SaveChanges();
            }
            else if (!string.IsNullOrEmpty(tweet.user_id))
            {
                tweetEntity = new Tweet();
                tweetEntity.message = tweet.message;
                tweetEntity.user_id = tweet.user_id;
                tweetEntity.created = DateTime.Now;
                contextEntities.Tweets.Add(tweetEntity);
                contextEntities.SaveChanges();
            }
        }
        public void RegisterOrUpdateAccount(Person person)
        {
            Person personEntity = contextEntities.People.FirstOrDefault(x => x.user_id == person.user_id);
            if (personEntity != null)
            {
                contextEntities.SaveChanges();
            }
            else if (!string.IsNullOrEmpty(person.user_id))
            {
                person.joined = DateTime.Now;
                person.active = true;
                contextEntities.People.Add(person);
                contextEntities.SaveChanges();
            }
        }
        public void DeleteTweet(int tweet_id)
        {
            var tweetEntity = contextEntities.Tweets.FirstOrDefault(x => x.tweet_id == tweet_id);
            if (tweetEntity != null)
            {
                contextEntities.Tweets.Remove(tweetEntity);
                contextEntities.SaveChanges();
            }
        }
        public void DeleteAccount(string user_id)
        {
            var peopleEntity = contextEntities.People.FirstOrDefault(x => x.user_id == user_id);
            if (peopleEntity != null)
            {
                peopleEntity.active = false;
                contextEntities.SaveChanges();
            }
        }
        public void AddFollowing(string following_user_id, string user_id)
        {
            var peopleEntity = contextEntities.People.FirstOrDefault(x => x.user_id == user_id);
            var followingEntity = contextEntities.People.FirstOrDefault(x => x.user_id == following_user_id);
            if (peopleEntity != null)
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@p0", peopleEntity.user_id));
                parameterList.Add(new SqlParameter("@p1", followingEntity.user_id));
                contextEntities.Database.ExecuteSqlCommand("INSERT INTO Following(user_id, following_id) values (@p0, @p1)", parameterList.ToArray());
            }
        }
        public Person GetPersonDetails(string user_id)
        {
            var peopleEntity = contextEntities.People.FirstOrDefault(x => x.user_id == user_id);
            return peopleEntity;
        }

        public List<Person> GetAllPersonDetails()
        {
            return contextEntities.People.ToList();
        }
        public List<Person> GetAllPersonDetails(string searchName)
        {
            return contextEntities.People.Where(x=>x.fullName.Contains(searchName)).ToList();
        }
        public List<Person> GetAllFollowingDetailsUser(string user_id)
        {
            return contextEntities.People.FirstOrDefault(x => x.user_id == user_id).People.ToList();
        }
        public List<Person> GetAllFollowerDetailsUser(string user_id)
        {
            return contextEntities.People.FirstOrDefault(x => x.user_id == user_id).Person1.ToList();
        }
    }
}
