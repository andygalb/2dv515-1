using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationSystem
{
    public class UserService : IUserService
    {
        private List<User> users;
        private List<Rating> ratings;

        public UserService()
        {
            users = new List<User>();
            ratings = new List<Rating>();

            LoadUsers();
            LoadRatings();
        }

        public List<Rating> GetRatings(String userID) {

            if (userID == null) { return ratings; }
            else
            {
                List<Rating> specificList = ratings.FindAll(
                    delegate (Rating r)
                    {
                        return r.userID == userID;
                    });
                return specificList;
            }
           }

        public List<User> GetUsers() => users;

        private void LoadUsers()
        {
            using (var reader = new StreamReader("users.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    User u = new User();
                    u.name = values[0];
                    u.userID = values[1];
                    users.Add(u);
                }
            }
        }

        private void LoadRatings()
        {
            using (var reader = new StreamReader("ratings.csv"))
            {
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Rating r = new Rating();
                    r.userID = values[0];
                    r.filmName = values[1];
                    r.score = float.Parse(values[2], CultureInfo.InvariantCulture);
                    ratings.Add(r);

                }
            }
        }
    }
}
